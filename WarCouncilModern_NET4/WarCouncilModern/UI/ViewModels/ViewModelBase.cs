using System.Runtime.CompilerServices;
using TaleWorlds.Library;
using WarCouncilModern.Initialization;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.UI.ViewModels
{
    // الكلاس الأساسي لـ ViewModels يرث مباشرة من كلاس اللعبة
    public abstract class ViewModelBase : ViewModel
    {
        protected static readonly IModLogger Logger = SubModule.Logger;

        // Use 'new' to hide the base method and avoid the override error.
        new protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }

    }
}