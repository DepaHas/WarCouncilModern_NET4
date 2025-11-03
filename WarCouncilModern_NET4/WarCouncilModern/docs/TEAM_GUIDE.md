Beta v1.3.1 (25/09/25)​
Released the XML Editor. This is a program that allows you to:
Edit your module’s XML files with schema-aware validation that prevents invalid changes.
Safely modify official and other module data via XSLT, generating non-destructive transformation rules.
Accomplish the above two steps through UI rather than text editors.
You can access the XML Editor through the Mount & Blade II: Bannerlord Modding Kit on Steam. After installing the Kit, you’ll find the XML Editor inside the XMLEditor folder in the game’s root directory.
Check the new Getting Started with XML Editor modding documentation page for all the details.
Released the Beta Modding Kit, which is now accessible via the Steam Betas system.
Beta v1.3.0 Changes​
Greetings!

Beta v1.3.0 has been released today. Here are the relevant modding changes.
Added audio modding support for FMOD Banks and Events.
Engine Changes
Added a functionality that makes interpolated atmosphere moddable. Modders can override any interpolated atmosphere used on the world map and in missions by placing their files in Module\Atmospheres\Interpolated, which will override the base game version. Important: missions fetch only a slice of the selected atmosphere set (not fully dynamic). For example, Aserai towns and villages use semicloudy_aserai.xml.
This also fixes a bug reported by hunharibo where assigning a custom culture to a town and “Taking a walk through that town” caused a crash.
Added save/load compatibility for campaign map changes.
Party positions, map event & siege positions, and party AI targets now adjust appropriately on load if a campaign map change is detected.
Fixed a crash that occurred when loading saves created on a different version of a (modded) campaign map; such saves now load without crashing on the default campaign map.
Distance Cache data now loads from all active modules (static map info such as settlement data, neighbours, and land ratios used by mobile party AI). This helps with overall campaign map performance.
Modders can create a navigation cache for their custom map through the SettlementPositionScript component in the editor - by clicking on the “ComputeAndSaveSettlementDistanceCache” button. The file gets generated under ModuleData\DistanceCaches and is named “settlements_distance_cache_Default.bin”.
Added automatic cache rebuilding: if a cache is missing or invalid, the game now recreates it on first launch.
Added a way for modders to mark valid and invalid terrain nodes on custom maps by overriding the PartyNavigationModel.
Added the ability to store up to 10 RGL logs.
In response to the community request by @poheniks.
Mission Changes
Added a new weapon flag that allows attacking while crouched - hurray for mods with guns!
In response to the community request by @Lucon.
Added a new capability for modders to alter AI behavior via the agent component system. To use it, create a class that inherits AgentComponent, override OnAIInputSet (AI inputs are passed by reference and invoked each tick after the AI makes its decision), then add the component to the agent.
In response to the community request on the Modding Discord by Kemo III.
Agents can now continue running to their target (instead of slowing to a walk when close) if the NeverSlowDown flag is set.
In response to the community request by @Gotha.
Added new parameters to weapon selection in the AgentDrivenProperties class to support modding. These allow modders to adjust troop weapon choice behavior by influencing whether AI troops prefer melee, ranged, or polearm weapons in specific situations:
AiWeaponFavorMultiplierMelee
AiWeaponFavorMultiplierRanged
AiWeaponFavorMultiplierPolearm
In response to the community request by @order_without_power.
Provided more control over StaticBodyProperties for modders.
In response to the community request by RandyOglestein.
Added support for handling entity callbacks of non-scripted entities via MeleeHitCallback.
Fixed a bug that caused an exception when a weapon was removed from an agent upon impact.
In response to the community request by @Kommissar here.
Campaign Changes
Added a new general XML merging algorithm. Modules can now add new elements to the XMLs or change attributes during merge using unique attributes and XSD-driven metadata.
Added missing XSD files (e.g., soln_skins.xsd).
Documented unique attributes for each element in existing XSD files and formatted them.
Modding documentation covering this new XML merging system will be released at a later point.
Introduced a new approach for creating classes that inherit GameModel, enforcing use of the base model rather than inheriting base-model implementations, without breaking mod support.
Added a new capability for MBObjectManager to create objects from an XML node and type name.
Made CampaignTime variables such as CampaignStartTime moddable through CampaignTimeModel. Previously hardcoded, CampaignTime is now exposed as a model with every parameter accessible, allowing modders to override values such as the number and length of days and weeks.
In response to the community request by @MitchPTI here.
Improved FactionManager moddability by refactoring the class, removing bandit faction checks, and moving related logic to DiplomacyModel.
Added three overridable methods in DiplomacyModel: GetShallowDiplomaticStance, GetDefaultDiplomaticStance, and IsAtConstantWar.
Added IFaction.FactionsAtWarWith for easier access to enemy factions.
In response to the community request by @Spinozart1 here.
Added a new ability to set the starting level of buildings in a settlement via settlements.xml. Since all building types must be added to a town or castle, any building without a specified level in settlements.xml is automatically added with its starting level.
Added a new DefaultHeroAgentLocationModel to manage hero locations at settlements.
Added a new GetPartyStrength function that accepts parameters to compute a party’s “virtual” strength under specified conditions (e.g., calculating its land-attacker strength while the party is currently flying).
Increased the maximum hair type limit from 32 to 64.
In response to the community request by @conleyc86 here.
Changed MobileParty.RemoveParty() from public to internal to prevent misuse that could lead to crashes for modders.
In response to the community request by Carter Drake on the Modding Discord.
Added automatic registration of texts defined in ModuleData/global_strings.xml to the global text manager. Existing GameText variations can now be overridden.
In response to the community request by Vombora on the Modding Discord.
Added a new VillageTradeModel to make TradeBoundDistanceLimit modifiable.
In response to the community request by @Alexander Drakos, @svelok, @ClayBullet, and @NPC99 here.
Added a new ShouldStayInKingdomUntil parameter to control when mercenaries and vassals can leave a kingdom.
Added a new ability to add a crafting piece to a crafting template via XML.
Added new callbacks in CraftedDataView — OnWeaponMeshBuilt, OnHolsterMeshBuilt, and OnHolsterMeshWithWeaponBuilt — allowing modders to modify crafted weapon meshes after they are built.
In response to the community request by @fedeita here.
Added an option to override Tier levels of uncraftable items (e.g., quivers, bolts, bows, crossbows, armor pieces, shields) via their dedicated XMLs.
Made managed_core_parameters moddable.
In response to the community request by @KingKilo here.
Made the TradeBound setter public.
In response to the community request by @Midnightknight here.
Enabled character skills to have multiple character attributes.
Added a new IsSettlementBusy event to settlements to indicate when they are used by issues or quests.
Added a new CustomSettlementComponent for settlements.
Added a couple of functions to PartySizeLimitModel to support clan tier effects.
Added a new GetModel function to CampaignGameStarter to ease model creation and moddability.
Moved the equipment ID used for brides in the marriage scene notification to culture XMLs.
Moved SetCustomName from MobileParty to PartyBase, enabling settlements to have custom names.
Replaced the CalculateHighCourageProbability function with GetSurrenderChance for surrender and bribe calculations of villager, caravan, and bandit parties.
Replaced character creation occupations with objects instead of enums, making the process of defining occupations easier.
Updated MapScreen.cs to work with IInteractablePoint instead of PartyBase.
Updated kingdom IsBanditFaction and IsOutlaw decisions to derive from the ruling clan’s IsBanditFaction and IsOutlaw checks.
Renamed IMapEntity to IInteractablePoint.
Split the Clan.Lords property into AliveLords and DeadLords.
Refactored mobile party creation functions in clan-related code paths.
Refactored the character creation base system.
Refactored the HeroCreator class and updated function names for clarity; moved default assumptions to HeroCreatorModel.
Refactored party creation and renamed custom party component creation functions.
Refactored the building effect system to use increment types (Add / AddFactor) and updated all effect value calculations to use ExplainedNumber for consistent results.
Removed companion templates from the culture XSD.
Removed the hardcoded CultureCode enum for cultures. CultureCode is no longer required to add a new culture.
Removed the RetirementSettlementComponent.
Removed TextObject.Empty, and replaced uses with IsEmpty() and a new TextObject.GetEmpty().
Removed traits affecting the skills of the characters, and assigned skill templates to characters instead of removed traits.
Fixed a bug that caused troop XP variables to reset in various cases by refactoring the TroopRoster class algorithms.
Fixed a bug that prevented defected kingdoms from being shown in the OnClanChangedKingdom event.
Fixed a bug that prevented enums with the Flags attribute from loading correctly.
Fixed a crash that occurred when a weightless equippable item was added.
Modding Toolkit
Added three new prefabs: crows, seagulls, and a lavender flower.
Added extensive boundary checks to the scene editor.
Fixed a bug that prevented changing an entity’s scale after setting it to 0 in the modding toolkit.
Fixed a bug that prevented adding a vertex to an edge in the navmesh in the Scene Editor.
Multiplayer Modding
Added a new IsGameModeAllowChargeDamageOnFriendly on MissionMultiplayerGameModeBase for modding, allowing team bump damage for horses.
In response to the community request by @takeoshigeru.
Added a new OffhandWeaponDefendSpeedMultiplier (shield defend speed) to AgentDrivenProperties.
In response to the community request by @takeoshigeru.
Added a new CalculateRemainingMomentum in AgentApplyDamageModel for modding.
In response to the community request by @takeoshigeru.
Made GetScoreForKill in MissionMultiplayerGameModeBase moddable.
In response to the community request by @takeoshigeru here.
Changed the WarmupTimeLimit server option to WarmupTimeLimitInSeconds to allow more precise values.
In response to the community request by @takeoshigeru here.
Made DecideWeaponCollisionReaction moddable, allowing behaviors where weapons don’t get stuck on a player and can, for example, cleave.
In response to the community request by @Gotha.
Increased the maximum friendly-fire server option limit from 100% to 2000%.
In response to the community request by Tark on the Modding Discord.
Fixed a bug that caused GetDefendCollisionResults to bypass StrikeMagnitudeCalculationModel, affecting moddability.
In response to the community request by @takeoshigeru here.

