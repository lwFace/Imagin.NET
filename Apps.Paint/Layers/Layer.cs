using Imagin.Common;
using Imagin.Common.Data;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public abstract class Layer : NamedObject, System.ICloneable, ILockable
    {
        public const double IndexThicknessOffset = 10.0;

        [field: NonSerialized]
        public event EventHandler<EventArgs> Locked;

        [field: NonSerialized]
        public event EventHandler<EventArgs> Unlocked;

        [Hidden]
        public Document Document { get; set; }

        [Featured]
        public override string Name
        {
            get => base.Name;
            set => base.Name = value;
        }

        BlendModes blendMode = BlendModes.Normal;
        [Hidden]
        public BlendModes BlendMode
        {
            get => blendMode;
            set => this.Change(ref blendMode, value);
        }

        bool disabled = true;
        [Hidden]
        public bool Disabled
        {
            get => disabled;
            set => this.Change(ref disabled, value);
        }

        uint index = 0;
        [Hidden]
        public uint Index
        {
            get => index;
            set
            {
                this.Change(ref index, value);
                this.Changed(() => IndexThickness);
            }
        }

        [Hidden]
        public Thickness IndexThickness => new Thickness(index * IndexThicknessOffset, 0, 0, 0);

        protected bool IsParentVisible
        {
            get
            {
                var parent = Parent;
                while (parent != null)
                {
                    if (!parent.IsVisible)
                        return false;

                    parent = parent.Parent;
                }
                return true;
            }
        }

        protected bool IsParentLocked
        {
            get
            {
                var parent = Parent;
                while (parent != null)
                {
                    if (parent.IsLocked)
                        return true;

                    parent = parent.Parent;
                }
                return false;
            }
        }

        bool isLocked = false;
        [Hidden]
        public bool IsLocked
        {
            get => isLocked;
            set
            {
                if (Parent == null || !IsParentLocked)
                {
                    this.Change(ref isLocked, value);
                    if (this is GroupLayer)
                    {
                        ((GroupLayer)this).Each<Layer>(i =>
                        {
                            i.isLocked = value;
                            i.Changed(() => i.IsLocked);
                        });
                    }

                    if (value)
                        Locked?.Invoke(this, EventArgs.Empty);

                    if (!value)
                        Unlocked?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        bool isSelected = false;
        [Hidden]
        public bool IsSelected
        {
            get => isSelected;
            set => this.Change(ref isSelected, value);
        }

        bool isVisible = true;
        [Hidden]
        public virtual bool IsVisible
        {
            get => isVisible;
            set 
            {
                if (Parent == null || (IsParentVisible && !IsParentLocked))
                {
                    this.Change(ref isVisible, value);
                    if (this is GroupLayer)
                    {
                        ((GroupLayer)this).Each<Layer>(i =>
                        {
                            i.isVisible = value;
                            i.Changed(() => i.IsVisible);
                        });
                    }
                }
            }
        }

        double opacity = 1;
        [Range(0.0, 1.0, 0.01)]
        [RangeFormat(RangeFormat.Slider)]
        public double Opacity
        {
            get => opacity;
            set
            {
                if (!IsLocked && !IsParentLocked)
                    this.Change(ref opacity, value);
            }
        }

        void Each<T>(Action<T> action, LayerCollection layers = null) where T : Layer
        {
            if (layers == null)
            {
                if (this is GroupLayer)
                {
                    layers = ((GroupLayer)this).Layers;
                }
                else return;
            }

            foreach (var i in layers)
            {
                if (i is T)
                    action((T)i);
                
                if (i is GroupLayer)
                    Each(action, ((GroupLayer)i).Layers);
            }
        }

        uint GetIndex()
        {
            uint result = 0;

            var layer = this;
            while (layer.Parent != null)
            {
                result++;
                layer = layer.Parent;
            }

            return result;
        }

        GroupLayer parent = null;
        [Hidden]
        public GroupLayer Parent
        {
            get => parent;
            set
            {
                this.Change(ref parent, value);

                Index = GetIndex();
                Each<Layer>(i => i.Index = i.GetIndex());
            }
        }

        LayerStyle style;
        public LayerStyle Style
        {
            get => style;
            set => this.Change(ref style, value);
        }

        LayerType type;
        [Hidden]
        public virtual LayerType Type
        {
            get => type;
            private set => this.Change(ref type, value);
        }

        public Layer(LayerType type) : this(type, string.Empty) { }

        public Layer(LayerType type, string name) : base(name)
        {
            Style = new LayerStyle(this);
            Type = type;
        }

        object System.ICloneable.Clone()
        {
            return Clone();
        }
        public abstract Layer Clone();

        public System.Drawing.Color WithOpacity(Color color) => color.A((color.A.Double() * Opacity).Round().Byte()).Int32();

        public WriteableBitmap WithOpacity(WriteableBitmap pixels)
        {
            var result = pixels.Clone();
            result.ForEach((x, y, color) => WithOpacity(color).Double());
            return result;
        }

        public override string ToString() => nameof(Layer);
    }
}