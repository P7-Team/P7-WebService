using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Models;

namespace WebService.Interfaces
{
    public interface IFileStore
    {
        public void StoreFile(SourceFile sourceFile);
        public void StoreFile(Result resultFile);
        public void StoreFiles(IEnumerable<BatchFile> batchFiles);
    }
}
