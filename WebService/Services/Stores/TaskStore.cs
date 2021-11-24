using SqlKata.Execution;
using System.Linq;
using WebService.Models;
using WebService.Interfaces;

namespace WebService.Services.Stores
{
    public class TaskStore
    {
        private readonly QueryFactory _db;
        private const string _table = "Runs";
        private readonly IRepository<Task, (long id, int number, int subnumber)> _taskRepository;

        public TaskStore(IRepository<Task, (long id, int number, int subnumber)> taskRepository, QueryFactory db)
        {
            _taskRepository = taskRepository;
            _db = db;
        }

        /// <summary>
        /// Stores the details of a task, then stores its association with a file as a run. 
        /// </summary>
        /// <param name="task">Task the should be stored.</param>
        /// <param name="batchFile">The file associated with task.</param>
        public void Store(Task task,BatchFile batchFile)
        {
            _taskRepository.Create(task);
            
            Run run = new Run(task.Id, task.Number,task.SubNumber);
            run.Path = batchFile.Path;
            run.FileName = batchFile.Filename;

            CreateRunInDB(run);

        }

        private void CreateRunInDB(Run run)
        {
            _db.Query(_table).Insert(new
            {
                id = run.Id,
                number = run.Number,
                subnumber = run.SubNumber,
                path = run.Path,
                filename = run.FileName
            });
        }
    }
}

