using System.Collections.ObjectModel;
using TaleWorlds.Core.ViewModelCollection;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using WarCouncilModern.UI.Commands;
using WarCouncilModern.UI.Services;
using WarCouncilModern.UI.ViewModels;

namespace WarCouncilModern.UI.ViewModels
{
    public class CouncilOverviewViewModel : ViewModelBase
    {
        private readonly ICouncilUiService _uiService;
        private CouncilItemViewModel? _selectedCouncil;

        [DataSourceProperty]
        public string Title { get; }

        [DataSourceProperty]
        public ObservableCollection<CouncilItemViewModel> Decisions { get; set; }

        [DataSourceProperty]
        public CouncilItemViewModel? SelectedCouncil
        {
            get => _selectedCouncil;
            set
            {
                if (value != _selectedCouncil)
                {
                    _selectedCouncil = value;
                    OnPropertyChangedWithValue(value, nameof(SelectedCouncil));
                }
            }
        }

        public CouncilOverviewViewModel(ICouncilUiService uiService)
        {
            _uiService = uiService;
            Title = new TextObject("{=WC_OverviewTitle}War Council Overview").ToString();
            Decisions = new ObservableCollection<CouncilItemViewModel>();
        }

        public void SelectCouncil(CouncilItemViewModel item)
        {
            SelectedCouncil = item;
            Logger?.Info($"Council selected: {item?.Name}");
        }
    }
}
