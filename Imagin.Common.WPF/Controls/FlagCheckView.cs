using Imagin.Common.Collections.Generic;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class FlagCheckView : UserControl
    {
        Handle handle = false;

        public static DependencyProperty FlagsProperty = DependencyProperty.Register(nameof(Flags), typeof(object), typeof(FlagCheckView), new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnFlagsChanged));
        public object Flags
        {
            get => GetValue(FlagsProperty);
            set => SetValue(FlagsProperty, value);
        }
        static void OnFlagsChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<FlagCheckView>().OnFlagsChanged(new OldNew(e));

        public static DependencyProperty ItemsProperty = DependencyProperty.Register(nameof(Items), typeof(Collection<CheckableObject<object>>), typeof(FlagCheckView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Collection<CheckableObject<object>> Items
        {
            get => (Collection<CheckableObject<object>>)GetValue(ItemsProperty);
            protected set => SetValue(ItemsProperty, value);
        }

        public static DependencyProperty ItemsPanelProperty = DependencyProperty.Register(nameof(ItemsPanel), typeof(ItemsPanelTemplate), typeof(FlagCheckView), new FrameworkPropertyMetadata(default(ItemsPanelTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ItemsPanelTemplate ItemsPanel
        {
            get => (ItemsPanelTemplate)GetValue(ItemsPanelProperty);
            set => SetValue(ItemsPanelProperty, value);
        }

        public static DependencyProperty ItemTemplateProperty = DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(FlagCheckView), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public FlagCheckView() : base()
        {
            DefaultStyleKey = typeof(FlagCheckView);
            Items = new Collection<CheckableObject<object>>();
            Items.Added += OnItemAdded;
        }

        protected virtual void OnFlagsChanged(OldNew input)
        {
            //If flags were changed externally
            if (!handle)
            {
                //If the old type != new type, clear references to old type
                if (input.Old?.GetType() != input.New?.GetType())
                {
                    Items.Clear();

                    //If new type is enum, get new fields 
                    if (input.New != null && input.New.GetType().IsEnum)
                    {
                        input.New.GetType().GetEnumValues().ForEach(i =>
                        {
                            if (i.To<Enum>().GetAttribute<HiddenAttribute>()?.Hidden != true)
                            {
                                var isChecked = Flags.To<Enum>().HasAll(i as Enum);
                                Items.Add(new CheckableObject<object>(i, isChecked));
                            }
                        });
                    }
                }
                //Else, if both old and new type is identical, set check states manually
                else Items.ForEach(i => i.IsChecked = Flags.To<Enum>().HasAll(i.Value as Enum));
            }
            //Internal changes are ignored here
        }

        protected virtual void OnItemAdded(object sender, EventArgs<CheckableObject<object>> e)
        {
            e.Value.Checked += OnItemChecked;
            e.Value.Unchecked += OnItemUnchecked;
        }

        protected virtual void OnItemChecked(object sender, EventArgs e)
        {
            var i = sender as CheckableObject<object>;
            handle.Invoke(() => SetCurrentValue(FlagsProperty, Flags.To<Enum>().Add(i.Value as Enum)));
        }

        protected virtual void OnItemUnchecked(object sender, EventArgs e)
        {
            var i = sender as CheckableObject<object>;
            handle.Invoke(() => SetCurrentValue(FlagsProperty, Flags.To<Enum>().Remove(i.Value as Enum)));
        }
    }
}
