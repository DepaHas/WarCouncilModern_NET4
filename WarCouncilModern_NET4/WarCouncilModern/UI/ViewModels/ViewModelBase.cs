using System.ComponentModel;
using System.Runtime.CompilerServices;
using TaleWorlds.Library;

namespace WarCouncilModern.UI.ViewModels
{
    public class ViewModelBase : ViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
