using System;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services.Stores;
using Xunit;
using System.IO;


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
            //TODO
            throw new NotImplementedException();
        }
        public void Delete((int id, int number, int subnumber) identifier)
        {
            throw new NotImplementedException();
        }

    }

    public class TaskStoreTests
    {
        [Fact (Skip = "Need to change RunDepository first due to changes for BathFile")]
        public void Store_GivenTask_SavesUsingGivenRepositories()
        {
            
            //Arrange
            Task testTask = new Task();
            Batch testBatch = new Batch(123);
            string path = Path.GetTempFileName();
            Stream data = new MemoryStream();
            BatchFile testFile = new BatchFile(".exe", "UTF-8", data, testBatch);
            MockTaskRepository taskRep = new MockTaskRepository(1, 1, 4);
            MockRunRepository runRep = new MockRunRepository(1, 1, 4);
            TaskStore taskStore = new TaskStore(taskRep,runRep);

            //Act
            taskStore.Store(testTask, testFile);

            //Assert
            Assert.Equal(testTask , taskRep.CalledTask);
            
        }


    }
}
