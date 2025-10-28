using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace WarCouncilModern
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad()
        {
            ModLogger.Init();
            ModLogger.Info("OnSubModuleLoad start");
            try
            {
                // علّق تسجيلات تعريفات ونماذج الحفظ الثقيلة أثناء التطوير المبكر
                // RegisterDefinitions(); // تعليق مؤقت إن كان موجوداً
            }
            catch (Exception ex)
            {
                ModLogger.Error("OnSubModuleLoad exception", ex);
            }
            ModLogger.Info("OnSubModuleLoad end");
            base.OnSubModuleLoad();
        }

        protected override void OnGameStart(Game game, IGameStarter starterObject)
        {
            ModLogger.Info("OnGameStart start");
            try
            {
                // سجل Assemblies المحمّلة لمساعدة التشخيص
                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                    ModLogger.Info($"Assembly Loaded: {a.FullName}");

                if (Campaign.Current == null)
                {
                    ModLogger.Warn("Campaign.Current is null — skipping campaign init");
                    return;
                }

                // إضافة سلوك اختبار بسيط بأمان
                SafeAddBehavior(starterObject, typeof(WarCouncilTestBehavior));

                // لاحقاً: قم بإضافة سلوكك الحقيقي تدريجياً واحرص على تغطيته بـ try/catch
                // SafeAddBehavior(starterObject, typeof(WarCouncilBehavior));
            }
            catch (Exception ex)
            {
                ModLogger.Error("OnGameStart exception", ex);
            }
            ModLogger.Info("OnGameStart end");
            base.OnGameStart(game, starterObject);
        }

        private void SafeAddBehavior(IGameStarter starter, Type behaviorType)
        {
            try
            {
                if (starter == null) { ModLogger.Warn("starter is null in SafeAddBehavior"); return; }

                var ctor = behaviorType.GetConstructor(Type.EmptyTypes);
                if (ctor == null) { ModLogger.Warn($"No parameterless ctor for {behaviorType.FullName}"); return; }
                var behavior = (CampaignBehaviorBase)ctor.Invoke(null);

                // حاول التحويل إلى CampaignGameStarter أولاً
                if (starter is CampaignGameStarter campaignStarter)
                {
                    campaignStarter.AddBehavior(behavior);
                    ModLogger.Info($"Added behavior {behaviorType.FullName} to CampaignGameStarter");
                    return;
                }

                // بعض إصدارات الـ API قد تستخدم GameStarterBase أو أنواع أخرى. حاول البحث عن طريقة AddBehavior عبر reflection كخطة احتياطية:
                var addMethod = starter.GetType().GetMethod("AddBehavior");
                if (addMethod != null)
                {
                    addMethod.Invoke(starter, new object[] { behavior });
                    ModLogger.Info($"Added behavior {behaviorType.FullName} via reflection to {starter.GetType().FullName}");
                    return;
                }

                ModLogger.Warn($"No AddBehavior found on starter type {starter.GetType().FullName}; behavior not added.");
            }
            catch (Exception ex)
            {
                ModLogger.Error($"Failed to add behavior {behaviorType.FullName}", ex);
            }
        }
    }
}