Thank you again for all your feedback and suggestions. If you have any questions or would like to make further requests, please discuss them below (or HERE).

v1.2.1-v1.2.7 Changes​
Greetings!

v1.2.x version series has been released to Live today. The hotfixes made during the Beta involved many modding-related changes so here's an overview of them:

v1.2.7​
Removed the readonly parameter from the Multiplayer Compression Info classes.
In response to the community request by Byako, @CaptainFracas, @Maroon, @mentalrob

Beta v1.2.5 (23/11/23)​
Fixed a crash that occurred on the resource browser.
World Map Moddability
Removed the hardcoded texture (main_map_snow_flowmap) previously required for Dynamic Campaign (weather) Effects (located in the Terrain Property Browser) on the world map. It is now selected from the editor as a regular texture. Modders now no longer have to use the asset override mechanism (by having the same flowmap texture name as the native flowmap texture) to add a snow layer to the world map - instead, it's recommended that you use your own texture with a different name and select it in your custom world map scene.
Fixed a bug that prevented modded campaign maps from using high-resolution heightmaps. A low-resolution main_map_physics_heightmap texture is not required for campaign heightmaps and custom worldmaps can now be used without the Aurelian’s MapFix mod if saved as Main_map (as opposed to modded_main_map).
Fixed a bug that prevented world map floras from appearing. Previously, you had to enter and exit a mission scene to force the flora to appear.
Added modding support for the world map dynamic atmosphere.
Added modding support for color grade grid for the world map.
Added modding support for the battle scene index grid for the world map.
Added visualization sliders to the editor for easier tweaking of the world map snow map.
Added a console option to reduce framerate when the editor is not focused.
Improved layer editing and visualization in the Editor.
Overall bug fixes.
In response to reports from various modders (@Cozur, @hunharibo, @Lemmy1916, @Le_Profyteur and others). Massive thanks to @NPC99 for his continuous detailed feedback on world map modding.
Assertions will now be shown in the Editor Console once inside the "Editor Mode".
In response to the community request by Poheniks and FoozleMcDoozle
Added bulk shader compilation for mods. It's accessible via the “Compile Shaders” checkbox while publishing the module using the “Publish Module” functionality in the editor. It allows you to compile & ship the unique material shaders in your modules. This prevents long initial mod loading times.
Added a new modding documentation page to cover the new shader compilation.
In response to the community request by @皮w蛋, @Le_Profyteur, Irish, @Dawa, SomewhereTropical and others on Modding Discord and here: https://forums.taleworlds.com/index...es-just-like-shaders-in-native-module.457489/
Added the ability to define and use your own decals.
Added a new modding documentation page to cover the new ability to define and use your own decals.
Fixed a bug that prevented texture move operations.
Added the ability to set tickrate for dedicated custom servers using the "/tickrate" command line argument or the "set_server_tickrate" console command.
In response to the community request by @Gotha

Beta v1.2.4 (28/09/23)​
Fixed a crash that occurred in the Cloth Editor if the cloth simulation wasn’t enabled in the config.
Fixed a crash that occurred if a mod changed the maximum number of workshops that a player could buy through the DefaultWorkshopModel.
In response to the community report by ing32
Removed the ConsiderMapEventsAndSiegesInternal function and created the PartyDiplomaticHandlerCampaignBehavior class. The effects of diplomatic changes can now be controlled via the latter class.
Fixed several issues related to the moddability of reloading ranged weapons.
In response to the community request by @DarthKiller and @Lucon
Allowed the defining of class divisions with no perks.
In response to the community report by @takeoshigeru here: https://forums.taleworlds.com/index.php?threads/allow-defining-class-divisions-with-no-perks.459551/

Beta v1.2.3 (04/08/23)​
Fixed a crash that occurred on modded scenes due to a limit on the number of entities used.
In response to the community report by @Tark here: https://forums.taleworlds.com/index...rt-on-custom-map-stable-version-1-1-5.459517/

Beta v1.2.2 (21/07/23)​
Fixed a crash that occurred when entering a town of a modded custom culture.


We continue to work on other modding-related features/issues:
Making campaign time moddable
In response to the community request by @MitchPTI here: https://forums.taleworlds.com/index.php?threads/please-make-campaign-time-customisable.444719/
Add a server-side option for enabling team bump damage for horses
In response to the community request by @Gotha
Add a model for shield speed
In response to the community request by @takeoshigeru
Add a weapon flag that enables attacking while crouched
In response to the community request by @Lucon, @Terco_Viejo and others here: https://forums.taleworlds.com/index...ile-crouching-available-in-bannerlord.453706/
Add the ability for modders to manipulate the behavior of the melee attack after a hit
In response to the community request by Namidaka
Move trade bound selection in TryToAssignTradeBoundForVillage to a model
In response to the community request by @Alexander Drakos and @svelok here: https://forums.taleworlds.com/index...nce-limit-is-breaking-custom-map-mods.453406/
Make StaticBodyProperties setter public and add a parameter for staticbodyproperty to CreateBasicHero
In response to the community request by RandyOglestein on the Modding Discord
Making relationships between factions more moddable
In response to the community request by @Spinozart1 here: https://forums.taleworlds.com/index...check-condition-for-peace-declaration.456837/
Add functionality that allows multiple RGL logs to be stored
In response to the community request by Poheniks
WarmupTimeLimit in seconds
In response to the community request by @takeoshigeru here: https://forums.taleworlds.com/index.php?threads/warmuptimelimit-in-seconds.457855/
Score for a kill should depend on the game mode so could you make it virtual so it can be overridden.
In response to the community request by @takeoshigeru here: https://forums.taleworlds.com/index...ltiplayergamemodebase-getscoreforkill.458202/
Make the interpolated atmosphere moddable
In response to the community request by @hunharibo
Other requests in earlier stages of development.

Thank you again for all your feedback and suggestions. If you have any questions or would like to make further requests, please discuss them below (or HERE).

v1.2.0 Changes​
Greetings!

Patch Beta v1.2.0 came out today and it's quite chunky! Here are the modding-related changes that came with it.

Editor
Fixed a crash that occurred when using the "Auto Generate Nav Mesh" functionality.
Fixed a crash that occurred when opening the Cloth editor via the console without first entering Editor mode.
Fixed a crash that occurred after closing the flora editor.
Modified the warning message that explains soft borders need to be present on the scene or flora may not appear correctly.
In response to the community request by @NPC99
Path points can now be snapped to the water level.
Fixed a bug that prevented the Max Height change option from working when using it for a single action with the Elevation or the Paint tool.

Combat
Added two new callbacks that allow modders to manually assign targets for AI agents. Agent.SetTargetAgent() allows modders to set a target agent but if they do not disable automatic target selection, our low-level AI will try to pick a new and different target agent when possible. To avoid that, Agent.SetAutomaticTargetSelection() can be used to disable automatic target selection, giving full control over the target selection process to the modders.
In response to the community request by @Hexnibbler here: https://forums.taleworlds.com/index.php?threads/api-request.447710/
Added agent references to the AttackInformation struct to allow modders to use agent information in their damage calculations.

Campaign
Pathfinding can now be used with excluded face IDs. This enables you to allow or restrict the usage of a specific nav mesh face under set conditions. For example, AI agents on foot can be allowed to enter a specific area while restricting access to mounted AI agents. This also works on the world map, allowing you to restrict access of parties to certain areas based on set conditions.
In response to the community request by @Space_Pot8to here: https://forums.taleworlds.com/index.php?threads/more-pathfinding-options-for-mobileparty.452701/
Batte types are now part of their map event within the map event system.
Moved some of the MapEventHelper functions in Helpers to the MapEvent class. They’re now directly connected to their map event.
Separated PartyVisuals from PartyBase.cs. Deleted the IPartyVisual interface. All party visuals are now stored in PartyVisualManager.cs which is part of the Sandbox.View project.
Moved the Prosperity value from Settlement.cs to Town.cs.
Moved the skill level requirement for epic perk calculations to the CharacterDevelopmentModel, allowing modders to override them.
In response to the community request by gallickgunner on the Modding Discord.
Changed the way companion skills are defined (sandboxcore_skill_sets.xml), making them more easily moddable.
Removed the neutral faction. This used to be a catch-all faction to avoid crashes and bugs in some circumstances. The code has been refactored to work without such a design.
Fixed a bug that caused a custom race to not be applied on save/load for heroes generated with a template from a different race. For example, if a hero named Richard is created using a hero template with an Elf race, Richard will still be an Elf after save/load.
In response to the bug report by @hunharibo here: https://forums.taleworlds.com/index...-hero-objects-when-loading-a-savegame.454091/

