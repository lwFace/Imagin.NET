using Desktop.Tiles;
using Imagin.Common;
using Imagin.Common.Collections;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Linq;
using System;
using System.Xml.Serialization;

namespace Desktop
{
    [Serializable]
    public class Screen : Collection<Tile>, IChange, ILimit
    {
        public static Limit DefaultLimit = new Limit(25, Limit.Actions.RemoveFirst);

        public int Index => Get.Current<MainViewModel>().Screens.IndexOf(this) + 1;

        Limit limit = DefaultLimit;
        [XmlIgnore]
        public Limit Limit
        {
            get => limit;
            set
            {
                limit = value;
                limit.Coerce(this);
            }
        }

        public Screen() : base() { }

        void OnChanged(object sender) => OnChanged();

        void Subscribe(Tile item, bool i)
        {
            item.As<IChange>().Changed -= OnChanged;
            if (i)
            {
                item.As<IChange>().Changed += OnChanged;
            }
        }

        protected override void OnAdded(Tile item)
        {
            base.OnAdded(item);
            Subscribe(item, true);
            limit.Coerce(this);
        }

        protected override void OnInserted(Tile item, int index)
        {
            base.OnInserted(item, index);
            Subscribe(item, true);
            limit.Coerce(this);
        }

        protected override void OnRemoved(Tile item)
        {
            base.OnRemoved(item);
            Subscribe(item, false);
        }
    }
}