using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using WebService.Interfaces;
using WebService.Models;

namespace WebService.Services.Repositories
{
    public class BatchRepository : IRepository<Batch, int>
    {
        private readonly QueryFactory db;
        private const string table = "Batch";
        private readonly TaskRepository taskRepository;

        public BatchRepository(QueryFactory db, TaskRepository taskRepository)
        {
            this.db = db;
            this.taskRepository = taskRepository;
        }

        public void Create(Batch item)
        {
            // Create batch row in DB, and get the id
            var batchId = db.Query(table).InsertGetId<int>(new { ownedBy = item.OwnerUsername });
            item.Id = batchId;

            // Create tasks in DB with reference to the batch
            foreach (Task task in item.Tasks)
            {
                task.Id = batchId;
                taskRepository.Create(task);
            }
        }

        public Batch Read(int identifier)
        {
            int id = db.Query(table).Where("id", identifier).Select("id").First();
            List<Task> tasks = (List<Task>)db.Query("Task").Select("id as Id", "number as Number", "subNumber as SubNumber", "allocatedTo as AllocatedTo").Where("id", identifier).Get();
            return new Batch(id, tasks);
        }

        public void Update(Batch item)
        {
            foreach (Task task in item.Tasks)
            {
                taskRepository.Update(task);
            }
        }

        public void Delete(int identifier)
        {
            db.Query(table).Where("id", identifier).Delete();
        }
    }
}
