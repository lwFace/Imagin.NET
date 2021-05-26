using System.Text.RegularExpressions;

namespace Imagin.Common.Controls
{
    public class AlphaNumericBox : BaseRegexBox
    {
        protected override Regex regex => new Regex("^[a-zA-Z0-9]*$");

        public AlphaNumericBox() : base() { }
    }
}