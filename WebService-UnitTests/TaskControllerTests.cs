using System;
using System.Net.Http;
using WebService.Controllers;
using Xunit;
using Xunit.Abstractions;

namespace WebService_UnitTests
{
    public class TaskControllerTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public TaskControllerTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestTest()
        {
            Assert.Equal("Something", "Something");
        }

        [Fact]
        public void GetReturnsSomething()
        {
            // Arrange
            // var controller = new TaskController();
            //
            //
            // // Act
            // string n = controller.GetTask(2);
            //
            // _testOutputHelper.WriteLine(n);
            //
            // // Assert

        }
    }
}