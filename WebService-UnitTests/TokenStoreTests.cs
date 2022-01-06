using WebService.Interfaces;
using WebService.Services;
using Xunit;

namespace WebService_UnitTests
{
    public class TokenStoreTests
    {
        [Fact]
        public void Store_DataIsStoredOverMultipleInstances()
        {
            // Setup
            const string testToken = "ThisIsAWeirdToken!2157623";
            const string testUsername = "user";
            ITokenStore instance1 = new TokenStore();
            ITokenStore instance2 = new TokenStore();
            
            // Act
            instance1.Store(testToken, testUsername);
            string actualUsername = instance2.Fetch(testToken);

            // Assert
            Assert.Equal(testUsername, actualUsername);
        }

        [Fact]
        public void Store_TokenAlreadyExists_NoOverridingChosen()
        {
            // Setup
            const string testToken = "Token";
            const string testUser1 = "user1";
            const string testUser2 = "user2";
            ITokenStore store = new TokenStore();
            store.Store(testToken, testUser1);
            const bool overrideExisting = false;

            // Act
            store.Store(testToken, testUser2, overrideExisting);
            string actual = store.Fetch(testToken);

            // Assert
            Assert.Equal(testUser1, actual);
        }

        [Fact]
        public void Fetch_TokenNotExists_ReturnEmptyString()
        {
            // Setup
            const string nonExistingToken = "toktok";
            ITokenStore store = new TokenStore();
            
            // Act
            string actual = store.Fetch(nonExistingToken);

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public void Fetch_TokenExists_ReturnsAssociatedUser()
        {
            // Setup
            const string token = "tuktuk";
            const string user = "SuperUser";
            ITokenStore store = new TokenStore();
            store.Store(token, user);

            // Act
            string actual = store.Fetch(token);

            // Assert
            Assert.Equal(user, actual);

        }
    }
}