using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Common.Controls
{
    public enum Types
    {
        Reference,
        Value
    }

    public class Source : IEnumerable
    {
        public class Ancestor
        {
            public readonly string Name;

            public readonly object Value;

            public Ancestor(string name, object value)
            {
                Name = name;
                Value = value;
            }
        }

        //...........................................................................................

        readonly Array Items;

        //...........................................................................................

        public readonly bool Indeterminate;

        //...........................................................................................

        public int Count => Items.Length;

        public object First => this[0];

        //...........................................................................................

        public Types DataType => Type.IsValueType ? Controls.Types.Value : Controls.Types.Reference;

        //...........................................................................................

        public readonly Ancestor Parent;

        //...........................................................................................

        public readonly Type Type;

        public IEnumerable<Type> Types => Items.Select(i => i.GetType());

        //...........................................................................................

        public object this[int index]
        {
            get
            {
                var i = 0;
                foreach (var j in Items)
                {
                    if (i == index)
                        return j;

                    i++;
                }
                return null;
            }
        }

        //...........................................................................................

        public Source(object input, Ancestor ancestor)
        {
            Parent = ancestor;
            Items = Split(input);

            Type = Count > 1 ? SharesWith(Types) : First.GetType();
            Type = Type.Equals(typeof(object)) ? null : Type; //No anonymous objects
        }

        //...........................................................................................

        Type SharesWith(IEnumerable<Type> types)
        {
            if (types == null)
                return null;

            Type a = null;
            foreach (var i in types)
            {
                if (a == null)
                {
                    a = i;
                    continue;
                }

                var b = i;

                //Compare a and b
                while (a != typeof(object))
                {
                    while (b != typeof(object))
                    {
                        if (a == b)
                            goto next;

                        b = b.BaseType;
                    }
                    a = a.BaseType;
                    b = i;
                }

                return null;
                next: continue;
            }
            return a;
        }

        //...........................................................................................

        public static Array Split(object input) => input.GetType().IsArray ? (Array)input : Array<object>.New(input);

        //...........................................................................................

        public IEnumerator GetEnumerator() => Items.GetEnumerator();
    }
}