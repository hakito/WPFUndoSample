using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

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
            DataGrid.CollectionChanged += DataGrid_CollectionChanged;
            DataGrid.Add(new DataRecord());
            DataGrid.Add(new DataRecord());
            DataGrid.Add(new DataRecord());

            LoggingEnabled = true;
        }

        private void DataGrid_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (var item in e.NewItems.Cast<DataRecord>())
                    item.PropertyChangedExt += (p, b, a) => OnPropertyChanged($"{nameof(DataGrid)}.{p}", b, a);
            AppendLog($"DataGrid changed with action {e.Action}");
        }

        public void OnPropertyChanged(string propertyName, object before, object after)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName != nameof(Log))
                AppendLog($"{propertyName} set from '{before}' to '{after}'");

        }

        private void AppendLog(string text)
        {
            if (LoggingEnabled)
                Log += (string.IsNullOrEmpty(Log) ? "" : "\n") + text;
        }
    }
}
