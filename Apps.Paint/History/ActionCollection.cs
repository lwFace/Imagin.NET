using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Paint
{
    [Serializable]
    public class ActionCollection : Base
    {
        Stack<BaseAction> r;
        public Stack<BaseAction> R
        {
            get => r;
            private set => this.Change(ref r, value);
        }

        Stack<BaseAction> u;
        public Stack<BaseAction> U
        {
            get => u;
            private set => this.Change(ref u, value);
        }

        public ActionCollection() : base()
        {
            r = new Stack<BaseAction>();
            u = new Stack<BaseAction>();
        }

        public void Add(BaseAction value)
        {
            r.Clear();
            u.Push(value);
        }

        public bool Any()
        {
            return r.Any() || u.Any();
        }

        public void Clear()
        {
            r.Clear();
            u.Clear();
        }

        public void Redo()
        {
            var action = r.Pop();
            u.Push(action);
            action.Execute();
        }

        public void Repeat()
        {
            if (u.Any())
            {
                var action = u.Peek();
                if (action.IsRepeatable)
                    Add(action.Clone());
            }
        }

        public void Undo()
        {
            var action = u.Pop();
            r.Push(action);
            action.Reverse();
        }
    }
}