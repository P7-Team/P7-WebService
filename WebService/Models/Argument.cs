using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebService.Interfaces;

namespace WebService.Models
{
    public class Argument : IAggregateRoot<(string path, string filename, int number)>
    {
        public string Path { get; set; }
        public string Filename { get; set; }
        public int Number { get; set; }
        public string Value { get; set; }

        public Argument(string path, string filename, int number, string value)
        {
            Path = path;
            Filename = filename;
            Number = number;
            Value = value;
        }

        public (string path, string filename, int number) GetIdentifier()
        {
            return (Path, Filename, Number);
        }
    }
}
