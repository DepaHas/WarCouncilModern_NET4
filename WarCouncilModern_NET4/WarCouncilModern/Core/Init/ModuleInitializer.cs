#nullable enable
using System;
using WarCouncilModern.Core.Manager;
using WarCouncilModern.Core.State;
using WarCouncilModern.Core.Services;
using WarCouncilModern.Core.Settings;
using WarCouncilModern.CouncilSystem.Behaviors;
using WarCouncilModern.Utilities;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Core.Init
{
    public class ModuleInitializer
    {
        private WarCouncilManager? _manager;
        private IFeatureRegistry? _featureRegistry;
        private IModLogger _logger;
        private IModStateTracker? _stateTracker;
        private WarCouncilCampaignBehavior? _behavior;
        private ICouncilMeetingService? _meetingService;
        private IDecisionProcessingService? _decisionService;
        private IAdvisorService? _advisorService;
        private IModSettings _settings;

        public ModuleInitializer()
        {
            _logger = GlobalLog.Instance;
            _settings = new StubModSettings();
        }

        public void Initialize(
            WarCouncilCampaignBehavior behavior,
            IFeatureRegistry featureRegistry,
            IModLogger? logger,
            IModStateTracker stateTracker,
            ICouncilMeetingService meetingService,
            IDecisionProcessingService decisionService,
            IAdvisorService? advisorService,
            IModSettings? settings
        )
        {
            _behavior = behavior ?? throw new ArgumentNullException(nameof(behavior));
            _featureRegistry = featureRegistry ?? throw new ArgumentNullException(nameof(featureRegistry));
            _logger = logger ?? _logger;
            _stateTracker = stateTracker ?? throw new ArgumentNullException(nameof(stateTracker));
            _meetingService = meetingService ?? throw new ArgumentNullException(nameof(meetingService));
            _decisionService = decisionService ?? throw new ArgumentNullException(nameof(decisionService));
            _advisor_service_fix(advisorService);
            _settings = settings ?? _settings;

            CreateManagerSafely();
        }

        private void _advisor_service_fix(IAdvisorService? svc)
        {
            if (svc == null)
            {
                _advisorService = new StubAdvisorService(_logger);
            }
            else
            {
                _advisorService = svc;
            }
        }

        private void CreateManagerSafely()
        {
            try
            {
                _manager = new WarCouncilManager(
                    _behavior!,
                    _meetingService!,
                    _decisionService!,
                    _advisorService!,
                    _settings,
                    _logger,
                    _stateTracker!,
                    _featureRegistry!
                );
            }
            catch (Exception ex)
            {
                _logger.Error("Error creating WarCouncilManager", ex);
                throw;
            }
        }

        public WarCouncilManager? Manager => _manager;

        public void Shutdown()
        {
            _manager = null;
        }
    }
}
