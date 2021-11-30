using WebService;
using WebService.Models.DTOs;
using Xunit;

namespace WebService_UnitTests
{
    public class UserDTOTests
    {
        [Fact]
        public void MapToUser_Creates_aNewUserInstance()
        {
            // Setup
            const string testUsername = "TestUser";
            const string testPassword = "TestPassword";

            UserDTO dto = new UserDTO()
            {
                Username = testUsername,
                Password = testPassword
            };
            
            // Assert
            Assert.IsType<User>(dto.MapToUser());
        }

        [Fact]
        public void MapToUser_NewUserInstance_HaveSameFieldValue()
        {
            // Setup
            const string testUsername = "TestUser";
            const string testPassword = "TestPassword";

            UserDTO dto = new UserDTO()
            {
                Username = testUsername,
                Password = testPassword
            };
            
            // Act
            User user = dto.MapToUser();
            
            // Assert
            Assert.Equal(dto.Username, user.Username);
            Assert.Equal(dto.Password, user.Password);
        }
    }
}