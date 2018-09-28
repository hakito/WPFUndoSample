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

        private bool IsPerformingUndoOrRedo;

        public string Log { get; set; } = "Debug Log";
        public int UndoStackSize { get; set; }
        public int RedoStackSize { get; set; }

        public ObservableCollection<IUndoable> UndoStack = new ObservableCollection<IUndoable>();
        public ObservableCollection<IUndoable> RedoStack = new ObservableCollection<IUndoable>();

        public DebugModel()
        {
            UndoStack.CollectionChanged += (s, a) => UndoStackSize = UndoStack.Count;
            RedoStack.CollectionChanged += (s, a) => RedoStackSize = RedoStack.Count;
        }

        public void AddPropertyUndo(object target, string propertyName, object before, object after)
        {
            AppendLog($"{target.GetType()}.{propertyName} set from '{before}' to '{after}'");
            if (IsPerformingUndoOrRedo)
                return;
            UndoStack.Add(new PropertyUndoable(target, propertyName, before));
            RedoStack.Clear();
        }

        public void AppendLog(string text)
        {
            Log += (string.IsNullOrEmpty(Log) ? "" : "\n") + text;
        }

        public void Undo()
        {
            PerformUndo(UndoStack, RedoStack, (u) => u.Undo());
        }

        public void Redo()
        {
            PerformUndo(RedoStack, UndoStack, (u) => u.Redo());
        }

        private void PerformUndo(ICollection<IUndoable> From, ICollection<IUndoable> To, Action<IUndoable> action)
        {
            IsPerformingUndoOrRedo = true;
            try
            {
                var undoable = From.Last();
                From.Remove(undoable);
                action(undoable);
                To.Add(undoable);
            }
            finally
            {
                IsPerformingUndoOrRedo = false;
            }
        }
    }
}
