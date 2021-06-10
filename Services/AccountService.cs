using Architecture;
using AutoMapper;
using Business;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ApplicationException = Architecture.ApplicationException;
using BC = BCrypt.Net.BCrypt;

namespace Services
{
    public class AccountService : ServiceBase<Account>, IAccountService
    {
        private readonly EmailSettings _appSettings;
        private readonly IEmailService _emailService;

        public AccountService(IAccountRepository repository, IOptions<EmailSettings> appSettings, IEmailService emailService) : base(repository)
        {
            _appSettings = appSettings.Value;
            _emailService = emailService;
        }

        public async Task<(Account, string, string)> AuthenticateAsync(Account model, string ipAddress)
        {
            var account = (await this._repositoryBase.GetByIncludingAsync((e => e.Email == model.Email), false, (e => e.RefreshTokens))).FirstOrDefault();

            if (account == null || !account.IsVerified || !BC.Verify(model.Password, account.PasswordHash))
            {
                throw new ApplicationException("Email or password is incorrect");
            }

            var jwtToken = this.GenerateJwtToken(account);
            var refreshToken = this.GenerateRefreshToken(ipAddress);

            account.RefreshTokens.Add(refreshToken);
            await this._repositoryBase.UpdateAsync(account.Id, account);
            await this._repositoryBase.SaveChangesAsync();

            return (account, jwtToken, refreshToken.Token);
        }

        public async Task<(Account, string, string)> RefreshTokenAsync(string token, string ipAddress)
        {
            var (refreshToken, account) = await this.GetRefreshToken(token);

            var newRefreshToken = this.GenerateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            account.RefreshTokens.Add(newRefreshToken);

            await this._repositoryBase.UpdateAsync(account.Id, account);
            await this._repositoryBase.SaveChangesAsync();

            var jwtToken = this.GenerateJwtToken(account);

            return (account, jwtToken, newRefreshToken.Token);
        }

        public async Task RevokeTokenAsync(string token, string ipAddress)
        {
            var (refreshToken, account) = await this.GetRefreshToken(token);

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;

            await this._repositoryBase.UpdateAsync(account.Id, account);
            await this._repositoryBase.SaveChangesAsync();
        }

        public async Task RegisterAsync(Account model, string origin)
        {
            var account = await this._repositoryBase.GetByAsync((e => e.Email == model.Email));

            if (account != null)
            {
                this.SendAlreadyRegisteredEmail(model.Email, origin);
                return;
            }

            var isFirstAccount = this._repositoryBase.GetAll().Count() == 0;
            model.Role = isFirstAccount ? Role.Admin : Role.User;
            model.VerificationToken = this.RandomTokenString();
            model.PasswordHash = BC.HashPassword(model.Password);

            this.SendVerificationEmail(model, origin);
        }

        public async Task VerifyEmailAsync(string token)
        {
            var account = await this._repositoryBase.GetByAsync((e => e.VerificationToken == token));

            if (account == null)
            {
                throw new ApplicationException("Verification failed");
            }

            account.Verified = DateTime.UtcNow;
            account.VerificationToken = null;

            await this._repositoryBase.UpdateAsync(account.Id, account);
            await this._repositoryBase.SaveChangesAsync();
        }

        public async Task ForgotPasswordAsync(ForgotPasswordRequestDTO model, string origin)
        {
            var account = await this._repositoryBase.GetByAsync((e => e.Email == model.Email));

            if (account == null)
            {
                return;
            }

            account.ResetToken = this.RandomTokenString();
            account.ResetTokenExpires = DateTime.UtcNow.AddDays(24);

            await this._repositoryBase.UpdateAsync(account.Id, account);
            await this._repositoryBase.SaveChangesAsync();

            this.SendPasswordResetEmail(account, origin);
        }

        public async Task ValidateResetTokenAsync(ValidateResetTokenRequestDTO model)
        {
            var account = await this._repositoryBase.GetByAsync((e => e.ResetToken == model.Token && e.ResetTokenExpires > DateTime.UtcNow));

            if (account == null)
            {
                throw new ApplicationException("Invalid token");
            }
        }

        public async Task ResetPasswordAsync(ResetPasswordRequestDTO model)
        {
            var account = await this._repositoryBase.GetByAsync((e => e.ResetToken == model.Token && e.ResetTokenExpires > DateTime.UtcNow));

            if (account == null)
            {
                throw new ApplicationException("Invalid token");
            }

            account.PasswordHash = BC.HashPassword(model.Password);
            account.PasswordReset = DateTime.UtcNow;
            account.ResetToken = null;
            account.ResetTokenExpires = null;

            await this._repositoryBase.UpdateAsync(account.Id, account);
            await this._repositoryBase.SaveChangesAsync();
        }

