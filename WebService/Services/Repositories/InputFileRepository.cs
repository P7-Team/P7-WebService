using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using WebService.Interfaces;
using WebService.Models;
using WebService.Services;

namespace WebService.Services.Repositories
{
    public class InputFileRepository : IRepository<InputFile, (string path, string filename)>
    {
        private readonly QueryFactory _db;
        private const string table = "File";

        public InputFileRepository(QueryFactory db)
        {
            _db = db;
        }

        public void Create(InputFile item)
        {
            _db.Query(table).Insert(new
            {
                path = item.Path,
                filename = item.Filename,
                encoding = item.Encoding,
                includedIn = item.BatchId
            });
        }

        public InputFile Read((string path, string filename) identifier)
        {
            return _db.Query(table).Where(new
            {
                path = identifier.path,
                filename = identifier.filename
            }).First();
        }

        public void Update(InputFile item)
        {
            _db.Query(table).Where(new
            {
                path = item.Path,
                filename = item.Filename
            }).Update(new { encoding = item.Encoding, includedIn = item.BatchId });
        }

        public void Delete((string path, string filename) identifier)
        {
            _db.Query(table).Where(new
            {
                path = identifier.path,
                filename = identifier.filename
            }).Delete();
        }
    }
}
