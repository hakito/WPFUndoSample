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

        public void AddPropertyUndo(object target, string propertyName, object before, object after)
        {
            AppendLog($"{target.GetType()}.{propertyName} set from '{before}' to '{after}'");
            UndoManager.AddPropertyUndo(target, propertyName, before, after);
        }

        public void AppendLog(string text)
        {
            Log += (string.IsNullOrEmpty(Log) ? "" : "\n") + text;
        }
    }
}
