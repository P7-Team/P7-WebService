using SqlKata.Execution;
using System.Linq;
using WebService.Models;
using WebService.Interfaces;

namespace WebService.Services.Stores
{
    public class TaskStore : IStore<Task>
    {
        private readonly IRepository<Task, (int id, int number, int subnumber)> _taskRepository;
        private readonly IRepository<Run, (int id, int number, int subnumber, string path, string filename)> _runRepository;

        public TaskStore(IRepository<Task, (int id, int number, int subnumber)> taskRepository, 
                         IRepository<Run, (int id, int number, int subnumber, string path, string filename)> runRepository)
        {
            _taskRepository = taskRepository;
            _runRepository = runRepository;
        }

        /// <summary>
        /// Stores the details of a task, then stores its association with a file as a run. 
        /// </summary>
        /// <param name="task">Task that should be stored.</param>
        /// <param name="batchFile">The input file associated with task.</param>
        public void Store(Task task, BatchFile batchFile)
        {
            Run run = new Run(task.Id, task.Number, task.SubNumber, batchFile.Path, batchFile.Filename);

            _taskRepository.Create(task);
            _runRepository.Create(run);
        }

        public void Store(Task task)
        {
            Store(task, task.Input);
        }
    }
}

