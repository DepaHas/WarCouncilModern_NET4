using System;
using System.Windows.Input;
using WarCouncilModern.UI.Commands;
using WarCouncilModern.UI.Dto;

namespace WarCouncilModern.UI.ViewModels
{
    public class CouncilItemViewModel : ViewModelBase
    {
        private readonly WarCouncilDto _dto;
        private readonly CouncilOverviewViewModel _parent;

        public Guid Id => _dto.SaveId;
        public string Title => _dto.Title;
        public int MemberCount => _dto.MemberCount;
        public int ActiveDecisionsCount => _dto.Decisions.Count;
        public string Status => _dto.Status;

        public ICommand OpenCouncilDetailCommand { get; }

        public CouncilItemViewModel(WarCouncilDto dto, CouncilOverviewViewModel parent)
        {
            _dto = dto;
            _parent = parent;
            OpenCouncilDetailCommand = new DelegateCommand(ExecuteOpenCouncilDetail);
        }

        private void ExecuteOpenCouncilDetail(object obj)
        {
            _parent.SelectCouncil(this);
        }
    }
}
