# WarCouncilModern Project Analysis

This document provides a detailed analysis of the WarCouncilModern project's architecture, components, and data flow.

## 1. Overall Architecture

The project follows a clean, decoupled architecture that emphasizes **Separation of Concerns**. It can be broken down into three main layers:

1.  **Core Layer (Backend):** Contains the primary business logic, data models, and services that drive the mod's features. It is completely independent of the UI.
2.  **UI Layer (Frontend):** Implements the user interface using the **Model-View-ViewModel (MVVM)** pattern. It is designed to be completely decoupled from the backend, communicating exclusively through a dedicated UI service.
3.  **Initialization & Utilities Layer:** Acts as the glue for the application. It includes the mod's entry point (`SubModule`), dependency injection setup, and common utilities like logging and an abstraction layer for the game's API.

## 2. Key Components and Their Responsibilities

### `Initialization`

-   **`SubModule.cs`**: This is the **entry point** of the mod.
    -   **Dependency Injection:** It manually instantiates and wires together all major services, managers, and view models. This process is known as manual Dependency Injection.
    -   **Lifecycle Management:** It hooks into the game's lifecycle events (`OnSubModuleLoad`, `OnGameStart`) to initialize the mod and register campaign behaviors.
    -   **Service Locator:** It exposes key services as static properties, making them globally accessible.

### `CouncilSystem/Behaviors`

-   **`WarCouncilCampaignBehavior.cs`**: This class is the **source of truth for persisted data**.
    -   **Persistence:** It owns the list of `WarCouncil` objects and uses `[SaveableField]` to ensure they are saved and loaded with the player's campaign save file.
    -   **State Management:** All changes to the list of councils (additions, removals) are ultimately performed on the collection within this behavior.
    -   **Lifecycle Events:** It listens to game events like `OnGameLoaded` to perform "rehydration" (restoring object references from saved IDs).

### `Core/Manager`

-   **`WarCouncilManager.cs`**: This is the central **façade** for interacting with the mod's data.
    -   **Business Logic:** It contains the core logic for creating, retrieving, and modifying `WarCouncil` entities and their members/decisions.
    -   **Data Abstraction:** It abstracts away the direct interaction with `WarCouncilCampaignBehavior`. Services and other components talk to the manager, not the behavior.
    -   **Thread Safety:** It uses `lock` statements to ensure that data access is thread-safe.

### `Models/Entities`

-   **`WarCouncil.cs` & `WarDecision.cs`**: These are pure **data objects (Entities)**.
    -   **Data Holders:** They contain the data for councils and decisions but have no complex business logic.
    -   **Persistence:** Fields are marked with `[SaveableField]` to integrate with Bannerlord's save system.
    -   **Data Integrity:** They manage their own internal state (e.g., adding a decision to a council is done via a method `AddDecision` rather than exposing the list directly).

### `UI/Services`

-   **`CouncilUiService.cs`**: This is the **cornerstone of the UI layer**.
    -   **Mediator:** It acts as the sole mediator between the UI (ViewModels) and the backend (managers and services). ViewModels **do not** and **should not** talk to the backend directly.
    -   **UI State Management:** It maintains UI-specific state, such as the `OperationState` (e.g., `Initializing`, `Voting`), which informs the UI what is currently happening.
    -   **UI Thread Safety:** It uses an `IUiInvoker` to ensure that any updates to `ObservableCollection`s happen on the main UI thread, preventing crashes.
    -   **Data Transformation:** It fetches data from the backend (via an `ICouncilProvider`) and transforms it into Data Transfer Objects (`WarCouncilDto`) suitable for the UI.

### `UI/ViewModels`

-   **`CouncilOverviewViewModel.cs`**: A prime example of the **MVVM pattern** in action.
    -   **Data Binding:** It exposes data (like the list of councils) through `ObservableCollection`s, which the UI (View) binds to.
    -   **Commanding:** It would typically expose `ICommand` properties for UI actions (e.g., buttons).
    -   **Decoupling:** It depends only on `ICouncilUiService`, not on any backend components.

### `Utilities`

-   **`GameApi.cs`**: An abstraction layer over the Bannerlord game API.
    -   **Decoupling:** It isolates the rest of the codebase from direct calls to the game's native API. This makes the code easier to test and reason about.
    -   **Single Responsibility:** Its only job is to interact with the game engine to fetch information (e.g., find a hero) or trigger actions (e.g., change relations).
-   **`IModLogger.cs` & `ModLogger.cs`**: A simple abstraction for logging, allowing the logging implementation to be changed without affecting the rest of the code.

## 3. Data Flow Example: Proposing a New Decision

1.  **View (UI):** The user clicks a "Propose" button. This button is bound to a command in a `ViewModel` (e.g., `DecisionDetailViewModel`).
2.  **ViewModel:** The command in the ViewModel calls a method on the `ICouncilUiService`, for example `ProposeDecisionAsync(...)`, passing in UI-level data.
3.  **`CouncilUiService`:**
    -   Sets its `CurrentOperation` to `Proposing` to show a loading indicator in the UI.
    -   Calls the appropriate method on the backend service, `IWarDecisionService.ProposeDecision(...)`.
4.  **`WarDecisionService`:**
    -   Contains the business logic for validating the proposal.
    -   Creates a new `WarDecision` object.
    -   Uses the `IWarCouncilManager` to add the new decision to the correct `WarCouncil`.
5.  **`WarCouncilManager`:**
    -   Retrieves the `WarCouncil` object from the `WarCouncilCampaignBehavior`.
    -   Calls the `AddDecision` method on the `WarCouncil` entity.
6.  **`WarCouncil` Entity:** The new `WarDecision` is added to its internal list of decisions.
7.  **Event System:** After the decision is added, an event `CouncilEvents.OnDecisionProposed` is fired.
8.  **`CouncilUiService` (Reacting):**
    -   It listens for the `OnDecisionProposed` event.
    -   When the event is caught, it uses the `IUiInvoker` to switch to the UI thread.
    -   It updates its `ObservableCollection` of councils/decisions with the new data (as a DTO).
9.  **ViewModel & View (Updating):**
    -   The `ObservableCollection` in the `CouncilOverviewViewModel` automatically updates because it's bound to the collection in the `CouncilUiService`.
    -   The UI automatically reflects the new decision, as it is bound to the ViewModel's collection.
10. **`CouncilUiService` (Finalizing):** The `CurrentOperation` is set back to `None`.
