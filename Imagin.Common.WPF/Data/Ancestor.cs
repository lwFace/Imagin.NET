using System;
using System.Windows.Data;

namespace Imagin.Common.Data
{
    /// <summary>
    /// Design-time is not supported!
    /// </summary>
    public class Ancestor : Binding
    {
        public Ancestor(Type type) : this(".", type) { }

        public Ancestor(string path, Type type) : base(path)
        {
            Mode = BindingMode.OneWay;
            RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor);
            RelativeSource.AncestorType = type;
        }
    }
}