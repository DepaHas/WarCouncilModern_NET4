using System;
using System.Threading;
using System.Threading.Tasks;
using WarCouncilModern.UI.Interfaces;

namespace WarCouncilModern.UI
{
    public class UiInvoker : IUiInvoker
    {
        private readonly SynchronizationContext _syncContext;

        public UiInvoker(SynchronizationContext syncContext)
        {
            _syncContext = syncContext ?? throw new ArgumentNullException(nameof(syncContext));
        }

        public void InvokeOnUi(Action action)
        {
            _syncContext.Post(_ => action(), null);
        }

        public async Task InvokeOnUiAsync(Func<Task> func)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            _syncContext.Post(async _ =>
            {
                try
                {
                    await func();
                    tcs.SetResult(true);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }, null);
            await tcs.Task;
        }
    }
}
