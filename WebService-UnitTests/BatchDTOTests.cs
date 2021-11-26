using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebService.Models;
using WebService.Models.DTOs;
using Xunit;
using System.Linq;

namespace WebService_UnitTests
{
    class MockFormFile : IFormFile
    {
        private Stream _stream;

        public MockFormFile(Stream stream)
        {
            _stream = stream;
        }
        public string ContentDisposition => throw new NotImplementedException();

        public string ContentType => throw new NotImplementedException();

        public string FileName => throw new NotImplementedException();

        public IHeaderDictionary Headers => throw new NotImplementedException();

        public long Length => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public void CopyTo(Stream target)
        {
            throw new NotImplementedException();
        }

        public System.Threading.Tasks.Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Stream OpenReadStream()
        {
            return _stream;
        }
    }

    public class BatchDTOTests
    {
        [Fact]
        public void MapToBatch_MapsExecutableInformationToSourceProperty()
        {
            // Create BatchDTO and set values relating to the executable
            BatchDTO dto = new BatchDTO();
            MemoryStream stream = new MemoryStream();
            dto.Executable = new MockFormFile(stream);
            dto.ExecutableEncoding = "testEncoding";
            dto.ExecutableFileExtension = ".test";
            dto.ExecutableLanguage = "testLanguage";

            Batch batch = dto.MapToBatch();
            SourceFile source = batch.SourceFile;

            Assert.Equal(batch.Id, source.BatchId);
            Assert.Equal(stream, source.Data);
            Assert.Equal("testEncoding", source.Encoding);
            Assert.Equal(".test", source.OriginalExtension);
            Assert.Equal("testLanguage", source.Language);
        }

        [Fact]
        public void MapToBatch_MapsInputToInputFilesProperty()
        {
            BatchDTO dto = new BatchDTO();
            dto.InputEncoding = "testEncoding";
            dto.InputFileExtension = ".test";
            MemoryStream stream1 = new MemoryStream(new byte[] { 0x1 });
            MemoryStream stream2 = new MemoryStream(new byte[] { 0x2 });
            dto.Input = new List<MockFormFile>()
            {
                new MockFormFile(stream1),
                new MockFormFile(stream2)
            };

            Batch batch = dto.MapToBatch();
            List<BatchFile> inputs = batch.InputFiles;

            Assert.True(inputs.TrueForAll(input => input.BatchId == batch.Id));
            Assert.True(inputs.TrueForAll(input => input.Encoding == "testEncoding"));
            Assert.True(inputs.TrueForAll(input => input.OriginalExtension == ".test"));
            Assert.Contains(inputs, input => input.Data == stream1);
            Assert.Contains(inputs, input => input.Data == stream2);
        }

        [Fact]
        public void MapToBatch_MapsReplicationFactor()
        {
            BatchDTO dto = new BatchDTO();
            dto.ReplicationFactor = 87;

            Batch batch = dto.MapToBatch();

            Assert.Equal(87, batch.ReplicationFactor);
        }
    }
}
