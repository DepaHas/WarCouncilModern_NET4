using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaleWorlds.Library;

namespace WarCouncilModern.UI.ViewModels
{
    public class ViewModelBase : ViewModel
    {
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}
