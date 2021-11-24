using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Interfaces;
using WebService.Models;

namespace WebService.Services.Repositories
{
    public class ResultRepository : IRepository<Result, (string path, string filename)>
    {
        private readonly QueryFactory _db;
        private const string table = "Result";

        public ResultRepository(QueryFactory db)
        {
            _db = db;
        }

        public (string path, string filename) Create(Result item)
        {
            return _db.Query(table).InsertGetId<(string path, string filename)>(new
            {
                path = item.Path,
                filename = item.Filename,
                isVerified = item.Verified,
                task_id = item.Task.Id,
                task_number = item.Task.Number,
                task_subnumber = item.Task.SubNumber
            });
        }

        public Result Read((string path, string filename) identifier)
        {
            return _db.Query(table).Where(new
            {
                path = identifier.path,
                filename = identifier.filename
            })
                .Join("Task", j =>
                    j.On("Result.task_id", "Task.id")
                    .On("Result.task_number", "Task.number")
                    .On("Result.task_subnumber", "Task.subNumber")
                 )
                .First<Result>();
        }

        public void Update(Result item)
        {
            _db.Query(table).Where(new
            {
                path = item.Path,
                filename = item.Filename
            }).Update(new
            {
                isVerified = item.Verified,
                task_id = item.Task.Id,
                task_number = item.Task.Number,
                task_subnumber = item.Task.SubNumber
            });
        }

        public void Delete((string path, string filename) identifier)
        {
            _db.Query(table).Where(new
            {
                path = identifier.path,
                filename = identifier.filename,
            }).Delete();
        }
    }
}
