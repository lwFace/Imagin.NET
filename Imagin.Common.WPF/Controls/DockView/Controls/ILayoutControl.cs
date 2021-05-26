namespace Imagin.Common.Controls
{
    public interface ILayoutControl
    {
        double ActualHeight { get; }

        double ActualWidth { get; }

        DockView DockView { get; }

        LayoutRootControl Root { get; }
    }
}