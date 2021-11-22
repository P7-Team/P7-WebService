using System.IO;

namespace WebService.Interfaces
{
    public interface IFileSaver
    {
        void Store(string path, string filename, string extension, Stream data);
    }
}