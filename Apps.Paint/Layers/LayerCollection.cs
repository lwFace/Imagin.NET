using System;
using System.Collections.ObjectModel;
using System.Windows.Media.Imaging;

namespace Paint
{
    [Serializable]
    public class LayerCollection : ObservableCollection<Layer>
    {
        public event EventHandler<EventArgs> Changed;

        public readonly Document Document;

        public LayerCollection(Document document) : base()
        {
            Document = document;
        }

        public void Each<T>(Action<T> action, LayerCollection layers = null) where T : Layer
        {
            layers = layers ?? this;
            for (var i = layers.Count - 1; i >= 0; i--)
            {
                if (layers[i] is T)
                    action((T)layers[i]);

                if (layers[i] is GroupLayer)
                    Each(action, (layers[i] as GroupLayer).Layers);
            }
        }

        /// <summary>
        /// This is how you add, dog!
        /// </summary>
        /// <param name="index"></param>
        /// <param name="layer"></param>
        new public void Add(Layer layer)
        {
            layer.Document = Document;
            base.Add(layer);
            Changed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// This is how you insert, dog!
        /// </summary>
        /// <param name="index"></param>
        /// <param name="layer"></param>
        new public void Insert(int index, Layer layer)
        {
            layer.Document = Document;
            base.Insert(index, layer);
            Changed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// This is how you remove, dog!
        /// </summary>
        /// <param name="index"></param>
        /// <param name="layer"></param>
        new public bool Remove(Layer layer)
        {
            if (base.Remove(layer))
            {
                layer.Document = null;

                if (layer is GroupLayer)
                    Each<Layer>(i => i.Document = null, (layer as GroupLayer).Layers);

                Changed?.Invoke(this, EventArgs.Empty);
                return true;
            }
            return false;
        }

        /// <summary>
        /// This is how you remove at, dog!
        /// </summary>
        /// <param name="index"></param>
        /// <param name="layer"></param>
        new public void RemoveAt(int index)
        {
            if (index < Count)
            {
                var layer = this[index];
                layer.Document = null;

                if (layer is GroupLayer)
                    Each<Layer>(i => i.Document = null, (layer as GroupLayer).Layers);
            }
            base.RemoveAt(index);
            Changed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// This is how you clear, dog!
        /// </summary>
        /// <param name="index"></param>
        /// <param name="layer"></param>
        new public void Clear()
        {
            Each<Layer>(i => i.Document = null);
            base.Clear();
            Changed?.Invoke(this, EventArgs.Empty);
        }

        public void Render(WriteableBitmap input)
        {
            input.ForEach((x, y, color) =>
            {
                System.Windows.Media.Color? result = null;
                //Let's assume we start with the last layer in the collection (or the bottom most layer)
                Each<VisualLayer>(i =>
                {
                    if (i is PixelLayer)
                    {
                        var other = (i as PixelLayer).Pixels;
                        if (x < other.PixelWidth && y < other.PixelHeight)
                        {
                            var b = other.GetPixel(x, y);
                            if (result == null)
                            {
                                //If no color has been recorded yet, record the first layer (or the bottom most)
                                result = b;
                                return;
                            }
                            //If a color has been recorded, let's say this is the second layer; in what order are the colors blended?
                            result = VisualLayer.Blend(i.BlendMode, b, result.Value);
                        }
                    }
                });
                return result ?? color;
            });
        }
    }
}