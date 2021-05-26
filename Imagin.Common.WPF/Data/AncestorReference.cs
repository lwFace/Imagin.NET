using Imagin.Common.Converters;
using System;
using System.Windows.Data;

namespace Imagin.Common.Data
{
    /// <summary>
    /// Design-time is not supported!
    /// </summary>
    public class AncestorReference : Binding
    {
        public AncestorReference(Type type) : this(".", type) { }

        public AncestorReference(string path, Type type) : base(path)
        {
            Converter = new DefaultConverter<Object, Reference>(i => new Reference(i), null);
            Mode = BindingMode.OneWay;
            RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor) { AncestorType = type };
        }
    }
}