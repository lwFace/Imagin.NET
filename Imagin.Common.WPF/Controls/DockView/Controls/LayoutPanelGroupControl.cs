using Imagin.Common.Linq;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Imagin.Common.Controls
{
    public sealed class LayoutPanelGroupControl : LayoutContentGroupControl
    {
        TabPanel TabPanel;

        public LayoutPanelGroupControl(LayoutRootControl root) : base(root)
        {
            SetCurrentValue(TabStripPlacementProperty, Dock.Bottom);
        }

        void Update() => TabPanel.If(i => i != null, i => i.Visibility = (Source == null || Source.Count != 1).Visibility());

        protected override void OnItemsChanged()
        {
            base.OnItemsChanged();
            Update();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            TabPanel = this.GetVisualChildren<TabPanel>().FirstOrDefault();
            TabPanel.Loaded += OnTabPanelLoaded;
        }

        void OnTabPanelLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            TabPanel.Loaded -= OnTabPanelLoaded;
            Update();
        }

        public bool Contains(Models.Panel panel)
        {
            foreach (var i in Items)
            {
                if (ReferenceEquals(i, panel))
                    return true;
            }
            return false;
        }

        public void Write()
        {
            System.Console.WriteLine("LayoutPanelGroupControl.Write");
            foreach (var i in Source)
                System.Console.WriteLine($"the panel = {i.GetType().Name}");
        }
    }
}