using System;
using System.IO;
using WebService.Interfaces;

namespace WebService.Services
{
    public class FileDeleter : IFileDeleter
    {
        public bool Delete(string path, string filename, string extension)
        {
            if (!File.Exists(path + filename + extension))
            {
                return false;
            }

            try
            {
                File.Delete(path + filename + extension);
            }
            catch (Exception e)
            {
                // If anything went wrong, then it was unsuccessful
                return false;
            }

            return true;
        }
    }
}