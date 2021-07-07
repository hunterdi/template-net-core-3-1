using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Architecture
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JWTSettings _jwtSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<JWTSettings> appSettings)
        {
            _next = next;
            _jwtSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, ApplicationDataContext dataContext)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                await AttachAccountToContext(context, dataContext, token);
            }
            else
            {
                await _next.Invoke(context);
            }

        }

        private async Task AttachAccountToContext(HttpContext context, ApplicationDataContext dataContext, string token)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;

            var accountId = context.AccountID(_jwtSettings);
            // attach account to context on successful jwt validation
            var account = await dataContext.Accounts.FindAsync(accountId);

            if (account == null)
            {
                throw new UnauthorizedAccessException("Unauthorized");
            }
            else
            {
                context.Items["Account"] = account;
                await _next.Invoke(context);
            }
        }
    }
}
