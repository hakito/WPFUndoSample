using System.Reflection;

namespace UndoSample.UndoRedo
{
    class PropertyUndoable : IUndoable
    {
        object Target { get; }
        PropertyInfo PropertyInfo { get; }
        object OldValue { get; }
        object NewValue { get; }

        public bool CanRedo => true;

        public bool CanUndo => true;

        public void Redo()
        {
            PropertyInfo.SetValue(Target, NewValue);
        }

        public void Undo()
        {
            PropertyInfo.SetValue(Target, OldValue);
        }

        public PropertyUndoable(object target, string property, object oldValue)
        {
            Target = target;
            PropertyInfo = target.GetType().GetProperty(property);
            NewValue = PropertyInfo.GetValue(Target);
            OldValue = oldValue;
        }
    }
}
