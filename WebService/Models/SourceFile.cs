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

        /// <summary>
        /// Contructor used by the <see cref="SourceFileRepositoryy"/> to convert the DB representation to a SourceFile object
        /// </summary>
        /// <param name="path">The path to where the SourceFile is stored on the filesystem</param>
        /// <param name="filename">The name of the Source file stored on the filesystem</param>
        /// <param name="batchId">The ID of the batch that is associated with the SourceFile</param>
        /// <param name="language">The language the SourceFile is written in</param>
        /// <param name="encoding">The encoding used for the SourceFile</param>
        public SourceFile(string path, string filename, int batchId, string language, string encoding) : base(path,
            filename, encoding, batchId)
        {
            Language = language;
        }
    }
}
