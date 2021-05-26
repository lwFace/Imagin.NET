using Imagin.Common;
using Imagin.Common.Input;
using Paint.Adjust;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Paint
{
    public class Wrapper : BaseObject
    {
        public Wrapper() { }
    }

    public class EffectWrapper : Wrapper
    {
        AdjustmentEffect adjustment;
        public AdjustmentEffect Adjustment
        {
            get => adjustment;
            set => this.Change(ref adjustment, value);
        }

        WrapperCollection child = new WrapperCollection();
        public WrapperCollection Child
        {
            get => child;
            set => this.Change(ref child, value);
        }

        public EffectWrapper(AdjustmentEffect adjustment)
        {
            Adjustment = adjustment;
        }
    }

    public class TemplateWrapper : Wrapper
    {
        object source;
        public object Source
        {
            get => source;
            set => this.Change(ref source, value);
        }

        DataTemplate template;
        public DataTemplate Template
        {
            get => template;
            set => this.Change(ref template, value);
        }

        public TemplateWrapper(object source, DataTemplate template) : base() 
        {
            Source = source;
            Template = template;
        }
    }

    public class WrapperCollection : ObservableCollection<Wrapper> { }

    public partial class EffectView : UserControl
    {
        TemplateWrapper origin = null;

        public static DependencyProperty AdjustmentsProperty = DependencyProperty.Register("Adjustments", typeof(EffectCollection), typeof(EffectView), new FrameworkPropertyMetadata(default(EffectCollection), FrameworkPropertyMetadataOptions.None, OnAdjustmentsChanged));
        public EffectCollection Adjustments
        {
            get => (EffectCollection)GetValue(AdjustmentsProperty);
            set => SetValue(AdjustmentsProperty, value);
        }
        protected static void OnAdjustmentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as EffectView).OnAdjustmentsChanged((EffectCollection)e.OldValue, (EffectCollection)e.NewValue);
        }

        public static DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(WriteableBitmap), typeof(EffectView), new FrameworkPropertyMetadata(default(WriteableBitmap), FrameworkPropertyMetadataOptions.None, OnSourceChanged));
        public WriteableBitmap Source
        {
            get => (WriteableBitmap)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        protected static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as EffectView).OnSourceChanged((WriteableBitmap)e.NewValue);
        }

        public static DependencyProperty SourceTemplateProperty = DependencyProperty.Register("SourceTemplate", typeof(DataTemplate), typeof(EffectView), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.None, OnSourceTemplateChanged));
        public DataTemplate SourceTemplate
        {
            get => (DataTemplate)GetValue(SourceTemplateProperty);
            set => SetValue(SourceTemplateProperty, value);
        }
        protected static void OnSourceTemplateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as EffectView).OnSourceTemplateChanged((DataTemplate)e.NewValue);
        }

        public EffectView() => InitializeComponent();

        void Update()
        {
            var result = new WrapperCollection();

            Wrapper a = new TemplateWrapper(Source, SourceTemplate), b = null;
            origin = (TemplateWrapper)a;

            if (Adjustments.Count > 0)
            {
                a = new EffectWrapper(Adjustments[0]);
                if (Adjustments.Count > 1)
                {
                    var j = 0;
                    foreach (var i in Adjustments)
                    {
                        if (j == 0)
                        {
                            j++;
                            continue;
                        }

                        b = new EffectWrapper(i);
                        (b as EffectWrapper).Child.Add(a);
                        a = b;
                    }
                }
            }

            result.Add(a);
            TreeView.ItemsSource = result;
        }

        protected virtual void OnAdjustmentsChanged(EffectCollection old, EffectCollection @new)
        {
            if (old != null)
            {
                old.Added -= OnAdjustmentAdded;
                old.Removed -= OnAdjustmentRemoved;
                old.Cleared -= OnAdjustmentsCleared;
            }

            if (@new != null)
            {
                @new.Added += OnAdjustmentAdded;
                @new.Removed += OnAdjustmentRemoved;
                @new.Cleared += OnAdjustmentsCleared;
            }
            Update();
        }

        void OnAdjustmentsCleared(object sender, EventArgs e)
        {
            Update();
        }

        void OnAdjustmentRemoved(object sender, EventArgs<AdjustmentEffect> e)
        {
            Update();
        }
        
        void OnAdjustmentAdded(object sender, EventArgs<AdjustmentEffect> e)
        {
            Update();
        }

        void OnSourceChanged(WriteableBitmap input)
        {
            if (origin != null)
                origin.Source = input;
        }

        void OnSourceTemplateChanged(DataTemplate input)
        {
            if (origin != null)
                origin.Template = input;
        }
    }
}