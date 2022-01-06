using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Data;
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
            this._db = db;
        }

        public int Create(Batch item)
        {
            // Create batch row in DB, and get the id
            return _db.Query(table).InsertGetId<int>(new { ownedBy = item.OwnerUsername });
        }

        public Batch Read(int identifier)
        {
            return _db.Query(table).Select("id as Id", "ownedBy as OwnerUsername")
                    .Where("id", identifier).FirstOrDefault<Batch>();
        }

        public List<Batch> Read(string user)
        {
            if (_db.Connection.State == ConnectionState.Closed)
            {
                _db.Connection.Open();
            }
            return _db.Query(table).Where("ownedBy", user).Get<Batch>().ToList();
        }

        public void Update(Batch item)
        {
            _db.Query(table).Where("id", item.GetIdentifier()).Update(new { ownedBy = item.OwnerUsername });
        }

        public void Delete(int identifier)
        {
            _db.Query(table).Where("id", identifier).Delete();
        }
    }
}
