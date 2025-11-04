using TaleWorlds.Library;

namespace WarCouncilModern.UI.ViewModels
{
    public class CouncilItemViewModel : ViewModel
    {
        private string _name;

        public string Name
        {
            get => _name;
            set => SetField(ref _name, value, nameof(Name));
        }

        public CouncilItemViewModel(string name)
        {
            _name = name;
        }
    }
}