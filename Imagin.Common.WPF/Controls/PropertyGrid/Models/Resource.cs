using System;
using System.Windows;

namespace Imagin.Common.Controls
{
    public class ResourceModel : MemberModel
    {
        public override bool CanWrite => true;

        public override ModelTypes ModelType => ModelTypes.Resource;

        public override Type Type => Value?.GetType();

        public ResourceModel(MemberData data) : base(data) { }

        protected override object GetValue(object input) => (input as ResourceDictionary)[Name];

        protected override void SetValue(object input, object value) => (input as ResourceDictionary)[Name] = value;

        public override void Apply(MemberAttributes input) { }

        public override void Subscribe() { }

        public override void Unsubscribe() { }
    }
}