Multiplayer
Dedicated Servers
Fixed various crashes that occurred due to warmup behavior.
Merged the Custom Server Helper module into the base game. Players do not have to manually enable it to download maps from Dedicated Servers anymore.
In response to a popular community request.
Dedicated servers are now targeting .NET 6 instead of .NET Core 3.1.
Added a new console command "set_server_bandwidth_limit_in_mbps" and launch option "serverbandwidthlimitmbps" for limiting dedicated server bandwidth usage.
The "AutoTeamBalanceThreshold" option now accepts numbers instead of enum indices. This means that you can now set the auto team balance threshold from 0 to 30 players.
Added a new "DisableInactivityKick" option which if set to “true” disables automatic player kicks.
Added a new command line argument "use_parent_console” which forces the dedicated server to use the console it is launched from.
In response to the community request by @takeoshigeru
The welcome message is no longer displayed as a separate popup but is printed into the chat instead.
In response to the community request by @Gotha
Fixed a crash that occurred due to missing classes and perks of a modded faction.
Improved moddability of the AgentStatCalculateModel. GetWeaponDamageMultiplier and GetEffectiveSkill methods now accept the relevant agent as a parameter.
In response to the community request by @takeoshigeru here: https://forums.taleworlds.com/index...m-all-agentstatcalculatemodel-methods.457022/
Added CalculateStrikeMagnitudeForMissile to the StrikeMagnitudeModel, making Missile damage moddable. Removed CalculateSpeedBonusMultiplierForMissile and integrated it into the default implementation of CalculateStrikeMagnitudeForMissile.
In response to the community request by @takeoshigeru here: https://forums.taleworlds.com/index...el-calculatestrikemagnitudeformissile.457036/
Moved some MP code into the Multiplayer module (modders might need to reference these new DLLs).
Fixed a bug with the RoundUp calculation of the Captain mode troop counts.
Fixed a bug that caused newly added faction banners not to show up on the Multiplayer screens.
Fixed a bug that prevented the scoreboard from resetting after a TDM warmup.

Audio
Added a new category called "music" to the module_sounds.xml. This allows modders to add music across all aspects of the game more easily.

Other
Fixed a bug that caused some textures added by new modules to be rendered incorrectly.


We continue to work on other modding-related features/issues:
Making campaign time moddable
In response to the community request by @MitchPTI here: https://forums.taleworlds.com/index.php?threads/please-make-campaign-time-customisable.444719/
Add a server-side option for enabling team bump damage for horses
In response to the community request by @Gotha
Add a model for shield speed
In response to the community request by @takeoshigeru
Add a weapon flag that enables attacking while crouched
In response to the community request by @Lucon, @Terco_Viejo and others here: https://forums.taleworlds.com/index...ile-crouching-available-in-bannerlord.453706/
Add server settings for FPS and packages per second limits
In response to the community request by @Gotha
We're likewise working on the known world map modding issues kindly pointed out by @NPC99 and others, more specifically around the physics height map, terrain physics, modded world map crashes,...
Other requests in earlier stages of development.

Thank you again for all your feedback and suggestions. If you have any questions or would like to make further requests, please discuss them below (or HERE).

v1.1.0 Changes​
Greetings!

Patch v1.1.0 came to Beta last week. Here are the modding-related changes that came with it (apologies for posting with delay).
Improved scene checker code (editor side) in order to detect various spawn path placement problems.
The map camera implementation was separated from the map screen code for easier modding.
In response to the community request by Jansen via the modding discord.
A new attribute is_moving was added to the monster usage system. It allows specifying different actions in moving or stationary situations.
Enabled the ability to give banner tableaus to armor pieces. This change adds native support for armor pieces to have banner textures. It works the same way as shields, banner bearer items etc.
Fixed a bug that caused issues with asset importing if there were two modules with the same id.
Fixed SpCultures default xml to throw warnings when it has no cultures defined.
Animation clips can now be searched by their flags in the resource viewer.
Increased the ModifiedDamage compression info limits from 500 to 2000 and clamped the out of limit damage numbers to the min/max values.
AddGameMenu and GetGameMenu are now public.
In response to the community request by @azakhi here: https://forums.taleworlds.com/index...-it-hard-to-manage-menus.452923/#post-9816318
Game menu options can work with related objects.
In response to the community request by @azakhi here: https://forums.taleworlds.com/index...ed-code-makes-it-hard-to-manage-menus.452923/
Fixed a crash that happens if the main hero talks with a hero that does not have any proper conversation lines. Now he/she says a default conversation line if proper dialog lines were not added.
We continue to work on other modding-related features/issues:
More pathfinding options for MobileParty
In response to the community request by @Space_Pot8to here: https://forums.taleworlds.com/index.php?threads/more-pathfinding-options-for-mobileparty.452701/
Generate doxygen tagfiles from the official API documentation
In response to the community request by @Aragas via Discord.
Other requests in earlier stages of development.
Thank you again for all your feedback and suggestions. If you have any questions or would like to make further requests, please discuss them below (or HERE).


1.9.0 Changes​
Greetings!

Patch e1.9.0 comes with more modding-related changes. Here they are!
Changed Official node in SubModule.xml to ModuleType that has possible values of "Official", "OfficialOptional" and "Community". The default value is “Community”.
Increased the number of bones defined in monsters.xml to extend modding support of the animation system.
Hero.SetSkillValue() function is now public. Modders can now directly change the hero skill value.
In response to the community request by @ChizuNoYama here: https://forums.taleworlds.com/index...-modifiers-changes.443896/page-8#post-9809889
Added a "Quick Save Scene" option to the editor.
In response to the community request by @Owenwb here: https://forums.taleworlds.com/index.php?threads/incremental-save-button.441894/
Decreased the road generation minimum width value to 0.1 meters.
In response to the community report by @NPC99 on the modding discord.
The volunteer upgrade tier cap is now calculated from a model, allowing modders to change the value to their needs.
Unsealed and removed internals from some combat related classes and methods.
In response to the community request by @Captain_Octavius here: https://forums.taleworlds.com/index...-modifiers-changes.443896/page-8#post-9799573
Removed the unused GameMenuSelectionBehavior.
Fixed a visual bug in relation to tree_far trees.
In response to the community report by @Bullero on the modding discord.
Fixed a crash that occurred on load if an issue behavior had been removed from the game.
Adjusted the algorithm responsible for resizing the helmets to correctly fit the different head shapes, removing the bug that caused helmets to be too large.
Was pointed out by many community members, including @KingKilo.
Increased the theoretical limit for concurrent players on a custom server to around 1000.
In response to the community request by the closed custom servers group.
Our modding documentation has moved to a new address (which has SSL enabled) - https://moddocs.bannerlord.com/ We have also published new pages on it to accompany our recent Custom Servers release:
Hosting a Custom Server
Creating a Custom Game mode
What Makes a Multiplayer Scene
We continue to work on other modding-related features/issues:
Documentation on implementing new flora
In response to the community request @FoozleMcDoozle (will be published in the next few days).
Add a tag to hide the horse tail
In response to the community request by @Ellis1 here: https://forums.taleworlds.com/index.php?threads/add-command-to-hide-horse-tail-xml-files.447575/
Other requests in earlier stages of development.

Thank you again for all your feedback and suggestions. If you have any questions or would like to make further requests, please discuss them below (or HERE).

1.8.0 Changes​
Greetings!

