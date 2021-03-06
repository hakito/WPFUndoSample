﻿using System;
using System.Reflection;
using YaUnd.EventArgs;

namespace YaUnd.Undoables
{
    class PropertyUndoable : UndoBase
    {
        object Target { get; }
        PropertyInfo PropertyInfo { get; }
        public PropertyChangedVerboseEventArgs Args { get; }

        public override bool CanRedo => CanUndo;
        public override bool CanUndo { get; }

        public override void DoRedo()
        {
            PropertyInfo.SetValue(Target, Args.After);
        }

        public override void DoUndo()
        {
            PropertyInfo.SetValue(Target, Args.Before);
        }

        public PropertyUndoable(object target, PropertyChangedVerboseEventArgs args)
        {
            Target = target;

            try
            {
                PropertyInfo = target.GetType().GetTypeInfo().GetDeclaredProperty(args.PropertyName);
                if (PropertyInfo == null)
                    return;
                CanUndo = true;
            }
            catch(Exception)
            { }

            Args = args;
        }
    }
}
