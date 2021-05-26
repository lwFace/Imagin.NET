using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Imagin.Common
{
    [ContentProperty(nameof(Templates))]
    public class DefaultTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Default { get; set; } = new DataTemplate();

        public List<DataTemplate> Templates { get; set; } = new List<DataTemplate>();

        public override DataTemplate SelectTemplate(object item, DependencyObject container) => Templates.FirstOrDefault(i => (Type)i.DataType == item?.GetType()) ?? Default;
    }
}