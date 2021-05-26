using Imagin.Common.Linq;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    public class ConverterData<T>
    {
        public readonly int ActualParameter;

        public T ActualValue => Value is T i ? i : default;

        public readonly object Parameter;

        public readonly object Value;

        public ArgumentOutOfRangeException InvalidParameter => new ArgumentOutOfRangeException(nameof(ActualParameter));

        public ConverterData(object value, object parameter)
        {
            Value = value;
            Parameter = parameter;
            int.TryParse(Parameter?.ToString(), out ActualParameter);
        }
    }

    [ValueConversion(typeof(Object), typeof(Object))]
    public abstract class Converter<Input, Output> : IValueConverter
    {
        protected virtual bool AllowNull => false;

        protected abstract Value<Output> ConvertTo(ConverterData<Input> input);

        protected abstract Value<Input> ConvertBack(ConverterData<Output> input);

        protected virtual bool Is(object input) => input is Input;

        public virtual object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (AllowNull || Is(value))
            {
                var result = ConvertTo(new ConverterData<Input>(value, parameter));
                if (result.ActualValue.Not<Nothing>())
                    return result.ActualValue;
            }

            return Binding.DoNothing;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Output i)
            {
                var result = ConvertBack(new ConverterData<Output>(value, parameter));
                if (result.ActualValue.Not<Nothing>())
                    return result.ActualValue;
            }
            return Binding.DoNothing;
        }
    }

    [ValueConversion(typeof(Object), typeof(Object))]
    public class DefaultConverter<Input, Output> : Converter<Input, Output>
    {
        readonly Func<Input, Output> convertTo;

        readonly Func<Output, Input> convertBack;

        public DefaultConverter(Func<Input, Output> convertTo, Func<Output, Input> convertBack)
        {
            this.convertTo = convertTo;
            this.convertBack = convertBack;
        }

        protected override Value<Output> ConvertTo(ConverterData<Input> input) => convertTo(input.ActualValue);

        protected override Value<Input> ConvertBack(ConverterData<Output> input) => convertBack(input.ActualValue);
    }
}