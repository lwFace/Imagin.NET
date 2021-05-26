using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Math;
using System;
using System.Windows;

namespace Desktop
{
    public partial class MainWindow : Window
    {
        public MainWindow() : base()
        {
            Get.Current<MainViewModel>().View = this;
            InitializeComponent();
        }

        void OnExit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        void OnAbout(object sender, RoutedEventArgs e)
        {
            new AboutWindow().ShowDialog();
        }

        void OnOptions(object sender, RoutedEventArgs e)
        {
            new OptionsWindow().Show();
        }

        /*
        /// <summary>
        /// While one <see cref="System.Windows.Controls.ContextMenu"/> is open, opening another elsewhere changes it's corresponding <see cref="FolderTile"/>'s
        /// position to the first <see cref="FolderTile"/>'s position. This is unwanted behaviour related to <see cref="Imagin.Common.DraggableCanvas"/>! The following mitigates this.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnContextMenuOpened(object sender, RoutedEventArgs e)
        {
            PART_ItemsControl.IsEnabled = false;
        }

        /// <summary>
        /// While one <see cref="System.Windows.Controls.ContextMenu"/> is open, opening another elsewhere changes it's corresponding <see cref="FolderTile"/>'s
        /// position to the first <see cref="FolderTile"/>'s position. This is unwanted behaviour related to <see cref="Imagin.Common.DraggableCanvas"/>! The following mitigates this.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnContextMenuClosed(object sender, RoutedEventArgs e)
        {
            PART_ItemsControl.IsEnabled = true;
        }
        */

        void OnSelected(object sender, EventArgs<DoubleRegion> e)
        {
            Get.Current<MainViewModel>().OnSelected(e.Value);
            PART_SelectionCanvas.Selection.X = 0;
            PART_SelectionCanvas.Selection.Y = 0;
            PART_SelectionCanvas.Selection.Height = 0;
            PART_SelectionCanvas.Selection.Width = 0;
            PART_Grid.Visibility = Visibility.Collapsed;
        }

        protected override void OnDeactivated(EventArgs e)
        {
            base.OnDeactivated(e);
            if (Get.Current<MainViewModel>().Screen != null)
            {
                foreach (var i in Get.Current<MainViewModel>().Screen)
                {
                    if (i.IsSelected)
                        i.IsSelected = false;
                }
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            if (WindowState != WindowState.Maximized)
                WindowState = WindowState.Maximized;
        }
    }
}