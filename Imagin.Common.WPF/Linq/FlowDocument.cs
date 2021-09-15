using System.Collections.Generic;
using System.Windows.Documents;

namespace Imagin.Common.Linq
{
    public static class FlowDocumentExtensions
    {
        /// <summary>
        /// Gets all <see cref="Paragraph"/>s in the given <see cref="FlowDocument"/>.
        /// </summary>
        /// <param name="flowDocument"></param>
        /// <returns></returns>
        public static IEnumerable<Paragraph> Paragraphs(this FlowDocument flowDocument) => flowDocument.GetLogicalChildren<Paragraph>();
    }
}