using System;
using System.Collections.Generic;

namespace UndoSample.UndoRedo
{
    public static class UndoManager
    {
        public static bool IsPerformingUndoOrRedo;
        public static event EventHandler<StackChangeEventArgs> StackChanged;

        private static Stack<IUndoable> UndoStack = new Stack<IUndoable>();
        private static Stack<IUndoable> RedoStack = new Stack<IUndoable>();

        public static void Undo()
        {
            PerformUndo(UndoStack, RedoStack, (u) => u.Undo());
        }

        public static void Redo()
        {
            PerformUndo(RedoStack, UndoStack, (u) => u.Redo());
        }

        public static bool Push(IUndoable undoable)
        {
            if (!undoable.CanUndo)
                return false;

            UndoStack.Push(undoable);
            RedoStack.Clear();
            NotifyOfStackChange();
            return true;
        }

        public static void AddPropertyUndo(object target, PropertyChangedVerboseEventArgs args)
        {
            if (IsPerformingUndoOrRedo)
                return;

            Push(new PropertyUndoable(target, args));
        }

        private static void PerformUndo(Stack<IUndoable> From, Stack<IUndoable> To, Action<IUndoable> action)
        {
            IsPerformingUndoOrRedo = true;
            try
            {
                var undoable = From.Pop();
                action(undoable);
                To.Push(undoable);
                NotifyOfStackChange();
            }
            finally
            {
                IsPerformingUndoOrRedo = false;
            }
        }

        private static void NotifyOfStackChange()
        {
            StackChanged?.Invoke(null, new StackChangeEventArgs(UndoStack.Count, RedoStack.Count));
        }
    }
}