Here are the modding-related changes that came with patch 1.8.0.
We released API documentation for Bannerlord. You can find it here: apidoc.bannerlord.com
Modders can now define & add different races like Orcs, Elves,... through the skins.xml file. Custom skeleton support has been added for humanoid characters.
In response to the open letter from the community.
1654773199251.png​
Fixed various issues with the implementation of a modded world map, removed the hardcoded assumption that the world map is named "Main_map", fixed the hardcoded map size,...
In response to the reports from various community members. Special thanks to @NPC99 for his continuous testing and feedback. @hunharibo thank you for your report as well, we now fetch the terrain size from the overridden scene.
Enabled warning & assertion texts to help modders with common errors (the Modding Kit must be installed).
The Atmosphere Curve Editor is now available through the Atmosphere Inspector. It allows you to change the visuals of an atmosphere.
In response to the community request by @NPC99.
Added the Color Grade Manager script to the main map scene. It allows the dynamic color grade and atmosphere effects to be previewed within the editor. Color grade textures can be changed from worldmap_color_grades.xml file.
In response to the community request by @NPC99.
Added exposure compensation to materials for glow effects.
In response to the community request by @KingKilo here: https://forums.taleworlds.com/index.php?threads/exposure-compensation-for-materials.443900/
Changed the physics object colors of barriers that only players can pass through to green for clarity.
Implemented a new method of using generated banner textures in item materials. A new channel called "TableauMaskMap" has been added to explicitly mark where the banner texture will be applied. This leaves the alpha channel on the original texture to be used for the item's own requirements for tassel etc. "use_tableau_mask_as_separate_texture" flag needs to be enabled for this to take effect.
Language files are now referenced in language folders in the "language_data.xml" (they were previously automatically searched for and added).
Added XSD for game texts, they are now added via SubModule.xml.
In response to the community request by @Areldir here: https://forums.taleworlds.com/index...-modifiers-changes.443896/page-5#post-9752303
Players can now override the TournamentModel.GetParticipantArmor to change the tournament participant armor.
In response to the community request by @TheREALHayster here: https://forums.taleworlds.com/index.php?threads/tournaments.444054/
Changed the SettlementComponent list to single SettlementComponent in Settlement.cs. Settlements can no longer have multiple SettlementComponents.
"NonSerializedObjectTypeRecords" were removed from MBObjectManager. There are now only ObjectTypeRecords and their appropriate functions.
Removed XML support from game menus and moved related content to code (EncounterGameMenuBehavior.cs).
Removed XML support from conversations and moved related content to code.
Modders can now use the OnCrimeRatingChanged event for various purposes. This event will trigger with the change amount and the faction it is related with.
In response to the community request by @slaur4 here: https://forums.taleworlds.com/index...mpaign-event-for-crime-rating-changes.449493/
Added IRandomOwner for easier random usages.
Added a dev config option to show localization IDs of texts (relevant for translators).
Enabled modders to go above the maximum tier cap for troop upgrades (above tier 6).
In response to the community request by @Lord Boogie Blue here: https://forums.taleworlds.com/index.php?threads/please-allow-tier-7-troops-in-the-game-again.446673/
Modders can now modify the voices of agents (via voice_definitions.xml).
In response to the community request by @Jance here: https://forums.taleworlds.com/index...ml-file-overrideable-with-mbproj-xslt.445811/
The skill leveling system is now fully moddable (Skill Leveling Manager).
Removed the campaign cookie system.
Refactored the Quest system to make every quest use only one dedicated menu instead of creating multiple menus per quest.
Fixed a bug that prevented the navmesh grid generation on big scenes from working correctly.
Fixed a crash that occurred when generating rivers.
Fixed a bug that caused different instances of a model to use the same factor color.
Fixed a bug that prevented the addition of custom troops to villager parties. Villager parties can now use different troop types.
In response to the community report by @bfmsc here: https://forums.taleworlds.com/index...g-in-partytemplate-and-spculture-xmls.449384/
Fixed a bug that caused decals to discard season visibility settings.
Fixed a crash that occurred when exiting editor scenes while retreating outside of the border.
Fixed a freeze issue on the campaign map that was caused by modded troops not having any upgrades.
In response to the community report by @irish09 here: https://forums.taleworlds.com/index.php?threads/1-6-5-endless-freeze-on-campaign-map.449650/
Fixed a crash that occurred when clicking on the Leave edit mode button.
Fixed a bug that caused the second editor window to freeze.
Fixed a crash that occurred when placing decals on an empty editor scene.
Fixed a bug that caused civilian battle sets (for troop spawn) placed in editor scenes to be processed wrongly.
Improved the warning messages for incorrect battle set placement.
Added an experimental search functionality to the inventory screen. It can be enabled with "ui.set_inventory_search_enabled [1/0]" while the inventory screen is open. We think modders can use this functionality to test and find their items in-game more easily.
Extended the functionality of the UI debug mode. Added a new command ("ui.set_screen_debug_information_enabled [True/False]") to help modders find and examine GauntletLayers more easily. This new panel will show the currently loaded GauntletLayers, list included widgets, their visual properties and more in real time.
Added the Core.FaceGen.UpdateDeformKeys boolean. Previously, all the sliders in the body generation screen were cleared and added again after gender change. Since we don't use different deform keys between genders, we've moved this functionality to this boolean. If your mod uses different deform keys between genders, your mod should set this value to true. If not done, some deform keys might not show up as sliders in the body generation screen. Can be toggled with "facegen.toggle_update_deform_keys" command.
Added Scene.GetAllEntitiesWithScriptComponent method to gather all entities with the given script type.
Introduced a "CustomScale" value to font properties, used by Gauntlet. This value is used to scale the whole font. It’s used especially for fonts that generally have bigger characters compared to other fonts used in the game. Modders can use this value to make their fonts bigger or smaller across the whole font.
Added a new boolean to the Hero tooltip, isNear. It's used to hide tooltip entries of a hero tooltip that shouldn't be visible if the main hero is not near, like available quests by hero.
Improved the spritesheet generator performance, especially for generating numerous sheets at the same time.
Changed the banner editor layout to support more colors. Useful for mods expanding the available color set.
Changed how we determine singleplayer and multiplayer modules in the SubModule.xml.
<SingleplayerModule/> and <MultiplayerModule/> have been renamed to <ModuleCategory/>
The correct usage is now <ModuleCategory value="Singleplayer"/> or <ModuleCategory value="Multiplayer"/>
The default ModuleCategory is Singleplayer.
Fixed a bug that prevented mods from overwriting already existing custom widgets in Gauntlet, consistently.
We have also published a new page on the modding documentation:
WEAPON SMITHING & CRAFTING PIECES
We continue to work on other modding-related features/issues:
Documentation on implementing new flora
In response to the community request @FoozleMcDoozle.
Incremental Save Button
In response to the community request by @Owenwb here: https://forums.taleworlds.com/index.php?threads/incremental-save-button.441894/
Add a tag to hide the horse tail
In response to the community request by @Ellis1 here: https://forums.taleworlds.com/index.php?threads/add-command-to-hide-horse-tail-xml-files.447575/
Can't use editor paths to build roads with a width less than 1 metre
In response to the community report by @NPC99.
Unseal, remove internals from some combat related classes, methods
In response to the community request by @Captain_Octavius here: https://forums.taleworlds.com/index...-modifiers-changes.443896/page-8#post-9799573
Other requests in earlier stages of development.
Thank you again for all your feedback and suggestions. If you have any questions or would like to make further requests, please discuss them below (or HERE).

1.7.2 Changes​
Greetings!

Here are the modding-related changes that came with patch 1.7.2.
InquiryData now supports expiration. This allows for the implementation of timed events, for example, "Click yes in 10 seconds".
Replaced the "HeroDeveloper" field with the "IHeroDeveloper" interface. This allows modders to change HeroDeveloper of heroes with their own custom classes. As such, they can now change how skill/attribute/focus points work and how heroes gain levels.
In response to the community request by @Midnightknight here: https://forums.taleworlds.com/index.php?threads/hero-generation.445601/
Added the OnPartyInteraction function to the IMapEntity interface. This allows modders to use custom interaction with custom map objects.
Fixed a bug that prevented moved texture files from being moved again in the resource browser.
Fixed a bug that caused new Sprites to not override existing ones. Meaning if a sprite is loaded after a sprite with the same name, it wouldn't override the previous one. Because of this modders had to load their overriding sprites before Native or SandBox. This has been fixed.
Fixed a bug that caused some GUI textures to appear as missing when launching campaign via the modding tools (also in 1.7.1 latest changes).
We have also published a new page on the modding documentation:
CREATING CUSTOM BANNER ICONS & COLORS
And updated page:
GENERATING AND LOADING UI SPRITE SHEETS
We continue to work on other modding-related features/issues:
Automated API documentation
In response to the modding open letter.
Documentation on implementing new flora
In response to the community request from @FoozleMcDoozle.
Support for custom skins/races
In response to the modding open letter.
Enable the modifying of equipment of tournament participants
In response to the community request by @TheREALHayster here: https://forums.taleworlds.com/index.php?threads/tournaments.444054/
The addition of an Atmosphere Curve Editor button to allow for accurate viewing (colours) of the campaign map directly from the scene editor
In response to the community request from @NPC99.
Other requests in earlier stages of development.
Thank you again for all your feedback and suggestions. If you have any questions or would like to make further requests, please discuss them below (or HERE).

1.7.0 Changes​
Greetings!

Here are the modding-related changes that came with v170.
Added new dependency types in SubModule.xml & other improvements.
<DependedModule/> Node now supports the Optional attribute. If Optional is true, the launcher will only check if the Depended module is loaded before the current submodule. If Optional is false, the launcher will also check if the Depended module is enabled or not.
<ModulesToLoadAfterThis/> Node has been added. Modules given in this scope will be forced to load after the current submodule. Essentially an inverse dependence.
<IncompatibleModules/> Node has been added. If any module given in this scope exists and is enabled, the module defined in the node will be disabled. Doesn't depend on load order.
Syntax examples for <ModulesToLoadAfterThis/> and <IncompatibleModules/> have been added to the Native SubModule.xml. DependedModule-Optional syntax has been added to the rest of the SubModule.xmls.
In response to the community request from @Pickysaurus.
Launcher sprite sheets are now located in the Modules/Native/LauncherGUI so that modders can overwrite or add new images for the launcher.
In response to the community request from @Lucon via the modding discord.
Cultural feats have been moved into CultureObject and are now loaded from XMLs. This allows for much easier replacing or adding of new culture-specific advantages/disadvantages.
Child, wanderer, notable, lord and rebel hero templates were moved into CultureObject and are now loaded from XMLs.
Mercenary troops for each culture can now be determined in XMLs.
In response to the community request from @Mordorsen here: https://forums.taleworlds.com/index.php?threads/tavern-mercenary-pool-in-bannerlord.447675/
Added a new XML (weapon_descriptions.xml) to make weapon descriptions moddable.
Removed the PartyBase.Leader, MobileParty.Leader variables. Moved the PartyLeader variable to the PartyComponent.
Added more informative errors and warnings for invalid/missing XMLs.
Made reload animations moddable. They now support up to 15 reload phases.
Fixed a bug that caused the covers_head flag and its usage on helmets to crash the game.
In response to the reports from @MFKB and @John_M here: https://forums.taleworlds.com/index...d-to-hide-head-xml-files.434623/#post-9755920
Added TickRequirement.TickParallel to provide a multithread tick mechanism for scripts. Can be used with OnTickParallel callback.
Added a custom base party speed set option for the custom party component.
Added custom harness and mount set options for the custom party component.
Added the AvoidHostileActions parameter option for custom party component which prevents the party from being hostile with the encountered party.
Fixed a crash that occurred in the editor when closing the scene.
Fixed a crash that occurred in the editor when trying to add the ladder spawner.
Added slope and height filters to the smoothen brush.
In response to the community request from @GourmetBean via the modding discord.
Added the ability to select the relevant path by selecting any of the path nodes in the editor.
Added a tool that checks the validity of destructible states of destructible components.
Reinforced the scene problem checking tool with new additions to the navigation mesh controls.
Fixed a bug that caused terrain nodes to only be visible from certain angles and distances on newly created terrains.
In response to the reports from @Space_Pot8to, @NPC99, @Klassix, @Levante and others here and on the modding discord: https://forums.taleworlds.com/index...in-paint-layers-on-1-6-2.447149/#post-9752030
Fixed a bug that prevented the modification of newly created terrains.
Fixed a bug that caused the importer to create redundant mesh vertices.
Fixed the activation delay parameter of the particle system.
We have also published a new page on the modding documentation:
What Makes a Hideout Scene
We continue to work on the following issues:
Documentation on implementing new flora.
In response to the request from @FoozleMcDoozle.
Other requests (in earlier stages of development).
Thank you for all your feedback and suggestions. If you have any questions or would like to make further requests, please discuss them below (or HERE).

