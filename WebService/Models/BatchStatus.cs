using Microsoft.JSInterop.Infrastructure;

namespace WebService.Models
{
    public class BatchStatus
    {
        private bool Finished { get; set; }
        private int Total { get; set; }
        private int TasksDone { get; set; }
        private int Id { get; set; }

        public BatchStatus(int id, bool finished, int tasksDone, int total)
        {
            Id = id;
            Finished = finished;
            TasksDone = tasksDone;
            Total = total;
        }
    }
}