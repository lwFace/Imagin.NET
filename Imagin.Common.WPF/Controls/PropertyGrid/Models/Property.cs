using System;
using System.Reflection;

namespace Imagin.Common.Controls
{
    public class PropertyModel : MemberModel
    {
        public override bool CanWrite => Member.GetSetMethod(true) != null;

        new public PropertyInfo Member => (PropertyInfo)base.Member;

        public override ModelTypes ModelType => ModelTypes.Property;

        public override Type Type => Member.PropertyType;

        public PropertyModel(MemberData data) : base(data) { }

        protected override void SetValue(object source, object value) => Member.SetValue(source, value, null);

        protected override object GetValue(object input) => Member.GetValue(input);

        public override void Subscribe() { }

        public override void Unsubscribe() { }
    }
}