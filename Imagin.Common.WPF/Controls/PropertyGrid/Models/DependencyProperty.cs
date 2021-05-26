using Imagin.Common.Linq;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;

namespace Imagin.Common.Controls
{
    public class DependencyPropertyModel : PropertyModel
    {
        public override Type DeclaringType => Property.OwnerType;

        public override ModelTypes ModelType => ModelTypes.DependencyProperty;

        public readonly DependencyProperty Property;

        public readonly DependencyPropertyDescriptor PropertyDescriptor;

        public DependencyPropertyModel(DependencyPropertyData data) : base(data)
        {
            Property = data.Property;
            PropertyDescriptor = DependencyPropertyDescriptor.FromProperty(Property, Property.OwnerType);
        }

        protected override void SetValue(object input, object value) => (input as DependencyObject).SetCurrentValue(Property, value);

        protected override object GetValue(object input) => (input as DependencyObject).GetValue(Property);

        public override void Subscribe()
        {
            foreach (var i in Source)
                PropertyDescriptor.AddValueChanged(i, OnDependencyPropertyChanged);
        }

        public override void Unsubscribe()
        {
            foreach (var i in Source)
                PropertyDescriptor.RemoveValueChanged(i, OnDependencyPropertyChanged);
        }

        void OnDependencyPropertyChanged(object sender, EventArgs e) => Handle.If(i => !i, i => Update());
    }
}