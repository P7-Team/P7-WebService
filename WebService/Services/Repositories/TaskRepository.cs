using System;
using System.Collections.Generic;
using SqlKata.Execution;
using System.Linq;
using MySql.Data.MySqlClient;
using WebService.Interfaces;
using WebService.Models;

namespace WebService.Services.Repositories
{
    public class TaskRepository : IRepository<Task, (int id, int number, int subnumber)>
    {
        private readonly QueryFactory _db;
        private const string _table = "Task";

        public TaskRepository(QueryFactory db)
        {
            _db = db;
        }

        public (int id, int number, int subnumber) Create(Task item)
        {
            _db.Query(_table).Insert(new
            {
                id = item.Id,
                number = item.Number,
                subNumber = item.SubNumber,  
            });

            return item.GetIdentifier();
        }

        public Task Read((int id, int number, int subnumber) identifier)
        {
            try
            {
                return _db.Query(_table).Select("id as Id", "number as Number", "subnumber as Subnumber")
                    .Where(new { 
                        id = identifier.id,
                        number = identifier.number,
                        subnumber = identifier.subnumber
                    }).FirstOrDefault<Task>();
            }
            catch (MySqlException e)
            {
                // This removes duplicate entries, if attempted...
                Console.WriteLine(e);
                return null;
            }

        }

        public List<Task> Read(int batchId)
        {
            return _db.Query(_table)
                .Where(new { 
                    id = batchId,
                }).Get<Task>().ToList();
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

        public void Delete((int id, int number, int subnumber) identifier)
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
