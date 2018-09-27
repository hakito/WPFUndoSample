using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace UndoSample
{
    class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool LoggingEnabled { get; }

        public int UndoStackSize { get; set; }
        public int RedoStackSize { get; set; }
        public string TextBox { get; set; }

        public ObservableCollection<DataRecord> DataGrid {get;set;}

        public string Log { get; set; }



        public ViewModel()
        {
            UndoStackSize = 0;
            TextBox = "A text box";            
            DataGrid = new ObservableCollection<DataRecord>();
            DataGrid.Add(new DataRecord());
            DataGrid.Add(new DataRecord());
            DataGrid.Add(new DataRecord());
            DataGrid.CollectionChanged += DataGrid_CollectionChanged;
            LoggingEnabled = true;
        }

        private void DataGrid_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            AppendLog($"DataGrid changed with action {e.Action}");
        }

        public void OnPropertyChanged(string propertyName, object before, object after)
        {            
            //Perform property validation
            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }

            if (LoggingEnabled && propertyName != nameof(Log))
                AppendLog($"{propertyName} set from '{before}' to '{after}'");

        }

        private void AppendLog(string text)
        {
            Log += (string.IsNullOrEmpty(Log) ? "" : "\n") + text;
        }
    }
}
