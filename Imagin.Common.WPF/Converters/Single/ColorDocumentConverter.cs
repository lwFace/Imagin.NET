using Imagin.Common.Controls;
using Imagin.Common.Models;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(ColorDocument), typeof(Document))]
    public class ColorDocumentConverter : Converter<ColorDocument, Document>
    {
        protected override Value<Document> ConvertTo(ConverterData<ColorDocument> input) => input.ActualValue as Document;

        protected override Value<ColorDocument> ConvertBack(ConverterData<Document> input) => input.ActualValue as ColorDocument;
    }
}