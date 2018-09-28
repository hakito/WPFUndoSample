namespace UndoSample.UndoRedo
{
    public class StackChangeEventArgs
    {
        public int UndoStackSize { get; }
        public int RedoStackSize { get; }

        public StackChangeEventArgs(int undoStackSize, int redoStackSize)
        {
            UndoStackSize = undoStackSize;
            RedoStackSize = redoStackSize;
        }
    }
}
