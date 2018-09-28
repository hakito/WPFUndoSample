using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using UndoSample.UndoRedo;

namespace UndoSample
{
    class DataModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
        }

        private void DataGrid_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (var item in e.NewItems.Cast<DataRecord>())
                    item.PropertyChanged += (s, a) => DebugModel?.AddPropertyUndo(s, (PropertyChangedVerboseEventArgs) a);
            DebugModel?.AppendLog($"DataGrid changed with action {e.Action}");
        }

        public void OnPropertyChanged(string propertyName, object before, object after)
        {
            var args = new PropertyChangedVerboseEventArgs(propertyName, before, after);
            PropertyChanged?.Invoke(this, args);
            DebugModel?.AddPropertyUndo(this, args);
        }
    }
}
