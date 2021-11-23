using System.IO;
using WebService.Interfaces;

namespace WebService.Services
{
    public class FileFetcher : IFileFetcher
    {
        public Stream Fetch(string path, string filename, string extension)
        {
            Stream fileStream = File.OpenRead(path + filename + extension);
            return fileStream;
        }
    }
}