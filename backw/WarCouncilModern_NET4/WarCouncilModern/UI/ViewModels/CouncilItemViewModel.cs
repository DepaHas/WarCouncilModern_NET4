using System;
using System.Windows.Input;
using TaleWorlds.Localization;
using WarCouncilModern.UI.Commands;
using WarCouncilModern.UI.Dto;

namespace WarCouncilModern.UI.ViewModels
{
    public class CouncilItemViewModel : ViewModelBase
    {
        private readonly WarCouncilDto _dto;
        private readonly CouncilOverviewViewModel _parent;

        public Guid Id => _dto.SaveId;
        public string Name { get; }
        public int MemberCount => _dto.MemberCount;
        public int ActiveDecisionsCount => _dto.Decisions.Count;
        public string Status => _dto.Status;

        public ICommand SelectCommand { get; }

        public CouncilItemViewModel(WarCouncilDto dto, CouncilOverviewViewModel parent)
        {
            _dto = dto;
            _parent = parent;
            Name = new TextObject(_dto.Title).ToString();
            SelectCommand = new DelegateCommand(ExecuteSelect);
        }

        private void ExecuteSelect(object? obj)
        {
            _parent.SelectCouncil(this);
        }
    }
}
