using System.Windows;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// Improves performance of <see cref="System.Windows.Controls.Primitives.ToolBarOverflowPanel"/>.
    /// </summary>
    public class ToolBarOverflowPanel : System.Windows.Controls.Primitives.ToolBarOverflowPanel
    {
        static readonly Thickness elementPadding = new Thickness(5);

        Size panelSize;

        /// <summary>
        /// Overrides the <see cref="MeasureOverride"/> for better panel size.
        /// </summary>
        /// <param name="constraint"></param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size constraint)
        {
            base.MeasureOverride(constraint); 

            panelSize = new Size(0, 0);
            for (int i = 0; i < Children.Count; i++)
            {
                UIElement element = Children[i];
                Size elementDesiredSize = element.DesiredSize;

                panelSize.Width = System.Math.Max(elementDesiredSize.Width + elementPadding.Left + elementPadding.Right, panelSize.Width);
                panelSize.Height += elementDesiredSize.Height + elementPadding.Top + elementPadding.Bottom;
            }
            return panelSize;
        }

        /// <summary>
        /// Overrides the <see cref="ArrangeOverride"/> for vertical arrangement.
        /// </summary>
        /// <param name="arrangeBounds"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            //base.ArrangeOverride(arrangeBounds);
            double yOffset = 0;
            double xOffset = elementPadding.Left;

            double maxMargin = 0;
            for (int j = 0; j < Children.Count; j++)
            {
                FrameworkElement possibleMarginElement = Children[j] as FrameworkElement;
                if (possibleMarginElement != null)
                {
                    maxMargin = System.Math.Max(maxMargin, possibleMarginElement.Margin.Left);
                }
            }
            xOffset = maxMargin;

            for (int i = 0; i < Children.Count; i++)
            {
                yOffset += elementPadding.Top;

                UIElement element = Children[i];
                Size elementDesiredSize = element.DesiredSize;

                FrameworkElement marginElement = element as FrameworkElement;
                Thickness thicknessCopy = new Thickness(0);
                if (marginElement != null)
                {
                    xOffset -= marginElement.Margin.Left < 0 ? 0 : marginElement.Margin.Left;

                    TranslateTransform translateTransform = marginElement.RenderTransform != null ? marginElement.RenderTransform as TranslateTransform : null;

                    if (translateTransform != null) xOffset -= translateTransform.X;

                    element.Arrange(new Rect(xOffset, yOffset, elementDesiredSize.Width, elementDesiredSize.Height));
                }
                yOffset += elementPadding.Bottom + elementDesiredSize.Height;
                xOffset = maxMargin;
            }
            return panelSize;
        }
    }
}