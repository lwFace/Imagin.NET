using System.Threading.Tasks;

namespace Imagin.Common
{
    public class HandleTask
    {
        public delegate Task Delegate();

        readonly Delegate Action;

        bool start = false;

        bool restart = false;

        public HandleTask(Delegate action) => Action = action;

        async public Task Invoke()
        {
            if (start)
            {
                restart = true;
                return;
            }

            start = true;
            await Action.Invoke();
            start = false;

            if (restart)
            {
                restart = false;
                _ = Invoke();
            }
        }
    }
}