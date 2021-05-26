using Imagin.Common.Linq;
using Paint.Adjust;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Paint
{
    /// <summary>
    /// For displaying multiple effects on the same <see cref="UIElement"/>! :)
    /// </summary>
    public static class EffectExtensions
    {
        static Dictionary<Border, AdjustmentEffectCollection> listeners = new Dictionary<Border, AdjustmentEffectCollection>();

        static void Listen(Border border, AdjustmentEffectCollection effects)
        {
            if (effects != null)
                effects.Changed += OnEffectsChanged;

            if (border != null)
                border.Loaded += OnParentLoaded;
        }

        static void Unlisten(Border border, AdjustmentEffectCollection effects)
        {
            if (effects != null)
                effects.Changed -= OnEffectsChanged;

            if (border != null)
                border.Loaded -= OnParentLoaded;
        }

        public static readonly DependencyProperty EffectsProperty = DependencyProperty.RegisterAttached("Effects", typeof(AdjustmentEffectCollection), typeof(EffectExtensions), new PropertyMetadata(default(AdjustmentEffectCollection), OnEffectsChanged));
        public static AdjustmentEffectCollection GetEffects(Border d)
        {
            return (AdjustmentEffectCollection)d.GetValue(EffectsProperty);
        }
        public static void SetEffects(Border d, bool value)
        {
            d.SetValue(EffectsProperty, value);
        }
        static void OnEffectsChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var border = sender as Border;
            if (e.NewValue != null)
            {
                var effects = (AdjustmentEffectCollection)e.NewValue;

                var oldEntry = listeners.FirstOrDefault(i => i.Value.Equals(effects));
                for (var i = 0; i < listeners.Count; i++)
                {
                    var entry = listeners.ElementAt(i);
                    if (entry.Value.Equals(effects))
                    {
                        listeners.Remove(entry.Key);
                        break;
                    }
                }

                if (!listeners.ContainsKey(border))
                    listeners.Add(border, effects);

                Unlisten(border, effects);
                Listen(border, effects);
            }

            Update(border);
        }

        static void OnParentLoaded(object sender, RoutedEventArgs e)
        {
            var effects = GetEffects(sender as Border);
            if (listeners.ContainsKey(sender as Border))
            {
                OnEffectsChanged(sender, new DependencyPropertyChangedEventArgs(EffectsProperty, effects, effects));
                return;
            }
            Unlisten(sender as Border, null);
        }

        static void OnEffectsChanged(object sender, EventArgs e)
        {
            var effects = (AdjustmentEffectCollection)sender;
            if (listeners.ContainsValue(effects))
            {
                Update(listeners.First(i => i.Value.Equals(effects)).Key);
                return;
            }
            Unlisten(null, effects);
        }

        static void Update(Border border)
        {
            var effects = GetEffects(border);

            Border parent = border, lastParent = null;

            UIElement content = null;
            while (parent != null)
            {
                content = parent.Child;
                lastParent = parent;
                parent = content as Border;
            }

            if (lastParent != null)
                lastParent.Child = null;

            border.Child = null;
            border.Child = content;

            if (effects?.Count > 0)
            {
                Border a = new Border(), b = null;

                var c = border.Child;
                border.Child = null;

                a.Child = c;
                foreach (var i in effects)
                {
                    a.Effect = i;

                    b = new Border();
                    b.Child = a;
                    a = b;
                }

                border.Child = a;
            }
        }
    }
}