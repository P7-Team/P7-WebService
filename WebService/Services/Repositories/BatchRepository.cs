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

        public BatchRepository(QueryFactory db)
        {
            this.db = db;
        }

        public int Create(Batch item)
        {
            // Create batch row in DB, and get the id
            return db.Query(table).InsertGetId<int>(new { ownedBy = item.OwnerUsername });
        }

        public Batch Read(int identifier)
        {
            int id = db.Query(table).Where("id", identifier).Select("id").First();
            List<Task> tasks = (List<Task>)db.Query("Task").Select("id as Id", "number as Number", "subNumber as SubNumber", "allocatedTo as AllocatedTo").Where("id", identifier).Get();
            return new Batch(id, tasks);
        }

        public void Update(Batch item)
        {
        }

        public void Delete(int identifier)
        {
            db.Query(table).Where("id", identifier).Delete();
        }
    }
}
