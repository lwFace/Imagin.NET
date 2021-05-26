using Imagin.Common;
using System.Collections.ObjectModel;

namespace Notes
{
    public class Lines : Imagin.Common.Collections.Generic.Collection<List.Line>
    {
        public List List { get; private set; }

        //For XML serialization
        public Lines() { }

        public Lines(List list)
        {
            List = list;
        }

        protected override void OnChanged()
        {
            base.OnChanged();
            List?.Changed(() => List.Count);
            List?.Changed(() => List.Title);
        }
    }
}