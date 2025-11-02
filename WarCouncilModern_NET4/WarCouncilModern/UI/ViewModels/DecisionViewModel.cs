using System.Collections.ObjectModel;
using TaleWorlds.Library;
using WarCouncilModern.UI.Commands;

namespace WarCouncilModern.UI.ViewModels
{
    public class DecisionViewModel : ViewModel
    {
        private string _title = string.Empty;
        private string _description = string.Empty;
        private int _votesFor;
        private int _votesAgainst;

        [DataSourceProperty]
        public string Title
        {
            get => _title;
            set
            {
                if (value != _title)
                {
                    _title = value;
                    OnPropertyChangedWithValue(value, nameof(Title));
                }
            }
        }

        [DataSourceProperty]
        public string Description
        {
            get => _description;
            set
            {
                if (value != _description)
                {
                    _description = value;
                    OnPropertyChangedWithValue(value, nameof(Description));
                }
            }
        }

        [DataSourceProperty]
        public int VotesFor
        {
            get => _votesFor;
            set
            {
                if (value != _votesFor)
                {
                    _votesFor = value;
                    OnPropertyChangedWithValue(value, nameof(VotesFor));
                }
            }
        }

        [DataSourceProperty]
        public int VotesAgainst
        {
            get => _votesAgainst;
            set
            {
                if (value != _votesAgainst)
                {
                    _votesAgainst = value;
                    OnPropertyChangedWithValue(value, nameof(VotesAgainst));
                }
            }
        }

        public DelegateCommand VoteYesCommand { get; }
        public DelegateCommand VoteNoCommand { get; }

        public DecisionViewModel()
        {
            VoteYesCommand = new DelegateCommand(_ => OnVoteYes());
            VoteNoCommand = new DelegateCommand(_ => OnVoteNo());
        }

        private void OnVoteYes() => VotesFor++;
        private void OnVoteNo() => VotesAgainst++;
    }
}
