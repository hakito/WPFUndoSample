using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace UndoSample
{
    class DataModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool Initialized;
        public DebugModel DebugModel;

        public string TextBox { get; set; }
        public int Slider { get; set; }
        public ObservableCollection<DataRecord> DataGrid {get;set;}

        public DataModel()
        {
            TextBox = "A text box";            
            DataGrid = new ObservableCollection<DataRecord>();
            DataGrid.CollectionChanged += DataGrid_CollectionChanged;            
            DataGrid.Add(new DataRecord());
            DataGrid.Add(new DataRecord());
            DataGrid.Add(new DataRecord());

            Initialized = true;
        }

        private void DataGrid_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (var item in e.NewItems.Cast<DataRecord>())
                    item.PropertyChangedExt += (s, p, b, a) => DebugModel?.AddPropertyUndo(s, p, b, a);
            DebugModel?.AppendLog($"DataGrid changed with action {e.Action}");
        }

        public void OnPropertyChanged(string propertyName, object before, object after)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            DebugModel?.AddPropertyUndo(this, propertyName, before, after);
        }
    }
}
