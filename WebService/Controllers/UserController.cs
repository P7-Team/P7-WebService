using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using WebService.AuthenticationHelpers;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services;

namespace WebService.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly ITokenValidator _tokenValidator;
        private readonly IAuthenticatorService _authenticatorService;
        public UserController(ITokenValidator tokenValidator, IAuthenticatorService authenticatorService)
        {
            _authenticatorService = authenticatorService;
            _tokenValidator = tokenValidator;
        }

        [HttpPost]
        [Route("signup")]
        [AllowAnonymous]
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
        [Route("login")]
        [AllowAnonymous]
        public IActionResult Login(AuthenticateRequest model)
        {
            var response = _authenticatorService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }
        
        [HttpDelete]
        [AuthenticationHelpers.Authorize]
        [Route("logout")]
        public IActionResult Logout()
        {
            //StringValues token;
            //bool success = HttpContext.Request.Query.TryGetValue("tkn", out token);
            

            return Ok();
        }
    }
}