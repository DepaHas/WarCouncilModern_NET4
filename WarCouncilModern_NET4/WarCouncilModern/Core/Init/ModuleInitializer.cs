#nullable enable
using System;
using WarCouncilModern.Core.Manager;
using WarCouncilModern.Core.State;
using WarCouncilModern.Core.Services;
using WarCouncilModern.Models.Persistence;
using WarCouncilModern.Core.Settings;
using WarCouncilModern.Utilities;
using WarCouncilModern.Utilities.Interfaces;

namespace WarCouncilModern.Core.Init
{
    /// <summary>
    /// مسؤول تهيئة اعتمادات المود وإنشاء WarCouncilManager بشكل آمن.
    /// لاحظ: استدعاء new WarCouncilManager(...) يمرِّر الوسائط بالترتيب positional.
    /// إن اختلف توقيع باني WarCouncilManager لديك، أرسل header الباني لأعيد كتابة الاستدعاء بدقة.
    /// </summary>
    public class ModuleInitializer
    {
        private WarCouncilManager? _manager;
        private IFeatureRegistry? _featureRegistry;
        private IModLogger _logger;
        private IModStateTracker? _stateTracker;
        private IPersistenceAdapter? _persistence;
        private ICouncilMeetingService? _meetingService;
        private IDecisionProcessingService? _decisionService;
        private IAdvisorService? _advisorService;
        private IModSettings _settings;

        public ModuleInitializer()
        {
            // قيم افتراضية لتجنّب CS8618 وللسقوف الآمنة أثناء التطوير
            _logger = GlobalLog.Instance;
            _settings = new StubModSettings();
        }

        /// <summary>
        /// تهيئة الاعتمادات المطلوبة للمود.
        /// استخدم هذا الأسلوب من SubModule أو من مكان تهيئة أعلى.
        /// </summary>
        public void Initialize(
            IFeatureRegistry featureRegistry,
            IModLogger? logger,
            IModStateTracker stateTracker,
            IPersistenceAdapter persistence,
            ICouncilMeetingService meetingService,
            IDecisionProcessingService decisionService,
            IAdvisorService? advisorService,
            IModSettings? settings
        )
        {
            _featureRegistry = featureRegistry ?? throw new ArgumentNullException(nameof(featureRegistry));
            _logger = logger ?? _logger;
            _stateTracker = stateTracker ?? throw new ArgumentNullException(nameof(stateTracker));
            _persistence = persistence ?? throw new ArgumentNullException(nameof(persistence));
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
                _logger.Warn("AdvisorService was null; using StubAdvisorService as fallback.");
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
                // مرّر positional لتجنّب مشكلة named-argument mismatch
                _manager = new WarCouncilManager(
                    _meetingService!,
                    _decisionService!,
                    _advisorService!,
                    _settings,
                    _logger,
                    _persistence!,
                    _stateTracker!,
                    _featureRegistry!
                );

                _logger.Info("WarCouncilManager instantiated successfully.");
            }
            catch (MissingMethodException mex)
            {
                _logger.Error("Constructor mismatch when creating WarCouncilManager", mex);
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error("Unexpected error creating WarCouncilManager", ex);
                throw;
            }
        }

        public WarCouncilManager? Manager => _manager;

        public void Shutdown()
        {
            try
            {
                _logger.Info("ModuleInitializer.Shutdown called.");
                _manager = null;
            }
            catch (Exception ex)
            {
                _logger.Error("Error during ModuleInitializer.Shutdown", ex);
            }
        }
    }
}