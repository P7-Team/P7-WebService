using System.IO;

namespace WebService.Services
{
    public class FileHelper
    {
        public static void ExtractFilenameAndPath(string abspath, out string path, out string filename)
        {
            int filenameBeginIdx = abspath.LastIndexOf(Path.DirectorySeparatorChar) + 1;
            int pathEndIdx = abspath.Length;
            path = abspath[0..filenameBeginIdx];
            filename = abspath[filenameBeginIdx..pathEndIdx];
        }
    }
}