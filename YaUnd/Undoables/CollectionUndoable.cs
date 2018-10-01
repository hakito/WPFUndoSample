using System.Collections;
using System.Collections.Specialized;

namespace YaUnd.Undoables
{
    internal class CollectionUndoable : UndoBase
    {
        private IList Target;
        private NotifyCollectionChangedEventArgs Args;

        public override bool CanRedo => CanUndo;

        public override bool CanUndo { get; }


        public CollectionUndoable(object target, NotifyCollectionChangedEventArgs args)
        {
            this.Target = target as IList;
            this.Args = args;
            CanUndo = Target != null && args != null && CheckArgs(args);
        }

        public override void DoRedo()
        {
            switch (Args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Target.Insert(Args.NewStartingIndex, Args.NewItems[0]);
                    break;
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Move:
                    throw new System.NotImplementedException();
                case NotifyCollectionChangedAction.Remove:
                    Target.RemoveAt(Args.OldStartingIndex);                    
                    break;
            }
        }

        public override void DoUndo()
        {
            switch(Args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    Target.RemoveAt(Args.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Move:
                    throw new System.NotImplementedException();
                case NotifyCollectionChangedAction.Remove:
                    Target.Insert(Args.OldStartingIndex, Args.OldItems[0]);
                    break;
            }
            
        }

        private bool CheckArgs(NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    return args.NewItems.Count == 1;
                case NotifyCollectionChangedAction.Remove:
                    return args.OldItems.Count == 1;
                default:
                    return false;
            }
        }
    }
}