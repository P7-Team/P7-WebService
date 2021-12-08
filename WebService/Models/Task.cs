using System;
//TODO : TIME!
using WebService.Interfaces;

namespace WebService.Models
{
    public class Task : IAggregateRoot<(int, int, int)>
    {
        public int Id { get; set; }

        public SourceFile Executable { get; set; }

        public BatchFile Input { get; set; } 

        public int Number { get; set; }

        public int SubNumber { get; set; } //For byzentine checking

        public DateTime? StartedOn {get; set;}

        public DateTime? FinishedOn {get; set;}

        public string AllocatedTo { get; private set; }


        public void SetAllocatedTo(User user)
        {
            AllocatedTo = user.Username;
        }

        public void UnAllocate()
        {
            AllocatedTo = null;
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

        public (int, int, int) GetIdentifier()
        {
            return (Id, Number, SubNumber);
        }
    }
}