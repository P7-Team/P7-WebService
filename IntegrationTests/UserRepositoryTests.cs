using MySql.Data.MySqlClient;
using SqlKata.Compilers;
using SqlKata.Execution;
using System;
using System.IO;
using WebService;
using WebService.Services;
using Xunit;

namespace IntegrationTests
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            var connection = new MySqlConnection("");

            var compiler = new MySqlCompiler();

            Db = new QueryFactory(connection, compiler);
        }

        public void Dispose()
        {
            // ... clean up test data from the database ...
        }

        public QueryFactory Db { get; private set; }
    }

    public class UserRepositoryTests : IClassFixture<DatabaseFixture>
    {
        // change to skip = null, in order to run integration tests
        // Check https://josephwoodward.co.uk/2019/01/skipping-xunit-tests-based-on-runtime-conditions for conditional skip
        const string skip = "Integration Test, should not be run along with unit tests";

        UserRepository repository;

        public UserRepositoryTests(DatabaseFixture fixture)
        {
            repository = new UserRepository(fixture.Db);
        }

        // Inserts user into the database
        private void InsertUser(User user)
        {
            repository.Create(user);
        }

        // Inserts user in DB, and ensures that it can also be read by repository
        // Throws assertion error if it fails
        private void EnsureUser(User user)
        {
            repository.Create(user);
            User result = repository.Read(user.GetIdentifier());
            Assert.Equal(user.Username, result.Username);
        }

        [Fact(Skip = skip)]
        public void Create_GivenUser_InsertsUserInDB()
        {
            User user = new User("testUser", "testPwd") { ContributionPoints = 11 };
            repository.Create(user);

            User result = repository.Read(user.GetIdentifier());

            Assert.Equal("testUser", result.Username);
            Assert.Equal("testPwd", result.Password);
            Assert.Equal(11, result.ContributionPoints);
        }

        [Fact(Skip = skip)]
        public void Read_GivenIdentifierForExistingUser_ReturnsUser()
        {
            // Create the user in the DB
            User user = new User("readTestUser", "testPwd") { ContributionPoints = 123 };
            InsertUser(user);

            User result = repository.Read(user.GetIdentifier());

            Assert.Equal("readTestUser", result.Username);
            Assert.Equal("testPwd", result.Password);
            Assert.Equal(123, result.ContributionPoints);
        }

        [Fact(Skip = skip)]
        public void Update_GivenValidUser_UpdatesUserInDB()
        {
            // Create the user that should be updated in the DB
            User oldUser = new User("updateTestUser", "oldPassword") { ContributionPoints = 2 };
            InsertUser(oldUser);

            User newUser = new User("updateTestUser", "newPassword") { ContributionPoints = 88 };
            repository.Update(newUser);

            User result = repository.Read(oldUser.GetIdentifier());

            Assert.Equal("updateTestUser", result.Username);
            Assert.Equal("newPassword", result.Password);
            Assert.Equal(88, result.ContributionPoints);
        }

        [Fact(Skip = skip)]
        public void Delete_GivenIdForUserInDB_DeletesUserFromDB()
        {
            User user = new User("deleteTestUser", "testPwd") { ContributionPoints = 94 };
            // Insert user to be deleted, and ensure that it is in the DB
            EnsureUser(user);

            repository.Delete(user.GetIdentifier());
            User resultAfterDelete = repository.Read(user.GetIdentifier());

            Assert.Null(resultAfterDelete);
        }
    }
}
