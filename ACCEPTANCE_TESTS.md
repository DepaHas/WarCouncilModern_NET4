# Acceptance Tests

This document outlines the manual testing procedures required to verify feature correctness before merging.

## Feature: Full Decision Lifecycle (Propose -> Vote -> Tally)

**Goal:** Verify that a new decision can be proposed, voted on, and processed correctly. The state must be correctly persisted after a save/load cycle.

**Run this test 3 times.**

### Test Steps:

1.  **Start Game & Load Save:**
    *   Launch the game and load a campaign save file.

2.  **Create a Council:**
    *   Open the developer console.
    *   Execute the command to create a new council for a kingdom (e.g., `war_council.create_council_for_player_kingdom`).
    *   **Expected:** The log shows a confirmation that a new council has been created with a unique `councilId`. Note this ID.

3.  **Propose a Decision:**
    *   Note the `councilId` from the previous step.
    *   Execute the command to propose a new decision, providing the `councilId`, a title, a description, and a payload.
        *   Example command: `war_council.propose_decision <councilId> "Declare War" "We must declare war on the Vlandians." "{ 'target': 'faction_vlandia' }"`
    *   **Expected:**
        *   The log confirms the proposal request was sent.
        *   The UI state (if visible) updates with the new active decision.
        *   Note the `decisionId` that is logged or displayed.

4.  **Cast Votes:**
    *   Execute the command to cast a "Yea" (true) vote from the player character.
        *   Example: `war_council.cast_vote <councilId> <decisionId> true`
    *   **Expected:** The log confirms the vote request was sent.
    *   (Optional) Cast votes for other heroes if the dev tools support it to test different outcomes.

5.  **Save the Game:**
    *   Save the game to a new slot.

6.  **Load the Game:**
    *   Load the save file you just created.

7.  **Verify State (Pre-Tally):**
    *   Inspect the council and decision created earlier.
    *   **Expected:**
        *   The council and decision exist.
        *   The player's vote is correctly recorded.
        *   The decision's status is still "Proposed" or "VotingOpen".

8.  **Tally the Decision:**
    *   Execute the command to tally and process the decision.
        *   Example: `war_council.tally_decision <councilId> <decisionId>`
    *   **Expected:**
        *   The log confirms the tally request was sent.
        *   A log entry appears showing the decision was processed and its final status (e.g., "Approved" or "Rejected").
        *   The UI state (if visible) updates to show one less active decision.

9.  **Save and Load Again:**
    *   Save the game to a new slot.
    *   Load this new save file.

10. **Verify Final State:**
    *   Inspect the decision.
    *   **Expected:**
        *   The decision's final status ("Approved", "Rejected", "Executed", etc.) is correctly persisted.

**Pass Criteria:** All expected outcomes are met across all three test runs. No errors are logged, and no data is lost or duplicated.
