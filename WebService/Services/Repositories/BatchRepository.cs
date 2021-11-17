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
        private readonly QueryFactory _db;
        private const string table = "Batch";

        public BatchRepository(QueryFactory db)
        {
            _db = db;
        }

        public void Create(Batch item)
        {
            // Batch model does not currently contain enough information
            // to create a row in the database
            throw new NotImplementedException();
        }

        public Batch Read(int identifier)
        {
            int id = _db.Query(table).Where("id", identifier).Select("id").First();
            List<Task> tasks = (List<Task>)_db.Query("Task").Select("id as Id", "number as Number", "subNumber as SubNumber", "allocatedTo as AllocatedTo").Where("id", identifier).Get();
            return new Batch(id, tasks);
        }

        public void Update(Batch item)
        {
            TaskRepository taskRepository = new TaskRepository(_db);
            foreach (Task task in item.Tasks)
            {
                taskRepository.Update(task);
            }
        }

        public void Delete(int identifier)
        {
            _db.Query(table).Where("id", identifier).Delete();
        }
    }
}
