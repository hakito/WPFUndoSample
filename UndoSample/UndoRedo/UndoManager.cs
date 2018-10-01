using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace UndoSample.UndoRedo
{
    public static class UndoManager
    {
        public static bool IsBatchCollectingReverse { get; private set; }
        public static bool IsPerformingUndoOrRedo { get; private set; }
        public static event EventHandler<StackChangeEventArgs> StackChanged;

        private static Stack<IUndoable> InverseStack = new Stack<IUndoable>();
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
            if (IsBatchCollectingReverse)
            {
                InverseStack.Push(undoable);
                return false;
            }

            if (IsPerformingUndoOrRedo)
                return false;

            if (!undoable.CanUndo)
                return false;

            UndoStack.Push(undoable);
            RedoStack.Clear();
            NotifyOfStackChange();
            return true;
        }

        public static void PushFromEvent(object target, PropertyChangedVerboseEventArgs args)
        {
            Push(new PropertyUndoable(target, args));                       
        }

        internal static void PushFromEvent(object sender, NotifyCollectionChangedEventArgs e)
        {
            Push(new CollectionUndoable(sender, e));
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

        internal static void EndBatchCollectReverse()
        {
            if (!IsBatchCollectingReverse)
                throw new InvalidOperationException("Batch collection not started.");
            IsBatchCollectingReverse = false;
            
            while (InverseStack.Count > 0)
            {
                var undoable = InverseStack.Pop();
                Push(undoable);
            }
        }

        internal static void StartBatchCollectReverse()
        {
            if (IsBatchCollectingReverse)
                throw new InvalidOperationException("Batch collection already started.");
            IsBatchCollectingReverse = true;
        }
    }
}
