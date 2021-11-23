namespace WebService.Interfaces
{
    public interface IFileDeleter
    {
        /// <summary>
        /// Delete the file at the specified path
        /// </summary>
        /// <param name="path">The directory path of the file to delete</param>
        /// <param name="filename">The filename of the file to delete</param>
        /// <param name="extension">The extension of the file to delete</param>
        /// <returns>true if file was deleted, false if file did not exists or it was unsuccessful</returns>
        bool Delete(string path, string filename, string extension);
    }
}