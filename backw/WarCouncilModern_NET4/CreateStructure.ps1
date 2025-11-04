# ==============================================================================
# PowerShell Script: Project Structure Creator (Final Version with ${} Fix)
# ==============================================================================

# 1. تعيين المسار الرئيسي (Root Path)
$Root = "WarCouncilModern"

# 2. تعريف هيكل الشجرة (The Structure)
$Structure = @(
    "Core/"
    "Core/Init/"
    "Core/Init/ModuleInitializer.cs"
    "Core/Init/StartupGuards.cs"
    "Core/Manager/"
    "Core/Manager/WarCouncilManager.cs"
    "Core/Manager/BehaviorRegistry.cs"
    "Core/Manager/CampaignContext.cs"
    "Core/Services/"
    "Core/Services/CouncilMeetingService.cs"
    "Core/Services/AdvisorService.cs"
    "Core/Services/DecisionProcessingService.cs"
    "Core/Settings/"
    "Core/Settings/ModSettings.cs"
    "Core/State/"
    "Core/State/ModStateTracker.cs"
    "Core/Utilities/"
    "Core/Utilities/FeatureRegistry.cs"
    
    "Models/"
    "Models/Entities/"
    "Models/Entities/WarHero.cs"
    "Models/Entities/WarCamp.cs"
    "Models/Entities/WarReport.cs"
    "Models/Entities/WarDecision.cs"
    "Models/Entities/WarCouncil.cs"
    "Models/DataTransfer/"
    "Models/DataTransfer/WarCouncilDto.cs"
    "Models/DataTransfer/WarReportDto.cs"
    "Models/Persistence/"
    "Models/Persistence/SaveableEtClass1.cs"
    "Models/Persistence/WarCouncilSaveProfile.cs"
    
    "CouncilSystem/"
    "CouncilSystem/Behaviors/"
    "CouncilSystem/Behaviors/WarCouncilTestBehavior.cs"
    "CouncilSystem/Behaviors/CouncilEventBehavior.cs"
    "CouncilSystem/Behaviors/CouncilDecisionBehavior.cs"
    "CouncilSystem/Behaviors/CouncilDiplomacyBehavior.cs"
    "CouncilSystem/Events/"
    "CouncilSystem/Events/CouncilEventBus.cs"
    "CouncilSystem/Events/CouncilEventArgs.cs"
    "CouncilSystem/Commands/"
    "CouncilSystem/Commands/CouncilCommandHandler.cs"
    "CouncilSystem/Commands/DebugConsoleCommands.cs"
    "CouncilSystem/Strategies/"
    "CouncilSystem/Strategies/DecisionStrategy.cs"
    "CouncilSystem/Strategies/WarPlanStrategy.cs"
    "CouncilSystem/Strategies/DiplomacyStrategy.cs"
    
    "Save/"
    "Save/WarCouncilSaveDefiner.cs"
    "Save/SaveVersionManager.cs"
    
    "UI/"
    "UI/Screens/"
    "UI/Screens/CouncilOverviewScreen.cs"
    "UI/Screens/DecisionDetailScreen.cs"
    "UI/Dialogues/"
    "UI/Dialogues/CouncilDialogueFlow.cs"
    "UI/ViewModels/"
    "UI/ViewModels/CouncilOverviewVM.cs"
    "UI/ViewModels/DecisionViewModel.cs"
    "UI/Components/"
    "UI/Components/UIHelpers.cs"
    
    "Utilities/"
    "Utilities/Logging/"
    "Utilities/Logging/ModLogger.cs"
    "Utilities/Logging/LogFormatter.cs"
    "Utilities/Reflection/"
    "Utilities/Reflection/ReflectionHelpers.cs"
    
    "Docs/"
    "Docs/README.md"
    "Docs/CHANGELOG.md"
    "Docs/ARCHITECTURE.md"
    "Docs/TEAM_GUIDE.md"
    "Docs/TEST_PLAN.md"
    
    "Script/"
    "Script/PostBuildCopy.ps1"
    "Script/DeployDebugLog.ps1"
)

# 3. حلقة إنشاء الشجرة
Write-Host "Starting structure creation..." -ForegroundColor Cyan

foreach ($Item in $Structure) {
    # دمج المسار الرئيسي مع مسار العنصر
    $FullPath = Join-Path -Path $Root -ChildPath $Item
    
    # 3.1. التحقق من وجود العنصر بالفعل
    if (Test-Path $FullPath) {
        Write-Host "  [SKIP] ${FullPath} (Already Exists)" -ForegroundColor Yellow # تم الإصلاح
        continue
    }

    # 3.2. تحديد نوع العنصر وإنشائه
    if ($Item.EndsWith("/")) {
        $Type = "Directory"
        $PathToCreate = $FullPath
    } else {
        $Type = "File"
        $ParentDir = Split-Path -Parent $FullPath
        if (-not (Test-Path $ParentDir)) {
            New-Item -Path $ParentDir -ItemType Directory -Force | Out-Null
        }
        $PathToCreate = $FullPath
    }
    
    # تم الإصلاح باستخدام ${} لضمان عمل المتغيرات بشكل صحيح
    Write-Host "  [CREATE] ${Type}: ${PathToCreate}" -ForegroundColor Green 
    New-Item -Path $PathToCreate -ItemType $Type -Force | Out-Null
}

Write-Host "`n=========================================="
Write-Host "Structure creation complete in: ${Root}" -ForegroundColor Cyan
Write-Host "=========================================="