﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarCouncilModern.Core.Services;
using WarCouncilModern.Core.Settings;
using WarCouncilModern.Core.State;
using WarCouncilModern.Models;
using WarCouncilModern.Models.Entities;
using WarCouncilModern.Models.Persistence;
using WarCouncilModern.Utilities;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Core.Manager
{
    /// <summary>
    /// واجهة مدير المجالس لتسهيل الاختبار والحقن
    /// </summary>
    public interface IWarCouncilManager
    {
        IReadOnlyList<WarCouncil> Councils { get; }
        WarCouncil CreateCouncil(string kingdomId, string name = "Default Council", string leaderHeroId = "");
        WarCouncil GetCouncilById(Guid saveId);
        WarCouncil GetCouncilByFaction(string factionId);       
        bool RemoveCouncil(Guid saveId);
        bool AddMemberToCouncil(Guid councilId, WarHero hero);
        bool RemoveMemberFromCouncil(Guid councilId, Guid heroSaveId);
        WarDecision ProposeDecision(Guid councilId, string title, string description, Guid proposedBy);
        Task<bool> ProcessDecisionAsync(Guid councilId, Guid decisionId);
        event Action<WarCouncil, WarDecision> DecisionProposed;
        event Action<WarCouncil> CouncilCreated;    
        event Action<WarCouncil> CouncilRemoved;
    }

    /// <summary>
    /// مدير المجالس: نقطة التنسيق بين النماذج والخدمات.    
    /// يتعامل مع state tracking, feature flags, persistence adapter, و logging.
    /// </summary>
    public class WarCouncilManager : IWarCouncilManager
    {
        private readonly List<WarCouncil> _councils = new List<WarCouncil>();
        private readonly object _locker = new object();

        private readonly ICouncilMeetingService _meetingService;
        private readonly IDecisionProcessingService _decisionService;
        private readonly IAdvisorService _advisorService;
        private readonly IModSettings _settings;
        private readonly IModLogger _logger;
        private readonly IPersistenceAdapter _persistence;
        private readonly IModStateTracker _stateTracker;
        private readonly IFeatureRegistry _featureRegistry;

        public IReadOnlyList<WarCouncil> Councils
        {
            get
            {
                lock (_locker) return _councils.ToList().AsReadOnly();
            }
        }

        public event Action<WarCouncil, WarDecision> DecisionProposed;
        public event Action<WarCouncil> CouncilCreated;
        public event Action<WarCouncil> CouncilRemoved;

        /// <summary>
        /// Constructor with required dependencies injected.
        /// </summary>
        public WarCouncilManager(
            ICouncilMeetingService meetingService,
            IDecisionProcessingService decisionService,
            IAdvisorService advisorService,
            IModSettings settings,
            IModLogger logger,
            IPersistenceAdapter persistence,
            IModStateTracker stateTracker,
            IFeatureRegistry featureRegistry)
        {
            _meetingService = meetingService ?? throw new ArgumentNullException(nameof(meetingService));
            _decisionService = decisionService ?? throw new ArgumentNullException(nameof(decisionService));
            _advisorService = advisorService ?? throw new ArgumentNullException(nameof(advisorService));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _persistence = persistence ?? throw new ArgumentNullException(nameof(persistence));
            _stateTracker = stateTracker ?? throw new ArgumentNullException(nameof(stateTracker));
            _featureRegistry = featureRegistry ?? throw new ArgumentNullException(nameof(featureRegistry));
        }

        #region Council lifecycle

        /// <summary>
        /// Creates a new War Council for the specified faction.
        /// </summary>
        public WarCouncil CreateCouncil(string kingdomId, string name = "Default Council", string leaderHeroId = "")
        {
            if (string.IsNullOrWhiteSpace(kingdomId)) throw new ArgumentException("kingdomId is required", nameof(kingdomId));

            var council = new WarCouncil(kingdomId, CouncilStructure.Undefined)
            {
                Name = name
            };
            if (!string.IsNullOrEmpty(leaderHeroId))
            {
                council.AssignLeaderByHeroId(leaderHeroId);
            }
            lock (_locker) _councils.Add(council);

            _logger.Info($"[WarCouncilManager] Created council for kingdom '{kingdomId}'.");
            _stateTracker.RecordEvent("CouncilCreated", Guid.Empty, new { kingdomId });

            SafeInvoke(() =>
            {
                try { _advisorService.InitializeForCouncil(council); }
                catch (Exception ex) { _logger.Warn($"Advisor initialization failed: {ex.Message}"); }
            });

            CouncilCreated?.Invoke(council);
            return council;
        }

        /// <summary>
        /// Returns council by SaveId or null.
        /// </summary>
        public WarCouncil GetCouncilById(Guid id)
        {
            lock (_locker) return _councils.FirstOrDefault(c => c.SaveId == id.ToString());
        }

        /// <summary>
        /// Returns the first council matching the factionId or null.
        /// </summary>
        public WarCouncil GetCouncilByFaction(string kingdomId)
        {
            if (string.IsNullOrWhiteSpace(kingdomId)) return null;
            lock (_locker) return _councils.FirstOrDefault(c => c.KingdomStringId == kingdomId);
        }

        /// <summary>
        /// Removes council by SaveId.
        /// </summary>
        public bool RemoveCouncil(Guid id)
        {
            WarCouncil removed = null;
            lock (_locker)
            {
                var c = _councils.FirstOrDefault(x => x.SaveId == id.ToString());
                if (c == null) return false;
                _councils.Remove(c);
                removed = c;
            }

            if (removed != null)
            {
                _logger.Info($"[WarCouncilManager] Removed council for kingdom '{removed.KingdomStringId}'.");
                _stateTracker.RecordEvent("CouncilRemoved", Guid.Empty, new { removed.KingdomStringId });
                CouncilRemoved?.Invoke(removed);
            }

            return true;
        }

        /// <summary>
        /// Clears all councils (use carefully).
        /// </summary>
        public void ClearAll()
        {
            lock (_locker) _councils.Clear();
            _logger.Info("[WarCouncilManager] Cleared all councils.");
            _stateTracker.RecordEvent("AllCouncilsCleared", Guid.Empty, null);
        }

        #endregion

        #region Members management

        /// <summary>
        /// Adds a member to the specified council.
        /// </summary>
        public bool AddMemberToCouncil(Guid councilId, WarHero hero)
        {
            var c = GetCouncilById(councilId);
            if (c == null)
            {
                _logger.Warn($"AddMember failed: Council {councilId} not found.");
                return false;
            }

            var added = false;
            try
            {
                c.AddMemberById(hero.HeroStringId);
                added = true;
                _logger.Debug($"Added member '{hero.Name}' to council for kingdom '{c.KingdomStringId}'.");
                _stateTracker.RecordEvent("MemberAdded", Guid.Empty, new { hero.HeroStringId, hero.Name });
            }
            catch (Exception ex)
            {
                _logger.Error($"AddMemberToCouncil error: {ex}", ex);
            }

            return added;
        }

        /// <summary>
        /// Removes a member from the specified council.
        /// </summary>
        public bool RemoveMemberFromCouncil(Guid councilId, Guid heroId)
        {
            var c = GetCouncilById(councilId);
            if (c == null) return false;

            var removed = c.RemoveMemberById(heroId.ToString());
            if (removed) _stateTracker.RecordEvent("MemberRemoved", Guid.Empty, new { heroId });
            return removed;
        }

        #endregion

        #region Decisions

        /// <summary>
        /// Proposes a new decision in the specified council.
        /// </summary>
        public WarDecision ProposeDecision(Guid councilId, string title, string description, Guid proposedBy)
        {
            var c = GetCouncilById(councilId);
            if (c == null)
            {
                _logger.Warn($"ProposeDecision failed: Council {councilId} not found.");
                return null;
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                _logger.Warn("ProposeDecision failed: title empty.");
                return null;
            }

            // استخدم المُشيد الصحيح حسب التواقيع
            var decision = new WarDecision(Guid.NewGuid().ToString(), title, proposedBy.ToString(), "Proposed");
            decision.Description = description ?? string.Empty;
            c.AddDecision(decision);

            _logger.Info($"Decision proposed in council for kingdom '{c.KingdomStringId}': {title} ({decision.DecisionId}).");
            _stateTracker.RecordEvent("DecisionProposed", Guid.Empty, new { c.KingdomStringId, title });

            SafeInvoke(() =>
            {
                try { _advisorService.OnDecisionProposed(c, decision); }
                catch (Exception ex) { _logger.Warn($"AdvisorService.OnDecisionProposed threw: {ex.Message}"); }
            });

            try
            {
                var autoSchedule = _featureRegistry.IsEnabled("AutoScheduleMeetingOnProposal");
                if (autoSchedule)
                {
                    _meetingService.ScheduleMeetingForDecision(c, decision);
                    _logger.Debug("Meeting scheduled automatically for proposed decision.");
                }
            }
            catch (Exception ex)
            {
                _logger.Warn($"Meeting scheduling failed: {ex.Message}");
            }

            DecisionProposed?.Invoke(c, decision);

            var autoProcess = _featureRegistry.IsEnabled("AutoDecisionProcessing");
            if (autoProcess)
            {
                // run async fire-and-forget but capture errors
                _ = ProcessDecisionAsync(councilId, new Guid(decision.DecisionId));
            }

            return decision;
        }

        /// <summary>
        /// Processes a decision asynchronously using the decision service.
        /// </summary>
        public async Task<bool> ProcessDecisionAsync(Guid councilId, Guid decisionId)
        {
            var c = GetCouncilById(councilId);
            if (c == null) return false;

            var d = c.Decisions.FirstOrDefault(x => x.DecisionId == decisionId.ToString());
            if (d == null) return false;

            try
            {
                _logger.Info($"Processing decision {d.DecisionId} for council for kingdom {c.KingdomStringId}.");
                var result = await _decisionService.ProcessDecisionAsync(c, d).ConfigureAwait(false);

                d.Description = result ? "Executed" : "Failed";
                _logger.Info($"Decision {d.DecisionId} processing result: {d.Description}.");
                _stateTracker.RecordEvent("DecisionProcessed", Guid.Empty, new { c.KingdomStringId, d.Description });
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"Decision processing error: {ex}", ex);
                d.Description = "Error";
                _stateTracker.RecordEvent("DecisionProcessError", Guid.Empty, new { ex.Message });
                return false;
            }
        }

        #endregion

        #region Persistence helpers

        /// <summary>
        /// Exports all councils via the persistence adapter.
        /// </summary>
        public void ExportAllForSave()
        {
            try
            {
                lock (_locker) _persistence.ExportCouncils(_councils);
                _logger.Info("[WarCouncilManager] Exported councils for save.");
                _stateTracker.RecordEvent("ExportedForSave", Guid.Empty, new { Count = _councils.Count });
            }
            catch (Exception ex)
            {
                _logger.Error($"ExportAllForSave failed: {ex}", ex);
            }
        }

        /// <summary>
        /// Imports councils from persistence adapter, replacing in-memory state.
        /// </summary>
        public void ImportFromSave()
        {
            try
            {
                var imported = _persistence.ImportCouncils()?.ToList() ?? new List<WarCouncil>();
                lock (_locker)
                {
                    _councils.Clear();
                    _councils.AddRange(imported);
                }

                _logger.Info($"Imported {_councils.Count} councils from save.");
                _stateTracker.RecordEvent("ImportedFromSave", Guid.Empty, new { Count = _councils.Count });
            }
            catch (Exception ex)
            {
                _logger.Error($"ImportFromSave failed: {ex}", ex);
            }
        }

        #endregion

        #region Utilities

        private void SafeInvoke(Action action)
        {
            try { action(); }
            catch (Exception ex) { _logger.Warn($"SafeInvoke caught: {ex.Message}"); }
        }

        #endregion
    }
}