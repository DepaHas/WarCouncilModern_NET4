# Architecture Overview

Layers:
- Core: Settings, registry, compatibility.
- Initialization: SubModule and init helpers.
- Behaviors: CampaignBehaviorBase implementations.
- Save: Saveable entities and migration.
- UI: Screens and registry.
- Logging: ModLogger and formats.

Register flow:
SubModule -> FeatureRegistry -> Add Behaviors / UI / SaveDefiners