using System.Collections.Generic;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// Specifies an object capable of tokenizing a <see cref="string"/>.
    /// </summary>
    public interface ITokenizer
    {
        object Source
        {
            get;
        }

        IEnumerable<object> Tokenize(string TokenString, char Delimiter);

        object ParseToken(string Text);

        string ToString(object Token);
    }
}