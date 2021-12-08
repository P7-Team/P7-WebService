using System;
using WebService.Models;

namespace WebService.Services
{
    public class TaskWrapper
    {
        public DateTime LastPing;
        public User user;
        public Task Task;
        public DateTime AssignedAt;

        public TaskWrapper(Task task)
        {
            user = null;
            Task = task;
            LastPing = DateTime.Now;
        }
    }
}