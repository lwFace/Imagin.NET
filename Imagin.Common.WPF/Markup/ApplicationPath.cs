using Imagin.Common.Configuration;
using System;
using System.Windows.Markup;

namespace Imagin.Common.Markup
{
    public class ApplicationPath : MarkupExtension
    {
        readonly string RelativePath = string.Empty;

        public ApplicationPath(string relativePath) : base()
        {
            RelativePath = relativePath;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => $@"{Get.Where<SingleApplication>().Data.FolderPath}\{RelativePath}";
    }
}