using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace UndoSample.UndoRedo
{
    public static class UndoManager
    {
        public static bool IsPerformingUndoOrRedo;

        public static ObservableCollection<IUndoable> UndoStack = new ObservableCollection<IUndoable>();
        public static ObservableCollection<IUndoable> RedoStack = new ObservableCollection<IUndoable>();

        public static void Undo()
        {
            PerformUndo(UndoStack, RedoStack, (u) => u.Undo());
        }

        public static void Redo()
        {
            PerformUndo(RedoStack, UndoStack, (u) => u.Redo());
        }

        private static void PerformUndo(ICollection<IUndoable> From, ICollection<IUndoable> To, Action<IUndoable> action)
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

        public static void AddPropertyUndo(object target, PropertyChangedVerboseEventArgs args)
        {
            if (IsPerformingUndoOrRedo)
                return;

            UndoStack.Add(new PropertyUndoable(target, args));
            RedoStack.Clear();
        }
    }
}
