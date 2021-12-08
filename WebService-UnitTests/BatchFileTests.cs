using System.IO;
using System.Text;
using WebService.Models;
using Xunit;

namespace WebService_UnitTests
{
    public class BatchFileTests
    {
        [Fact]
        public void ValidForFileSystem_PathIsNull_ReturnsFalse()
        {
            // Setup
            BatchFile file = new BatchFile("", Encoding.UTF8.EncodingName, new MemoryStream(), new Batch());
            file.WithFileName("F");

            // Assert
            Assert.False(file.ValidForFileSystem());
        }

        [Fact]
        public void ValidForFileSystem_FilenameIsNull_ReturnsFalse()
        {
            BatchFile file = new BatchFile("", Encoding.UTF8.EncodingName, new MemoryStream(), new Batch());
            file.WithPath("F");

            // Assert
            Assert.False(file.ValidForFileSystem());
        }
        
        [Fact]
        public void ValidForFileSystem_FilenameAndPathIsNull_ReturnsFalse()
        {
            BatchFile file = new BatchFile("", Encoding.UTF8.EncodingName, new MemoryStream(), new Batch());

            // Assert
            Assert.False(file.ValidForFileSystem());
        }
        
        [Fact]
        public void ValidForFileSystem_FilenameAndPathIsNotNull_ReturnsTrue()
        {
            BatchFile file = new BatchFile("", Encoding.UTF8.EncodingName, new MemoryStream(), new Batch());
            file.WithPath("Yay!").WithFileName("ItWorks");
            
            // Assert
            Assert.True(file.ValidForFileSystem());
        }
    }
}