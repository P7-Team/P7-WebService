using System.Collections.Generic;
using Microsoft.JSInterop.Infrastructure;

namespace WebService.Models
{
    public class BatchStatus
    {
        public bool Finished { get; set; }
        public int Total { get; set; }
        public int TasksDone { get; set; }
        public int Id { get; set; }
        public List<string> Files { get; set; }

        public BatchStatus(int id, bool finished, int tasksDone, int total)
        {
            Id = id;
            Finished = finished;
            TasksDone = tasksDone;
            Total = total;
            Files = new List<string>();
        }

        public void AddFile(string fileId)
        {
            Files.Add(fileId);
        }
    }
}