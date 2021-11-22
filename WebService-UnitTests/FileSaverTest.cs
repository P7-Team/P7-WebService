using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WebService.Interfaces;
using WebService.Services;
using Xunit;

namespace WebService_UnitTests
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
    
    public class FileSaverTest : IClassFixture<FileFixture>
    {
        private readonly FileFixture _fileFixture;

        public FileSaverTest(FileFixture fixture)
        {
            _fileFixture = fixture;
        }

        [Fact]
        public void Store_FileNotAlreadyExist_FileCreatedAndDataSaved()
        {
            string path = Path.GetTempFileName();
            // Setup
            string filename = "test_file1";
            string extension = ".txt";

            IFileSaver fileSaver = new FileSaver();
            string textData = "This is awesome data that is stored in the file";
            MemoryStream data = new MemoryStream();
            data.Write(Encoding.UTF8.GetBytes(textData));
            data.Position = 0;
            long dataLength = data.Length;

            bool existedBefore = File.Exists(path + filename + extension);

            // Act
            fileSaver.Store(path, filename, extension, data);
            _fileFixture.FilesToDelete.Add(path + filename + extension);

            // Assert
            Assert.False(existedBefore);

            Stream actualData = File.OpenRead(path + filename + extension);
            byte[] buffer = new byte[dataLength];
            actualData.ReadAsync(buffer, 0, (int)dataLength).Wait();
            string actual = Encoding.UTF8.GetString(buffer);

            Assert.Equal(textData, actual);
            
            // Cleanup
            actualData.Close();
            data.Close();
        }
    }
}