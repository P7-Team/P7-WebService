using System.ComponentModel.DataAnnotations;

namespace WebService.Models
{
    public class AuthenticateRequest
    {
        [Required] public string Username { get; set; }

        [Required] public string Password { get; set; }
    }
}