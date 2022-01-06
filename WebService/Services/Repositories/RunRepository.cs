using SqlKata.Execution;
using System.Linq;
using WebService.Interfaces;
using WebService.Models;

namespace WebService.Services.Repositories
{
    public class RunRepository : IRepository<Run, (int id, int number, int subnumber, string path, string filename)>
    {
        private readonly QueryFactory _db;
        private const string _table = "Runs";

        public RunRepository(QueryFactory db)
        {
            _db = db;
        }
        public (int id, int number, int subnumber, string path, string filename) Create(Run item)
        {
            _db.Query(_table).Insert(new
            {
                task_id = item.Id,
                task_number = item.Number,
                task_subnumber = item.SubNumber,
                path = item.Path,
                filename = item.FileName
            });
            return item.GetIdentifier();
        }

        public Run Read((int id, int number, int subnumber, string path, string filename) identifier)
        {
            return _db.Query(_table).Select(
                    "task_id as Id",
                    "task_number as Number",
                    "task_subnumber as Subnumber",
                    "path as Path",
                    "filename as FileName"
                )
                .Where(MatchesPrimaryKey(identifier)).FirstOrDefault<Run>();
        }

        public void Update(Run item)
        {
            _db.Query(_table).Where(MatchesPrimaryKey(item)).Update(new
            {
                path = item.Path,
                filename = item.FileName
            });
        }
        public void Delete((int id, int number, int subnumber, string path, string filename) identifier)
        {
            _db.Query(_table).Where(MatchesPrimaryKey(identifier)).Delete();
        }

        /// <summary>
        ///  Creates an object with members corresponding to the elements of the primary key,
        ///  and values that defined by the given identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns>Anonymous object that can be in a Where clause to select a specific row</returns>
        private object MatchesPrimaryKey((int id, int number, int subnumber, string path, string filename) identifier)
        {
            return new { 
                task_id = identifier.id,
                task_number = identifier.number,
                task_subnumber = identifier.subnumber,
                path = identifier.path,
                filename = identifier.filename
            };
        }

        private object MatchesPrimaryKey(Run run) => MatchesPrimaryKey(run.GetIdentifier());
    }
}
