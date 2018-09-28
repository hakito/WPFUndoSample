using System;
using System.ComponentModel;
using System.IO;

namespace UndoSample
{
    class DataRecord : INotifyPropertyChanged
    {
        static int lastId = 0;

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangedEventHandlerExt PropertyChangedExt;

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public DataRecord()
        {
            Id = ++lastId;
            Name = Path.GetFileName(Path.GetTempFileName());
            Date = DateTime.Now;
        }

        public void OnPropertyChanged(string propertyName, object before, object after)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            PropertyChangedExt?.Invoke(this, propertyName, before, after);
        }
    }
}
