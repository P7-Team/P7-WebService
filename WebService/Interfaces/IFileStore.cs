using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Models;

namespace WebService.Interfaces
{
    public interface IFileStore
    {
        /// <summary>
        /// Handles storing the <see cref="SourceFile"/> object
        /// </summary>
        /// <param name="sourceFile">The <see cref="SourceFile"/> to store</param>
        public void StoreFile(SourceFile sourceFile);
        /// <summary>
        /// Handles storing the <see cref="Result"/> object
        /// </summary>
        /// <param name="resultFile">The <see cref="Result"/> to store</param>
        public void StoreFile(Result resultFile);
        /// <summary>
        /// Handles storing an enumerable of <see cref="BatchFile"/> objects
        /// </summary>
        /// <param name="batchFiles">The <see cref="BatchFile"/>s to store</param>
        public void StoreFiles(IEnumerable<BatchFile> batchFiles);
    }
}
