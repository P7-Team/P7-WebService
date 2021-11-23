using SqlKata.Execution;
using SqlKata.Compilers;
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
        private const string _table = "Task";

        public TaskRepository(QueryFactory db)
        {
            _db = db;
        }

        public void Create(Task item)
        {
            _db.Query(_table).Insert(new
            {
                id = item.Id,
                number = item.Number,
                subNumber = item.SubNumber,  //For byzentine checking
            });
        }

        public Task Read((long id, int number, int subnumber) identifier)
        {
            return _db.Query(_table).Select("is as Id", "number as Number", "subnumber as Subnumber")
                .Where(new { 
                id = identifier.id
                number = identifier.number
                subnumber = identifier.subnumber
                }).First<Task>();
        }

        public void Update(Task item)
        {
            _db.Query(_table).Where(new
            {
                id = item.Id,
                number = item.Number,
                subNumber = item.SubNumber,
            }).Update(new
            {
                startedOn = item.StartedOn,
                finishedOn = item.FinishedOn,
                allocatedTo = item.AllocatedTo,
            });
        }

        public void Delete((long id, int number, int subnumber) identifier)
        {
            _db.Query(_table).Where(new
            {
                id = identifier.id,
                number = identifier.number,
                subNumber = identifier.subnumber,
            }).Delete();
        }
    }
}
