using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class PropertyTemplateSelector : DataTemplateSelector
    {
        public ResourceDictionary Resources { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is MemberModel member)
            {
                var actualType = member.ActualType ?? member.Type;
                foreach (var i in member.Collection.Control.OverrideTemplates)
                {
                    if (i.DataType.Equals(actualType))
                        return i;
                }

                var templateType = member.TemplateType;
                foreach (DictionaryEntry i in Resources)
                {
                    var k = i.Key.As<Type>();
                    if (k?.Equals(templateType) == true)
                        return i.Value as DataTemplate;
                }
            }
            return base.SelectTemplate(item, container);
        }
    }
}