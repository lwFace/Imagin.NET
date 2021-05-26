using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Math;
using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Desktop.Tiles
{
    [Serializable]
    public abstract class Tile : Base, IChange, ILockable, IPoint2D, ISize, ISelectable
    {
        [field: NonSerialized]
        public event ChangedEventHandler Changed;

        [field: NonSerialized]
        public event EventHandler<EventArgs> Locked;

        [field: NonSerialized]
        public event EventHandler<EventArgs> Unlocked;

        [field: NonSerialized]
        public event SelectedEventHandler Selected;

        bool isLocked = false;
        public bool IsLocked
        {
            get => isLocked;
            set => this.Change(ref isLocked, value);
        }

        [field: NonSerialized]
        bool isSelected = false;
        [XmlIgnore]
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                this.Change(ref isSelected, value);
                if (value)
                {
                    OnSelected();
                }
            }
        }

        Point2D position = new Point2D(0, 0);
        public Point2D Position
        {
            get => position;
            set => this.Change(ref position, value);
        }

        DoubleSize size = new DoubleSize(250d, 250d);
        public DoubleSize Size
        {
            get => size;
            set => this.Change(ref size, value);
        }

        string title = string.Empty;
        public virtual string Title
        {
            get => title;
            set => this.Change(ref title, value);
        }

        public Tile() : base() { }

        protected virtual void OnChanged() => Changed?.Invoke(this);

        protected virtual void OnSelected()
        {
            Selected?.Invoke(this, new SelectedEventArgs(this));
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Position):
                case nameof(Size):
                case nameof(Title):
                    OnChanged();
                    break;

                case nameof(IsLocked):
                    OnChanged();

                    if (IsLocked)
                        Locked?.Invoke(this, EventArgs.Empty);

                    if (!IsLocked)
                        Unlocked?.Invoke(this, EventArgs.Empty);

                    break;
            }
        }

        [field: NonSerialized]
        ICommand deleteTileCommand;
        [XmlIgnore]
        public ICommand DeleteTileCommand
        {
            get
            {
                deleteTileCommand = deleteTileCommand ?? new RelayCommand<object>(i => Get.Current<MainViewModel>().Screen.Remove(i as Tile), i => i is Tile);
                return deleteTileCommand;
            }
        }
    }
}