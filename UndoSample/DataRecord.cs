using System;
using System.IO;

namespace UndoSample
{
    class DataRecord
    {
        static int lastId = 0;

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date {get;set;}

        public DataRecord()
        {
            Id = ++lastId;
            Name = Path.GetTempFileName();
            Date = DateTime.Now;
        }
    }
}
