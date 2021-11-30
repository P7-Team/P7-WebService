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
            // Create Batch and assign owner
            Batch batch = new Batch() { OwnerUsername = owner.Username };

            // Get replication factor
            if (formdata.ContainsKey("replicationfactor"))
                batch.ReplicationFactor = int.Parse(formdata["replicationfactor"]);

            MarshallFiles(formdata, files, batch);

            return batch;
        }

        private static void MarshallFiles(Dictionary<string, string> formdata, List<(string name, Stream data)> files, Batch batch)
        {
            // Simply return if there are no files
            if (files.Count < 1)
                return;

            MarshallSourceFile(formdata, files, batch);
            MarshallInputFiles(formdata, files, batch);
        }

        private static void MarshallSourceFile(Dictionary<string, string> formdata, List<(string name, Stream data)> files, Batch batch)
        {
            // Get source file details
            SourceFile source = new SourceFile(formdata["executableExtension"],
                formdata["executableEncoding"],
                files.Where(file => file.name == "executable").First().data,
                batch,
                formdata["executableLanguage"]);
            batch.SourceFile = source;
        }

        private static void MarshallInputFiles(Dictionary<string, string> formdata, List<(string name, Stream data)> files, Batch batch)
        {
            // Get input file details
            foreach ((string name, Stream data) inputfile in files.Where(file => file.name != "executable"))
            {
                BatchFile file = new BatchFile(formdata["extension"], formdata[inputfile.name], inputfile.data, batch);
                batch.InputFiles.Add(file);
            }
        }
    }
}
