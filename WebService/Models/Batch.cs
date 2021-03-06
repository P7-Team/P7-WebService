using System.Collections.Generic;
using System.Linq;
using WebService.Interfaces;
using WebService.Models;


namespace WebService.Models
{
    public class Batch : IAggregateRoot<int>
    {
        public List<Task> Tasks { get; set; } = new List<Task>();

        public int Id { get; set; }
        public string OwnerUsername { get; set; }
        public List<BatchFile> InputFiles { get; set; }
        public SourceFile SourceFile { get; set; }
        public int ReplicationFactor { get; set; }

        public Batch() { }

        public Batch(int id)
        {
            Id = id;
            Tasks = new List<Task>();
        }

        public Batch(int id, string owner)
        {
            Id = id;
            OwnerUsername = owner;
        }

        public Batch(int id, List<Task> tasks)
        {
            Id = id;
            Tasks = tasks;
        }

        public Task GetTask(int entry)
        {
            if (entry < TasksCount())
            {
                return Tasks[entry];
            }

            return null;
        }
        
        public Task GetTask(int number,int subNumber)
        {
            foreach (var task in Tasks)
            {
                if (task.Number == number && task.SubNumber == subNumber)
                {
                    return task;
                }
            }

            return null;
        }

        public void RemoveTask(Task task)
        {
            Tasks.Remove(task);
        }

        public void RemoveTask(int number, int subNumber)
        {
            Tasks.RemoveAll(x => x.Id == Id && x.Number == number && x.SubNumber == subNumber);
        }

        public int TasksCount()
        {
            return Tasks.Count;
        }

        public void AddTask(Task task, int replications = 1)
        {
            task.Id = Id;
            task.Number = Tasks.Count;
            if (replications < 0)
            {
                replications = 1;
            }

            for (int i = 0; i < replications; i++)
            {
                task.SubNumber = i;
                Tasks.Add(task);
            }
        }

        public int GetIdentifier()
        {
            return Id;
        }
    }
}