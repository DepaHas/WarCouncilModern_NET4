using System;
using WarCouncilModern.UI.Dto;

namespace WarCouncilModern.UI.ViewModels
{
    public class CouncilItemViewModel : ViewModelBase
    {
        private readonly WarCouncilDto _dto;

        public Guid Id => _dto.SaveId;
        public string Title => _dto.Title;
        public int MemberCount => _dto.MemberCount;
        public int ActiveDecisionsCount => _dto.Decisions.Count;
        public string Status => _dto.Status;

        public CouncilItemViewModel(WarCouncilDto dto)
        {
            _dto = dto;
        }
    }
}
