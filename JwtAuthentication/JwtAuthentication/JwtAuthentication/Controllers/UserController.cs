using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtAuthentication.Auth;
using JwtAuthentication.Models;
using JwtAuthentication.Services;
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
        public IActionResult CreateUser([FromBody] User user)
        {
            _userService.CreateUser(user);
            return Ok(new
            {
                message = "Create User Done!",
                status = 200
            });
        }

        [HttpPost]
        public IActionResult Authenticate(AuthenticateRequest user)
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
