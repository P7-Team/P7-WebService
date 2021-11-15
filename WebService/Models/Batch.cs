using System.Collections.Generic;
using System.Linq;
using WebService.Models;


namespace WebService.Models
{
    public class Batch
    {
        private List<Task> _tasks;

        public int Id { get; }

        public Task GetTask(int entry)
        {
            if (entry < TasksCount())
            {
                return _tasks[entry];
            }

            return null;
        }

        public void RemoveTask(Task task)
        {
            _tasks.Remove(task);
        }

        public void RemoveTask(int number, int subNumber)
        {
            _tasks.RemoveAll(x => x.Id == Id && x.Number == number && x.SubNumber == subNumber);
        }

        public int TasksCount()
        {
            return _tasks.Count;
        }

        public void AddTask(Task task, int replications = 1)
        {
            task.Id = Id;
            task.Number = _tasks.Count;
            if (replications < 0)
            {
                replications = 1;
            }

            for (int i = 0; i < replications; i++)
            {
                task.SubNumber = i;
                _tasks.Add(task);
            }
        }

        public Batch(int id)
        {
            Id = id;
            _tasks = new List<Task>();
        }
    }
}