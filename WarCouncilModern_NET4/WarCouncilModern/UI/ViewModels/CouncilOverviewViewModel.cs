using System.Collections.ObjectModel;
using TaleWorlds.Library;
using WarCouncilModern.UI.Services;
using WarCouncilModern.UI.Commands;
using WarCouncilModern.UI.Dto;

namespace WarCouncilModern.UI.ViewModels
{
    public class CouncilOverviewViewModel : TaleWorlds.Library.ViewModel
    {
        private readonly ICouncilUiService _uiService;

        public CouncilOverviewViewModel(ICouncilUiService uiService)
        {
            _uiService = uiService;
            ProposeCommand = new DelegateCommand(async _ => await _uiService.ExecuteProposeNewDecision(), _ => _uiService.CanProposeNewDecision);
        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(IsLoading));
            OnPropertyChanged(nameof(Decisions));
        }

        [DataSourceProperty]
        public string Title => "War Council Overview";

        [DataSourceProperty]
        public bool IsLoading => _uiService.IsLoading;

        [DataSourceProperty]
        public ObservableCollection<WarDecisionDto> Decisions => _uiService.Decisions;

        public DelegateCommand ProposeCommand { get; }
        public DelegateCommand CloseCommand => new DelegateCommand(_ => OnClose());

        private void OnClose()
        {
            // Logic to close the screen
        }

    }
}
