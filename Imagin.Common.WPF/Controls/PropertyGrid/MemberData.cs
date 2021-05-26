using System.Reflection;
using System.Windows;

namespace Imagin.Common.Controls
{
    public class MemberData
    {
        public readonly MemberCollection Collection;

        public readonly MemberInfo Member;

        public readonly Source Source;

        protected MemberData(MemberData data) : this(data.Collection, data.Source, data.Member) { }

        public MemberData(MemberCollection collection, Source source, MemberInfo member)
        {
            Collection = collection;
            Source = source;
            Member = member;
        }
    }

    public class DependencyPropertyData : MemberData
    {
        public readonly DependencyProperty Property;

        public DependencyPropertyData(MemberData data, DependencyProperty property) : base(data)
        {
            Property = property;
        }
    }
}