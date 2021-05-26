using Imagin.Common.Globalization.Engine;
using Imagin.Common.Globalization.Extensions;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Imagin.Common.Globalization.Converters
{
    /// <summary>
    /// Takes given value as resource key and translates it. If no text is found, the received value is returned.
    /// </summary>
    public class TranslateConverter : TypeValueConverterBase, IValueConverter, IMultiValueConverter
    {
        #region IMultiValueConverter

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length > 0)
                return this.Convert(values[0], targetType, parameter, culture);

            return null;
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        
        #endregion

        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                try
                {
                    culture = LocalizeDictionary.Instance.SpecificCulture;
                    var _key = value.ToString();

                    var result = LocExtension.GetLocalizedValue(targetType, _key, culture, null);

                    if (result == null)
                    {
                        var missingKeyEventResult = LocalizeDictionary.Instance.OnNewMissingKeyEvent(this, _key);

                        if (LocalizeDictionary.Instance.OutputMissingKeys
                            && !string.IsNullOrEmpty(_key) && (targetType == typeof(String) || targetType == typeof(object)))
                        {
                            if (missingKeyEventResult.MissingKeyResult != null)
                                result = missingKeyEventResult.MissingKeyResult;
                            else
                                result = "Key: " + _key;
                        }
                    }
                    return result;
                }
                catch
                { }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}