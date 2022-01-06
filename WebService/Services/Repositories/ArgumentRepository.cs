using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Interfaces;
using WebService.Models;

namespace WebService.Services.Repositories
{
    public class ArgumentRepository : IRepository<Argument, (string path, string filename, int number)>
    {
        private Dictionary<string, string> _columnsToProperties;
        private readonly QueryFactory _db;
        private const string _table = "Argument";

        public ArgumentRepository(QueryFactory db)
        {
            _db = db;
            _columnsToProperties = new Dictionary<string, string>()
            {
                { "path", "Path" }, { "filename", "Filename" }, { "number", "Number"}, { "arg", "Value" }
            };
        }

        public (string path, string filename, int number) Create(Argument item)
        {
            _db.Query(_table).Insert(new
            {
                path = item.Path,
                filename = item.Filename,
                number = item.Number,
                arg = item.Value
            });
            return item.GetIdentifier();
        }

        public void Delete((string path, string filename, int number) identifier)
        {
            _db.Query(_table).Where(MatchesPrimaryKey(identifier)).Delete();
        }

        public Argument Read((string path, string filename, int number) identifier)
        {
            // Defines the columns that should be selected, and renames them to match the names of the properties
            List<string> ColumnsToSelect = _columnsToProperties.Select(pair => pair.Key + " as " + pair.Value).ToList();

            return _db.Query(_table).Select(ColumnsToSelect.ToArray())
                .Where(MatchesPrimaryKey(identifier)).FirstOrDefault<Argument>();
        }

        public void Update(Argument item)
        {
            _db.Query(_table).Where(MatchesPrimaryKey(item)).Update(new
            {
                path = item.Path,
                filename = item.Filename,
                number = item.Number,
                arg = item.Value
            });
        }

        /// <summary>
        ///  Creates an object with members corresponding to the elements of the primary key,
        ///  and values that defined by the given identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns>Anonymous object that can be in a Where clause to select a specific row</returns>
        private object MatchesPrimaryKey((string path, string filename, int number) identifier)
        {
            return new { path = identifier.path, filename = identifier.filename, number = identifier.number };
        }

        private object MatchesPrimaryKey(Argument argument) => MatchesPrimaryKey(argument.GetIdentifier());
    }
}
