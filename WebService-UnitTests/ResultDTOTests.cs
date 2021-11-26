using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WebService.Models;
using WebService.Models.DTOs;
using Xunit;

namespace WebService_UnitTests
{
    public class ResultDTOTests
    {
        [Fact]
        public void MapToResult_MapsTaskDetailsToTask()
        {
            ResultDTO dto = new ResultDTO() { BatchId = 1, TaskNumber = 2, TaskSubNumber = 3 };

            Result result = dto.MapToResult();
            Task task = result.Task;

            Assert.Equal(1, task.Id);
            Assert.Equal(2, task.Number);
            Assert.Equal(3, task.SubNumber);

            Assert.Equal(1, result.BatchId);
        }

        [Fact]
        public void MapToResult_MapsBatchId()
        {
            ResultDTO dto = new ResultDTO() { BatchId = 1, TaskNumber = 2, TaskSubNumber = 3 };

            Result result = dto.MapToResult();

            Assert.Equal(1, result.BatchId);
        }


        [Fact]
        public void MapToResult_MapsResultFileDetailsToBatchFile()
        {
            ResultDTO dto = new ResultDTO();
            MemoryStream stream = new MemoryStream(new byte[] { 0xB, 0xE, 0xE, 0xF });
            dto.Result = new MockFormFile(stream);
            dto.FileExtension = ".test";
            dto.Encoding = "testEncoding";

            Result result = dto.MapToResult();

            Assert.Equal(stream, result.Data);
            Assert.Equal(".test", result.OriginalExtension);
            Assert.Equal("testEncoding", result.Encoding);
        }
    }
}