1.6.5 Changes​
Greetings,

Here's an overview of all the modding-related changes that came with v165.
MapConversationTableau's atmosphere selection is now moddable. Modders can change the SandBoxViewSubModule.MapConversationDataProvider with SetMapConversationDataProvider and set their own custom atmospheres.
In response to the request from @NPC99.
Modders can now add and remove additional Vec3 targets to mission namemarkers with AddGenericMarker and RemoveGenericMarker.
Encyclopedia Home page tabs (settlements, clans, kingdoms etc.) are now moddable.
The code that awards charm XP was moved from ChangeRelationAction.ApplyInternal to CharacterRelationCampaignBehavior.
In response to the request from @guiskj here: https://forums.taleworlds.com/index...-applyplayerrelation-without-charm-xp.438780/
Fixed an issue with modder-created WeaponUsageData instances not being included in the crafting templates.
In response to the request from @KingKilo here: https://forums.taleworlds.com/index...-allow-new-usages-for-crafted-weapons.444547/
Whether or not a hero can give out volunteers can now be changed with newly added VolunteerProductionModel.CanHaveRecruits.
Added OnAfterSessionStart event.
In response to a discussion on the modding discord & this request by @Space_Pot8to here: https://forums.taleworlds.com/index.php?threads/modifying-the-native-encounter-menu.446867/.
Removed characterobjects of removed heroes.
Errors with XMLs will now show an error with the faulty line and details (also in e1.6.4 latest changes).
Ensured that the HostileRelationship tag works.
In response to the bug report by @Bannerman Man here: https://forums.taleworlds.com/index...ation-tag-to-apply-will-never-trigger.430046/
Fixed some layers not being rendered on newly created terrains.
Fixed major memory leaks that occurred on every terrain edit operation.
We continue to work on the following issues:
Documentation on implementing new flora.
In response to the request from @FoozleMcDoozle.
The addition of new attributes/dependencies to SubModule.xml & naming convention improvements.
In response to the request from @Pickysaurus.
Add slope and height filters for the "smoothen" brush.
In response to the request from @GourmetBean on the modding discord.
Other requests (in earlier stages of development).
Thank you for all your feedback. If you have any questions or would like to make further requests, please discuss them below (or HERE).

1.6.4 Changes​
Greetings,

Here's an overview of all the modding-related changes that came with v164.
Font atlas generator is now available to modders. Modders can generate new font files with custom TTF files and use these fonts in-game. We've also created documentation on how to use this tool: http://docs.modding.bannerlord.com/asset-management/how_to_add_custom_fonts/
StoryModeEvents.RemoveListeners do not need to be explicitly called anymore. Now, StoryModeEvents is added as a CampaignEventReceiver.
We have also implemented a fix that should take care of the long modding-module loading times in response to the request from @KingKilo. Please let us know if this change helped with your issue.

We continue to work on the following issues:
Documentation on implementing new flora.
In response to the request from @FoozleMcDoozle
Allow modders to add new atmosphere (backgrounds) of quick conversations
Add an option to the ChangeRelationAction.ApplyPlayerRelation call to NOT award Charm XP
In response to the request from @guiskj here: https://forums.taleworlds.com/index...-applyplayerrelation-without-charm-xp.438780/
Add option to change crafted item weapon "type" (skill it gains / perks it benefits) or allow new usages for crafted weapons
In response to the request from @KingKilo here: https://forums.taleworlds.com/index...-allow-new-usages-for-crafted-weapons.444547/
Moddability for CanHaveRecruits and the ability to determine which notables can have recruits.
The addition of OnAfterSessionLaunchedEvent so modders can more easily modify some campaign content.
In response to the following community request from @Space_Pot8to: https://forums.taleworlds.com/index.php?threads/modifying-the-native-encounter-menu.446867/
Other requests (in earlier stages of development).
Thank you for all your feedback. If you have any questions or would like to make further requests, please discuss them below (or HERE).

1.6.3 Changes​
Greetings,

Patch v163 brings a number of modding-related changes as well. They've been included in the patch notes but here's an overview of them as well and some additional information.
Heightmap selection dialogue now remembers the last used folder.
Added a warning to catch duplicate removals of entities.
Animations now support switching the item on the right hand to the left, and back. The appropriate flag for this is "switch_item_between_hands" which can be set in the editor. The addition of "hand_switch_data" to the animation is then also required to be set in the editor.
In response to the animation-related request from @Lucon.
Preview example of usage: 

Damage Particles are now moddable. A new GameModel named DamageParticleModel has been added. By creating and using a new model class inherited from DamageParticleModel and overriding needed functions modders can choose damage particles very precisely according to collision properties.
In response to the following community request from @Jance : https://forums.taleworlds.com/index...select-a-blood-particle-system-on-hit.443913/
banner_icons.xml is now moddable. Modders can include their own banner_icons.xml in their modules under "{MODULE_NAME}/ModuleData/banner_icons.xml". Sigils, backgrounds and colours defined in these XMLs will extend the native banner icons.
Added editor visuals that show ranged siege weapon trajectories.
Languages are now moddable. Modders can now change options for all languages by providing their own options file. (see Native/ModuleData/Languages/[ID]/[id]_options.xml).
Moved occupation field to the Hero class from CharacterObject class, making occupations moddable.
In response to a community request made via the modding discord.
Made character creation modder friendly.
CharacterCreationStages are now moddable. Modders can decide explicitly which stage types should be included and in what order in CharacterCreationContentBase. They can use their own stage types.
Fixed a few bugs that could crash the game when new cultures are used in character creation.
Exposed CharacterCreation.CharacterCreationMenus to modders.
Most functions', fields' enums' protection levels in StoryModeCharacterCreationContent and SandboxCharacterCreationContent have been changed to protected from private.
In response to a community request made via the modding discord.
Fixed a crash that occurred while sieging settlements with custom cultures.
Fixed a crash that occurred if the submodule.xml had a comment in it.

We continue to work on the following issues:
Allow modders to add new atmosphere (backgrounds) of quick conversations
Investigate the long modding-module loading times
In response to the request from @KingKilo
Add an option to the ChangeRelationAction.ApplyPlayerRelation call to NOT award Charm XP
In response to the request from @guiskj here: https://forums.taleworlds.com/index...-applyplayerrelation-without-charm-xp.438780/
Add option to change crafted item weapon "type" (skill it gains / perks it benefits) or allow new usages for crafted weapons
In response to the request from @KingKilo here: https://forums.taleworlds.com/index...-allow-new-usages-for-crafted-weapons.444547/
Make Font Atlas Generator available for modders & share documentation on How to Add Custom Fonts To Bannerlord
Documentation on implementing new flora.
In response to the request from @FoozleMcDoozle
Other requests (in earlier stages of development).
We have also updated the following documentation page as the system has changed a bit with v162 and the addition of the <AlwaysLoad/> spritesheet config: http://docs.modding.bannerlord.com/asset-management/generating_and_loading_ui_sprite_sheets/

We also continue to work on the world-map-related issues that prevent you from using custom maps successfully, thank you for your patience.

Thank you for all your feedback. If you have any questions or would like to make further requests, please discuss them below (or HERE).


1.6.2 Changes​
Greetings,

