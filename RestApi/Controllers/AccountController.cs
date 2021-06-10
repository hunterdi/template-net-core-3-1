using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Architecture;
using AutoMapper;
using Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace RestApi
{
    public class AccountController : BaseController<Account, AccountDTO>
    {
        protected readonly IAccountService _accountService;

        public AccountController(IAccountService accountService, IMapper mapper): base(accountService, mapper)
        {
            this._accountService = accountService;
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult<AccountDTO>> Authenticate([FromBody] LoginDTO model)
        {
            var domain = this._mapper.Map<Account>(model);
            var (account, jwtToken, refreshToken) = await this._accountService.AuthenticateAsync(domain, IpAddress());

            var response = this._mapper.Map<AccountDTO>(account);
            response.JwtToken = jwtToken;
            response.RefreshToken = refreshToken;

            this.SetTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AccountDTO>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var (account, jwtToken, newRefreshToken) = await _accountService.RefreshTokenAsync(refreshToken, IpAddress());
            
            var response = this._mapper.Map<AccountDTO>(account);
            response.JwtToken = jwtToken;
            response.RefreshToken = newRefreshToken;

            this.SetTokenCookie(newRefreshToken);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequestDTO model)
        {
            // accept token from request body or cookie
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token is required" });
            }

            // users can revoke their own tokens and admins can revoke any tokens
            if (!_account.OwnsToken(token) && _account.Role != Role.Admin)
            {
                return Unauthorized(new { message = "Unauthorized" });
            }

            await _accountService.RevokeTokenAsync(token, IpAddress());
            return Ok(new { message = "Token revoked" });
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] AccountInsertDTO model)
        {
            var domain = this._mapper.Map<Account>(model);
            _accountService.RegisterAsync(domain, Request.Headers["origin"]);
            return Ok(new { message = "Registration successful, please check your email for verification instructions" });
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequestDTO model)
        {
            await this._accountService.VerifyEmailAsync(model.Token);
            return Ok(new { message = "Verification successful, you can now login" });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDTO model)
        {
            await this._accountService.ForgotPasswordAsync(model, Request.Headers["origin"]);
            return Ok(new { message = "Please check your email for password reset instructions" });
        }

        [HttpPost("validate-reset-token")]
        public async Task<IActionResult> ValidateResetToken([FromBody] ValidateResetTokenRequestDTO model)
        {
            await _accountService.ValidateResetTokenAsync(model);
            return Ok(new { message = "Token is valid" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO model)
        {
            await _accountService.ResetPasswordAsync(model);
            return Ok(new { message = "Password reset successful, you can now login" });
        }

        [Authorize(Role.Admin)]
        [HttpGet]
        public override async Task<ActionResult<ICollection<AccountDTO>>> GetAll()
        {
            var accounts = await this._accountService.GetAllAsync();
            var dtos = this._mapper.Map<ICollection<AccountDTO>>(accounts);

            return Ok(accounts);
        }

        [Authorize(Role.Admin)]
        [HttpGet("{id}")]
        public override async Task<ActionResult<AccountDTO>> GetById([FromRoute] Guid id)
        {
            // users can get their own account and admins can get any account
            if (id != _account.Id)
            {
                return Unauthorized(new { message = "Unauthorized" });
            }

            var account = await _accountService.GetByIdAsync(id);
            var dto = this._mapper.Map<AccountDTO>(account);

            return Ok(dto);
        }

        [Authorize(Role.Admin)]
        [HttpPost]
        public async Task<ActionResult<AccountDTO>> Create([FromBody] AccountInsertDTO model)
        {
            var domain = this._mapper.Map<Account>(model);
            var account = await _accountService.CreateAsync(domain);
            return Ok(account);
        }

        [Authorize(Role.Admin)]
        [HttpPut("{id}")]
        public async Task<ActionResult<AccountDTO>> Update([FromRoute] Guid id, [FromBody] AccountUpdateDTO model)
        {
            // users can update their own account and admins can update any account
            if (id != _account.Id)
            {
                return Unauthorized(new { message = "Unauthorized" });
            }

            // only admins can update role
            if (_account.Role != Role.Admin)
            {
                model.Role = null;
            }

            var account = await this._accountService.GetByIdAsync(id);

            _mapper.Map(model, account);

            account = await _accountService.UpdateAsync(id, account);
            return Ok(account);
        }

        [Authorize(Role.Admin)]
        [HttpDelete("{id}")]
        public override async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // users can delete their own account and admins can delete any account
            if (id != _account.Id)
            {
                return Unauthorized(new { message = "Unauthorized" });
            }

            await _accountService.DeleteAsync(id);
            return Ok(new { message = "Account deleted successfully" });
        }

        private void SetTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }
    }
}
