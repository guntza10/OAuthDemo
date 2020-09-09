using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtAuthentication.Interfaces;
using JwtAuthentication.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

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
