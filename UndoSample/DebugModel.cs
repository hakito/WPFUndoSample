using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using UndoSample.UndoRedo;

namespace UndoSample
{
    class DebugModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Log { get; set; } = "Debug Log";
        public int UndoStackSize { get; set; }
        public int RedoStackSize { get; set; }

        public DebugModel()
        {
            UndoManager.UndoStack.CollectionChanged += (s, a) => UndoStackSize = UndoManager.UndoStack.Count;
            UndoManager.RedoStack.CollectionChanged += (s, a) => RedoStackSize = UndoManager.RedoStack.Count;
        }

        public void AddPropertyUndo(object target, PropertyChangedVerboseEventArgs a)
        {
            AppendLog($"{target.GetType()}.{a.PropertyName} set from '{a.Before}' to '{a.After}'");
            UndoManager.AddPropertyUndo(target, a);
        }

        public void AppendLog(string text)
        {
            Log += (string.IsNullOrEmpty(Log) ? "" : "\n") + text;
        }
    }
}
