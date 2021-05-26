using System.Collections.Generic;

namespace Imagin.Common.Controls
{
    public interface ITokenizer<TToken>
    {
        object Source
        {
            get;
        }

        IEnumerable<TToken> Tokenize(string TokenString, char Delimiter);

        TToken ParseToken(string Text);

        string ToString(TToken Token);
    }
}