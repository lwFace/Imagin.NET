using Imagin.Common;
using Imagin.Common.Data;
using Imagin.Common.Linq;
using System;

namespace Vault
{
    [DisplayName("File extensions")]
    [Serializable]
    public class FileExtensions : Base
    {
        Filter filter = Filter.Include;
        public Filter Filter
        {
            get => filter;
            set => this.Change(ref filter, value);
        }

        string extensions = string.Empty;
        [StringFormat(StringFormat.Tokens)]
        public string Extensions
        {
            get => extensions;
            set => this.Change(ref extensions, value);
        }

        public FileExtensions() : base() { }

        public override string ToString() => extensions.NullOrEmpty() ? "All" : $"{filter} .{extensions.Replace(";", ", .").TrimEnd('.').TrimEnd(' ').TrimEnd(',')}";
    }
}