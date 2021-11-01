using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using WebService.Controllers;
using WebService.Interfaces;
using WebService.Models;
using Xunit;
using Xunit.Abstractions;
using Task = WebService.Models.Task;

namespace WebService_UnitTests
{
    public class TaskControllerTests
    {
        [Fact]
        public void GetReadyTask_ContextIsEmpty_ReturnsNull()
        {
            // Arrange
            ITaskContext context = new TaskContext();

            TaskController controller = new TaskController(context);

            // Act
            Task task = controller.GetReadyTask();
            
            // Assert
            Assert.Null(task);
        }

        [Fact]
        public void GetReadyTask_ContextContainsTask_ReadyTask_ReturnsReadyTask()
        {
            // Arrange
            ITaskContext context = new TaskContext();
            Task expectedTask = new Task(true);
            context.Add(expectedTask);

            TaskController controller = new TaskController(context);

            // Act
            Task actualTask = controller.GetReadyTask();

            // Assert
            Assert.Same(expectedTask, actualTask);
        }

        [Fact]
        public void GetReadyTask_ContextContainsTask_NotReadyTask_ReturnsNull()
        {
            // Arrange
            ITaskContext context = new TaskContext();
            context.Add(new Task(false));

            TaskController controller = new TaskController(context);
            
            // Act
            Task actualTask = controller.GetReadyTask();
            
            // Assert
            Assert.Null(actualTask);
        }

        [Fact]
        public void GetReadyTask_ContextContainsMultipleTasks_OneReadyTask_ReturnsReadyTask()
        {
            // Arrange
            ITaskContext context = new TaskContext();
            Task expectedTask = new Task(true);
            
            context.Add(new Task(false));
            context.Add(expectedTask);

            TaskController controller = new TaskController(context);

            // Act
            Task actualTask = controller.GetReadyTask();
            
            // Assert
            Assert.Same(expectedTask, actualTask);
        }

        [Fact]
        public void GetReadyTask_ContextContainsMultipleTasks_TwoReadyTasks_ReturnsFirstReadyTask()
        {
            // Arrange
            ITaskContext context = new TaskContext();
            Task expectedTask = new Task(true);
            context.Add(expectedTask);
            context.Add(new Task(true));

            TaskController controller = new TaskController(context);
            
            // Act
            Task actualTask = controller.GetReadyTask();
            
            // Assert
            Assert.Same(expectedTask, actualTask);
        }
    }
}