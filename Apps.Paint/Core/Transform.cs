using Imagin.Common;
using Imagin.Common.Math;
using System;
using System.Runtime.CompilerServices;

namespace Paint
{
    [Serializable]
    public abstract class Transform : Base
    {
        [Serializable]
        public enum Modes
        {
            None,
            Distort,
            Perspective,
            Rotate,
            Scale,
            Skew,
            Warp
        }

        readonly VisualLayer VisualLayer;

        public abstract Modes Mode { get; }

        public Transform(VisualLayer visualLayer)
        {
            VisualLayer = visualLayer;
        }

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            VisualLayer.Changed(() => VisualLayer.Transform);
        }
    }

    public class DistortTransform : Transform
    {
        public override Modes Mode => Modes.Distort;

        public DistortTransform(VisualLayer layer) : base(layer) { }
    }

    public class PerspectiveTransform : Transform
    {
        public override Modes Mode => Modes.Perspective;

        public PerspectiveTransform(VisualLayer layer) : base(layer) { }
    }

    public class RotateTransform : Transform
    {
        public override Modes Mode => Modes.Rotate;

        public RotateTransform(VisualLayer layer) : base(layer) { }
    }

    public class ScaleTransform : Transform
    {
        public override Modes Mode => Modes.Scale;

        public ScaleTransform(VisualLayer layer) : base(layer) { }
    }

    public class SkewTransform : Transform
    {
        public override Modes Mode => Modes.Skew;

        Vector2<int> topLeft;
        public Vector2<int> TopLeft
        {
            get => topLeft;
            set => this.Change(ref topLeft, value);
        }

        Vector2<int> topRight;
        public Vector2<int> TopRight
        {
            get => topRight;
            set => this.Change(ref topRight, value);
        }

        Vector2<int> bottomLeft;
        public Vector2<int> BottomLeft
        {
            get => bottomLeft;
            set => this.Change(ref bottomLeft, value);
        }

        Vector2<int> bottomRight;
        public Vector2<int> BottomRight
        {
            get => bottomRight;
            set => this.Change(ref bottomRight, value);
        }

        public SkewTransform(VisualLayer layer) : base(layer) { }
    }

    public class WarpTransform : Transform
    {
        public override Modes Mode => Modes.Warp;

        public WarpTransform(VisualLayer layer) : base(layer) { }
    }
}
