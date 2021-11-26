using System;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services.Stores;
using Xunit;
using System.IO;
using Newtonsoft.Json;


//TODO: Finish later.
namespace WebService_UnitTests
{
    public class MockTaskRepository : IRepository<Task, (int id, int number, int subnumber)>
    {
        private Task _task;

        public MockTaskRepository(int id, int number, int subnumber)
        {
            _task = new Task(id, number, subnumber);
        }

        public Task CalledTask { get; private set; }

        public (int id, int number, int subnumber) Create(Task item)
        {
            CalledTask = item;
            
            return (item.Id, item.Number,item.SubNumber);
        }

        public Task Read((int id, int number, int subnumber) identifier)
        {
            throw new NotImplementedException();
        }

       public void Update(Task item)
        {
            throw new NotImplementedException();
        }

        public void Delete((int id, int number, int subnumber) identifier)
        {
            throw new NotImplementedException();
        }
    }

    public class MockRunRepository : IRepository<Run, (int id, int number, int subnumber)>
    {
        private Run _run;

        public MockRunRepository(int id, int number, int subnumber)
        {
            _run = new Run(id,number,subnumber);
        }

        public Run CalledRun { get; private set; }

        public (int id, int number, int subnumber) Create(Run item)
        {
            CalledRun = item;
            return (item.Id, item.Number, item.SubNumber);
        }

        public Run Read((int id, int number, int subnumber) identifier)
        {
            throw new NotImplementedException();
        }

        public void Update(Run item)
        {
            CalledRun.Path = item.Path;
            CalledRun.FileName = item.FileName;            
        }
        public void Delete((int id, int number, int subnumber) identifier)
        {
            throw new NotImplementedException();
        }

    }

    public class TaskStoreTests
    {
        [Fact]
        public void Store_GivenRepositories_SavesTask()
        {
            
            //Arrange
            Task testTask = new Task(1, 2, 4);
            Batch testBatch = new Batch(123);
            
            Stream data = new MemoryStream();
            BatchFile testFile = new BatchFile(".mp3", "UTF-8", data, testBatch);
            testFile.Path = Path.GetTempPath();
            testFile.Filename = Path.GetTempFileName();

            MockTaskRepository taskRep = new MockTaskRepository(0, 0, 0);
            MockRunRepository runRep = new MockRunRepository(0, 0, 0);

            TaskStore taskStore = new TaskStore(taskRep,runRep);

            //Act
            taskStore.Store(testTask, testFile);

            //Assert
            Assert.Equal(testTask, taskRep.CalledTask);
        }

        [Fact]
        public void Store_GivenRepositories_SavesRun()
        {

            //Arrange
            Task testTask = new Task(3, 5, 2);
            Batch testBatch = new Batch(123);

            Stream data = new MemoryStream();
            BatchFile testFile = new BatchFile(".py", "UTF-8", data, testBatch);
            testFile.Path = Path.GetTempPath();
            testFile.Filename = Path.GetTempFileName();

            MockTaskRepository taskRep = new MockTaskRepository(0, 0, 0);
            MockRunRepository runRep = new MockRunRepository(0, 0, 0);

            TaskStore taskStore = new TaskStore(taskRep, runRep);

            //Act
            taskStore.Store(testTask, testFile);

            //Assert
            Run assertRun = new Run(testTask.Id, testTask.Number, testTask.SubNumber);
            assertRun.Path = testFile.Path;
            assertRun.FileName = testFile.Filename;

            //To avoid direct object comparison, need to look at their content.
            var comapare1 = JsonConvert.SerializeObject(assertRun);
            var comapare2 = JsonConvert.SerializeObject(runRep.CalledRun);

            Assert.Equal(comapare1, comapare2);
        }       
    }
}
