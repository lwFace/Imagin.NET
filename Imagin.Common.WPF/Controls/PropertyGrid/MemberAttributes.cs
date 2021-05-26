using Imagin.Common.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public sealed class MemberAttributes : Dictionary<Type, Attribute>
    {
        public static List<Attribute> Default => new List<Attribute>()
        {
            new AlternativeNameAttribute(string.Empty),
            new System.ComponentModel.BrowsableAttribute(true),
            new CategoryAttribute(null),
            new System.ComponentModel.CategoryAttribute(null),
            new DateTimeFormatAttribute(DateTimeFormat.Default),
            new DescriptionAttribute(null),
            new System.ComponentModel.DescriptionAttribute(null),
            new DisplayNameAttribute(null),
            new System.ComponentModel.DisplayNameAttribute(null),
            new EnumFormatAttribute(EnumFormat.Default),
            new FeaturedAttribute(false),
            new HiddenAttribute(false),
            new IndexAttribute(-1),
            new LockedAttribute(false),
            new LongFormatAttribute(null),
            new RangeAttribute(null, null, null),
            new RangeFormatAttribute(RangeFormat.UpDown),
            new ReadOnlyAttribute(false),
            new System.ComponentModel.ReadOnlyAttribute(false),
            new StringFormatAttribute(StringFormat.Default),
            new ValidateAttribute(typeof(LocalValidateHandler)),
            new UpdateSourceTriggerAttribute(UpdateSourceTrigger.PropertyChanged),
        };

        public bool Hidden => Get<HiddenAttribute>().Hidden || !Get<System.ComponentModel.BrowsableAttribute>().Browsable;

        public MemberAttributes(MemberInfo member) : base()
        {
            Default.ForEach(i => Add(i.GetType(), i));
            foreach (Attribute attribute in member.GetCustomAttributes(true))
            {
                var type = attribute.GetType();
                if (ContainsKey(type))
                    this[type] = attribute;
            }
        }

        public Attribute Get<Attribute>() where Attribute : System.Attribute => (Attribute)this[typeof(Attribute)];

        public void Set<Attribute>(Attribute input) where Attribute : System.Attribute => this[typeof(Attribute)] = input;
    }
}