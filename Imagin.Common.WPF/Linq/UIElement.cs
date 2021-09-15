using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Imagin.Common.Linq
{
    public static class UIElementExtensions
    {
        public static Task Animate(this UIElement input, Storyboard story)
        {
            var result = new TaskCompletionSource<object>();

            EventHandler handler = default;
            handler = (s, e) =>
            {
                story.Completed -= handler;
                result.TrySetResult(null);
            };

            story.Completed += handler;
            story.Begin();
            return result.Task;
        }

        //...........................................................................................

        public static Task FadeIn(this UIElement input, Duration duration = default)
        {
            var animation = new DoubleAnimation()
            {
                From = 0.0, To = 1.0,
                Duration = duration == default ? 0.3.Seconds().Duration() : duration
            };
            Storyboard.SetTarget(animation, input);
            Storyboard.SetTargetProperty(animation, new PropertyPath(nameof(UIElement.Opacity)));

            var result = new Storyboard();
            result.Children.Add(animation);
            return input.Animate(result);
        }

        public static Task FadeOut(this UIElement input, Duration duration = default, EventHandler Callback = null)
        {
            var animation = new DoubleAnimation()
            {
                From = 1.0, To = 0.0,
                Duration = duration == default ? 0.5.Seconds().Duration() : duration
            };
            Storyboard.SetTarget(animation, input);
            Storyboard.SetTargetProperty(animation, new PropertyPath(nameof(UIElement.Opacity)));

            var result = new Storyboard();
            result.Children.Add(animation);
            return input.Animate(result);
        }
    }
}