        public override async Task<ICollection<Account>> GetAllAsync()
        {
            var accounts = await this._repositoryBase.GetAllAsync();
            return accounts;
        }

        public override async Task<Account> GetByIdAsync(Guid id)
        {
            var account = await this._repositoryBase.GetByIdAsync(id);
            if (account == null) throw new KeyNotFoundException("Account not found");
            return account;
        }

        public override async Task<Account> CreateAsync(Account model)
        {
            var exist = await this._repositoryBase.GetByAsync((e => e.Email == model.Email));

            if (exist != null)
            {
                throw new ApplicationException($"Email '{model.Email}' is already registered");
            }

            var account = model;
            account.Verified = DateTime.UtcNow;
            account.PasswordHash = BC.HashPassword(model.PasswordHash);

            await this._repositoryBase.CreateAsync(account);
            await this._repositoryBase.SaveChangesAsync();

            return account;
        }

        public override async Task<Account> UpdateAsync(Guid id, Account model)
        {
            var account = await this.GetByIdAsync(id);
            var exist = await this._repositoryBase.GetByAsync((e => e.Email == model.Email));

            if (account.Email != model.Email && exist != null)
            {
                throw new ApplicationException($"Email '{model.Email}' is already taken");
            }

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.PasswordHash))
            {
                account.PasswordHash = BC.HashPassword(model.PasswordHash);
            }

            await this._repositoryBase.CreateAsync(account);
            await this._repositoryBase.SaveChangesAsync();

            return account;
        }

        public override async Task<Account> DeleteAsync(Guid id)
        {
            var account = await this.GetByIdAsync(id);
            await this._repositoryBase.DeleteAsync(account);
            await this._repositoryBase.SaveChangesAsync();

            return account;
        }

        private async Task<(RefreshToken, Account)> GetRefreshToken(string token)
        {
            var account = await this._repositoryBase.GetByAsync((e => e.RefreshTokens.Any(t => t.Token == token)));
            if (account == null)
            {
                throw new ApplicationException("Invalid token");
            }
            var refreshToken = account.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive) 
            { 
                throw new ApplicationException("Invalid token");
            }
            return (refreshToken, account);
        }

        private string GenerateJwtToken(Account account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", account.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                CreatedByIp = ipAddress
            };
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }

        private void SendVerificationEmail(Account account, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var verifyUrl = $"{origin}/account/verify-email?token={account.VerificationToken}";
                message = $@"<p>Please click the below link to verify your email address:</p>
                             <p><a href=""{verifyUrl}"">{verifyUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to verify your email address with the <code>/accounts/verify-email</code> api route:</p>
                             <p><code>{account.VerificationToken}</code></p>";
            }

            _emailService.Send(
                to: account.Email,
                subject: "Sign-up Verification API - Verify Email",
                html: $@"<h4>Verify Email</h4>
                         <p>Thanks for registering!</p>
                         {message}"
            );
        }

        private void SendAlreadyRegisteredEmail(string email, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
                message = $@"<p>If you don't know your password please visit the <a href=""{origin}/account/forgot-password"">forgot password</a> page.</p>";
            else
                message = "<p>If you don't know your password you can reset it via the <code>/accounts/forgot-password</code> api route.</p>";

            _emailService.Send(
                to: email,
                subject: "Sign-up Verification API - Email Already Registered",
                html: $@"<h4>Email Already Registered</h4>
                         <p>Your email <strong>{email}</strong> is already registered.</p>
                         {message}"
            );
        }

        private void SendPasswordResetEmail(Account account, string origin)
        {
            string message;
            if (!string.IsNullOrEmpty(origin))
            {
                var resetUrl = $"{origin}/account/reset-password?token={account.ResetToken}";
                message = $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                             <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
            }
            else
            {
                message = $@"<p>Please use the below token to reset your password with the <code>/accounts/reset-password</code> api route:</p>
                             <p><code>{account.ResetToken}</code></p>";
            }

            _emailService.Send(
                to: account.Email,
                subject: "Sign-up Verification API - Reset Password",
                html: $@"<h4>Reset Password Email</h4>
                         {message}"
            );
        }
    }
}
