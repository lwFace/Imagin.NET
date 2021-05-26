using Imagin.Common.Globalization;
using System.Globalization;

namespace Imagin.Common.Linq
{
    public static class LanguagesExtensions
    {
        public static CultureInfo Convert(this Languages language)
        {
            string result = null;
            switch (language)
            {
                case Languages.English:
                    result = "en";
                    break;
                case Languages.Italian:
                    result = "it-IT";
                    break;
                case Languages.Spanish:
                    result = "es-ES";
                    break;
            }
            return new CultureInfo(result);
        }
    }
}