using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebService.Models.DTOs
{
    public class UserDTO
    {
        [FromBody, BindRequired]
        public string Username { get; set; }
        [FromBody, BindRequired]
        public string Password { get; set; }

        /// <summary>
        /// Defines a new instance of a <see cref="User"/> and binds the required properties
        /// </summary>
        /// <returns>An instance of a <see cref="User"/></returns>
        public User MapToUser()
        {
            User user = new User(Username, Password);

            return user;
        }
    }
}