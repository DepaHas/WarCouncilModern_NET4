using System.Windows.Input;
using TaleWorlds.Library;

namespace WarCouncilModern.UI.ViewModels
{
    public class DecisionViewModel : ViewModel
    {
        [DataSourceProperty]
        public string Id { get; set; }

        [DataSourceProperty]
        public string Title { get; set; }

        [DataSourceProperty]
        public string Description { get; set; }

        [DataSourceProperty]
        public int VotesFor { get; set; }

        [DataSourceProperty]
        public int VotesAgainst { get; set; }

        public ICommand VoteCommand { get; }

        public DecisionViewModel()
        {
            Id = string.Empty;
            Title = string.Empty;
            Description = string.Empty;
        }
    }
}