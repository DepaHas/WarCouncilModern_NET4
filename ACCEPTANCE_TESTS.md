# Acceptance Tests

This document outlines the manual acceptance tests to be performed before submitting changes related to the core architecture refactoring.

## Test Case 1: Create -> Propose -> Vote -> Tally -> Save -> Load -> Verify

**Objective:** Verify the end-to-end lifecycle of a council and a decision, including persistence.

1.  **Start Game:** Launch a new campaign.
2.  **Verify Council Creation:** A council for the player's kingdom should be created automatically on game load.
    *   *Expected:* Log messages indicating the creation of the council and the addition of members.
3.  **Propose Decision (Manual Trigger):** From a debug console or test menu, trigger `WarDecisionService.ProposeDecision` for the player council.
    *   *Expected:* Log messages indicating the decision was proposed.
4.  **Cast Votes (Manual Trigger):** Trigger `WarDecisionService.RecordVote` for several members of the council. Cast a majority of 'Yea' votes.
    *   *Expected:* Log messages for each vote recorded.
5.  **Process Decision (Manual Trigger):** Trigger `WarDecisionService.ProcessDecision`.
    *   *Expected:* Log messages indicating the decision was 'Approved' and then 'Executed' by the `LogExecutionHandler`. The `OnDecisionExecuted` event should fire.
6.  **Save Game:** Save the game after the decision has been executed.
7.  **Load Game:** Load the saved game.
8.  **Verify State:**
    *   *Expected:* The game loads without errors. The `RebuildReferencesAfterLoad` logic runs successfully (check logs).
    *   *Expected:* The player council still exists and is not duplicated.
    *   *Expected:* The executed decision is present in the council's decision list with the correct status ('Executed') and all votes are intact.

## Test Case 2: Auto-Decision Processing Feature Flag

**Objective:** Verify that the `AutoDecisionProcessing` feature flag correctly bypasses the voting process.

1.  **Enable Feature:** In `Core/State/FeatureRegistry.cs`, temporarily change `IsEnabled` to return `true`.
2.  **Start Game:** Launch a new campaign.
3.  **Propose Decision (Manual Trigger):** As in Test Case 1, propose a new decision.
4.  **Process Decision (Manual Trigger):** Trigger `WarDecisionService.ProcessDecision`.
    *   *Expected:* Log messages should indicate that the decision is being auto-processed due to the feature flag.
    *   *Expected:* The decision status should immediately become 'Executed' without any votes being cast.
5.  **Cleanup:** Revert the change in `FeatureRegistry.cs`.

## Test Case 3: Duplicate Council Prevention

**Objective:** Verify that starting a council for the same kingdom twice does not create a duplicate.

1.  **Start Game:** Launch a new campaign. A council is created automatically.
2.  **Trigger Council Creation Again (Manual Trigger):** From a debug console, call `CouncilService.StartCouncilForKingdom` for the player's kingdom again.
    *   *Expected:* Log messages should indicate that a council for the kingdom already exists.
    *   *Expected:* No new council is created. The existing council is returned. The total number of councils for the kingdom remains 1.
