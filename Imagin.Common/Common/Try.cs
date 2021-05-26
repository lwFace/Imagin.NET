using Imagin.Common.Analytics;
using System;
using System.Threading.Tasks;

namespace Imagin.Common
{
    public static class Try
    {
        public static Result Invoke(Action @try, Action<Exception> @catch = null)
        {
            try
            {
                @try();
                return new Success();
            }
            catch (Exception e)
            {
                @catch?.Invoke(e);
                return new Error(e);
            }
        }

        public static async Task<Result> InvokeAsync(Action @try, Action<Exception> @catch = null) => await Task.Run(() => Invoke(@try, @catch));
    }
}