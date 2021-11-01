namespace WebService.Models
{
    public class Task
    {
        public long Id { get; set; }
        
        public string Executable { get; set; }
        
        public string Input { get; set; }
        
        public bool IsReady { get; set; }

        public Task(bool isReady)
        {
            IsReady = isReady;
        }

        // For deserialization
        public Task()
        {
            IsReady = false;
        }
    }
}