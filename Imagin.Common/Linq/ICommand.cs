using System.Windows.Input;

namespace Imagin.Common.Linq
{
    public static class ICommandExtensions
    {
        public static void Execute(this ICommand input) => input.Execute(null);
    }
}