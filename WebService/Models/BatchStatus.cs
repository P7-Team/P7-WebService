using System.Collections.Generic;
using Microsoft.JSInterop.Infrastructure;

namespace WebService.Models
{
    public class BatchStatus
    {
        private bool Finished { get; set; }
        private int Total { get; set; }
        private int TasksDone { get; set; }
        private int Id { get; set; }
        private List<string> Files { get; set; }

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