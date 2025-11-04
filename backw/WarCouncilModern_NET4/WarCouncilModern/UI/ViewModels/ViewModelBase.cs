using System.Runtime.CompilerServices;
using TaleWorlds.Library;

namespace WarCouncilModern.UI.ViewModels
{
    public class ViewModelBase : ViewModel
    {
        // Use 'new' to hide the base method and avoid the override error.
        new protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}
