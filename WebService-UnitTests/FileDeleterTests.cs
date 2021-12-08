using System.IO;
using System.Text;
using WebService.Interfaces;
using WebService.Services;
using WebService_UnitTests.Fixtures;
using Xunit;

namespace WebService_UnitTests
{
    public class FileDeleterTests : IClassFixture<FileFixture>
    {
        private readonly FileFixture _fileFixture;

        public FileDeleterTests(FileFixture fixture)
        {
            _fileFixture = fixture;
        }

        [Fact]
        public void Delete_FileNotExists_ReturnFalse()
        {
            // Setup
            const string nonExistingPath = "Party";
            const string nonExistingFilename = "BG3";
            const string nonExistingExtension = ".win";
            IFileDeleter deleter = new FileDeleter();

            // Act
            bool actual = deleter.Delete(nonExistingPath, nonExistingFilename, nonExistingExtension);

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void Delete_FileExists_ReturnTrue()
        {
            // Setup
            string path = Path.GetTempPath();
            const string filename = "ThisIsMyFile";
            const string extension = ".life";
            CreateFileWithContent(path, filename, extension);

            IFileDeleter deleter = new FileDeleter();

            // Act
            bool actual = deleter.Delete(path, filename, extension);
            bool exists = File.Exists(path + filename + extension);

            // Assert
            Assert.True(actual);
            Assert.False(exists);
        }


        private void CreateFileWithContent(string path, string filename, string extension,
            string content = "This is how we party")
        {
            Stream fileStream = File.Create(path + filename + extension);
            fileStream.Write(Encoding.UTF8.GetBytes(content));
            fileStream.Close();
            _fileFixture.FilesToDelete.Add(path + filename + extension);
        }
        
    }
}