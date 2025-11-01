# Acceptance Tests

This document outlines the manual testing procedures required to verify feature correctness before merging.

## Feature: Propose Decision End-to-End Flow

**Goal:** Verify that a new decision can be proposed via the dev tools, is correctly persisted after a save/load cycle, and the UI state is updated.

**Run this test 3 times.**

### Test Steps:

1.  **Start Game & Load Save:**
    *   Launch the game and load a campaign save file.

2.  **Create a Council:**
    *   Open the developer console.
    *   Execute the command to create a new council for a kingdom (e.g., the player's kingdom).
    *   **Expected:** The log shows a confirmation that a new council has been created with a unique `councilId`.

3.  **Propose a Decision:**
    *   Note the `councilId` from the previous step.
    *   Execute the command to propose a new decision, providing the `councilId`, a title, a description, and a JSON string as the payload.
        *   Example Payload: `"{ 'target': 'faction_vlandia', 'value': -10 }"`
    *   **Expected:**
        *   The log shows a confirmation that the decision proposal request was sent.
        *   The log shows that the `WarDecisionService` received the request and noted that a payload was attached (`Payload attached: True`).
        *   The UI (if a debug view is open) should update to reflect the new active decision count for the relevant council.

4.  **Save the Game:**
    *   Save the game to a new slot.

5.  **Load the Game:**
    *   Load the save file you just created.

6.  **Verify State:**
    *   Using the developer console or debug UI, inspect the council that was created in step 2.
    *   **Expected:**
        *   The council exists and has not been duplicated.
        *   The decision proposed in step 3 exists within that council.
        *   The decision's `ExecutionPayload.RawPayload` field matches the string provided in step 3 exactly.
        *   The decision's status is "Proposed".

**Pass Criteria:** All expected outcomes are met across all three test runs. No errors are logged, and no data is lost or duplicated.
