using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Models;

namespace WebService.Interfaces
{
    public interface IFileStore
    {
        public void StoreSourceFile(SourceFile sourceFile);
        public void StoreInputFiles(IEnumerable<BatchFile> inputFiles);
    }
}
