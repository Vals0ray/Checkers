using System;

namespace Checkers.Models
{
    public class DBModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Board { get; set; }

        public string WhoMove { get; set; }

        public DateTime Date { get; set; }
    }
}