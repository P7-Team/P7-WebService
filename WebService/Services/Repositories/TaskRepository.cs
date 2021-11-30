using SqlKata.Execution;
using System.Linq;
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
            return (item.Id, item.Number, item.SubNumber);
        }

        public Task Read((int id, int number, int subnumber) identifier)
        {
            return _db.Query(_table).Select("is as Id", "number as Number", "subnumber as Subnumber")
                .Where(new { 
                id = identifier.id,
                number = identifier.number,
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
