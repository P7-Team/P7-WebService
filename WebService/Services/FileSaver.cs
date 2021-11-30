using System.IO;
using WebService.Interfaces;

namespace WebService.Services
{
    public class FileSaver : IFileSaver
    {
        public void Store(string path, string filename, string extension, Stream data)
        {
            string fullpath = path + filename + extension;
            Stream fileStream = File.Open(fullpath, FileMode.Create);
            data.CopyTo(fileStream);
            fileStream.Close();
            data.Close();
        }
    }
}