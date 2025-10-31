using System;
using System.Threading.Tasks;

namespace WarCouncilModern.UI.Platform
{
    public interface IUiInvoker
    {
        void InvokeOnUi(Action action);
        Task InvokeOnUiAsync(Func<Task> action);
    }
}
