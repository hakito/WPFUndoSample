namespace UndoSample.UndoRedo
{
    public interface IUndoable
    {
        bool CanRedo { get; }
        bool CanUndo { get; }
        void Undo();
        void Redo();
    }
}
