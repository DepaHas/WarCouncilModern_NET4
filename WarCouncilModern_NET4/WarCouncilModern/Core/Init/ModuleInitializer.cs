#nullable enable
using System;
using WarCouncilModern.Core.Manager;
using WarCouncilModern.Core.State;
using WarCouncilModern.Core.Services;
using WarCouncilModern.Models.Persistence;
using WarCouncilModern.Models.Settings;
using WarCouncilModern.Utilities.Interfaces;
using WarCouncilModern.Utilities.Logging;

namespace WarCouncilModern.Core.Init
{
    public class ModuleInitializer
    {
        private WarCouncilManager? _manager;
        private IFeatureRegistry? _featureRegistry;
        private IModLogger? _logger;
        private IModStateTracker? _stateTracker;
        private IPersistenceAdapter? _persistence;
        private ICouncilMeetingService? _meetingService;
        private IDecisionProcessingService? _decisionService;
        private IAdvisorService? _advisorService;
        private ModSettings? _settings;

        public ModuleInitializer()
        {
            // تهيئة افتراضية بسيطة لتجنّب أخطاء الحقول غير المهيأة
            _logger = new ModLogger("WarCouncil");
            _settings = ModSettings.CreateDefault();
        }

        public void Initialize(
            IFeatureRegistry featureRegistry,
            IModLogger logger,
            IModStateTracker stateTracker,
            IPersistenceAdapter persistence,
            ICouncilMeetingService meetingService,
            IDecisionProcessingService decisionService,
            IAdvisorService advisorService,
            ModSettings settings
        )
        {
            _featureRegistry = featureRegistry ?? throw new ArgumentNullException(nameof(featureRegistry));
            _logger = logger ?? _logger;
            _stateTracker = stateTracker ?? throw new ArgumentNullException(nameof(stateTracker));
            _persistence = persistence ?? throw new ArgumentNullException(nameof(persistence));
            _meeting_service_fix(meetingService);
            _decisionService = decisionService ?? throw new ArgumentNullException(nameof(decisionService));
            _advisor_service_fix(advisorService);
            _settings = settings ?? _settings ?? ModSettings.CreateDefault();

            // أنشئ الـ manager بالمرور positional لتجنّب named-argument mismatch
            _manager = new WarCouncilManager(
                _meetingService!,
                _decisionService!,
                _advisorService!,
                _settings,
                _logger!,
                _persistence!,
                _stateTracker!,
                _featureRegistry!
            );

            _logger?.Info("ModuleInitializer: WarCouncilManager created.");
        }

        private void _meeting_service_fix(ICouncilMeetingService? svc)
        {
            if (svc == null)
            {
                // ضع هنا stub أو throw حسب رغبتك؛ الآن نستخدم throw لتفادي التشغيل بغير خدمة مهمة
                throw new ArgumentNullException(nameof(svc), "MeetingService cannot be null");
            }
            _meetingService = svc;
        }

        private void _advisor_service_fix(IAdvisorService? svc)
        {
            if (svc == null)
            {
                // يمكنك استبدال الـ stub بأسلوبك الخاص
                _advisorService = new StubAdvisorService();
                _logger?.Warn("AdvisorService was null; using stub fallback.");
            }
            else
            {
                _advisorService = svc;
            }
        }

        public WarCouncilManager? Manager => _manager;

        public void Shutdown()
        {
            _logger?.Info("ModuleInitializer.Shutdown called.");
            _manager = null;
        }
    }
}