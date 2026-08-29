using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace LiveSplit.CatQuest3
{
    public class CatQuest3Component : IComponent
    {
        private readonly LiveSplitState _state;
        private readonly TimerModel _timerModel;

        private readonly MemoryManager _memory;
        private readonly CatQuest3State _gameState;

        private readonly CatQuest3Settings _settings;

        private bool _memoryInitialized;

        // ============================================================
        // HISTORY
        // ============================================================

        private bool _historyReady;

        private bool _lastHaltNavigation;

        private int _haltNavigationFrames;
        private bool _haltBeganWithMatchingIndices;

        private uint _lastChoiceYesRelayOnceCount;

        private bool _lastOverwriteConfirmationCandidate;

        // ============================================================
        // CONTINUE START STATE
        // ============================================================

        private bool _sceneStartHandledForCurrentCommand;

        private static readonly TimeSpan LoadScreenStartRealtimeCorrection =
            TimeSpan.FromSeconds(2.10);

        // ============================================================
        // CONSTRUCTOR
        // ============================================================

        public CatQuest3Component(
            LiveSplitState state)
        {
            _state =
                state;

            _timerModel =
                new TimerModel()
                {
                    CurrentState = state
                };

            _memory =
                new MemoryManager();

            _gameState =
                new CatQuest3State(
                    _memory,
                    0
                );

            _settings =
                new CatQuest3Settings();

            Trace.WriteLine(
                "CAT QUEST III AUTOSPLITTER LOADED"
            );
        }

        // ============================================================
        // COMPONENT INFO
        // ============================================================

        public string ComponentName =>
            "Cat Quest III Autosplitter";

        public System.Collections.Generic.IDictionary<string, Action>
            ContextMenuControls
        {
            get
            {
                return null;
            }
        }

        // ============================================================
        // SETTINGS
        // ============================================================

        public Control GetSettingsControl(
            LayoutMode mode)
        {
            return _settings;
        }

        public XmlNode GetSettings(
            XmlDocument document)
        {
            XmlElement settings =
                document.CreateElement(
                    "Settings"
                );

            XmlElement startTrigger =
                document.CreateElement(
                    "StartTrigger"
                );

            startTrigger.InnerText =
                ((int)_settings.StartTrigger)
                .ToString();

            settings.AppendChild(
                startTrigger
            );

            return settings;
        }

        public void SetSettings(
            XmlNode settings)
        {
            if (settings == null)
            {
                return;
            }

            XmlNode startTriggerNode =
                settings.SelectSingleNode(
                    "StartTrigger"
                );

            if (startTriggerNode == null)
            {
                return;
            }

            if (
                !int.TryParse(
                    startTriggerNode.InnerText,
                    out int modeValue
                )
            )
            {
                return;
            }

            if (
                !Enum.IsDefined(
                    typeof(StartTriggerMode),
                    modeValue
                )
            )
            {
                return;
            }

            _settings.StartTrigger =
                (StartTriggerMode)modeValue;
        }

        // ============================================================
        // MAIN UPDATE
        // ============================================================

        public void Update(
            IInvalidator invalidator,
            LiveSplitState state,
            float width,
            float height,
            LayoutMode mode)
        {
            // --------------------------------------------------------
            // ATTACH TO GAME
            // --------------------------------------------------------

            if (!_memory.IsAttached)
            {
                if (_memory.TryAttach())
                {
                    Trace.WriteLine(
                        "CAT QUEST III FOUND - PID: " +
                        _memory.Game.Id
                    );

                    _memoryInitialized =
                        false;

                    ResetHistory();
                }

                return;
            }

            // --------------------------------------------------------
            // INITIALIZE MEMORY
            // --------------------------------------------------------

            if (!_memoryInitialized)
            {
                if (!InitializeMemory())
                {
                    return;
                }

                ResetHistory();

                _memoryInitialized =
                    true;

                Trace.WriteLine(
                    "MEMORY INITIALIZATION COMPLETE"
                );
            }

            // --------------------------------------------------------
            // READ CURRENT GAME STATE
            // --------------------------------------------------------

            _gameState.Update();

            UpdateHaltNavigationState();

            // --------------------------------------------------------
            // FIRST VALID READ = BASELINE ONLY
            // --------------------------------------------------------

            if (!_historyReady)
            {
                UpdateHistory();

                _historyReady =
                    true;

                return;
            }

            // --------------------------------------------------------
            // AUTO START
            // --------------------------------------------------------

            CheckStartTrigger();

            // --------------------------------------------------------
            // EXISTING SPLITS
            // --------------------------------------------------------

            CheckShipKeySplit();

            // --------------------------------------------------------
            // SAVE HISTORY
            // --------------------------------------------------------

            UpdateHistory();

            // --------------------------------------------------------
            // CHEST DEBUG - ISOLATED
            // --------------------------------------------------------
            //
            // Everything above this point is the exact known-good
            // start-of-session update path. Chest memory scanning occurs
            // only after start detection and history are finished.

            try
            {
                _gameState.UpdateChestState();

                CheckChestDebug();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(
                    "CHEST DEBUG ERROR | " +
                    ex.Message
                );
            }
        }

        // ============================================================
        // MEMORY INITIALIZATION
        // ============================================================

        private bool InitializeMemory()
        {
            // --------------------------------------------------------
            // SaveGameKeyData getter
            // --------------------------------------------------------

            const string savePattern =
                "55 8B EC 57 83 EC 14 " +
                "BA ?? ?? ?? ?? " +
                "8D 6D 00 " +
                "E8 ?? ?? ?? ?? " +
                "8B C8 39 09 8B 40 18 " +
                "8B C8 8B F9 85 C0 75 04 " +
                "33 C0 EB 03 8B 47 0C";

            IntPtr saveGetter =
                _memory.FindSignature(
                    savePattern
                );

            Trace.WriteLine(
                "SaveGameKeyData getter: 0x" +
                saveGetter
                    .ToInt64()
                    .ToString("X")
            );

            if (
                saveGetter ==
                IntPtr.Zero
            )
            {
                return false;
            }

            // --------------------------------------------------------
            // Generic context
            // --------------------------------------------------------

            uint genericContext =
                _memory.ReadUInt32(
                    IntPtr.Add(
                        saveGetter,
                        0x08
                    )
                );

            Trace.WriteLine(
                "Generic context: 0x" +
                genericContext.ToString("X")
            );

            if (genericContext == 0)
            {
                return false;
            }

            _gameState.SetGenericContext(
                genericContext
            );

            // --------------------------------------------------------
            // Contexts.sharedInstance
            // --------------------------------------------------------

            uint contextsStaticStorage =
                _memory.FindContextsStaticStorage();

            Trace.WriteLine(
                "Contexts static storage: 0x" +
                contextsStaticStorage
                    .ToString("X")
            );

            if (
                contextsStaticStorage == 0
            )
            {
                return false;
            }

            _gameState.SetContextsStaticStorage(
                contextsStaticStorage
            );

            // TransitToGame is intentionally NOT required for initialization.
            // Mono may not JIT this method until an overwrite flow is used.
            // It is located later, only when an overwrite-related event occurs.

            return true;
        }

        // ============================================================
        // START TRIGGER ROUTER
        // ============================================================

        private void CheckStartTrigger()
        {
            if (
                _timerModel
                    .CurrentState
                    .CurrentPhase !=
                TimerPhase.NotRunning
            )
            {
                return;
            }

            switch (_settings.StartTrigger)
            {
                case StartTriggerMode.Manual:

                    return;

                case StartTriggerMode.Any:

                    CheckAnyStart();

                    return;

                case StartTriggerMode.Overwrite:

                    CheckOverwriteStart();

                    return;

                case StartTriggerMode.EmptySlot:

                    CheckEmptySlotStart();

                    return;

                case StartTriggerMode.Continue:

                    CheckContinueStart();

                    return;
            }
        }

        // ============================================================
        // OVERWRITE START DETECTION
        // ============================================================

        private bool IsOverwriteStart()
        {
            // Opening an overwrite confirmation adds one one-shot YES
            // listener to MessagePanel.onChoiceYesEvent.
            //
            // Confirming YES consumes that listener (1 -> 0).
            // In testing, cancelling the overwrite did not consume it,
            // and Delete Save did not use this same relay path.
            //
            // Use the PREVIOUS frame's confirmation identity so this still
            // works if the selected save has already begun changing on the
            // same frame that YES consumes the listener.

            bool yesListenerConsumed =
                _lastChoiceYesRelayOnceCount > 0 &&
                _gameState.ChoiceYesRelayOnceCount == 0;

            return
                yesListenerConsumed &&
                _lastOverwriteConfirmationCandidate &&
                _gameState.IsConfirmationPanelShowing;
        }

        private void CheckOverwriteStart()
        {
            if (!IsOverwriteStart())
            {
                return;
            }

            Trace.WriteLine(
                "OVERWRITE START"
            );

            _timerModel.Start();
        }

        // ============================================================
        // EMPTY-SLOT START DETECTION
        // ============================================================

        private bool IsEmptySlotStart()
        {
            bool haltJustStarted =
                !_lastHaltNavigation &&
                _gameState.HaltNavigation;

            return
                _gameState.IsStartingNewGame &&
                _gameState.SelectedSaveIsNewSave &&
                _gameState.SaveIndicesMatch &&
                haltJustStarted;
        }

        private void CheckEmptySlotStart()
        {
            if (!IsEmptySlotStart())
            {
                return;
            }

            Trace.WriteLine(
                "EMPTY-SLOT START"
            );

            _timerModel.Start();
        }

        // ============================================================
        // CONTINUE / LOAD-SCREEN START DETECTION
        // ============================================================

        private bool HasUnconsumedTitleSceneTransition()
        {
            return
                _gameState.StartingGameMode == 0 &&
                _gameState.HasChangeSceneCommand &&
                !_sceneStartHandledForCurrentCommand &&
                _gameState.CurrentSceneName ==
                    "TitleScene" &&
                !string.IsNullOrEmpty(
                    _gameState.TargetSceneName) &&
                _gameState.TargetSceneName !=
                    "TitleScene" &&
                _gameState.SceneChangeProcessStarted;
        }

        private bool IsLoadScreenStart()
        {
            // LOAD SCREEN -> selected existing save
            //
            // A real save-slot Load keeps SaveLoadPanel navigation halted
            // through the later, reliable title-scene transition. This is
            // used only to CLASSIFY the transition as a Load Screen start.
            //
            // We intentionally do not use SaveIndicesMatch here. Carousel
            // orientation made that value unreliable for slots 4-6.
            //
            // Returning from gameplay to the title can also halt navigation,
            // but its target scene is TitleScene, so it cannot satisfy the
            // title -> non-title transition above.

            return
                HasUnconsumedTitleSceneTransition() &&
                _gameState.HaltNavigation &&
                !_gameState.SelectedSaveIsNewSave;
        }

        private bool IsContinueButtonStart()
        {
            // PLAIN TITLE-SCREEN CONTINUE
            //
            // HasUnconsumedTitleSceneTransition() already guarantees a
            // TitleScene -> non-TitleScene transition. Plain Continue can
            // restore the save into ANY saved scene (overworld, cave,
            // dungeon, interior, etc.), so the target must not be restricted
            // to MainOverworld.
            //
            // Callers MUST check IsLoadScreenStart() first. A Load Screen
            // transition is classified by SaveLoadPanel navigation still
            // being halted at this same reliable scene-transition point.

            return
                HasUnconsumedTitleSceneTransition();
        }

        private void StartLoadScreenWithRealtimeCorrection()
        {
            _timerModel.Start();

            // The reliable Load Screen scene/fade signal arrives about
            // 2.10 seconds after the player's Load activation.
            //
            // Keep this run as REAL TIME. Moving both real-time start
            // timestamps backward makes LiveSplit begin at +2.10 seconds
            // and then continue as ordinary wall-clock Real Time.
            //
            // Game Time is deliberately untouched so a future load remover
            // can manage Game Time independently.
            _state.AdjustedStartTime =
                _state.AdjustedStartTime -
                LoadScreenStartRealtimeCorrection;

            _state.StartTimeWithOffset =
                _state.StartTimeWithOffset -
                LoadScreenStartRealtimeCorrection;

            Trace.WriteLine(
                "LOAD SCREEN REAL-TIME CORRECTION | +" +
                LoadScreenStartRealtimeCorrection.TotalSeconds.ToString("F2") +
                " seconds"
            );
        }

        private void CheckContinueStart()
        {
            // The user-facing "Continue" setting covers both ways of loading
            // an existing save, but they are kept separate internally:
            //
            //   CONTINUE BUTTON START = plain title-screen Continue
            //   LOAD SCREEN START     = Load menu -> chosen save
            //
            // Load Screen MUST be checked first because a Load into
            // MainOverworld also matches the plain Continue scene pattern.

            bool loadScreenStart =
                IsLoadScreenStart();

            bool continueButtonStart =
                !loadScreenStart &&
                IsContinueButtonStart();

            if (
                !loadScreenStart &&
                !continueButtonStart
            )
            {
                return;
            }

            _sceneStartHandledForCurrentCommand =
                true;

            if (loadScreenStart)
            {
                Trace.WriteLine(
                    "LOAD SCREEN START"
                );

                StartLoadScreenWithRealtimeCorrection();

                return;
            }

            Trace.WriteLine(
                "CONTINUE BUTTON START"
            );

            // Preserve plain Continue exactly: normal Real Time beginning
            // at zero, with no +2.10 second Load Screen correction.
            _timerModel.Start();
        }

        // ============================================================
        // ANY START
        // ============================================================

        private void CheckAnyStart()
        {
            bool overwriteStart =
                IsOverwriteStart();

            bool emptySlotStart =
                IsEmptySlotStart();

            bool loadScreenStart =
                IsLoadScreenStart();

            bool continueButtonStart =
                !loadScreenStart &&
                IsContinueButtonStart();

            if (
                !overwriteStart &&
                !emptySlotStart &&
                !loadScreenStart &&
                !continueButtonStart
            )
            {
                return;
            }

            if (overwriteStart)
            {
                Trace.WriteLine(
                    "ANY START - OVERWRITE"
                );

                _timerModel.Start();

                return;
            }

            if (emptySlotStart)
            {
                Trace.WriteLine(
                    "ANY START - EMPTY SLOT"
                );

                _timerModel.Start();

                return;
            }

            _sceneStartHandledForCurrentCommand =
                true;

            if (loadScreenStart)
            {
                Trace.WriteLine(
                    "ANY START - LOAD SCREEN"
                );

                StartLoadScreenWithRealtimeCorrection();

                return;
            }

            Trace.WriteLine(
                "ANY START - CONTINUE BUTTON"
            );

            _timerModel.Start();
        }

        // ============================================================
        // SHIP KEY SPLIT
        // ============================================================

        private void CheckShipKeySplit()
        {
            if (
                !_gameState.ShipKeyObtained
            )
            {
                return;
            }

            Trace.WriteLine(
                "SHIP KEY OBTAINED"
            );

            if (
                _timerModel
                    .CurrentState
                    .CurrentPhase ==
                TimerPhase.NotRunning
            )
            {
                Trace.WriteLine(
                    "SHIP KEY SPLIT IGNORED - TIMER NOT RUNNING"
                );

                return;
            }

            Trace.WriteLine(
                "SHIP KEY OBTAINED - SPLIT"
            );

            _timerModel.Split();
        }

        // ============================================================
        // CHEST DEBUG
        //
        // No timer action yet.
        //
        // CatQuest3State baselines all chest GUIDs already present in
        // the active save and exposes only GUIDs added during the most
        // recent update.
        // ============================================================

        private void CheckChestDebug()
        {
            if (
                _gameState.NewlyOpenedChestGuids ==
                null
            )
            {
                return;
            }

            foreach (
                string guid
                in _gameState.NewlyOpenedChestGuids
            )
            {
                Trace.WriteLine(
                    "CHEST OPENED | " +
                    ChestCatalog.GetDisplayName(
                        guid
                    ) +
                    " | GUID: " +
                    guid
                );
            }
        }

        // ============================================================
        // HALT NAVIGATION STATE
        // ============================================================

        private void UpdateHaltNavigationState()
        {
            if (!_gameState.HaltNavigation)
            {
                _haltNavigationFrames =
                    0;

                _haltBeganWithMatchingIndices =
                    false;

                return;
            }

            if (!_lastHaltNavigation)
            {
                _haltNavigationFrames =
                    1;

                _haltBeganWithMatchingIndices =
                    _gameState.SaveIndicesMatch;

                return;
            }

            if (_haltNavigationFrames < 1000000)
            {
                _haltNavigationFrames++;
            }
        }

        // ============================================================
        // HISTORY
        // ============================================================

        private void UpdateHistory()
        {
            _lastHaltNavigation =
                _gameState.HaltNavigation;

            _lastChoiceYesRelayOnceCount =
                _gameState.ChoiceYesRelayOnceCount;

            _lastOverwriteConfirmationCandidate =
                _gameState.IsConfirmationPanelShowing &&
                _gameState.ChoiceYesRelayOnceCount > 0 &&
                _gameState.IsStartingNewGame &&
                !_gameState.SelectedSaveIsNewSave &&
                _gameState.SaveIndicesMatch;

            if (
                !_gameState.HasChangeSceneCommand
            )
            {
                _sceneStartHandledForCurrentCommand =
                    false;
            }
        }

        private void ResetHistory()
        {
            _historyReady =
                false;

            _lastHaltNavigation =
                false;

            _haltNavigationFrames =
                0;

            _haltBeganWithMatchingIndices =
                false;

            _lastChoiceYesRelayOnceCount =
                0;

            _lastOverwriteConfirmationCandidate =
                false;

            _sceneStartHandledForCurrentCommand =
                false;
        }

        // ============================================================
        // DRAWING
        // ============================================================

        public void DrawHorizontal(
            Graphics graphics,
            LiveSplitState state,
            float height,
            Region clipRegion)
        {
        }

        public void DrawVertical(
            Graphics graphics,
            LiveSplitState state,
            float width,
            Region clipRegion)
        {
        }

        public float HorizontalWidth =>
            0;

        public float MinimumHeight =>
            0;

        public float VerticalHeight =>
            0;

        public float MinimumWidth =>
            0;

        public float PaddingTop =>
            0;

        public float PaddingBottom =>
            0;

        public float PaddingLeft =>
            0;

        public float PaddingRight =>
            0;

        // ============================================================
        // CLEANUP
        // ============================================================

        public void Dispose()
        {
            if (_settings != null)
            {
                _settings.Dispose();
            }
        }
    }
}