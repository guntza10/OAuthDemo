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
        public IActionResult Authenticate([FromBody]AuthenticateRequest user)
        {
            var response = _userService.Authenticate(user);

            if (response == null) return BadRequest(new
            {
                message = "Username or Password is incorrect!"
            });

            return Ok(response);
        }
    }
}
