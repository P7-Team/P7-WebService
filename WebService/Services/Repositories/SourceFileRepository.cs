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
        private const string _generalizedTable = "File";
        
        public SourceFileRepository(QueryFactory db)
        {
            _db = db;
        }
        
        public (string path, string filename) Create(SourceFile item)
        {
            _db.Query(_table).Insert(new
            {
                path = item.Path,
                filename = item.Filename,
                language = item.Language,
                batchId = item.BatchId,
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
            return _db.Query(_table)
                .Select("Source.path as path", "Source.filename as filename", "includedIn AS batchId", "language", "encoding")
                .Where("Source.path", identifier.path).Where("Source.filename", identifier.filename)
                .Join(_generalizedTable, j => j.On("Source.path", "File.path").On("Source.filename", "File.filename"))
                .FirstOrDefault<SourceFile>();
        }

        public SourceFile Read(int batchId)
        {
            return _db.Query(_table)
                .Select("Source.path as path", "Source.filename as filename", "includedIn AS batchId", "language", "encoding")
                .Where("Source.batchId", batchId)
                .Join(_generalizedTable, j => j.On("Source.path", "File.path").On("Source.filename", "File.filename"))
                .FirstOrDefault<SourceFile>();
        }

        public void Update(SourceFile item)
        {
            _db.Query(_table).Where(new
            {
                path = item.Path,
                filename = item.Filename
            }).Update(new
            {
                language = item.Language
            });
        }
    }
}
