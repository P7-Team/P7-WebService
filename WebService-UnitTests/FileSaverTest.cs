using System.IO;
using System.Text;
using WebService.Interfaces;
using WebService.Services;
using WebService_UnitTests.Fixtures;
using Xunit;

namespace WebService_UnitTests
{
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
            // Setup
            string path = Path.GetTempPath();
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

        [Fact]
        public void Store_FileAlreadyExists_OriginalDataOverriden()
        {
            // Setup
            string path = Path.GetTempPath();
            string filename = "ultimate";
            string extension = ".uha";
            const string content = "This is something new, something you have never seen before!";
            
            string originalContent = CreateRandomFile(path, filename, extension, out int originalContentLength);
            bool existedBefore = File.Exists(path + filename + extension);

            IFileSaver fileSaver = new FileSaver();
            Stream data = new MemoryStream();
            data.Write(Encoding.UTF8.GetBytes(content));
            data.Position = 0;
            int dataLength = (int)data.Length;
            _fileFixture.FilesToDelete.Add(path + filename + extension);

            // Act
            fileSaver.Store(path, filename, extension, data);

            Stream storedFile = File.OpenRead(path + filename + extension);
            byte[] buffer = new byte[dataLength];
            storedFile.Read(buffer, 0, dataLength);
            string actualContent = Encoding.UTF8.GetString(buffer);
            
            // Assert
            Assert.True(existedBefore);
            Assert.NotEqual(originalContent, actualContent);
            Assert.Equal(content, actualContent);



            // Cleanup
            data.Close();
            storedFile.Close();
        }

        [Fact]
        public void Store_ExtensionIsNull_FileNotExistedBefore_FileCreatedAndSavedWithoutExtension()
        {
            // Setup
            string path = Path.GetTempPath();
            string filename = "test_file3";
            string extension = null;

            IFileSaver fileSaver = new FileSaver();
            string textData = "This is awesome data that is stored in the file";
            MemoryStream data = new MemoryStream();
            data.Write(Encoding.UTF8.GetBytes(textData));
            data.Position = 0;
            long dataLength = data.Length;

            bool existedBefore = File.Exists(path + filename + extension);
            
            // Act
            fileSaver.Store(path, filename, extension, data);
            
            // Assert
            Assert.False(existedBefore);
            
            // Cleanup
            _fileFixture.FilesToDelete.Add(path + filename + extension);
            data.Close();
            
        }

        private string CreateRandomFile(string path, string filename, string extension, out int length)
        {
            string content = "Some original data in the stream";

            Stream file = File.Open(path + filename + extension, FileMode.Create);
            file.Write(Encoding.UTF8.GetBytes(content));
            length = (int)file.Length;
            file.Close();

            return content;
        }

    }
}