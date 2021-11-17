using WebService;
using WebService.Models;
using Xunit;

namespace WebService_UnitTests
{
    public class BatchTests
    {
        [Fact]
        public void AddTaskToBatch_No_Other_Tasks_Returns_Correct_Number()
        {
            Batch testBatch = new Batch(1);
            // Act
            testBatch.AddTask(new Task(false));
            // Assert
            Assert.Equal(0, testBatch.GetTask(0).Number);
        }

        [Fact]
        public void AddTaskToBatch_Multiple_Tasks_Returns_Correct_Number()
        {
            Batch testBatch = new Batch(1);
            // Act
            testBatch.AddTask(new Task(false));
            testBatch.AddTask(new Task(false));
            // Assert
            Assert.Equal(1, testBatch.GetTask(1).Number);
        }

        [Fact]
        public void AddTaskToBatch_No_Other_Tasks_Returns_Correct_SubNumber()
        {
            Batch testBatch = new Batch(1);
            // Act
            testBatch.AddTask(new Task(false));
            // Assert
            Assert.Equal(0, testBatch.GetTask(0).Number);
        }

        [Fact]
        public void AddTaskToBatch_Multiple_Replications_Returns_Correct_SubNumber()
        {
            Batch testBatch = new Batch(1);
            // Act
            testBatch.AddTask(new Task(false), 3);
            // Assert
            Assert.Equal(0, testBatch.GetTask(2).Number);
        }

        [Fact]
        public void AddTaskToBatch_Negative_Replication_Provided_One_Element_Added()
        {
            Batch testBatch = new Batch(1);
            // Act
            testBatch.AddTask(new Task(false), -1);
            // Assert
            Assert.Equal(0, testBatch.GetTask(0).Number);
        }


        [Fact]
        public void AddTaskToBatch_No_Other_Tasks_Returns_Correct_ID()
        {
            Batch testBatch = new Batch(1);
            // Act
            testBatch.AddTask(new Task(false));
            // Assert
            Assert.Equal(testBatch.Id, testBatch.GetTask(0).Id);
        }

        [Fact]
        public void TasksCount_Returns_Correct_Number()
        {
            Batch testBatch = new Batch(1);
            // Act
            testBatch.AddTask(new Task(false));
            // Assert
            Assert.Equal(1, testBatch.TasksCount());
        }

        [Fact]
        public void GetTask_Returns_Correct_Task()
        {
            Batch testBatch = new Batch(1);
            Task testTask = new Task(false);
            // Act
            testBatch.AddTask(testTask);
            // Assert
            Assert.Same(testTask, testBatch.GetTask(0));
        }

        [Fact]
        public void GetTask_Returns_Null_On_Out_Of_Index()
        {
            Batch testBatch = new Batch(1);
            // Act
            testBatch.AddTask(new Task(false));
            // Assert
            Assert.Null(testBatch.GetTask(20));
        }

        [Fact]
        public void RemoveTask_Task_No_Longer_In_Task_List()
        {
            Batch testBatch = new Batch(1);
            // ACT
            testBatch.AddTask(new Task(false));
            Task testTask = testBatch.GetTask(0);
            int tasksCount = testBatch.TasksCount();
            // Assert
            testBatch.RemoveTask(testTask);
            Assert.True(tasksCount == testBatch.TasksCount() + 1);
        }
    }
}