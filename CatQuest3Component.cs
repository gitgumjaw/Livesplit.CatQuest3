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

        private uint _lastChoiceYesRelayOnceCount;

        private bool _lastOverwriteConfirmationCandidate;

        // ============================================================
        // CONTINUE START STATE
        // ============================================================

        private bool _sceneStartHandledForCurrentCommand;

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
        // CONTINUE START DETECTION
        // ============================================================

        private bool IsContinueStart()
        {
            // StartingGameMode.LOAD = 0.

            if (
                _gameState.StartingGameMode != 0
            )
            {
                return false;
            }

            if (
                !_gameState.HasChangeSceneCommand
            )
            {
                return false;
            }

            if (
                _sceneStartHandledForCurrentCommand
            )
            {
                return false;
            }

            return
                _gameState.CurrentSceneName ==
                    "TitleScene" &&
                _gameState.TargetSceneName ==
                    "MainOverworld" &&
                _gameState.SceneChangeProcessStarted;
        }

        private void CheckContinueStart()
        {
            if (!IsContinueStart())
            {
                return;
            }

            _sceneStartHandledForCurrentCommand =
                true;

            Trace.WriteLine(
                "CONTINUE START"
            );

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

            bool continueStart =
                IsContinueStart();

            if (
                !overwriteStart &&
                !emptySlotStart &&
                !continueStart
            )
            {
                return;
            }

            string reason;

            if (overwriteStart)
            {
                reason =
                    "OVERWRITE";
            }
            else if (emptySlotStart)
            {
                reason =
                    "EMPTY SLOT";
            }
            else
            {
                reason =
                    "CONTINUE";

                _sceneStartHandledForCurrentCommand =
                    true;
            }

            Trace.WriteLine(
                "ANY START - " +
                reason
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
                    "CHEST OPENED | GUID: " +
                    guid
                );
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