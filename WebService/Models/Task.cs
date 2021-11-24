using System;
//TODO : TIME!
using WebService.Interfaces;

namespace WebService.Models
{
    public class Task : IAggregateRoot<(long, int, int)>
    {
        public long Id { get; set; } //TODO: Change to int and fix all errors that creates.

        public string Executable { get; set; }

        public string Input { get; set; } //Eventually list of file paths, currently only a single path for a single file associated with the Task.

        public int Number { get; set; }

        public int SubNumber { get; set; } //For byzentine checking

        public DateTime StartedOn {get; set;}

        public DateTime FinishedOn {get; set;}

        public string AllocatedTo { get; private set; }

        public void SetAllocatedTo(User user)
        {
            AllocatedTo = user.Username;
        }

        public bool IsReady { get; set; }

        public Task(int id, int number, int subnumber)
        {
            Id = id;
            Number = number;
            SubNumber = subnumber;
        }

        public Task(bool isReady)
        {
            IsReady = isReady;
        }


        // For deserialization
        public Task()
        {
            IsReady = false;
        }

        public override bool Equals(object obj)
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

        public (long, int, int) GetIdentifier()
        {
            return (Id, Number, SubNumber);
        }
    }
}