using System.ComponentModel;
using YaUnd;
using YaUnd.EventArgs;

namespace UndoSample
{
    class DebugModel : INotifyPropertyChanged
    {
#pragma warning disable CS0067
        public event PropertyChangedEventHandler PropertyChanged;

        public string Log { get; set; } = "Debug Log";
        public int UndoStackSize { get; set; }
        public int RedoStackSize { get; set; }

        public DebugModel()
        {
            UndoManager.StackChanged += UndoManager_StackChanged;
        }

        private void UndoManager_StackChanged(object sender, StackChangeEventArgs e)
        {
            UndoStackSize = e.UndoStackSize;
            RedoStackSize = e.RedoStackSize;
        }

        public void LogPropertyChange(object target, PropertyChangedVerboseEventArgs a)
        {
            AppendLog($"{target.GetType()}.{a.PropertyName} set from '{a.Before}' to '{a.After}'");            
        }

        public void AppendLog(string text)
        {
            Log += (string.IsNullOrEmpty(Log) ? "" : "\n") + text;
        }
    }
}
