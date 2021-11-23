using System;
using System.Collections.Generic;
using System.IO;

namespace WebService_UnitTests.Fixtures
{
    public class FileFixture : IDisposable
    {
        public List<string> FilesToDelete { get; private set; }

        public FileFixture()
        {
            FilesToDelete = new List<string>();
        }

        public void Dispose()
        {
            foreach (string file in FilesToDelete)
            {
                if (File.Exists(file))
                    File.Delete(file);
            }
        }
    }
}