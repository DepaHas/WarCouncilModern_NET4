using System;
using System.Collections.ObjectModel;
using System.Linq;
using WarCouncilModern.Core.Events;
using WarCouncilModern.Models.Entities;
using WarCouncilModern.UI.DTOs;
using WarCouncilModern.UI.Interfaces;

namespace WarCouncilModern.UI.Services
{
    public class CouncilUiService : ICouncilUiService, IDisposable
    {
        private readonly IUiInvoker _uiInvoker;
        private readonly ObservableCollection<WarCouncilDTO> _allCouncils;
        public ReadOnlyObservableCollection<WarCouncilDTO> AllCouncils { get; }

        public CouncilUiService(IUiInvoker uiInvoker)
        {
            _uiInvoker = uiInvoker;
            _allCouncils = new ObservableCollection<WarCouncilDTO>();
            AllCouncils = new ReadOnlyObservableCollection<WarCouncilDTO>(_allCouncils);

            CouncilEvents.OnCouncilCreated += OnCouncilCreated;
        }

        private void OnCouncilCreated(WarCouncil council)
        {
            var dto = new WarCouncilDTO
            {
                SaveId = council.SaveId,
                KingdomId = council.KingdomStringId,
                Title = council.Name,
                MemberCount = council.MemberHeroIds.Count(),
                ActiveDecisionsCount = council.Decisions.Count(d => d.Status == "Proposed" || d.Status == "VotingOpen"),
                CreatedAt = council.CreatedAt,
                Status = "Active"
            };

            _uiInvoker.InvokeOnUi(() => _allCouncils.Add(dto));
        }

        public void Dispose()
        {
            CouncilEvents.OnCouncilCreated -= OnCouncilCreated;
        }
    }
}
