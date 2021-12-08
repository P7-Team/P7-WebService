using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
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
        private readonly ITokenValidator _tokenValidator;
        private readonly UserRepository _userRepository;
        public UserController(ITokenValidator tokenValidator, UserRepository userRepository)
        {
            _tokenValidator = tokenValidator;
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("signup")]
        public IActionResult SignUp([FromBody] UserDTO userDTO)
        {
            User user = userDTO.MapToUser();

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
        public IActionResult Login([FromBody] UserDTO userDto)
        {
            User user = userDto.MapToUser();
            bool exists = UserExists(user);

            if (exists)
            {
                // Create session token
                string rawToken = user.Username + user.Password + user.ContributionPoints;
                HashAlgorithm sha = SHA256.Create();
                Token token = new Token(Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(rawToken))));
                new TokenStore().Store(token.Key, user.Username);
                return Ok(token);   
            }
            else
                return NotFound();
        }
        
        [HttpDelete]
        [Route("logout")]
        public IActionResult Logout()
        {
            StringValues token;
            bool success = HttpContext.Request.Query.TryGetValue("tkn", out token);

            if (!success)
            {
                return NotFound();
            }
            
            // Currently token may include '+', which C# automatically converts to ' '
            string tokenStr = token.ToString().Replace(" ", "+");
            _tokenValidator.Invalidate(tokenStr);

            return Ok();
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