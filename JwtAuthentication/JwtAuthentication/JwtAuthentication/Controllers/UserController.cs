using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtAuthentication.Auth;
using JwtAuthentication.Entity;
using JwtAuthentication.Exceptions;
using JwtAuthentication.Models;
using JwtAuthentication.Services;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllUser()
        {
            var result = _userService.GetAll();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] RegisterModel user)
        {
            try
            {
                var userData = user.Adapt<User>();
                var userCreated = _userService.CreateUser(userData, user.Password);
                return Ok(new
                {
                    message = "Create User Done!",
                    userInfo = userCreated,
                    status = 200
                });
            }
            catch
            {
                return BadRequest(new
                {
                    message = "User Doesn't Create!"
                });
            }
        }

        [HttpPost]
        public IActionResult Authenticate([FromBody] AuthenticateRequest user)
        {
            var response = _userService.Authenticate(user, ipAddress());

            if (response == null) return BadRequest(new
            {
                message = "Username or Password is incorrect!"
            });

            setTokenCookie(response.RefreshToken);

            return Ok(response);
        }

        [HttpPost]
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = _userService.RefreshToken(refreshToken, ipAddress());

            if (response == null)
            {
                var IsRevoke = _userService.RevokeToken(refreshToken, ipAddress());
                return Unauthorized(new
                {
                    message = "Invalid Token",
                    IsRevoke = IsRevoke
                });
            }

            setTokenCookie(refreshToken);

            return Ok(response);
        }

        [Authorize]
        [HttpPost]
        public IActionResult RevokeToken([FromBody] RevokeTokenRequest revokeToken)
        {
            var token = revokeToken.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required!" });

            var response = _userService.RevokeToken(token, ipAddress());

            if (!response)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }

        private void setTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
