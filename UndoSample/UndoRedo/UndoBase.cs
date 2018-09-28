using System;

namespace UndoSample.UndoRedo
{
    public abstract class UndoBase : IUndoable
    {
        public abstract bool CanRedo { get; }

        public abstract bool CanUndo { get; }

        public abstract void DoRedo();
        public abstract void DoUndo();

        public void Redo()
        {
            if (!CanUndo)
                throw new InvalidOperationException("This operation cannot be re-applied.");
            DoRedo();
        }

        public void Undo()
        {
            if (!CanUndo)
                throw new InvalidOperationException("This operation cannot be reverted.");
            DoUndo();
        }
    }
}
