using Imagin.Common.Linq;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Models
{
    public abstract class Panel : Content
    {
        bool canHide = true;
        public virtual bool CanHide
        {
            get => canHide;
            set => this.Change(ref canHide, value);
        }

        ImageSource icon = null;
        public ImageSource Icon
        {
            get => icon;
            set => this.Change(ref icon, value);
        }

        bool isSelected = false;
        public bool IsSelected
        {
            get => isSelected;
            set => this.Change(ref isSelected, value);
        }

        bool isVisible = true;
        public bool IsVisible
        {
            get => isVisible;
            set => this.Change(ref isVisible, value);
        }

        public string Name => GetType().Name;

        bool titleVisibility = true;
        public virtual bool TitleVisibility
        {
            get => titleVisibility;
            set => this.Change(ref titleVisibility, value);
        }

        public Panel(Uri icon = null) : base() => icon.If(i => i != null, i => Icon = new BitmapImage(i));
    }
}