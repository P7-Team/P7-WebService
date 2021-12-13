using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebService.Models;

namespace WebService.Interfaces
{
    public interface IFileStore
    {
        public string Directory { get; }
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

        /// <summary>
        /// Fetches a given file from the filesystem.
        /// </summary>
        /// <param name="file">The <see cref="BatchFile"/> representation of the file to fetch</param>
        /// <returns>A <see cref="Stream"/> of the data in the file, null if <see cref="BatchFile.Path"/> or <see cref="BatchFile.Filename"/> is null on the object or file could not be found</returns>
        public Stream FetchFile(BatchFile file);
        
        
        /// <summary>
        /// Deletes a given <see cref="BatchFile"/> from the filesystem
        /// </summary>
        /// <param name="file">The <see cref="BatchFile"/> representation of the file to delete</param>
        /// <returns>true if the file was successfully deleted, false otherwise</returns>
        public bool DeleteFile(BatchFile file);
        
        /// <summary>
        /// Deletes a given <see cref="SourceFile"/> from the filesystem
        /// </summary>
        /// <param name="file">The <see cref="SourceFile"/> representation of the file to delete</param>
        /// <returns>true if the file was successfully deleted, false otherwise</returns>
        
        public bool DeleteFile(SourceFile sourceFile);
        /// <summary>
        /// Deletes a given <see cref="Result"/> from the filesystem
        /// </summary>
        /// <param name="file">The <see cref="Result"/> representation of the file to delete</param>
        /// <returns>true if the file was successfully deleted, false otherwise</returns>
        public bool DeleteFile(Result resultFile);
    }
}
