namespace WebService.Models
{
    public class CompletedTask
    {
        public long TaskId { get; protected set; }
        
        public string Result { get; protected set; }

        public CompletedTask(long taskId, string result)
        {
            TaskId = taskId;
            Result = result;
        }

        public CompletedTask()
        {
            // For deserialization
        }
    }
}