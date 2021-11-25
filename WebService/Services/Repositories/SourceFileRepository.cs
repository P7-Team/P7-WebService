using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SqlKata.Execution;
using WebService.Interfaces;
using WebService.Models;

namespace WebService.Services.Repositories
{
    public class SourceFileRepository : IRepository<SourceFile, (string path, string filename)>
    {
        private QueryFactory _db;
        private const string _table = "Source";
        public SourceFileRepository(QueryFactory db)
        {
            _db = db;
        }
        
        public (string path, string filename) Create(SourceFile item)
        {
            _db.Query(_table).Insert(new
            {
                language = item.Language,
            });
            return (item.Path, item.Filename);
        }

        public void Delete((string path, string filename) identifier)
        {
            _db.Query(_table).Where(new
            {
                path = identifier.path,
                filename = identifier.filename,
            }).Delete();
        }

        public SourceFile Read((string path, string filename) identifier)
        {
            throw new NotImplementedException();
        }

        public void Update(SourceFile item)
        {
            throw new NotImplementedException();
        }
    }
}
