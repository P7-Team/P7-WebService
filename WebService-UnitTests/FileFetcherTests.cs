

using System.IO;
using System.Text;
using WebService.Interfaces;
using WebService.Services;
using WebService_UnitTests.Fixtures;
using Xunit;

namespace WebService_UnitTests
{
    public class FileFetcherTests : IClassFixture<FileFixture>
    {
        private readonly FileFixture _fileFixture;

        public FileFetcherTests(FileFixture fileFixture)
        {
            _fileFixture = fileFixture;
        }

        [Fact]
        public void Fetch_GivenFileNotExists_ThrowsFileNotFound()
        {
            // Setup
            IFileFetcher fetcher = new FileFetcher();
            const string nonExistingPath = "rubbish";
            const string nonExistingFilename = "congratulations";
            const string nonExistingExtension = ".halleluja";

            // Assert
            Assert.Throws<FileNotFoundException>(() =>
            {
                fetcher.Fetch(nonExistingPath, nonExistingFilename, nonExistingExtension);
            });
        }

        [Fact]
        public void Fetch_GivenFileExists_ContentMatchExpected()
        {
            // Setup
            const string expectedContent = "This is awesome content that cannot be contained";
            string path = Path.GetTempPath();
            const string filename = "123";
            const string extension = ".abc";
            CreateFileWithUtf8ContentAtPath(path, filename, extension, expectedContent);

            IFileFetcher fetcher = new FileFetcher();

            // Act
            Stream fileContent = fetcher.Fetch(path, filename, extension);
            string actualContent = ExtractContentFromStream(fileContent);

            // Assert
            Assert.Equal(expectedContent, actualContent);
            
            // Cleanup
            fileContent.Close();
        }


        private void CreateFileWithUtf8ContentAtPath(string path, string filename, string extension, string content)
        {
            Stream file = File.Create(path + filename + extension);
            file.Write(Encoding.UTF8.GetBytes(content));
            file.Close();
            _fileFixture.FilesToDelete.Add(path + filename + extension);
        }

        private string ExtractContentFromStream(Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer);

            return Encoding.UTF8.GetString(buffer);
        }
    }
}