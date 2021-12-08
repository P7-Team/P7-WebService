using System.IO;

namespace WebService.Interfaces
{
    public interface IFileFetcher
    {
        /// <summary>
        /// Fetch the data stored in a given file
        /// </summary>
        /// <param name="path">The directory path to the file to read from</param>
        /// <param name="filename">The name of the file to read from</param>
        /// <param name="extension">The extension of the file to read from</param>
        /// <returns>A stream containing the data from the specified file</returns>
        /// <exception cref="FileNotFoundException">If the specified combination of "path", "filename", and "extension" does not match a file in the filesystem</exception>
        Stream Fetch(string path, string filename, string extension);
    }
}