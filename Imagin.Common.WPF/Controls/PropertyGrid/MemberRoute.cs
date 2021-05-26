using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Linq;

namespace Imagin.Common.Controls
{
    public class MemberRoute : ObjectCollection
    {
        readonly PropertyGrid Control;

        public MemberRoute(PropertyGrid control) : base()
        {
            Control = control;
        }

        public PropertyGrid.Element Append(object input)
        {
            if (input is PropertyGrid.Element element)
            {
                input = element.Value;
                Add(element);
                return element;
            }
            element = new PropertyGrid.Owner(string.Empty, input);
            Add(element);
            return element;
        }

        public void Next(MemberModel input)
        {
            var result = new PropertyGrid.Child(input.Name, input.Value);
            Control.SetCurrentValue(PropertyGrid.SourceProperty, result);
        }

        public void Previous()
        {
            RemoveAt(Count - 1);
            Control.SetCurrentValue(PropertyGrid.SourceProperty, this.Last<object>());
        }
    }
}