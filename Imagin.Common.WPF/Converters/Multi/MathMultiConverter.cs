using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(object[]), typeof(double))]
    public class MathMultiConverter : MultiConverter<double>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values?.Length == 3)
            {
                var valuea = (double)values[0];
                var operation = (NumberOperation)values[1];
                var valueb = (double)values[2];

                switch (operation)
                {
                    case NumberOperation.Add:
                        return valuea + valueb;
                    case NumberOperation.Divide:
                        return valuea / valueb;
                    case NumberOperation.Multiply:
                        return valuea * valueb;
                    case NumberOperation.Subtract:
                        return valuea - valueb;
                }
            }
            else if (values?.Length > 0)
            {
                double? result = null;
                NumberOperation? m = null;
                for (var i = 0; i < values.Length; i++)
                {
                    if (result == null)
                    {
                        if (values[i] is double)
                        {
                            result = (double)values[i];
                            continue;
                        }
                        return default(double);
                    }

                    if (values[i] is NumberOperation)
                    {
                        m = (NumberOperation)values[i];
                    }
                    else if (m != null)
                    {
                        switch (m)
                        {
                            case NumberOperation.Add:
                                result += (double)values[i];
                                break;
                            case NumberOperation.Divide:
                                result /= (double)values[i];
                                break;
                            case NumberOperation.Multiply:
                                result *= (double)values[i];
                                break;
                            case NumberOperation.Subtract:
                                result -= (double)values[i];
                                break;
                        }
                        m = null;
                    }

                    return result;
                }
            }
            return default(double);
        }
    }
}