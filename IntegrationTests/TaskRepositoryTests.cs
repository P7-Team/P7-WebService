using System;
using System.Collections.Generic;
using System.Text;
using IntegrationTests.Fixture;
using Xunit;
using WebService.Services.Repositories;
using WebService.Models;

namespace IntegrationTests
{
    public class TaskRepositoryTests : IClassFixture<DatabaseFixture>
    {
        TaskRepository taskRepository;
        public TaskRepositoryTests(DatabaseFixture dbFixture)
        {
            dbFixture.Clean(new string[] { "Task" });

            taskRepository = new TaskRepository(dbFixture.Db);

        }

        // Conditional skip, change to null in order to run integration tests
        const string skip = "Integration Test, should not be run along with unit tests";
        //const string skip = null;

        [Fact(Skip = skip)]
        public void GivenTask_InsertedTaskCanBeRead()
        {
            //Arrange
            Task task = new Task();
            
            //Act

            //Assert

        }

        //GivenTask_InsertedTaskCanBeUpdated

        //GivenTask_InsertedTaskCanBeDeleted

    }
}
