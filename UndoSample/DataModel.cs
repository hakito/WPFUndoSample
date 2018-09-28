using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
            DataGrid.CollectionChanged += WatchItemPropertyChanges;            
            DataGrid.Add(new DataRecord());
            DataGrid.Add(new DataRecord());
            DataGrid.Add(new DataRecord());
            DataGrid.CollectionChanged += WatchCollectionChanges;
        }

        private void WatchCollectionChanges(object sender, NotifyCollectionChangedEventArgs e)
        {
            DebugModel?.AppendLog($"DataGrid changed with action {e.Action}");
            UndoManager.PushFromEvent(sender, e);
        }

        private void WatchItemPropertyChanges(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (var item in e.NewItems.Cast<DataRecord>())
                    item.PropertyChanged +=
                        (s, a) =>
                        {
                            var args = (PropertyChangedVerboseEventArgs)a;
                            DebugModel?.LogPropertyChange(s, args);
                            UndoManager.PushFromEvent(s, args);
                        };
            
        }

        public void OnPropertyChanged(string propertyName, object before, object after)
        {
            var args = new PropertyChangedVerboseEventArgs(propertyName, before, after);
            PropertyChanged?.Invoke(this, args);
            DebugModel?.LogPropertyChange(this, args);
            if (DebugModel != null)
                UndoManager.PushFromEvent(this, args);
        }
    }
}
