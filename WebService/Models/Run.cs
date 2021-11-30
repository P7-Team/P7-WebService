using System;
using WebService.Interfaces;

namespace WebService.Models 
{
    public class Run : IAggregateRoot<(int, int, int, string, string)>
    {
        public int Id { get; set; }

        public int Number { get; set; }

        public int SubNumber { get; set; }

        public string Path { get; set; }

        public string FileName { get; set; }

        public Run(int id, int number, int subnumber, string path, string filename)
        {
            Id = id;
            Number = number;
            SubNumber = subnumber;
            Path = path;
            FileName = filename;
        }

        public (int, int, int, string, string) GetIdentifier()
        {
            return (Id, Number, SubNumber, Path, FileName);
        }
    }
}
