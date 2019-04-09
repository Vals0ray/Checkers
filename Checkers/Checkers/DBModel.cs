using System;

namespace Checkers
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