using System.IO;

namespace WebService.Interfaces
{
    public interface IFileSaver
    {
        /// <summary>
        /// Stores a file to the underlying file system
        /// </summary>
        /// <param name="path">The directory path where the file should be stored</param>
        /// <param name="filename">The name of the file when stored</param>
        /// <param name="extension">The extension of the file when stored</param>
        /// <param name="data">The data that is to be stored in the file</param>
        void Store(string path, string filename, string extension, Stream data);
    }
}