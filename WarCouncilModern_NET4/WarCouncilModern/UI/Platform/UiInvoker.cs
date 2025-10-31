using System;
using System.Threading;
using System.Threading.Tasks;

namespace WarCouncilModern.UI.Platform
{
    public class UiInvoker : IUiInvoker
    {
        private readonly TaskScheduler _uiScheduler;

        public UiInvoker(TaskScheduler uiScheduler)
        {
            _uiScheduler = uiScheduler ?? TaskScheduler.Current;
        }

        public void InvokeOnUi(Action action)
        {
            if (TaskScheduler.Current == _uiScheduler)
            {
                action();
                return;
            }
            Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, _uiScheduler).Wait();
        }

        public Task InvokeOnUiAsync(Func<Task> action)
        {
            if (TaskScheduler.Current == _uiScheduler) return action();
            return Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, _uiScheduler).Unwrap();
        }
    }
}
