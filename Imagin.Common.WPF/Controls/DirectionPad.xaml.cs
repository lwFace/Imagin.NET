using Imagin.Common.Input;
using Imagin.Common.Linq;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// A rectangular <see cref="System.Windows.Controls.UserControl"/> that enables specifying a <see cref="CardinalDirection"/> relative to any given <see cref="CardinalDirection"/>. 
    /// </summary>
    public partial class DirectionPad : System.Windows.Controls.UserControl
    {
        #region Classes

        internal class ButtonViewModel : NamedObject
        {
            readonly public int DefaultColumn;

            readonly public int DefaultRow;

            readonly CardinalDirection _direction = CardinalDirection.Unknown;
            public CardinalDirection Direction => _direction;

            int _column = 0;
            public int Column
            {
                get => _column;
                set => this.Change(ref _column, value);
            }

            string _icon = string.Empty;
            public string Icon
            {
                get => _icon;
                set => this.Change(ref _icon, value);
            }

            int _row = 0;
            public int Row
            {
                get => _row;
                set => this.Change(ref _row, value);
            }

            public ButtonViewModel(int column, int row, int direction) : base()
            {
                DefaultColumn = Column = column;
                DefaultRow = Row = row;
                _direction = (CardinalDirection)direction;
            }
        }

        #endregion

        #region Properties

        ObservableCollection<ButtonViewModel> _directions = new ObservableCollection<ButtonViewModel>()
        {
            new ButtonViewModel(1, 1, 0),
            new ButtonViewModel(2, 1, 1),
            new ButtonViewModel(3, 1, 2),
            new ButtonViewModel(1, 2, 3),
            new ButtonViewModel(2, 2, 4),
            new ButtonViewModel(3, 2, 5),
            new ButtonViewModel(1, 3, 6),
            new ButtonViewModel(2, 3, 7),
            new ButtonViewModel(3, 3, 8)
        };

        public static DependencyProperty DirectionProperty = DependencyProperty.Register(nameof(Direction), typeof(CardinalDirection), typeof(DirectionPad), new FrameworkPropertyMetadata(CardinalDirection.Origin, OnDirectionChanged));
        public CardinalDirection Direction
        {
            get => (CardinalDirection)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }
        static void OnDirectionChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as DirectionPad).OnDirectionChanged(new OldNew<CardinalDirection>(e));

        public static DependencyProperty ELabelProperty = DependencyProperty.Register(nameof(ELabel), typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
        public string ELabel
        {
            get => (string)GetValue(ELabelProperty);
            set => SetValue(ELabelProperty, value);
        }

        public static DependencyProperty EIconProperty = DependencyProperty.Register(nameof(EIcon), typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnIconChanged));
        public string EIcon
        {
            get => (string)GetValue(EIconProperty);
            set => SetValue(EIconProperty, value);
        }

        public static DependencyProperty NLabelProperty = DependencyProperty.Register(nameof(NLabel), typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
        public string NLabel
        {
            get => (string)GetValue(NLabelProperty);
            set => SetValue(NLabelProperty, value);
        }

        public static DependencyProperty NIconProperty = DependencyProperty.Register(nameof(NIcon), typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnIconChanged));
        public string NIcon
        {
            get => (string)GetValue(NIconProperty);
            set => SetValue(NIconProperty, value);
        }

        public static DependencyProperty NELabelProperty = DependencyProperty.Register(nameof(NELabel), typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
        public string NELabel
        {
            get => (string)GetValue(NELabelProperty);
            set => SetValue(NELabelProperty, value);
        }

        public static DependencyProperty NEIconProperty = DependencyProperty.Register(nameof(NEIcon), typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnIconChanged));
        public string NEIcon
        {
            get => (string)GetValue(NEIconProperty);
            set => SetValue(NEIconProperty, value);
        }

        public static DependencyProperty NWLabelProperty = DependencyProperty.Register(nameof(NWLabel), typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
        public string NWLabel
        {
            get => (string)GetValue(NWLabelProperty);
            set => SetValue(NWLabelProperty, value);
        }

        public static DependencyProperty NWIconProperty = DependencyProperty.Register(nameof(NWIcon), typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnIconChanged));
        public string NWIcon
        {
            get => (string)GetValue(NWIconProperty);
            set => SetValue(NWIconProperty, value);
        }

        public static DependencyProperty OriginLabelProperty = DependencyProperty.Register(nameof(OriginLabel), typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
        public string OriginLabel
        {
            get => (string)GetValue(OriginLabelProperty);
            set => SetValue(OriginLabelProperty, value);
        }

        public static DependencyProperty OriginIconProperty = DependencyProperty.Register(nameof(OriginIcon), typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnIconChanged));
        public string OriginIcon
        {
            get => (string)GetValue(OriginIconProperty);
            set => SetValue(OriginIconProperty, value);
        }

        public static DependencyProperty SLabelProperty = DependencyProperty.Register(nameof(SLabel), typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
        public string SLabel
        {
            get => (string)GetValue(SLabelProperty);
            set => SetValue(SLabelProperty, value);
        }

        public static DependencyProperty SIconProperty = DependencyProperty.Register(nameof(SIcon), typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnIconChanged));
        public string SIcon
        {
            get => (string)GetValue(SIconProperty);
            set => SetValue(SIconProperty, value);
        }

        public static DependencyProperty SELabelProperty = DependencyProperty.Register(nameof(SELabel), typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
        public string SELabel
        {
            get => (string)GetValue(SELabelProperty);
            set => SetValue(SELabelProperty, value);
        }

        public static DependencyProperty SEIconProperty = DependencyProperty.Register(nameof(SEIcon), typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnIconChanged));
        public string SEIcon
        {
            get => (string)GetValue(SEIconProperty);
            set => SetValue(SEIconProperty, value);
        }

        public static DependencyProperty SWLabelProperty = DependencyProperty.Register(nameof(SWLabel), typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
        public string SWLabel
        {
            get => (string)GetValue(SWLabelProperty);
            set => SetValue(SWLabelProperty, value);
        }

        public static DependencyProperty SWIconProperty = DependencyProperty.Register(nameof(SWIcon), typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnIconChanged));
        public string SWIcon
        {
            get => (string)GetValue(SWIconProperty);
            set => SetValue(SWIconProperty, value);
        }

        public static DependencyProperty WLabelProperty = DependencyProperty.Register(nameof(WLabel), typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
        public string WLabel
        {
            get => (string)GetValue(WLabelProperty);
            set => SetValue(WLabelProperty, value);
        }

        public static DependencyProperty WIconProperty = DependencyProperty.Register(nameof(WIcon), typeof(string), typeof(DirectionPad), new FrameworkPropertyMetadata(default(string), OnIconChanged));
        public string WIcon
        {
            get => (string)GetValue(WIconProperty);
            set => SetValue(WIconProperty, value);
        }

        static void OnLabelChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as DirectionPad).OnLabelChanged();

        static void OnIconChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as DirectionPad).OnIconChanged();

        #endregion

        #region DirectionPad

        /// <summary>
        /// Initializes an instance of <see cref="DirectionPad"/>.
        /// </summary>
        public DirectionPad()
        {
            InitializeComponent();
            PART_Items.ItemsSource = _directions;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Shift all relative to the specified <see cref="CardinalDirection"/>.
        /// </summary>
        /// <param name="direction"></param>
        void Shift(CardinalDirection direction)
        {
            var _direction = (int)direction;

            var shift = new int[,]
            {
                {-1, -1},
                {-1,  0},
                {-1,  1},
                { 0, -1},
                { 0,  0},
                { 0,  1},
                { 1, -1},
                { 1,  0},
                { 1,  1}
            };

            int x = shift[_direction, 1], y = shift[_direction, 0];

            if (x != 0 || y != 0)
            {
                foreach (var d in _directions)
                {
                    d.Column += x;
                    d.Row += y;
                }
            }
            else OnDirectionChanged(new OldNew<CardinalDirection>(CardinalDirection.Origin));
        }

        /// <summary>
        /// Set <see cref="Direction"/> relative to the specified <see cref="ButtonViewModel"/>.
        /// </summary>
        /// <param name="button"></param>
        void Set(ButtonViewModel button)
        {
            var origin = button.Direction == CardinalDirection.Origin ? button : _directions.First(x => x.Direction == CardinalDirection.Origin);
            var result = (CardinalDirection)new int[,]
            {
                { 0, 1, 2 },
                { 3, 4, 5 },
                { 6, 7, 8 }
            }
            [origin.Row - 1, origin.Column - 1];
            SetCurrentValue(DirectionProperty, result);
        }

        /// <summary>
        /// Occurs when <see cref="Direction"/> changes.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnDirectionChanged(OldNew<CardinalDirection> input)
        {
            var positions = new int[,]
            {
                {0, 0},
                {0, 1},
                {0, 2},
                {1, 0},
                {1, 1},
                {1, 2},
                {2, 0},
                {2, 1},
                {2, 2}
            };

            int srow = positions[(int)input.New, 0],
                scolumn = positions[(int)input.New, 1];

            int y = srow, x = scolumn;

            foreach (var d in _directions)
            {
                if (x < scolumn + 3)
                {
                    d.Row = y;
                    d.Column = x++;
                    if (x == (scolumn + 3))
                    {
                        x = scolumn;
                        y++;
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when <see cref="NWLabel"/>, <see cref="NLabel"/>, <see cref="NELabel"/>, <see cref="WLabel"/>, <see cref="OriginLabel"/>, <see cref="ELabel"/>, <see cref="SWLabel"/>, <see cref="SLabel"/>, or <see cref="SELabel"/> changes.
        /// </summary>
        protected virtual void OnLabelChanged()
        {
            foreach (var i in _directions)
            {
                switch (i.Direction)
                {
                    case CardinalDirection.E:
                        i.Name = ELabel;
                        break;
                    case CardinalDirection.N:
                        i.Name = NLabel;
                        break;
                    case CardinalDirection.NE:
                        i.Name = NELabel;
                        break;
                    case CardinalDirection.NW:
                        i.Name = NWLabel;
                        break;
                    case CardinalDirection.Origin:
                        i.Name = OriginLabel;
                        break;
                    case CardinalDirection.S:
                        i.Name = SLabel;
                        break;
                    case CardinalDirection.SE:
                        i.Name = SELabel;
                        break;
                    case CardinalDirection.SW:
                        i.Name = SWLabel;
                        break;
                    case CardinalDirection.W:
                        i.Name = WLabel;
                        break;
                }
            }
        }

        /// <summary>
        /// Occurs when <see cref="NWIcon"/>, <see cref="NIcon"/>, <see cref="NEIcon"/>, <see cref="WIcon"/>, <see cref="OriginIcon"/>, <see cref="EIcon"/>, <see cref="SWIcon"/>, <see cref="SIcon"/>, or <see cref="SEIcon"/> changes.
        /// </summary>
        protected virtual void OnIconChanged()
        {
            foreach (var i in _directions)
            {
                switch (i.Direction)
                {
                    case CardinalDirection.E:
                        i.Icon = EIcon;
                        break;
                    case CardinalDirection.N:
                        i.Icon = NIcon;
                        break;
                    case CardinalDirection.NE:
                        i.Icon = NEIcon;
                        break;
                    case CardinalDirection.NW:
                        i.Icon = NWIcon;
                        break;
                    case CardinalDirection.Origin:
                        i.Icon = OriginIcon;
                        break;
                    case CardinalDirection.S:
                        i.Icon = SIcon;
                        break;
                    case CardinalDirection.SE:
                        i.Icon = SEIcon;
                        break;
                    case CardinalDirection.SW:
                        i.Icon = SWIcon;
                        break;
                    case CardinalDirection.W:
                        i.Icon = WIcon;
                        break;
                }
            }
        }

        ICommand shiftCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand ShiftCommand
        {
            get
            {
                shiftCommand = shiftCommand ?? new RelayCommand<object>(p =>
                {
                    var button = (ButtonViewModel)p;
                    Shift(button.Direction);
                      Set(button);
                },
                p => p is ButtonViewModel);
                return shiftCommand;
            }
        }

        #endregion
    }
}