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

        public SourceFile(FileStream data) : base("executable", data) { }
    }
}
