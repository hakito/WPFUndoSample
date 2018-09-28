using System.ComponentModel;

namespace UndoSample.UndoRedo
{
    public class PropertyChangedVerboseEventArgs : PropertyChangedEventArgs
    {
        public object Before { get; }
        public object After { get; }

        public PropertyChangedVerboseEventArgs(string propertyName, object before, object after)
            : base(propertyName)
        {
            Before = before;
            After = after;
        }
    }
}
