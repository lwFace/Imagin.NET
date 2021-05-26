using Imagin.Common.Converters;
using Imagin.Common.Linq;
using System;
using System.Windows.Data;

namespace Imagin.Common.Data
{
    public sealed class EnumerateBinding : Binding
    {
        public EnumerateBinding(string path) : base(path)
        {
            Converter = new DefaultConverter<object, object>
            (
                input => input?.GetType().Enumerate(Appearance.Visible), 
                input => throw new NotSupportedException()
            );
            Mode = BindingMode.OneTime;
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        }
    }
}