using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models.DTOs
{
    /// <summary>
    /// This class is used when receiving data for creating a batch.
    /// It specifies the optional and required information, and binds this from form data to the corresponding property.
    /// If a property with the [BindRequired] attribute is not specified, the modelbinder will cause a validation error.
    /// </summary>
    public class BatchDTO
    {
        [FromForm, BindRequired]
        public IFormFile Executable { get; set; }
        [FromForm, BindRequired]
        public IEnumerable<IFormFile> Input { get; set; }
        [FromForm, BindRequired]
        public string ExecutableLanguage { get; set; }
        [FromForm]
        public string ExecutableEncoding { get; set; }
        [FromForm]
        public string ExecutableFileExtension { get; set; }
        [FromForm]
        public string InputEncoding { get; set; }
        [FromForm]
        public string InputFileExtension { get; set; }
        [FromForm]
        public int ReplicationFactor { get; set; }
        [FromForm]
        public IEnumerable<string> Arguments { get; set; }

        /// <summary>
        /// Creates a new instance of Batch, and defines source and inputfiles as well as replicaion factor.
        /// </summary>
        /// <returns></returns>
        public Batch MapToBatch()
        {
            Batch batch = new Batch() { ReplicationFactor = ReplicationFactor };
            if(Executable != null)
                batch.SourceFile = new SourceFile(ExecutableFileExtension, ExecutableEncoding, Executable.OpenReadStream(), batch, ExecutableLanguage);
            if(Input != null)
                batch.InputFiles = Input.Select(file => new BatchFile(InputFileExtension, InputEncoding, file.OpenReadStream(), batch)).ToList();

            return batch;
        }
    }
}
