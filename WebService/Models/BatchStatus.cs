using Microsoft.JSInterop.Infrastructure;

namespace WebService.Models
{
    public class BatchStatus
    {
        private bool Finished { get; set; }
        private int Total { get; set; }
        private int Done { get; set; }
        private int Id { get; set; }

        public BatchStatus(int id, bool finished, int done, int total)
        {
            Id = id;
            Finished = finished;
            Done = done;
            Total = total;
        }
    }
}