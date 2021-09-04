using Imagin.Common.Linq;
using Paint.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Paint
{
    public class LayerArranger
    {
        readonly LayerView view;

        readonly Dictionary<Layer, UIElement> children = new Dictionary<Layer, UIElement>();

        LayerCollection layers;
        public LayerCollection Layers
        {
            get => layers;
            set
            {
                layers = value;
                if (value != null)
                {
                    value.Changed -= OnLayersChanged;
                    value.Changed += OnLayersChanged;
                    Update();
                }
            }
        }

        public LayerArranger(LayerView view)
        {
            this.view = view;
        }

        void OnLayersChanged(object sender, EventArgs e)
        {
            Update();
        }

        BlendEffect Effect(BlendModes blend)
        {
            var effects = Assembly.GetCallingAssembly().GetTypes().Select(i => i);
            var effectType = effects.First(i => i.Namespace == $"{nameof(Paint)}.{nameof(Effects)}" && i.Name.Equals($"{blend}Effect"));
            return (BlendEffect)Activator.CreateInstance(effectType);
        }

        /// <summary>
        /// Currently, it does not appear possible to blend two <see cref="UIElement"/>s (in this case, the <see cref="Grid"/>) using
        /// conventional <see cref="ShaderEffect"/>s; it is only possible to blend a <see cref="UIElement"/> with an <see cref="Image"/>
        /// (or <see cref="ImageBrush"/>). Consider the following scenario: Blend a group of layers (<see cref="GroupLayer"/>) with another
        /// group of layers. Both would ultimately produce a <see cref="Grid"/>. In order to blend, an <see cref="ImageSource"/> must be 
        /// supplied to the <see cref="ShaderEffect"/>. You cannot a bind a <see cref="Grid"/> to it! Therefore, the last layer in a group
        /// blends with the first layer immediately after the group. Unfortunately, this means a <see cref="GroupLayer"/> cannot specify a
        /// blend of its own. This also means adjustments apply to everything below regardless of depth.
        /// </summary>
        /// <param name="layers"></param>
        /// <returns></returns>
        Grid Build(LayerCollection layers)
        {
            Grid a = new Grid(), b = null;

            Layer last = null;
            layers.Each<Layer>(i =>
            {
                i.PropertyChanged -= OnLayerChanged;
                i.PropertyChanged += OnLayerChanged;

                if (last == null)
                {
                    if (i is VisualLayer)
                    {
                        var image = new Image();
                        image.Opacity = i.Opacity;
                        image.Source = (i as VisualLayer).Pixels;
                        image.Visibility = i.IsVisible.Visibility();

                        a.Children.Add(image);
                        children.Add(i, image);
                    }
                    else children.Add(i, a);

                    last = i;
                    return;
                }

                b = new Grid();

                children.Add(i, a);
                b.Children.Add(a);

                ShaderEffect effect = null;
                if (i is AdjustmentLayer)
                {
                    effect = (i as AdjustmentLayer).Effect;
                }
                else if (i is VisualLayer)
                {
                    effect = Effect(i.BlendMode);

                    var imageBrush = new ImageBrush();
                    imageBrush.ImageSource = (i as VisualLayer).Pixels;
                    imageBrush.Opacity = !i.IsVisible ? 0 : i.Opacity;
                    (effect as BlendEffect).BInput = imageBrush;
                }

                a.Effect = effect;

                a = b;
                last = i;
            });

            return a;
        }

        public void Update()
        {
            children.Clear();
            view.Arrangement.Child = null;
            view.Arrangement.Child = Build(layers);
        }

        void OnLayerChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var layer = (VisualLayer)sender;
            switch (e.PropertyName)
            {
                case nameof(Layer.BlendMode):

                    if (children.ContainsKey(layer))
                    {
                        if (children[layer].Effect != null)
                        {
                            var effect = Effect(layer.BlendMode);
                            effect.BInput = new ImageBrush(layer.Pixels);
                            children[layer].Effect = effect;
                        }
                    }
                    break;

                case nameof(Layer.IsVisible):

                    if (children.ContainsKey(layer))
                    {
                        if (children[layer] is Image)
                        {
                            children[layer].Visibility = layer.IsVisible.Visibility();
                            return;
                        }

                        var imageBrush = (ImageBrush)((BlendEffect)children[layer].Effect).BInput;
                        imageBrush.Opacity = !layer.IsVisible ? 0 : layer.Opacity;
                    }
                    break;

                case nameof(Layer.Opacity):

                    if (children.ContainsKey(layer))
                    {
                        if (children[layer] is Image)
                        {
                            children[layer].Opacity = layer.Opacity;
                            return;
                        }

                        var imageBrush = (ImageBrush)((BlendEffect)children[layer].Effect).BInput;
                        imageBrush.Opacity = layer.Opacity;
                    }
                    break;
            }
        }
    }
}