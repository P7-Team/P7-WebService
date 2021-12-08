using System.ComponentModel.DataAnnotations;

namespace WebService.Models
{
    public class LoginInput
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}