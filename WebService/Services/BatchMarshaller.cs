using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebService.Models;

namespace WebService.Services
{
    public class BatchMarshaller
    {
        /// <summary>
        /// Create a batch from multipart/formdata received by BatchController
        /// </summary>
        /// <param name="formdata">Dictionary containing formdata where key is the name of the formdata and the value is the value</param>
        /// <param name="files">List of tuples. Each tuple contains the name and content of a file sent as multipart data</param>
        /// <param name="owner">A User representing the owner of the batch</param>
        /// <returns>A batch containing owner, input and sourcefiles</returns>
        public static Batch MarshalBatch(Dictionary<string, string> formdata, List<(string name, Stream data)> files, User owner)
        {
            Batch batch = new Batch() { OwnerUsername = owner.Username };

            // Return batch with only username if there are no files
            if(files.Count < 1) 
                return batch;

            SourceFile source = new SourceFile(files.Where(file => file.name == "executable").First().data);
            source.Encoding = formdata["executableEncoding"];
            source.Language = formdata["executableLanguage"];
            batch.SourceFile = source;

            foreach ((string name, Stream data) inputfile in files.Where(file => file.name != "executable"))
            {
                BatchFile file = new BatchFile(inputfile.name, inputfile.data);
                file.Encoding = formdata[inputfile.name];
                batch.InputFiles.Add(file);
            }

            return batch;
        }

        internal static Batch MarshalBatch(Dictionary<string, string> formData, IEnumerable<(string Name, Stream)> enumerable, User user)
        {
            throw new NotImplementedException();
        }
    }
}
