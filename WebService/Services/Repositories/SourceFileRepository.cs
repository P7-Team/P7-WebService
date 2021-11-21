using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Interfaces;
using WebService.Models;

namespace WebService.Services.Repositories
{
    public class SourceFileRepository : IRepository<SourceFile, (string path, string filename)>
    {
        public void Create(SourceFile item)
        {
            throw new NotImplementedException();
        }

        public void Delete((string path, string filename) identifier)
        {
            throw new NotImplementedException();
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
