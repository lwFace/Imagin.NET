using System;

namespace Imagin.Common.Input
{
    public class TextEnteredEventArgs : EventArgs
    {
        public readonly string Text;

        public TextEnteredEventArgs(string text) : base() => Text = text;
    }
}