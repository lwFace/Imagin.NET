using System.Threading;
using System.Threading.Tasks;

namespace Imagin.Common.Threading
{
    public delegate void CancellableDelegate(CancellationToken Token);

    public delegate Task AsyncDelegate();

    public delegate void TaskCompletedEventHandler(CancellableTask task);

    public class CancellableTask : Base
    {
        public event TaskCompletedEventHandler Completed;

        readonly CancellableDelegate action;

        CancellationTokenSource tokenSource;

        public bool IsCancelled => tokenSource.IsCancellationRequested;

        public CancellableTask(CancellableDelegate Action)
        {
            action = Action;
        }

        public void Complete()
        {
            tokenSource?.Cancel();
        }

        public async Task Invoke()
        {
            tokenSource = new CancellationTokenSource();
            await Task.Run(() => action.Invoke(tokenSource.Token), tokenSource.Token);
            Completed?.Invoke(this);
        }
    }
}