using SqlKata.Execution;
using System.Linq;
using WebService.Interfaces;
using WebService.Models;

namespace WebService.Services.Repositories
{
    public class RunRepository : IRepository<Run, (int id, int number, int subnumber)>
    {
        private readonly QueryFactory _db;
        private const string _table = "Task";

        public RunRepository(QueryFactory db)
        {
            _db = db;
        }
        public (int id, int number, int subnumber) Create(Run item)
        {
            return _db.Query(_table).InsertGetId<(int id, int number, int subNumber)>(new
            {
                id = item.Id,
                number = item.Number,
                subNumber = item.SubNumber,
            });
        }

        public Run Read((int id, int number, int subnumber) identifier)
        {
            return _db.Query(_table).Select("is as Id", "number as Number", "subnumber as Subnumber")
                .Where(new
                {
                    id = identifier.id,
                    number = identifier.number,
                    subnumber = identifier.subnumber
                }).First<Run>();
        }

        public void Update(Run item)
        {
            _db.Query(_table).Where(new
            {
                id = item.Id,
                number = item.Number,
                subNumber = item.SubNumber,
            }).Update(new
            {
                path = item.Path,
                filename = item.FileName
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
