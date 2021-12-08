using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using WebService.Interfaces;
using WebService.Models;

namespace WebService.Services.Repositories
{
    public class ResultRepository : IRepository<Result, (string path, string filename)>
    {
        private readonly QueryFactory _db;
        private const string _table = "Result";
        private const string _generalizedTable = "File";

        public ResultRepository(QueryFactory db)
        {
            _db = db;
        }

        public (string path, string filename) Create(Result item)
        {
            try
            {
                _db.Query(_table).Insert(new
                {
                    path = item.Path,
                    filename = item.Filename,
                    isVerified = item.Verified,
                    task_id = item.TaskId,
                    task_number = item.TaskNumber,
                    task_subnumber = item.TaskSubnumber
                });

            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);                
            }
            return (item.Path, item.Filename);
        }

        public Result Read((string path, string filename) identifier)
        {
            return _db.Query(_table)
                .Select("Result.path AS Path", "Result.filename AS Filename", "encoding", "includedIn AS batchId", "isVerified AS verified", "task_id AS TaskId", "task_number AS TaskNumber", "task_subnumber AS TaskSubnumber")
                .Join(_generalizedTable, j => j.On("File.path", "Result.path").On("File.filename", "Result.filename"))
                .Where("Result.path", identifier.path).Where("Result.filename", identifier.filename)
                .FirstOrDefault<Result>();
        }
        
        public Result Read((int id, int number, int subnumber) identifier)
        {
            return _db.Query(_table)
                .Select("Result.path AS Path", "Result.filename AS Filename", "encoding", "includedIn AS batchId", "isVerified AS verified", "task_id AS TaskId", "task_number AS TaskNumber", "task_subnumber AS TaskSubnumber")
                .Join(_generalizedTable, j => j.On("File.path", "Result.path").On("File.filename", "Result.filename"))
                .Where("task_id", identifier.id).Where("task_number", identifier.number).Where("task_subnumber", identifier.subnumber)
                .FirstOrDefault<Result>();
        }

        public void Update(Result item)
        {
            _db.Query(_table).Where(new
            {
                path = item.Path,
                filename = item.Filename
            }).Update(new
            {
                isVerified = item.Verified,
                task_id = item.TaskId,
                task_number = item.TaskNumber,
                task_subnumber = item.TaskSubnumber
            });
        }

        public void Delete((string path, string filename) identifier)
        {
            _db.Query(_table).Where(new
            {
                path = identifier.path,
                filename = identifier.filename,
            }).Delete();
        }
    }
}
