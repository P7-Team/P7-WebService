using System;

namespace WebService.Models
{
    public class Task
    {
        public long Id { get; set; }

        public string Executable { get; set; }

        public string Input { get; set; }

        public int Number { get; set; }

        public int SubNumber { get; set; }

        public string AllocatedTo { get; private set; }

        public void SetAllocatedTo(User user)
        {
            AllocatedTo = user.Username;
        }

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

        public override bool Equals(object? obj)
        {
            return Equals(obj as Task);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Number, SubNumber);
        }

        private bool Equals(Task other)
        {
            return other != null &&
                   other.Id == Id &&
                   other.Number == Number &&
                   other.SubNumber == SubNumber;
        }
    }
}