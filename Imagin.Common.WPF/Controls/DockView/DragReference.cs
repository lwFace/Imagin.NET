using Imagin.Common.Models;
using System.Windows;

namespace Imagin.Common.Controls
{
    internal sealed class DragReference
    {
        public readonly Content[] Content;

        public readonly Point Start;

        public DragReference(Content[] content, Point start)
        {
            Content = content;
            Start = start;
        }
    }
}