As part of the v162 patch, we released a number of modding related adjustments. They’ve been included in the patch notes but we’ll list them here as well.
Made adjustments so that custom classes that inherit from LogEntry, Issue, Quest or InformationData should now be much less likely to break a save game when the mod is removed.
In response to the discussion on the modding discord. Requested by [BUTR] zijistark.
Fixed a problem that caused rebellions in settlements with a custom culture (introduced by a mod) to crash the game.
Did preliminary work to allow modders to add to existing banner icon groups and introduce whole new groups with BannerManager.LoadBannerIcons(string xmlPath).
MissionGauntletNameMarker (ALT key name markers in settlements etc.) is now more moddable as you’re able to add/remove new agent targets.
Added horizontal scrolling to troop trees in the encyclopedia (if it's required).
In response to the following request: https://forums.taleworlds.com/index.php?threads/add-a-horizontal-scrollbar-for-troop-trees.442885/
Added <AlwaysLoad/> spritesheet config to indicate that the spritesheet category should be loaded by default.
Minor faction hero templates are now added through XML and stored in the Clan object directly. The minor faction hero spawn chance and hero limit values are now also moddable.
Troops can now have individual upgrade costs and XP reward for killing.
In the editor, progress bars were added to the “Navigation Mesh Auto Generation”, “Navigation Mesh Grid Generation”, “NavMesh Debug Mark Elevation Problem Faces” and “Remove Unreachable Faces” features.

We continue to work on the following issues:
Implement a left_hand flag.
In response to the animation-related request from @Lucon.
Making occupations moddable.
Documentation on implementing new flora.
Other requests (in earlier stages of development).
We likewise continue to look into the reported world map issues.

Thank you for all your feedback. If you have any questions or would like to make further requests, please discuss them below (or HERE).
Last edited by a moderator: Sep 25, 2025
Dejan
Dejan
Community Manager


Jun 10, 2021
#2
1.6.1 Changes​
Greetings,

As part of the e1.6.1 patch, we made another set of modding-related adjustments. As they don’t all fit within the patch notes, we are sharing a list of the changed classes as well as some additional information here. Make sure to check the patch notes for some additional new adjustments.

Just like the OP notes:
This topic is dedicated to the discussion of the changes - whether you have questions about them or would like to make further requests (although you can do the latter HERE as well).

To begin, the 1.6.1 patch introduced the following changes based on community requests:
Campaign map size is no longer hardcoded, it is now set from the map scene.
In response to the open letter.
Fixed a bug where CampaignGameStarter object was reinitialized. Now, Behaviors and models will be added to CampaignGameStarter at MBSubModuleBase.InitializeGameStarter so other modules can add/remove other campaign behaviors before they are initialized.
In response to the open letter.
Harness' "covers_head" flag now works for helmets as well.
In response to this community request: https://forums.taleworlds.com/index.php?threads/add-command-to-hide-head-xml-files.434623/
Added a console command (crafting.reload_pieces {MODULE_NAME} {XML_NAME}) for modders to be able to reload crafting pieces XML of their choosing without needing to restart the game.
In response to this community request: https://forums.taleworlds.com/index...ion-mod-community.440886/page-10#post-9675239
Improved moddability of the keybinds. Modders can now introduce auxiliary key categories.
In response to this community request: https://forums.taleworlds.com/index...adding-custom-options-to-options-menu.438797/
Added a script that moves any object from point A to point B without any help from agents.
In response to a community request made via the modding discord.
A new interface called "IMapEntity" has been added to support custom interactable entities on the map.
In response to this community request: https://forums.taleworlds.com/index.php?threads/custom-map-entities.441314/
We also introduced a new documentation page since the OP was created:
Generating and Loading UI Sprite Sheets: http://docs.modding.bannerlord.com/asset-management/generating_and_loading_ui_sprite_sheets/
Beyond that, we continue to work on:
Making languages moddable.
Documentation on implementing new flora.
Documentation for using models to add attributes to items & determine the exact weapon damage.
Making UpgradeXpCost and GetXpValue moddable.
Minor faction hero templates being added through XML and storing them in the Clan object directly. Making minor faction hero spawn chance and hero limit values moddable.
Other requests (in earlier stages of development).
You can find the more detailed list of changed classes here:

Spoiler
Project name	Class name	INTERNAL CLASS CHANGE - REMOVED	INTERNAL FIELD/function CHANGE - REMOVED
TaleWorlds.MountAndBlade	BallistaAI	x	
TaleWorlds.MountAndBlade	BatteringRamAI	x	
TaleWorlds.MountAndBlade	MangonelAI	x	
TaleWorlds.MountAndBlade	SiegeLadderAI	x	
TaleWorlds.MountAndBlade	SiegeTowerAI	x	
TaleWorlds.MountAndBlade	TrebuchetAI	x	
SandBox.GauntletUI	PanelScreenStatus	x	
TaleWorlds.MountAndBlade.GauntletUI.Widgets	ChatMultiLineElement	x	
TaleWorlds.MountAndBlade.GauntletUI.Widgets	MapInfoBarWidget		x
TaleWorlds.MountAndBlade.ViewModelCollection	GenderBasedSelectedValue	x	
TaleWorlds.MountAndBlade.GauntletUI	ChatLineData	x	
SandBox	FamilyFeudIssueMissionBehavior	x	
SandBox	FamilyFeudIssue	x	
SandBox	FamilyFeudIssueQuest	x	
SandBox	NotableWantsDaughterFoundIssue	x	
SandBox	NotableWantsDaughterFoundIssueQuest	x	
SandBox	ProdigalSonIssue	x	
SandBox	ProdigalSonIssueQuest	x	
SandBox	RivalGangMovingInIssue	x	
SandBox	RivalGangMovingInIssueQuest	x	
SandBox	RuralNotableInnAndOutIssue	x	
SandBox	RuralNotableInnAndOutIssueQuest	x	
SandBox	SuspectNpc	x	
SandBox	TheSpyPartyIssue	x	
SandBox	TheSpyPartyIssueQuest	x	
StoryMode	ArzagosBannerPieceQuest	x	
StoryMode	AssembleTheBannerQuest	x	
StoryMode	BannerInvestigationQuest	x	
StoryMode	CreateKingdomQuest	x	
StoryMode	IstianasBannerPieceQuest	x	
StoryMode	MeetWithArzagosQuest	x	
StoryMode	SupportKingdomQuest	x	
StoryMode	RebuildPlayerClanQuest	x	
StoryMode	RescueFamilyQuest	x	
StoryMode	ConspiracyProgressQuest	x	
StoryMode	AssembleEmpireQuest	x	
StoryMode	MeetWithIstianaQuest	x	
StoryMode	StopConspiracyQuest	x	
StoryMode	WeakenEmpireQuest	x	
StoryMode	FindHideoutTutorialQuest	x	
StoryMode	LocateAndRescueTravellerTutorialQuest	x	
StoryMode	TalkToTheHeadmanTutorialQuest	x	
StoryMode	TravelToVillageTutorialQuest	x	
TaleWorlds.CampaignSystem	DeclareWarDecisionOutcome	x	
TaleWorlds.CampaignSystem	ExpelClanDecisionOutcome	x	
TaleWorlds.CampaignSystem	PolicyDecisionOutcome	x	
TaleWorlds.CampaignSystem	KingSelectionDecisionOutcome	x	
TaleWorlds.CampaignSystem	MakePeaceDecisionOutcome	x	
TaleWorlds.CampaignSystem	ClanAsDecisionOutcome	x	
TaleWorlds.CampaignSystem	SettlementClaimantPreliminaryOutcome	x	
TaleWorlds.CampaignSystem	ArmyNeedsSuppliesIssue	x	
TaleWorlds.CampaignSystem	ArmyNeedsSuppliesIssueQuest	x	
TaleWorlds.CampaignSystem	ArtisanCantSellProductsAtAFairPriceIssue	x	
TaleWorlds.CampaignSystem	ArtisanCantSellProductsAtAFairPriceIssueQuest	x	
TaleWorlds.CampaignSystem	ArtisanOverpricedGoodsIssue	x	
TaleWorlds.CampaignSystem	ArtisanOverpricedGoodsIssueQuest	x	
TaleWorlds.CampaignSystem	CapturedByBountyHuntersIssue	x	
TaleWorlds.CampaignSystem	CapturedByBountyHuntersIssueQuest	x	
TaleWorlds.CampaignSystem	CaravanAmbushIssue	x	
TaleWorlds.CampaignSystem	CaravanAmbushIssueQuest	x	
TaleWorlds.CampaignSystem	EscortMerchantCaravanIssue	x	
TaleWorlds.CampaignSystem	EscortMerchantCaravanIssueQuest	x	
TaleWorlds.CampaignSystem	ExtortionByDesertersIssue	x	
TaleWorlds.CampaignSystem	ExtortionByDesertersIssueQuest	x	
TaleWorlds.CampaignSystem	GangLeaderNeedsRecruitsIssue	x	
TaleWorlds.CampaignSystem	GangLeaderNeedsRecruitsIssueQuest	x	
TaleWorlds.CampaignSystem	GangLeaderNeedsToOffloadStolenGoodsIssue	x	
TaleWorlds.CampaignSystem	GangLeaderNeedsToOffloadStolenGoodsIssueQuest	x	
TaleWorlds.CampaignSystem	GangLeaderNeedsWeaponsIssue	x	
TaleWorlds.CampaignSystem	GangLeaderNeedsWeaponsIssueQuest	x	
TaleWorlds.CampaignSystem	HeadmanNeedsGrainIssue	x	
TaleWorlds.CampaignSystem	HeadmanNeedsGrainIssueQuest	x	
TaleWorlds.CampaignSystem	HeadmanNeedsToDeliverAHerdIssue	x	
TaleWorlds.CampaignSystem	HeadmanNeedsToDeliverAHerdIssueQuest	x	
TaleWorlds.CampaignSystem	HeadmanVillageNeedsDraughtAnimalsIssue	x	
TaleWorlds.CampaignSystem	HeadmanVillageNeedsDraughtAnimalsIssueQuest	x	
TaleWorlds.CampaignSystem	LadysKnightOutIssue	x	
TaleWorlds.CampaignSystem	LadysKnightOutIssueQuest	x	
TaleWorlds.CampaignSystem	LandLordCompanyOfTroubleIssue	x	
TaleWorlds.CampaignSystem	LandLordCompanyOfTroubleIssueQuest	x	
TaleWorlds.CampaignSystem	LandlordNeedsAccessToVillageCommonsIssue	x	
TaleWorlds.CampaignSystem	LandlordNeedsAccessToVillageCommonsIssueQuest	x	
TaleWorlds.CampaignSystem	LandLordNeedsManualLaborersIssue	x	
TaleWorlds.CampaignSystem	LandLordNeedsManualLaborersIssueQuest	x	
TaleWorlds.CampaignSystem	LandLordTheArtOfTheTradeIssue	x	
TaleWorlds.CampaignSystem	LandLordTheArtOfTheTradeIssueQuest	x	
TaleWorlds.CampaignSystem	LandlordTrainingForRetainersIssue	x	
TaleWorlds.CampaignSystem	LandlordTrainingForRetainersIssueQuest	x	
TaleWorlds.CampaignSystem	LesserNobleRevoltIssue	x	
TaleWorlds.CampaignSystem	LesserNobleRevoltIssueQuest	x	
TaleWorlds.CampaignSystem	LordNeedsGarrisonTroopsIssue	x	
TaleWorlds.CampaignSystem	LordNeedsGarrisonTroopsIssueQuest	x	
TaleWorlds.CampaignSystem	LordNeedsHorsesIssue	x	
TaleWorlds.CampaignSystem	LordNeedsHorsesIssueQuest	x	
TaleWorlds.CampaignSystem	LordsNeedsTutorIssue	x	
TaleWorlds.CampaignSystem	LordsNeedsTutorIssueQuest	x	
TaleWorlds.CampaignSystem	LordWantsRivalCapturedIssue	x	
TaleWorlds.CampaignSystem	LordWantsRivalCapturedIssueQuest	x	
TaleWorlds.CampaignSystem	MerchantArmyOfPoachersIssue	x	
TaleWorlds.CampaignSystem	MerchantArmyOfPoachersIssueQuest	x	
TaleWorlds.CampaignSystem	MerchantNeedsHelpWithOutlawsIssue	x	
TaleWorlds.CampaignSystem	MerchantNeedsHelpWithOutlawsIssueQuest	x	
TaleWorlds.CampaignSystem	NearbyBanditBaseIssue	x	
TaleWorlds.CampaignSystem	NearbyBanditBaseIssueQuest	x	
TaleWorlds.CampaignSystem	RaidAnEnemyTerritoryIssue	x	
TaleWorlds.CampaignSystem	RaidAnEnemyTerritoryQuest	x	
TaleWorlds.CampaignSystem	QuestSettlement	x	
TaleWorlds.CampaignSystem	ScoutEnemyGarrisonsIssue	x	
TaleWorlds.CampaignSystem	ScoutEnemyGarrisonsQuest	x	
TaleWorlds.CampaignSystem	TheConquestOfSettlementIssue	x	
TaleWorlds.CampaignSystem	TheConquestOfSettlementIssueQuest	x	
TaleWorlds.CampaignSystem	VillageNeedsToolsIssue	x	
TaleWorlds.CampaignSystem	VillageNeedsToolsIssueQuest	x	
Again, the classes that inherit from these are not specifically mentioned but are still affected by the change of their parent class. We will continue to review the code base as we develop the game and you highlight limitations that you are facing.

Thank you for all your feedback. If you have any questions or would like to make further requests, please discuss them below (or HERE).

1.6.0 Changes​
Greetings,

As part of the e1.6.0 patch, we released a number of modding-related adjustments. As they don’t all fit within the patch notes, we are sharing a list of the changed classes as well as some additional information here. This topic is dedicated to the discussion of the changes - whether you have questions about them or would like to make further requests (although you can do the latter HERE as well).

To begin with, the patch included the following changes in response to community requests:
Additional resources and documentation for skeletons for reins/horse harness and their implementation http://docs.modding.bannerlord.com/asset-management/horse_reins_simulation_creation/
In response to this community suggestion: https://forums.taleworlds.com/index.php?threads/the-community-needs-skeletons-for-3d-models.432261/
Ability to name a new paint layer upon creation
In response to this community suggestion: https://forums.taleworlds.com/index...w-paint-layer-at-the-time-of-creation.432989/
Added a new callback with the name OnBeforeMissionBehaviourInitialize() to MBSubModuleBase which is called before MissionBehaviour's OnBehaviourInitialize method.
This is in response to a community request for a cleaner way to remove Mission Behaviors from missions.
Added support for adding new custom battle maps outside the CustomBattle module (within other modules).
Beyond that, we continue to work on:
World map moddability
In response to the open letter.
AddBehaviors method moddability
In response to the open letter.
Adding a console command for reloading the pieces XML in crafting screen
In response to this community request: https://forums.taleworlds.com/index...ion-mod-community.440886/page-10#post-9675239
Adding a script for moving objects on a path from point A to point B
In response to a community request made via the modding discord.
Adding support for adding options to the options screen
In response to this community request:https://forums.taleworlds.com/index...adding-custom-options-to-options-menu.438797/
Create a new interface that will be implemented by PartyBase and change PartyVisual.PartyBase to that type
In response to this community request:https://forums.taleworlds.com/index.php?threads/custom-map-entities.441314/
Adding support for alternative weapon usage in XML's for non-crafted weapons.
Other requests (in earlier stages of development)
Of course, the core of the current discussion concerns access modifiers. The patch notes provided a rough summary of the changes made as part of the relevant code review:
Added and removed some internal modifiers to make them more consistent and reduce unnecessary limitations.
Made the character attraction for romance moddable by moving it to a game model.
Removed "All" lists for some of the default classes such as DefaultSkills, DefaultPerks, DefaultFeats, DefaultTraits, DefaultBuildingTypes, DefaultIssueEffects, DefaultPolicies, DefaultSiegeStrategies, DefaultSkillEffects, DefaultVillageType, DefaultSiegeEngineTypes, DefaultItems, DefaultItemCategories, DefaultCharacterAttributes.
Every instance of said object can now be accessed with a new list at its corresponding type or its extensions, for example, use SiegeEngineTypes.All instead of DefaultSiegeEngineTypes.All. SiegeEngineTypes.All will include all of the Siege Engine Types in the object manager. Lists like DefaultSiegeEngineTypes.All were mostly hardcoded.
Skill effects are now registered to the object manager which means that SkillEffect.All now includes skill effects added by mods.
Modifying which basic volunteers a hero gives out can now be changed through a game model (VolunteerProductionModel). The only way to achieve that previously was by rewriting the RecruitmentCampaignBehavior.
Campaign event receivers are now moddable (modders can add their custom campaign event receivers through Campaign.AddCampaignEventReceiver).
Removed the character attribute enum (adding new attributes wasn’t possible before). It is now using static references instead at DefaultCharacterAttributes and Attributes.All to reference CharacterAttributes.
You can find the more detailed list of changed classes here:
Spoiler: Internal Changes
Project name	Class name	INTERNAL CLASS CHANGE - ADDED	INTERNAL CLASS CHANGE - REMOVED	INTERNAL FIELD/function CHANGE - ADDED	INTERNAL FIELD/function CHANGE - REMOVED
SandBox.View	SandBoxMissionViews		x		
SandBox.View	PartyVisualCreator		x		
SandBox	MissionTacticManagerCampaignBehavior		x		
SandBox	AgentBehaviorManager		x		
SandBox	TreeNodeTablut		x		
SandBox	Tile		x		
SandBox	SandBoxMissionManager		x		
SandBox	LordConversationsCampaignBehavior		x		
SandBox	TournamentJoustingMissionController				x
Storymode	AchievementsCampaignBehavior		x		
Storymode	StoryModNotableSpawnModel		x		
StoryMode	RescueFamilyQuestBehavior		x		
StoryMode	ConspiracyQuestBase		x		x
StoryMode	StoryModeAgentDecideKilledOrUnconsciousModel		x		
StoryMode	StoryModeHeroDeathProbabilityCalculationModel		x		
StoryMode	StoryModePartySizeLimitModel		x		
StoryMode	StoryMode				x
StoryMode	StoryModeEvents				x
StoryMode	StoryModeHeroes				x
StoryMode	TutorialQuestPhase				x
TaleWorlds.CampaignSystem.ViewModelCollection	CampaignUIHelper				x
TaleWorlds.CampaignSystem.ViewModelCollection	DeclareWarDecisionItemVM		x		
TaleWorlds.CampaignSystem.ViewModelCollection	KingdomPolicyDecisionVM		x		
TaleWorlds.CampaignSystem.ViewModelCollection	KingSelectionDecisionVM		x		
TaleWorlds.CampaignSystem.ViewModelCollection	MakePeaceDecisionVM		x		
TaleWorlds.CampaignSystem.ViewModelCollection	QuestNotificationItemVM		x		
TaleWorlds.CampaignSystem.ViewModelCollection	SettlementOwnerChangedNotificationItemVM		x		
TaleWorlds.CampaignSystem.ViewModelCollection	SettlementUnderSiegeMapNotificationItemVM		x		
TaleWorlds.CampaignSystem	PlayerCaptivityCampaignBehavior		x		
TaleWorlds.CampaignSystem	PrisonerCaptureCampaignBehavior		x		
TaleWorlds.CampaignSystem	DefaultMilitaryPowerModel		x		
TaleWorlds.CampaignSystem	DefaultPrisonerDonationModel		x		
TaleWorlds.CampaignSystem	DefaultHeirSelectionCalculationModel		x		
TaleWorlds.CampaignSystem	DefaultSettlementGarrisonModel		x		
TaleWorlds.CampaignSystem	DefaultEncounter		x		
TaleWorlds.CampaignSystem	PlayerTownVisit		x		
TaleWorlds.CampaignSystem	AiPartyThinkBehavior		x		
TaleWorlds.CampaignSystem	PartyHealCampaignBehavior		x		
TaleWorlds.CampaignSystem	DefaultSettlementTaxModel		x		
TaleWorlds.CampaignSystem	SandboxStrikeMagnitudeModel		x		
TaleWorlds.CampaignSystem	GangLeaderNeedsRecruitsIssueBehavior		x		
TaleWorlds.CampaignSystem	HeadmanVillageNeedsDraughtAnimalsIssueBehavior		x		
TaleWorlds.CampaignSystem	LandLordNeedsManualLaborersIssueBehavior		x		
TaleWorlds.CampaignSystem	LandLordTheArtOfTheTradeIssueBehavior		x		
TaleWorlds.CampaignSystem	LesserNobleRevoltIssueBehavior		x		
TaleWorlds.CampaignSystem	MerchantArmyOfPoachersIssueBehavior		x		
TaleWorlds.CampaignSystem	MainPartyCurrentAction		x		
TaleWorlds.CampaignSystem	BarterGroup				x
TaleWorlds.CampaignSystem	BarterManager				x
TaleWorlds.CampaignSystem	Campaign			x	
TaleWorlds.CampaignSystem	CampaignEventDispatcher				x
TaleWorlds.CampaignSystem	CampaignEventReceiver				x
TaleWorlds.CampaignSystem	CampaignEvents				x
TaleWorlds.CampaignSystem	CampaignPeriodicEventManager		x	x	
TaleWorlds.CampaignSystem	SkillLevelingManager				x
TaleWorlds.CampaignSystem	SettlementEconomyModel				x
TaleWorlds.CampaignSystem	ConversationTagHelper		x		
TaleWorlds.CampaignSystem	KingdomPolicyDecision				x
TaleWorlds.CampaignSystem	MobilePartyHelper				x
TaleWorlds.CampaignSystem	BuildingHelper				x
TaleWorlds.CampaignSystem	Hero			x	
TaleWorlds.CampaignSystem	InventoryLogic.PartyEquipment	x			
TaleWorlds.CampaignSystem	ItemData		x		
TaleWorlds.CampaignSystem	TownMarketData				x
TaleWorlds.CampaignSystem	EmissarySystemCampaignBehavior		x		
TaleWorlds.CampaignSystem	IMapTracksCampaignBehavior		x		
TaleWorlds.CampaignSystem	RaidAnEnemyTerritoryIssue		x		
TaleWorlds.CampaignSystem	Town				x
TaleWorlds.CampaignSystem	Equipment.EquipmentType		x		
TaleWorlds.MountAndBlade.CustomBattle	ArmyCompositionGroupVM				x
TaleWorlds.MountAndBlade.CustomBattle	CustomBattleMenuVM				x
TaleWorlds.MountAndBlade.GauntletUI	MultiplayerIntermissionNextMapImageWidget		x		
TaleWorlds.MountAndBlade.GauntletUI	BrightnessDemoTextureProvider		x		
TaleWorlds.MountAndBlade.GauntletUI	MissionGauntletBoundaryCrossingView		x		
TaleWorlds.MountAndBlade.GauntletUI	MissionGauntletLeaveView		x		
TaleWorlds.MountAndBlade.View	CraftedDataViewManager		x		
TaleWorlds.MountAndBlade.View	FormationIndicatorMissionView.Indicator		x		
TaleWorlds.MountAndBlade.View	NodeComparer		x		
TaleWorlds.MountAndBlade.View	BannerVisualCreator		x		
TaleWorlds.MountAndBlade	Agent				x
TaleWorlds.MountAndBlade	AgentComponent				x
TaleWorlds.MountAndBlade	HumanAIComponent.BehaviorValues				x
TaleWorlds.MountAndBlade	AgentComponentExtensions				x
TaleWorlds.MountAndBlade	BehaviorComponent				x
TaleWorlds.MountAndBlade	BehaviorDefend				x
TaleWorlds.MountAndBlade	BehaviorDefendSiegeWeapon				x
TaleWorlds.MountAndBlade	BehaviorDefensiveRing				x
TaleWorlds.MountAndBlade	BehaviorDestroySiegeWeapons				x
TaleWorlds.MountAndBlade	ColumnFormation				x
TaleWorlds.MountAndBlade	LineFormation				x
TaleWorlds.MountAndBlade	DetachmentData		x		
TaleWorlds.MountAndBlade	DetachmentManager				x
TaleWorlds.MountAndBlade	OrderController				x
TaleWorlds.MountAndBlade	FacingOrder.FacingOrderEnum		x		
TaleWorlds.MountAndBlade	FacingOrder				x
TaleWorlds.MountAndBlade	FiringOrder.RangedWeaponUsageOrderEnum		x		
TaleWorlds.MountAndBlade	FiringOrder				x
TaleWorlds.MountAndBlade	FormOrder.FormOrderEnum		x		
TaleWorlds.MountAndBlade	FormOrder				x
TaleWorlds.MountAndBlade	RidingOrder.RidingOrderEnum		x		
TaleWorlds.MountAndBlade	RidingOrder				x
TaleWorlds.MountAndBlade	WeaponUsageOrder.WeaponUsageOrderEnum		x		
TaleWorlds.MountAndBlade	WeaponUsageOrder				x
TaleWorlds.MountAndBlade	CastleGateAI				x
TaleWorlds.MountAndBlade	RangedSiegeWeaponAi.ThreatSeeker				x
TaleWorlds.MountAndBlade	StrategicArea				x
TaleWorlds.MountAndBlade	TacticBreachWalls				x
TaleWorlds.MountAndBlade	TacticComponent				x
TaleWorlds.MountAndBlade	TacticDefendCastle.TacticState		x		
TaleWorlds.MountAndBlade	TacticDefendCastle				x
TaleWorlds.MountAndBlade	SiegeQuerySystem				x
TaleWorlds.MountAndBlade	TeamQuerySystem				x
TaleWorlds.MountAndBlade	ArcherPosition		x		x
TaleWorlds.MountAndBlade	AttackEntityOrderDetachment				x
TaleWorlds.MountAndBlade	SiegeLane		x		x
TaleWorlds.MountAndBlade	SiegeLane.LaneStateEnum		x		
TaleWorlds.MountAndBlade	SiegeLane.LaneDefenseStates		x		
TaleWorlds.MountAndBlade	TeamAIComponent				x
TaleWorlds.MountAndBlade	TeamAISallyOutAttacker				x
TaleWorlds.MountAndBlade	TeamAISallyOutDefender				x
TaleWorlds.MountAndBlade	TeamAISiegeAttacker				x
TaleWorlds.MountAndBlade	TeamAISiegeComponent				x
TaleWorlds.MountAndBlade	TeamAISiegeDefender				x
TaleWorlds.MountAndBlade	Formation				x
TaleWorlds.MountAndBlade	FormationDeploymentOrder				x
TaleWorlds.MountAndBlade	FormationDeploymentPlan				x
TaleWorlds.MountAndBlade	MissionAgentSpawnLogic				x
TaleWorlds.MountAndBlade	Mission				x
TaleWorlds.MountAndBlade	MissionTime				x
TaleWorlds.MountAndBlade	MissionTimeTracker		x		x
TaleWorlds.MountAndBlade	Mission.TeamCollection				x
TaleWorlds.MountAndBlade	MissionWeapon				x
TaleWorlds.MountAndBlade	ChatBox				x
TaleWorlds.MountAndBlade	CastleGate				x
TaleWorlds.MountAndBlade	DestructableComponent				x
TaleWorlds.MountAndBlade	SiegeWeaponMovementComponent				x
TaleWorlds.MountAndBlade	StandingPoint				x
TaleWorlds.MountAndBlade	StandingPointWithWeaponRequirement				x
TaleWorlds.MountAndBlade	TacticalPosition				x
TaleWorlds.MountAndBlade	UsableMachine				x
TaleWorlds.MountAndBlade	Team				x
TaleWorlds.MountAndBlade	WeaponData				x
TaleWorlds.MountAndBlade	WeaponInfo				x
TaleWorlds.MountAndBlade	MovementPath		x		
TaleWorlds.MountAndBlade	MovementOrder				x
TaleWorlds.MountAndBlade	MovementOrder.MovementOrderEnum		x		
TaleWorlds.MountAndBlade	MovementOrder.MovementStateEnum		x		
TaleWorlds.MountAndBlade	ArrangementOrder				x
TaleWorlds.GauntletUI	FloatInputTextWidget		x		
TaleWorlds.GauntletUI.PrefabSystem	CustomWidgetType		x		
Please note that classes that inherit from these are not specifically mentioned, but are still affected by the change of their parent class. This is also only a first pass and we will continue to review the code base as we develop the game and the modding community highlights limitations that you are facing.

Thank you everyone for your continued feedback and requests. If you have any questions or would like to make further requests, please discuss
