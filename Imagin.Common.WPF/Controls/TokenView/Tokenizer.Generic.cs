using System.Collections.Generic;
using System.Linq;

namespace Imagin.Common.Controls
{
    public abstract class Tokenizer<TToken> : ITokenizer, ITokenizer<TToken>
    {
        readonly object source;
        public object Source
        {
            get
            {
                return source;
            }
        }
        object ITokenizer.Source
        {
            get
            {
                return source;
            }
        }

        public abstract IEnumerable<TToken> Tokenize(string TokenString, char Delimiter);
        IEnumerable<object> ITokenizer.Tokenize(string TokenString, char Delimiter)
        {
            return Tokenize(TokenString, Delimiter).Cast<object>();
        }

        public abstract TToken ParseToken(string Text);
        object ITokenizer.ParseToken(string Text)
        {
            return ParseToken(Text);
        }

        public abstract string ToString(TToken Token);
        string ITokenizer.ToString(object Token)
        {
            return ToString((TToken)Token);
        }

        public Tokenizer(object Source = null)
        {
            source = Source;
        }
    }
}
