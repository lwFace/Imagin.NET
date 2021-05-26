using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Imagin.Common.Controls
{
    public partial class SliderMenuItem : MenuItem
    {
        const double ThumbHeight = 11.0d;

        Slider PART_Slider;

        SortedDictionary<double, double> tickValueMap;

        public static int GetSteps(DependencyObject i)
        {
            return (int)i.GetValue(StepsProperty);
        }
        public static void SetSteps(DependencyObject i, int value)
        {
            i.SetValue(StepsProperty, value);
        }
        public static readonly DependencyProperty StepsProperty = DependencyProperty.RegisterAttached("Steps", typeof(int), typeof(SliderMenuItem), new UIPropertyMetadata(1));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.RegisterAttached(nameof(Value), typeof(double), typeof(SliderMenuItem), new UIPropertyMetadata(1.0d, OnValueChanged));
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, System.Math.Round(value)); }
        }
        public static double GetValue(DependencyObject i)
        {
            return (double)i.GetValue(ValueProperty);
        }
        public static void SetValue(DependencyObject i, double value)
        {
            i.SetValue(ValueProperty, value);
        }
        static void OnValueChanged(DependencyObject i, DependencyPropertyChangedEventArgs e)
        {
            if (i is SliderMenuItem item)
                SetTickToValue(item, (double)e.NewValue);
        }

        /// <summary>
        /// Skip property for sub menu items.  Allows a menu item to 
        /// skip a tick mark.  Especially useful for separators.
        /// </summary>
        public static readonly DependencyProperty SkipProperty = DependencyProperty.RegisterAttached("Skip", typeof(bool), typeof(SliderMenuItem), new UIPropertyMetadata(false));
        public static bool GetSkip(DependencyObject i)
        {
            return (bool)i.GetValue(SkipProperty);
        }
        public static void SetSkip(DependencyObject i, bool value)
        {
            i.SetValue(SkipProperty, value);
        }

        public SliderMenuItem()
        {
            tickValueMap = new SortedDictionary<double, double>();
            InitializeComponent();
        }

        /// <summary>
        /// Sets the slider thumb to the closest match after the value changes.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="newValue"></param>
        static void SetTickToValue(SliderMenuItem item, double newValue)
        {
            // find tick spot where   
            double[] ticks = new double[item.tickValueMap.Keys.Count];
            item.tickValueMap.Keys.CopyTo(ticks, 0);

            // Find exact match
            if (item.tickValueMap.ContainsValue(newValue))
            {
                foreach (double tick in item.tickValueMap.Keys)
                {
                    if (item.tickValueMap[tick] == newValue)
                    {
                        item.PART_Slider.Value = tick;
                        return;
                    }
                }
            }

            // Find closest match
            for (int i = 1; i < item.tickValueMap.Count; i++)
            {

                double lowTick = ticks[i - 1];
                double highTick = ticks[i];

                double lowValue = item.tickValueMap[lowTick];
                double highValue = item.tickValueMap[highTick];

                //double newValue = (double)e.NewValue;

                if (newValue > lowValue &&
                    newValue < highValue)
                {
                    double valueScale = highValue - lowValue;
                    double tickScale = highTick - lowTick;

                    // set slider to closest tick match
                    double newTick = (newValue - lowValue) / (valueScale) * tickScale + lowTick;

                    item.PART_Slider.Value = newTick;
                }
            }
        }

        /// <summary>
        /// After the slider value has changed, update the Value property to 
        /// hold the scaled value.
        /// </summary>
        static void SetValueToTick(SliderMenuItem item, double tickValue)
        {
            if (item.tickValueMap.ContainsKey(tickValue))
            {
                item.Value = item.tickValueMap[tickValue];
                return;
            }

            double[] keys = new double[item.tickValueMap.Keys.Count];
            item.tickValueMap.Keys.CopyTo(keys, 0);

            int index = Array.BinarySearch<double>(keys, tickValue);

            Debug.Assert(index < 0, "What? How come I didn't find the key already?");

            index = ~index;

            Debug.Assert(index < item.Items.Count, "How did tick value go above 1000?");
            Debug.Assert(index != 0, "Insert location was before element 0.");

            double lowTick = keys[index - 1];
            double highTick = keys[index];

            double lowValue = item.tickValueMap[lowTick];
            double highValue = item.tickValueMap[highTick];

            double valueScale = highValue - lowValue;
            double sourceScale = highTick - lowTick;

            double newValue = (tickValue - lowTick) * valueScale / sourceScale + lowValue;
            item.Value = newValue;
        }

        /// <summary>
        /// Listen for change to slider value.  Enables binding where SliderMenuItem is the target, and a framework element is the source.
        /// </summary>
        void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SetValueToTick(this, (double)e.NewValue); //Find appropriate tick spot, and set slider value
        }

        /// <summary>
        /// A child menu item was clicked.  Set the value automatically.
        /// </summary>
        void SliderMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Value = (double)((DependencyObject)sender).GetValue(SliderMenuItem.ValueProperty);
        }

        /// <summary>
        /// Call base method to figure out menu item placement,
        /// then place tick marks at the centers of the menu items.
        /// </summary>
        /// <param name="arrangeBounds"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            Size returnSize = base.ArrangeOverride(arrangeBounds);

            FrameworkElement topElement = null;
            FrameworkElement bottomElement = null;

            for (int i = 0; i < Items.Count; i++)
            {
                FrameworkElement elem = Items[i] as FrameworkElement;

                Debug.Assert(elem != null, "Added an object that wasn't a FrameworkElement??");

                // Find the bottom element.  It must have a value greater or equal to the
                // top element
                if (topElement != null)
                {
                    if ((double)elem.GetValue(SliderMenuItem.ValueProperty) >=
                        (double)topElement.GetValue(SliderMenuItem.ValueProperty))
                    {
                        bottomElement = elem;
                    }
                }

                // Move along.  Nothing to see here.
                if ((bool)(elem.GetValue(SliderMenuItem.SkipProperty)))
                    continue;

                // set the first element
                if (topElement == null)
                    topElement = elem;
            }

            // Single element.  Not much of a slider, but don't crash
            if (bottomElement == null && topElement != null)
                bottomElement = topElement;

            // No elements.  Nothing to do.
            if (bottomElement == null && topElement == null)
                return returnSize;

            // Calculate top, bottom margins.
            // This margin enables the thumb stop at 0 and 100 to line up with
            // the center of the top and bottom menu items.
            Rect bound = LayoutInformation.GetLayoutSlot(topElement);

            double topMargin = bound.Top + ThumbHeight / 2;
            double pointZero = bound.Top + bound.Height / 2.0;

            bound = LayoutInformation.GetLayoutSlot(bottomElement);

            double bottomMargin = returnSize.Height - bound.Bottom + ThumbHeight / 2.0d;
            double pointOneHundred = bound.Top + bound.Height / 2.0d;

            // Set the margin.
            PART_Slider.Margin = new Thickness(0, topMargin, 0, bottomMargin);


            for (int i = 0; i < Items.Count; i++)
            {
                FrameworkElement elem = Items[i] as FrameworkElement;

                if (elem is MenuItem)
                    ((MenuItem)elem).Click += new RoutedEventHandler(SliderMenuItem_Click);

                // Move along.  Nothing to see here.
                if ((bool)(elem.GetValue(SliderMenuItem.SkipProperty)))
                    continue;

                // Grab the coordinates of the child menu item
                bound = LayoutInformation.GetLayoutSlot(elem);

                // Get the number of steps, or tick spots between this child menu item
                //  and the previous one.
                int steps = (int)elem.GetValue(SliderMenuItem.StepsProperty);

                // A value of 0 for Steps is like setting Skip = true
                if (steps < 1)
                    continue;

                // Calculate tick spot.
                double thisTickSpot = 1000.0d * (bound.Top - pointZero + bound.Height / 2.0d)
                    / (pointOneHundred - pointZero);

                // Calculate continuous tick spots.  Only allow continuous after the first element.
                if (PART_Slider.Ticks.Count > 0)
                {
                    double lastTickSpot = PART_Slider.Ticks[PART_Slider.Ticks.Count - 1];
                    double division = (thisTickSpot - lastTickSpot) / steps;

                    for (int current_step = 1; current_step < steps; current_step++)
                    {
                        double intermediateTickSpot = lastTickSpot + current_step * division;
                        PART_Slider.Ticks.Add(intermediateTickSpot);
                    }
                }

                PART_Slider.Ticks.Add(thisTickSpot);
                double sliderValue = (double)elem.GetValue(SliderMenuItem.ValueProperty);
                tickValueMap[thisTickSpot] = sliderValue;

            }

            // At end of arrange pass, set the tick to the inital value
            SetTickToValue(this, Value);

            return returnSize;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_Slider = Template.FindName(nameof(PART_Slider), this) as Slider;

            if (PART_Slider == null)
                throw new InvalidOperationException();

            PART_Slider.ValueChanged += Slider_ValueChanged;
        }
    }
}