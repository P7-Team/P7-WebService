using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using WebService.Models;
using WebService.Services;

namespace WebService.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ITokenValidator _tokenValidator;
        public UserController(ITokenValidator tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }

        [HttpPost]
        [Route("api/user/signup")]
        public IActionResult SignUp()
        {
            string content = ContentReader.ReadStreamContent(HttpContext.Request.Body);
            User user = JsonConvert.DeserializeObject<User>(content);

            // TODO: Check if user already exists, this needs to be updated
            bool exists = false;

            if (!exists)
                return Created(HttpContext.Request.Path.Value, new EmptyResult());
            else
                return UnprocessableEntity();
        }

        [HttpPost]
        [Route("api/user/login")]
        public IActionResult Login()
        {
            string content = ContentReader.ReadStreamContent(HttpContext.Request.Body);
            User user = JsonConvert.DeserializeObject<User>(content);
            
            // TODO: Check if user credentials match a persisted user
            bool exists = true;

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
        [Route("api/user/logout")]
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
    }
}