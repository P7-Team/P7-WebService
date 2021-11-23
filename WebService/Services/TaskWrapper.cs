using System;
using WebService.Models;

namespace WebService.Services
{
    public class TaskWrapper
    {
        public DateTime LastPing;
        public bool isDone;
        public User user;
        public Task Task;
        public DateTime AssignedAt;

        public TaskWrapper(Task task, User user)
        {
            this.user = user;
            isDone = false;
            Task = task;
            LastPing = DateTime.Now;
        }
    }
}