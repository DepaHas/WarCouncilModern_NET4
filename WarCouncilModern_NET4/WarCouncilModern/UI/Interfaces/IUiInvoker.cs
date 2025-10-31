using System;
using System.Threading.Tasks;

namespace WarCouncilModern.UI.Interfaces
{
    public interface IUiInvoker
    {
        void InvokeOnUi(Action action);
        Task InvokeOnUiAsync(Func<Task> func);
    }
}
