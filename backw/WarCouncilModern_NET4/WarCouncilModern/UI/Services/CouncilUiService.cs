using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using WarCouncilModern.Core.Events;
using WarCouncilModern.Core.Manager;
using WarCouncilModern.Core.Services;
using WarCouncilModern.Models.Entities;
using WarCouncilModern.UI.Dto;
using WarCouncilModern.UI.Enums;
using WarCouncilModern.UI.Platform;
using WarCouncilModern.UI.Providers;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.UI.Services
{
    public class CouncilUiService : ICouncilUiService
    {
        private readonly ICouncilProvider _councilProvider;
        private readonly IWarCouncilManager _manager; // Still needed for write operations
        private readonly IWarDecisionService _warDecisionService;
        private readonly IUiInvoker _uiInvoker;
        private readonly IModLogger _logger;

        private readonly ObservableCollection<WarCouncilDto> _allCouncils = new();
        public ObservableCollection<WarCouncilDto> AllCouncils => _allCouncils;

        private OperationState _currentOperation = OperationState.None;
        public OperationState CurrentOperation
        {
            get => _currentOperation;
            private set { _currentOperation = value; OnPropertyChanged(nameof(CurrentOperation)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public CouncilUiService(
            ICouncilProvider councilProvider,
            IWarCouncilManager manager,
            IWarDecisionService warDecisionService,
            IUiInvoker uiInvoker,
            IModLogger logger)
        {
            _councilProvider = councilProvider;
            _manager = manager;
            _warDecisionService = warDecisionService;
            _uiInvoker = uiInvoker;
            _logger = logger;
        }

        public async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            CurrentOperation = OperationState.Initializing;
            CouncilEvents.OnDecisionProposed += OnDecisionProposed_Background;
            CouncilEvents.OnDecisionProcessed += OnDecisionProcessed_Background;

            var councils = await _councilProvider.GetCouncilsAsync(cancellationToken);
            _uiInvoker.InvokeOnUi(() =>
            {
                _allCouncils.Clear();
                foreach (var council in councils)
                {
                    _allCouncils.Add(council);
                }
            });

            CurrentOperation = OperationState.None;
        }

        public async Task ProposeDecisionAsync(Guid councilId, string title, string description, string payload, CancellationToken cancellationToken = default)
        {
            CurrentOperation = OperationState.Proposing;
            try
            {
                await Task.Run(() =>
                {
                    var council = _manager.GetCouncilById(councilId);
                    if (council == null) return;
                    _warDecisionService.ProposeDecision(council, title, description, Hero.MainHero, payload);
                }, cancellationToken);
            }
            finally
            {
                CurrentOperation = OperationState.None;
            }
        }

        public async Task CastVoteAsync(Guid councilId, Guid decisionId, bool vote, CancellationToken cancellationToken = default)
        {
            CurrentOperation = OperationState.Voting;
            try
            {
                await Task.Run(() =>
                {
                    var council = _manager.GetCouncilById(councilId);
                    var decision = council?.Decisions.FirstOrDefault(d => new Guid(d.DecisionId) == decisionId);
                    if (decision == null) return;
                    _warDecisionService.RecordVote(decision, Hero.MainHero, vote);
                }, cancellationToken);
            }
            finally
            {
                CurrentOperation = OperationState.None;
            }
        }

        public async Task RequestTallyAndExecuteAsync(Guid councilId, Guid decisionId, CancellationToken cancellationToken = default)
        {
            CurrentOperation = OperationState.Tallying;
            try
            {
                await Task.Run(() =>
                {
                    var council = _manager.GetCouncilById(councilId);
                    var decision = council?.Decisions.FirstOrDefault(d => new Guid(d.DecisionId) == decisionId);
                    if (council == null || decision == null) return;
                    _warDecisionService.ProcessDecision(council, decision);
                }, cancellationToken);
            }
            finally
            {
                CurrentOperation = OperationState.None;
            }
        }

        private void OnDecisionProposed_Background(WarCouncil council, WarDecision decision)
        {
            _uiInvoker.InvokeOnUi(() =>
            {
                var councilDto = _allCouncils.FirstOrDefault(c => c.SaveId == new Guid(council.SaveId));
                if (councilDto != null)
                {
                    var decisionDto = new WarDecisionDto
                    {
                        DecisionGuid = new Guid(decision.DecisionId),
                        Title = decision.Title,
                        Description = decision.Description,
                        Status = decision.Status,
                        YeaCount = decision.GetYeaVotes(),
                        NayCount = decision.GetNayVotes()
                    };
                    councilDto.Decisions.Add(decisionDto);
                }
            });
        }

        private void OnDecisionProcessed_Background(WarCouncil council, WarDecision decision)
        {
            _uiInvoker.InvokeOnUi(() =>
            {
                var councilDto = _allCouncils.FirstOrDefault(c => c.SaveId == new Guid(council.SaveId));
                if (councilDto != null)
                {
                    var decisionDto = councilDto.Decisions.FirstOrDefault(d => d.DecisionGuid == new Guid(decision.DecisionId));
                    if (decisionDto != null)
                    {
                        if (decision.Status != "Proposed" && decision.Status != "VotingOpen")
                        {
                            councilDto.Decisions.Remove(decisionDto);
                        }
                        else
                        {
                            decisionDto.Status = decision.Status;
                        }
                    }
                }
            });
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            CouncilEvents.OnDecisionProposed -= OnDecisionProposed_Background;
            CouncilEvents.OnDecisionProcessed -= OnDecisionProcessed_Background;
        }
    }
}
