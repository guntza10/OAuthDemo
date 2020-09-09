using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JwtAuthentication.Interfaces;
using JwtAuthentication.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthentication.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IAppSettings appSettings)
        {
            _next = next;
            _appSettings = appSettings;
        }

        public Task Invoke(HttpContext httpContext, IUserService userService)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
      
            if (token != null) attachUserToContext(httpContext, userService, token);

            return _next(httpContext);
        }

        private void attachUserToContext(HttpContext httpContext, IUserService userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.SecretKey);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set ให้ token หมดอายุทันที เพราะโดยปกติมันจะต้องรอ 5 นาทีให้หลัง
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userName = jwtToken.Claims.FirstOrDefault(it => it.Type == "sub").Value;

                httpContext.Items["User"] = userService.GetByUsername(userName);
            }
            catch
            {
                // ไม่ต้องทำอะไร เมื่อมัน jwt validation fail
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class JwtMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtMiddleware>();
        }
    }
}
