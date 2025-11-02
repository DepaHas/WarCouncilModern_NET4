using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using TaleWorlds.Library;
using WarCouncilModern.UI.Commands;
using WarCouncilModern.UI.Services;

namespace WarCouncilModern.UI.ViewModels
{
    public class CouncilOverviewViewModel : ViewModel
    {
        private readonly ICouncilUiService _councilUiService;

        [DataSourceProperty]
        public MBBindingList<DecisionViewModel> Decisions { get; }

        [DataSourceProperty]
        public bool IsLoading { get; set; }

        [DataSourceProperty]
        public string Title { get; set; }

        public ICommand ProposeCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand OpenDetailCommand { get; }

        public CouncilOverviewViewModel(ICouncilUiService councilUiService)
        {
            _councilUiService = councilUiService;
            Decisions = new MBBindingList<DecisionViewModel>();
            Title = "War Council Overview";

            ProposeCommand = new DelegateCommand(_ => Propose());
            RefreshCommand = new DelegateCommand(async _ => await RefreshAsync());
            OpenDetailCommand = new DelegateCommand(OpenDetail);
        }

        private void Propose()
        {
            // Logic to open proposal modal will be here
        }

        private void OpenDetail(object? param)
        {
            if (param is DecisionViewModel decision)
            {
                // Logic to open detail view for the decision
            }
        }

        public async Task InitializeAsync()
        {
            await RefreshAsync();
        }

        public async Task RefreshAsync()
        {
            IsLoading = true;
            Decisions.Clear();

            await Task.Delay(500); // Simulate network latency

#if DEBUG
            // Populate with mock data in DEBUG mode
            Decisions.Add(new DecisionViewModel { Id = "1", Title = "Declare War on Vlandia", Description = "Vlandia's expansion must be stopped.", VotesFor = 5, VotesAgainst = 2 });
            Decisions.Add(new DecisionViewModel { Id = "2", Title = "Enact Defensive Pact with Battania", Description = "An alliance will secure our western border.", VotesFor = 7, VotesAgainst = 0 });
            Decisions.Add(new DecisionViewModel { Id = "3", Title = "Increase Noble Levies", Description = "We need more troops for the upcoming campaign.", VotesFor = 3, VotesAgainst = 4 });
#endif

            IsLoading = false;
        }
    }
}
