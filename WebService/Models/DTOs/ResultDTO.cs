using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models.DTOs
{
    public class ResultDTO
    {
        [FromForm, BindRequired]
        public int BatchId { get; set; }
        [FromForm, BindRequired]
        public int TaskNumber { get; set; }
        [FromForm, BindRequired]
        public int TaskSubNumber { get; set; }
        [FromForm, BindRequired]
        public IFormFile Result { get; set; }
        [FromForm]
        public string Encoding { get; set; }
        [FromForm]
        public string FileExtension { get; set; }

        public Result MapToResult()
        {
            return new Result(FileExtension, Encoding, Result?.OpenReadStream(), BatchId, false, new Task(BatchId, TaskNumber, TaskSubNumber));
        }
    }
}
