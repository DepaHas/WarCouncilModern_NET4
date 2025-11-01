using TaleWorlds.Library;

namespace WarCouncilModern.UI.ViewModels
{
    public class DecisionDetailViewModel : ViewModel
    {
        private string _title;
        private string _description;

        public string Title
        {
            get => _title;
            set
            {
                if (_title == value) return;
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description == value) return;
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public DecisionDetailViewModel(string title, string description)
        {
            _title = title;
            _description = description;
        }
    }
}