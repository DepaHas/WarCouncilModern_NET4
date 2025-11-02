using System.Collections.ObjectModel;
using TaleWorlds.Library;
using WarCouncilModern.Initialization;

namespace WarCouncilModern.UI.ViewModels
{
    public class CouncilOverviewViewModel : ViewModel
    {
        private bool _isLoading;
        private string _title = string.Empty;
        private DecisionViewModel? _selectedDecision;

        public ObservableCollection<DecisionViewModel> Decisions { get; } = new();

        [DataSourceProperty]
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChangedWithValue(value, nameof(IsLoading));
                }
            }
        }

        [DataSourceProperty]
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChangedWithValue(value, nameof(Title));
                }
            }
        }

        [DataSourceProperty]
        public DecisionViewModel? SelectedDecision
        {
            get => _selectedDecision;
            set
            {
                if (_selectedDecision != value)
                {
                    _selectedDecision = value;
                    OnPropertyChangedWithValue(value, nameof(SelectedDecision));
                }
            }
        }

        public MBBindingCommand ProposeCommand { get; }
        public MBBindingCommand RefreshCommand { get; }
        public MBBindingCommand OpenDetailCommand { get; }

        public CouncilOverviewViewModel()
        {
            Title = "War Council Overview";
            ProposeCommand = new MBBindingCommand(OnPropose);
            RefreshCommand = new MBBindingCommand(OnRefresh);
            OpenDetailCommand = new MBBindingCommand(OnOpenDetail);

#if DEBUG
            // Mock data for development
            Decisions.Add(new DecisionViewModel { Title = "Declare War", Description = "Proposal to declare war on Western Empire." });
            Decisions.Add(new DecisionViewModel { Title = "Trade Pact", Description = "Negotiate a trade pact with Battania." });
            Decisions.Add(new DecisionViewModel { Title = "Raise Army", Description = "Summon the vassals for a campaign." });
#endif
        }

        private void OnPropose() => SubModule.Logger.Info("ProposeCommand triggered.");
        private void OnRefresh() => SubModule.Logger.Info("RefreshCommand triggered.");
        private void OnOpenDetail() => SubModule.Logger.Info("OpenDetailCommand triggered.");
    }
}
