using Desktop.Tiles;
using Imagin.Common;
using Imagin.Common.Collections;
using Imagin.Common.Collections.Serialization;
using Imagin.Common.Configuration;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using Imagin.Common.Models;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Desktop
{
    public class MainViewModel : MainViewModel<MainWindow>
    {
        Screen screen = null;
        public Screen Screen
        {
            get => screen;
            set
            {
                this.Change(ref screen, value);
                this.Changed(() => TaskbarItemDescription);
            }
        }

        public string TaskbarItemDescription => Screen != null && Screens != null ? $"{Screens.IndexOf(Screen) + 1} / {Screens.Count}" : string.Empty;

        XmlWriter<Screen> screens;
        public XmlWriter<Screen> Screens
        {
            get => screens;
            set => this.Change(ref screens, value);
        }

        public MainViewModel() : base()
        {
            Screens = new XmlWriter<Screen>(nameof(Screen), $@"{Get.Current<App>().Data.FolderPath}\{nameof(Screens)}.xml", new Limit(10, Limit.Actions.ClearAndArchive), typeof(ClockTile), typeof(CountDownTile), typeof(FolderTile), typeof(ImageTile), typeof(NoteTile), typeof(Tile));
            Screens.Removed += OnScreenRemoved;

            Screens.Load();
            Screen = Screens.ElementAtOrDefault(Get.Current<Options>().Screen) ?? Screens.FirstOrDefault();
        }

        void OnScreenRemoved(object sender, EventArgs<Screen> e)
        {
            if (Screen == e.Value)
                Screen = Screens.FirstOrDefault();
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Screen):
                    Get.Current<Options>().Screen = Screen.Index - 1;
                    break;
            }
        }

        public void OnSelected(DoubleRegion selection)
        {
            var point
                = new Point2D(selection.X.NearestFactor(Get.Current<Options>().TileSnap), selection.Y.NearestFactor(Get.Current<Options>().TileSnap));
            var size
                = new DoubleSize(selection.Width.NearestFactor(Get.Current<Options>().TileSnap), selection.Height.NearestFactor(Get.Current<Options>().TileSnap)); ;

            var newTileWindow = new NewTileWindow();
            newTileWindow.ShowDialog();

            if (newTileWindow.Type != null)
            {
                var tile = Activator.CreateInstance(newTileWindow.Type) as Tile;
                tile.Position
                    = point;
                tile.Size
                    = size;

                Screen.Add(tile);
            }

            View.PART_Grid.Visibility = Visibility.Collapsed;
        }

        ICommand cancelCommand;
        public ICommand CancelCommand
        {
            get
            {
                cancelCommand = cancelCommand ?? new RelayCommand(() => View.PART_Grid.Visibility = Visibility.Collapsed);
                return cancelCommand;
            }
        }

        ICommand drawCommand;
        public ICommand DrawCommand
        {
            get
            {
                drawCommand = drawCommand ?? new RelayCommand(() => View.PART_Grid.Visibility = Visibility.Visible, () => Screen != null);
                return drawCommand;
            }
        }

        ICommand addScreenCommand;
        public ICommand AddScreenCommand
        {
            get
            {
                addScreenCommand = addScreenCommand ?? new RelayCommand(() =>
                {
                    var result = new Screen();
                    Screens.Add(result);
                    Screen = result;
                });
                return addScreenCommand;
            }
        }

        ICommand leftScreenCommand;
        public ICommand LeftScreenCommand
        {
            get
            {
                leftScreenCommand = leftScreenCommand ?? new RelayCommand(() =>
                {
                    if (Screen != null && Screens.Any<Screen>() && Screens.IndexOf(Screen) > 0)
                    {
                        var index = Screens.IndexOf(Screen);
                        index--;

                        if (index < 0)
                            return;

                        Screen = Screens[index];
                    }
                }, () =>
                {
                    return true;
                    if (Screen != null)
                    {
                        var index = Screens?.IndexOf(Screen) ?? -1;
                        if (index > 0 && index <= Screens.Count - 1)
                            return true;
                    }
                    return false;
                });
                return leftScreenCommand;
            }
        }

        ICommand rightScreenCommand;
        public ICommand RightScreenCommand
        {
            get
            {
                rightScreenCommand = rightScreenCommand ?? new RelayCommand(() =>
                {
                    if (Screen != null && Screens.Any<Screen>() && Screens.IndexOf(Screen) < Screens.Count - 1)
                    {
                        var index = Screens.IndexOf(Screen);
                        index++;

                        if (index > Screens.Count - 1)
                            return;

                        Screen = Screens[index];
                    }
                }, () =>
                {
                    return true;
                    if (Screen != null)
                    {
                        var index = Screens?.IndexOf(Screen) ?? -1;
                        if (index >= 0 && index < Screens.Count - 1)
                            return true;
                    }
                    return false;
                });
                return rightScreenCommand;
            }
        }

        ICommand removeScreenCommand;
        public ICommand RemoveScreenCommand
        {
            get
            {
                removeScreenCommand = removeScreenCommand ?? new RelayCommand<Screen>(i =>
                {
                    var result = Imagin.Common.Controls.Dialog.Show("Remove screen", $"Are you sure you want to remove 'Screen {i.Index}'? This cannot be undone.", Imagin.Common.Controls.DialogImage.Warning, Imagin.Common.Controls.DialogButtons.YesNo);
                    if (result == 0)
                        Screens.Remove(i);
                },
                i => i is Screen);
                return removeScreenCommand;
            }
        }

        ICommand removeTileCommand;
        public ICommand RemoveTileCommand => removeTileCommand = removeTileCommand ?? new RelayCommand<Tile>(i => Screen.Remove(i), i => i is Tile);

        ICommand selectScreenCommand;
        public ICommand SelectScreenCommand
        {
            get
            {
                selectScreenCommand = selectScreenCommand ?? new RelayCommand<Screen>(i => Screen = i, i => i is Screen);
                return selectScreenCommand;
            }
        }
    }
}