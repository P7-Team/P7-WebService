using System.ComponentModel.DataAnnotations;

namespace WebService.Models.DTOs
{
    public class ProviderDTO
    {
        [Required]
        public string Username;

        public User MapToUser()
        {
            return new User(Username, null);
        }
    }
}