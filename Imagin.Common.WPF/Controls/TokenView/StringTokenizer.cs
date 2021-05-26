using Imagin.Common.Linq;
using System;
using System.Collections.Generic;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// Tokenizes a <see cref="string"/> into multiple <see cref="string"/>s.
    /// </summary>
    public sealed class StringTokenizer : Tokenizer<string>
    {
        public override IEnumerable<string> Tokenize(string TokenString, char Delimiter)
        {
            var source = TokenString.Split(Array<char>.New(Delimiter), StringSplitOptions.RemoveEmptyEntries);
            foreach (var i in source)
                yield return i;
        }

        public override string ParseToken(string Text)
        {
            var Result = Text.Trim();
            return !Result.Empty() ? Result : null;
        }

        public override string ToString(string Token)
        {
            return Token;
        }

        public StringTokenizer() : base(null) { }
    }
}