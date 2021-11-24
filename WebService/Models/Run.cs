using System;
using WebService.Interfaces;

namespace WebService.Models 
{
    public class Run : IAggregateRoot<(int, int, int)>
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public int SubNumber { get; set; }

        public string Path { get; set; }

        public string FileName { get; set; }

        public Run(int id, int number, int subnumber)
        {
            Id = id;
            Number = number;
            SubNumber = subnumber;
        }

        public (int, int, int) GetIdentifier()
        {
            return (Id, Number, SubNumber);
        }
    }
}
