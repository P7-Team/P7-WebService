using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Models
{
    public class SourceFile : BatchFile
    {
        public string Language { get; set; }

        public SourceFile(string originalExtension, string encoding, Stream data, Batch batch, string language) : base(originalExtension, encoding, data, batch)
        {
            Language = language;
        }
    }
}
