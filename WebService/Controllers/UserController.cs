using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using WebService.AuthenticationHelpers;
using WebService.Interfaces;
using WebService.Models;
using WebService.Models.DTOs;
using WebService.Services;
using WebService.Services.Repositories;

namespace WebService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticatorService _authenticatorService;
        private readonly UserRepository _userRepository;

        public UserController(IAuthenticatorService authenticatorService, UserRepository userRepository)
        {
            _authenticatorService = authenticatorService;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("signup")]
        public IActionResult SignUp([FromBody] UserDTO userDto)
        {
            User user = userDto.MapToUser();
            user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            bool exists = UserExists(user);

            if (!exists)
            {
                _userRepository.Create(user);
                return Created(HttpContext.Request.Path.Value, new EmptyResult());
            }

            return UnprocessableEntity();
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public IActionResult Login(AuthenticateRequest model)
        {
            var response = _authenticatorService.Authenticate(model);

            if (response == null)
                return BadRequest(new {message = "Username or password is incorrect"});

            return Ok(response);
        }

        [HttpDelete]
        [AuthenticationHelpers.Authorize]
        [Route("logout")]
        public IActionResult Logout()
        {
            //StringValues token;
            //bool success = HttpContext.Request.Query.TryGetValue("tkn", out token);


            return Ok(HttpContext.Items["User"]);
        }

        /// <summary>
        /// Helper method which can be used to check whether a user exists
        /// </summary>
        /// <param name="user">The user instance to check if exists</param>
        /// <returns>A boolean representing whether a user exists in the DB</returns>
        private bool UserExists(User user)
        {
            return _userRepository.Read(user.GetIdentifier()) != null;
        }
    }
}