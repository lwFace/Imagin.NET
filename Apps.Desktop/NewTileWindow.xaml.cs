using Desktop.Tiles;
using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace Desktop
{
    public partial class NewTileWindow : BaseWindow
    {
        public Type Type { get; private set; } = null;

        ObservableCollection<Type> types = new ObservableCollection<Type>();
        public ObservableCollection<Type> Types
        {
            get => types;
            set => this.Change(ref types, value);
        }

        public NewTileWindow()
        {
            InitializeComponent();

            var query = from type in Assembly.GetExecutingAssembly().GetTypes() where type.IsClass && !type.IsAbstract && type.Namespace == $"{nameof(Desktop)}.{nameof(Tiles)}" && typeof(Tile).IsAssignableFrom(type) select type;
            foreach (var i in query)
                Types.Add(i);
        }

        ICommand selectCommand;
        public ICommand SelectCommand
        {
            get
            {
                selectCommand = selectCommand ?? new RelayCommand<Type>(i =>
                {
                    Type = i;
                    Close();
                },
                i => i != null);
                return selectCommand;
            }
        }
    }
}