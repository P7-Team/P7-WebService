using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using WebService.Interfaces;
using WebService.Models;

namespace WebService.Services.Repositories
{
    public class TaskRepository : IRepository<Task, (long id, int number, int subnumber)>
    {
        private readonly QueryFactory _db;
        private const string table = "Task";

        public TaskRepository(QueryFactory db)
        {
            _db = db;
        }

        public void Create(Task item)
        {
            _db.Query(table).Insert(new
            {
                id = item.Id,
                number = item.Number,
                subNumber = item.SubNumber,
                allocatedTo = item.AllocatedTo,
            });
        }

        public Task Read((long id, int number, int subnumber) identifier)
        {
            return _db.Query(table).Where(new
            {
                id = identifier.id,
                number = identifier.number,
                subNumber = identifier.subnumber,
            }).First();
        }

        public void Update(Task item)
        {
            _db.Query(table).Where(new
            {
                id = item.Id,
                number = item.Number,
                subNumber = item.SubNumber,
            }).Update(new
            {
                allocatedTo = item.AllocatedTo,
            });
        }

        public void Delete((long id, int number, int subnumber) identifier)
        {
            _db.Query(table).Where(new
            {
                id = identifier.id,
                number = identifier.number,
                subNumber = identifier.subnumber,
            }).Delete();
        }
    }
}
