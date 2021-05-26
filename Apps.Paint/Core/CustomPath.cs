using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Paint
{
    [Serializable]
    public class CustomPath : Base
    {
        public int Count => Points?.Count ?? 0;

        List<Point> preservedPoints = new List<Point>();

        [field: NonSerialized]
        PointCollection points = null;
        public PointCollection Points
        {
            get => points;
            set => this.Change(ref points, value);
        }

        public CustomPath() : base()
        {
            Points = new PointCollection();
        }

        public CustomPath(params Vector2<int>[] points) : base()
        {
            var result = new PointCollection();
            foreach (var i in points)
                result.Add(new Point(i.X, i.Y));

            Points = result;
        }

        public CustomPath(IList<Point> points) : base()
        {
            var result = new PointCollection();
            foreach (var i in points)
                result.Add(i);

            Points = result;
        }

        public CustomPath(params Point[] points) : base()
        {
            var result = new PointCollection();
            foreach (var i in points)
                result.Add(i);

            Points = result;
        }

        public PathGeometry Geometry()
        {
            var pathFigures = new List<PathFigure>();
            var pathSegments = new List<PathSegment>();

            for (var i = 0; i < points.Count; i++)
            {
                if (i == points.Count - 1)
                    break;

                var a = points[i];
                var b = points[i + 1];

                pathSegments.Add(new LineSegment(b, true));
                pathFigures.Add(new PathFigure(a, pathSegments, false));
                pathSegments.Clear();
            }

            return new PathGeometry(pathFigures);
        }

        public void Refresh(IEnumerable<Point> points)
        {
            Refresh(points.ToArray());
        }

        public void Refresh(params Point[] points)
        {
            Points.Clear();
            foreach (var i in points)
                Points.Add(i);
        }

        public void Preserve()
        {
            preservedPoints.Clear();
            points.ForEach(i => preservedPoints.Add(i));
        }

        public void Restore()
        {
            points = new PointCollection();
            preservedPoints.ForEach(i => points.Add(i));
        }

        public enum CombineMode
        {
            Complement,
            Replace,
            Xor,
            Intersect,
            Union,
            Exclude,
        }

        /*
        public static GraphicsPath ClipPath(GraphicsPath subjectPath, CombineMode combineMode, GraphicsPath clipPath)
        {
            GpcWrapper.Polygon.Validate(combineMode);

            GpcWrapper.Polygon basePoly = new GpcWrapper.Polygon(subjectPath);

            GraphicsPath clipClone = (GraphicsPath)clipPath.Clone();
            clipClone.CloseAllFigures();
            GpcWrapper.Polygon clipPoly = new GpcWrapper.Polygon(clipClone);
            clipClone.Dispose();

            GpcWrapper.Polygon clippedPoly = GpcWrapper.Polygon.Clip(combineMode, basePoly, clipPoly);

            GraphicsPath returnPath = clippedPoly.ToGraphicsPath();
            returnPath.CloseAllFigures();
            return returnPath;
        }

        public static PointSelection Combine(PointSelection subjectPath, CombineMode combineMode, PointSelection clipPath)
        {
            switch (combineMode)
            {
                case CombineMode.Complement:
                    return Combine(clipPath, CombineMode.Exclude, subjectPath);

                case CombineMode.Replace:
                    return clipPath.Clone();

                case CombineMode.Xor:
                case CombineMode.Intersect:
                case CombineMode.Union:
                case CombineMode.Exclude:
                    if (subjectPath.Count == 0 && clipPath.Count == 0)
                    {
                        return new PointSelection(); // empty path
                    }
                    else if (subjectPath.IsEmpty)
                    {
                        switch (combineMode)
                        {
                            case CombineMode.Xor:
                            case CombineMode.Union:
                                return clipPath.Clone();

                            case CombineMode.Intersect:
                            case CombineMode.Exclude:
                                return new PointSelection();

                            default:
                                throw new InvalidEnumArgumentException();
                        }
                    }
                    else if (clipPath.IsEmpty)
                    {
                        switch (combineMode)
                        {
                            case CombineMode.Exclude:
                            case CombineMode.Xor:
                            case CombineMode.Union:
                                return subjectPath.Clone();

                            case CombineMode.Intersect:
                                return new PointSelection();

                            default:
                                throw new InvalidEnumArgumentException();
                        }
                    }
                    else
                    {
                        GraphicsPath resultPath = ClipPath(subjectPath, combineMode, clipPath);
                        return new PointSelection(resultPath);
                    }

                default:
                    throw new InvalidEnumArgumentException();
            }
        }
        */
    }
}