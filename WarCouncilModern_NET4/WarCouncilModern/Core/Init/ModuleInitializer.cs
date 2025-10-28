using System;
using WarCouncilModern.Core.Manager;
using WarCouncilModern.Core.Services;
using WarCouncilModern.Core.Settings;
using WarCouncilModern.Core.State;
using WarCouncilModern.Models.Persistence;
using WarCouncilModern.Utilities;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Core.Init
{
    // Simplified initializer for development and wiring dependencies.
    public static class ModuleInitializer
    {
        private static WarCouncilManager? _manager;
        private static FeatureRegistry? _featureRegistry;
        private static ModLogger? _logger;
        private static ModStateTracker? _stateTracker;
        private static CouncilPersistenceAdapter? _persistence;
        private static StubCouncilMeetingService? _meetingService;
        private static StubDecisionProcessingService? _decisionService;
        private static StubAdvisorService? _advisorService;
        private static StubModSettings? _settings;

        public static IWarCouncilManager InitializeForDevelopment()
        {
            // Utilities
            _featureRegistry = new FeatureRegistry();
            _logger = new ModLogger();
            _stateTracker = new ModStateTracker(_logger);

            // Persistence (in-memory stub)
            _persistence = new CouncilPersistenceAdapter();

            // Settings and services (stubs)
            _settings = new StubModSettings();
            _meetingService = new StubCouncilMeetingService(_logger);
            _decisionService = new StubDecisionProcessingService(_logger);
            _advisorService = new StubAdvisorService(_logger);

            // Manager
            _manager = new WarCouncilManager(
                _meetingService,
                _decision_service: _decisionService,
                _advisorService,
                _settings,
                _logger,
                _persistence,
                _stateTracker,
                _featureRegistry
            );

            _logger.Info("[ModuleInitializer] WarCouncilManager initialized (development).");
            return _manager;
        }
    }
}