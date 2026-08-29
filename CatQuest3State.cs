using System.Collections.Generic;

namespace LiveSplit.CatQuest3
{
    public class CatQuest3State
    {
        private readonly MemoryManager _memory;

        private uint _genericContext;
        private uint _contextsStaticStorage;

        private uint _lastSaveGameData;
        private bool _baselineReady;

        // ============================================================
        // CHEST DETECTION
        // ============================================================

        private uint _lastChestSaveGameData;
        private bool _chestBaselineReady;

        private readonly HashSet<string> _knownChestGuids =
            new HashSet<string>();

        public List<string> NewlyOpenedChestGuids
        {
            get;
            private set;
        } = new List<string>();

        // ============================================================
        // SHIP KEY
        // ============================================================

        public bool HasShipKey { get; private set; }

        public bool HadShipKey { get; private set; }

        public bool ShipKeyObtained
        {
            get
            {
                return
                    _baselineReady &&
                    HasShipKey &&
                    !HadShipKey;
            }
        }

        // ============================================================
        // SCENE CHANGE
        // ============================================================

        public bool HasChangeSceneCommand { get; private set; }

        public bool SceneChangeProcessStarted { get; private set; }

        public string CurrentSceneName { get; private set; }

        public string TargetSceneName { get; private set; }

        // ============================================================
        // SAVE LOAD PANEL
        // ============================================================

        public uint SaveLoadPanelAddress { get; private set; }

        public uint StartingGameMode { get; private set; }

        public bool IsStartingNewGame
        {
            get
            {
                return
                    (StartingGameMode & 1) != 0;
            }
        }

        public bool IsStartingMewGame
        {
            get
            {
                return
                    (StartingGameMode & 2) != 0;
            }
        }

        public bool IsStartingNewGamePlus
        {
            get
            {
                return
                    (StartingGameMode & 4) != 0;
            }
        }

        public bool IsConfirmationPanelShowing
        {
            get;
            private set;
        }

        public bool HaltNavigation
        {
            get;
            private set;
        }

        // ============================================================
        // SELECTED SAVE
        // ============================================================

        public int TrackedSaveGameIndex
        {
            get;
            private set;
        }

        public int ScrollSelectedElementIndex
        {
            get;
            private set;
        }

        public bool SaveIndicesMatch
        {
            get
            {
                // The six save slots are presented as two repeating groups
                // of three carousel positions:
                //
                // tracked 0/3 -> scroll 2
                // tracked 1/4 -> scroll 3
                // tracked 2/5 -> scroll 4
                //
                // Using modulo 3 preserves the validated relationship for
                // slots 1-3 and correctly handles slots 4-6.
                return
                    TrackedSaveGameIndex >= 0 &&
                    TrackedSaveGameIndex < 6 &&
                    ScrollSelectedElementIndex >= 0 &&
                    ScrollSelectedElementIndex ==
                        (TrackedSaveGameIndex % 3) + 2;
            }
        }

        public uint LastInteractedSaveSlotState
        {
            get;
            private set;
        }

        public bool SelectedSaveUsesManualSave
        {
            get;
            private set;
        }

        public uint SelectedSaveGameProfileAddress
        {
            get;
            private set;
        }

        public uint SelectedSaveGameDataAddress
        {
            get;
            private set;
        }

        public bool SelectedSaveIsNewSave
        {
            get;
            private set;
        }

        // ============================================================
        // OVERWRITE CONFIRMATION
        // ============================================================

        public uint ChoiceYesRelayOnceCount
        {
            get;
            private set;
        }

        public uint ConfirmationCallbackMethodPtr
        {
            get;
            private set;
        }

        // ============================================================
        // BOSS DEATH DETECTION
        //
        // GameComponentsLookup:
        // AnimationDone = 11
        // BossTrait     = 41
        // UnitConfig    = 391
        // ============================================================

        private bool _bossDeathBaselineReady;

        private readonly HashSet<uint> _knownAnimationDoneBossEntities =
            new HashSet<uint>();

        public List<BossAnimationDoneEvent> NewlyAnimationDoneBosses
        {
            get;
            private set;
        } = new List<BossAnimationDoneEvent>();

        public sealed class BossAnimationDoneEvent
        {
            public uint EntityAddress { get; private set; }
            public uint UnitConfigAddress { get; private set; }
            public string UnitName { get; private set; }

            public BossAnimationDoneEvent(
                uint entityAddress,
                uint unitConfigAddress,
                string unitName)
            {
                EntityAddress = entityAddress;
                UnitConfigAddress = unitConfigAddress;
                UnitName = unitName;
            }
        }

        // ============================================================
        // CONSTRUCTOR / SETUP
        // ============================================================

        public CatQuest3State(
            MemoryManager memory,
            uint genericContext)
        {
            _memory =
                memory;

            _genericContext =
                genericContext;
        }

        public void SetGenericContext(
            uint genericContext)
        {
            _genericContext =
                genericContext;

            _baselineReady =
                false;

            _lastSaveGameData =
                0;

            ResetChestBaseline();
        }

        public void SetContextsStaticStorage(
            uint contextsStaticStorage)
        {
            if (_contextsStaticStorage != contextsStaticStorage)
            {
                ResetBossDeathBaseline();
            }

            _contextsStaticStorage =
                contextsStaticStorage;
        }

        // ============================================================
        // UPDATE
        // ============================================================

        public void Update()
        {
            UpdateShipKeyState();

            UpdateSceneChangeState();

            UpdateSaveLoadPanelState();

            UpdateOverwriteConfirmationState();
        }

        // ============================================================
        // SHIP KEY
        // ============================================================

        private void UpdateShipKeyState()
        {
            uint saveGameData =
                GetSaveGameData();

            if (saveGameData == 0)
            {
                _baselineReady =
                    false;

                _lastSaveGameData =
                    0;

                HasShipKey =
                    false;

                HadShipKey =
                    false;

                return;
            }

            uint obtainedKeys =
                GetObtainedKeys(
                    saveGameData
                );

            bool currentHasShipKey =
                _memory.HasShipKey(
                    obtainedKeys
                );

            if (
                !_baselineReady ||
                saveGameData !=
                    _lastSaveGameData
            )
            {
                HasShipKey =
                    currentHasShipKey;

                HadShipKey =
                    currentHasShipKey;

                _lastSaveGameData =
                    saveGameData;

                _baselineReady =
                    true;

                return;
            }

            HadShipKey =
                HasShipKey;

            HasShipKey =
                currentHasShipKey;
        }

        private uint GetSaveGameData()
        {
            if (_genericContext == 0)
            {
                return 0;
            }

            uint genericData =
                _memory.ReadPointer(
                    _genericContext + 0x24
                );

            if (genericData == 0)
            {
                return 0;
            }

            uint staticStorage =
                _memory.ReadPointer(
                    genericData + 0x04
                );

            if (staticStorage == 0)
            {
                return 0;
            }

            uint saveGameManager =
                _memory.ReadPointer(
                    staticStorage
                );

            if (saveGameManager == 0)
            {
                return 0;
            }

            return _memory.ReadPointer(
                saveGameManager + 0x18
            );
        }

        private uint GetObtainedKeys(
            uint saveGameData)
        {
            uint saveGameKeyData =
                _memory.ReadPointer(
                    saveGameData + 0x0C
                );

            if (saveGameKeyData == 0)
            {
                return 0;
            }

            return _memory.ReadPointer(
                saveGameKeyData + 0x08
            );
        }

        // ============================================================
        // CHEST DETECTION
        //
        // SaveGameData
        // +0x10 -> savedChestData
        //
        // SaveGameChestID
        // +0x08 -> obtainedKeys
        //
        // HashSet<ChestID>
        // +0x0C -> _slots
        // +0x18 -> _count
        // +0x1C -> _lastIndex
        //
        // Slot<ChestID> is a value type:
        //
        // Mono Dissector displayed:
        // +0x08 hashCode
        // +0x0C next
        // +0x10 value
        //
        // Those include the boxed-object header. Inside the array,
        // the actual 12-byte struct layout is:
        //
        // +0x00 hashCode
        // +0x04 next
        // +0x08 value
        //
        // ChestID inherits GenericDatabaseEntry:
        // +0x0C -> Guid
        // ============================================================

        public void UpdateChestState()
        {
            // Chest scanning is intentionally NOT called from Update().
            // The component calls this only after all established start
            // detection and history logic has completed for the frame.
            // This keeps chest debugging isolated from auto-start behavior.
            UpdateChestStateCore();
        }

        private void UpdateChestStateCore()
        {
            NewlyOpenedChestGuids.Clear();

            uint saveGameData =
                GetSaveGameData();

            if (saveGameData == 0)
            {
                ResetChestBaseline();

                return;
            }

            HashSet<string> currentChestGuids;

            if (
                !TryReadChestGuids(
                    saveGameData,
                    out currentChestGuids
                )
            )
            {
                return;
            }

            // --------------------------------------------------------
            // NEW SAVE OBJECT / FIRST VALID READ
            // --------------------------------------------------------
            //
            // Do not report anything already contained in the save.

            if (
                !_chestBaselineReady ||
                saveGameData !=
                    _lastChestSaveGameData
            )
            {
                SetChestBaseline(
                    saveGameData,
                    currentChestGuids
                );

                return;
            }

            // --------------------------------------------------------
            // COLLECTION WAS CLEARED / REPLACED
            // --------------------------------------------------------
            //
            // This can happen when beginning a new file. If the current
            // valid set contains fewer entries than our previous
            // baseline, treat the current set as the new baseline
            // instead of reporting old chests again later.

            if (
                currentChestGuids.Count <
                _knownChestGuids.Count
            )
            {
                SetChestBaseline(
                    saveGameData,
                    currentChestGuids
                );

                return;
            }

            // --------------------------------------------------------
            // FIND NEW CHESTS
            // --------------------------------------------------------

            foreach (
                string guid
                in currentChestGuids
            )
            {
                if (
                    _knownChestGuids.Contains(
                        guid
                    )
                )
                {
                    continue;
                }

                NewlyOpenedChestGuids.Add(
                    guid
                );
            }

            // --------------------------------------------------------
            // CURRENT STATE BECOMES NEW BASELINE
            // --------------------------------------------------------

            _knownChestGuids.Clear();

            foreach (
                string guid
                in currentChestGuids
            )
            {
                _knownChestGuids.Add(
                    guid
                );
            }
        }

        private bool TryReadChestGuids(
            uint saveGameData,
            out HashSet<string> chestGuids)
        {
            chestGuids =
                new HashSet<string>();

            // SaveGameData.savedChestData = +0x10
            uint savedChestData =
                _memory.ReadPointer(
                    saveGameData + 0x10
                );

            if (savedChestData == 0)
            {
                return false;
            }

            // SaveGameChestID.obtainedKeys = +0x08
            uint obtainedKeys =
                _memory.ReadPointer(
                    savedChestData + 0x08
                );

            if (obtainedKeys == 0)
            {
                return false;
            }

            // HashSet<ChestID>._count = +0x18
            int count =
                (int)_memory.ReadUInt32(
                    new System.IntPtr(
                        obtainedKeys + 0x18
                    )
                );

            // HashSet<ChestID>._lastIndex = +0x1C
            int lastIndex =
                (int)_memory.ReadUInt32(
                    new System.IntPtr(
                        obtainedKeys + 0x1C
                    )
                );

            if (
                count < 0 ||
                lastIndex < 0 ||
                count > 10000 ||
                lastIndex > 10000
            )
            {
                return false;
            }

            // Empty HashSet is valid.
            if (lastIndex == 0)
            {
                return true;
            }

            // HashSet<ChestID>._slots = +0x0C
            uint slots =
                _memory.ReadPointer(
                    obtainedKeys + 0x0C
                );

            if (slots == 0)
            {
                return false;
            }

            int validSlots =
                0;

            for (
                int i = 0;
                i < lastIndex;
                i++
            )
            {
                // Mono array data starts at +0x10.
                //
                // Slot<ChestID> is a 12-byte value type.
                uint slot =
                    slots
                    + 0x10u
                    + (uint)(i * 0x0C);

                int hashCode =
                    unchecked(
                        (int)_memory.ReadUInt32(
                            new System.IntPtr(
                                slot
                            )
                        )
                    );

                // Removed/free HashSet slots use a negative hash code.
                if (hashCode < 0)
                {
                    continue;
                }

                // Slot.value = +0x08 within the unboxed struct.
                uint chestId =
                    _memory.ReadPointer(
                        slot + 0x08
                    );

                if (chestId == 0)
                {
                    continue;
                }

                // GenericDatabaseEntry.Guid = +0x0C
                uint guidString =
                    _memory.ReadPointer(
                        chestId + 0x0C
                    );

                if (guidString == 0)
                {
                    continue;
                }

                string guid =
                    _memory.ReadMonoString(
                        guidString
                    );

                if (
                    string.IsNullOrEmpty(
                        guid
                    )
                )
                {
                    continue;
                }

                chestGuids.Add(
                    guid
                );

                validSlots++;
            }

            // If the HashSet says it contains entries but we couldn't
            // recover a single valid value, assume our read was bad
            // rather than replacing the baseline with an empty set.
            if (
                count > 0 &&
                validSlots == 0
            )
            {
                return false;
            }

            return true;
        }

        private void SetChestBaseline(
            uint saveGameData,
            HashSet<string> chestGuids)
        {
            _knownChestGuids.Clear();

            foreach (
                string guid
                in chestGuids
            )
            {
                _knownChestGuids.Add(
                    guid
                );
            }

            _lastChestSaveGameData =
                saveGameData;

            _chestBaselineReady =
                true;
        }

        private void ResetChestBaseline()
        {
            _knownChestGuids.Clear();

            NewlyOpenedChestGuids.Clear();

            _lastChestSaveGameData =
                0;

            _chestBaselineReady =
                false;
        }

        // ============================================================
        // BOSS DEATH DETECTION
        // ============================================================

        public void UpdateBossDeathState()
        {
            NewlyAnimationDoneBosses.Clear();

            uint contexts = GetContexts();
            if (contexts == 0)
            {
                ResetBossDeathBaseline();
                return;
            }

            uint gameContext = _memory.ReadPointer(contexts + 0x1C);
            if (gameContext == 0)
            {
                ResetBossDeathBaseline();
                return;
            }

            HashSet<uint> currentDoneBossEntities = new HashSet<uint>();
            Dictionary<uint, uint> currentUnitConfigs = new Dictionary<uint, uint>();

            if (!TryReadAnimationDoneBosses(gameContext, currentDoneBossEntities, currentUnitConfigs))
            {
                return;
            }

            if (!_bossDeathBaselineReady)
            {
                _knownAnimationDoneBossEntities.Clear();
                foreach (uint entity in currentDoneBossEntities)
                    _knownAnimationDoneBossEntities.Add(entity);

                _bossDeathBaselineReady = true;
                return;
            }

            foreach (uint entity in currentDoneBossEntities)
            {
                if (_knownAnimationDoneBossEntities.Contains(entity))
                    continue;

                uint unitConfig = 0;
                currentUnitConfigs.TryGetValue(entity, out unitConfig);

                string unitName = string.Empty;

                if (unitConfig != 0)
                {
                    // UnitConfig.unitName = +0x1C
                    uint unitNameString =
                        _memory.ReadPointer(
                            unitConfig + 0x1C
                        );

                    if (unitNameString != 0)
                    {
                        unitName =
                            _memory.ReadMonoString(
                                unitNameString
                            );
                    }
                }

                NewlyAnimationDoneBosses.Add(
                    new BossAnimationDoneEvent(
                        entity,
                        unitConfig,
                        unitName
                    )
                );
            }

            _knownAnimationDoneBossEntities.Clear();
            foreach (uint entity in currentDoneBossEntities)
                _knownAnimationDoneBossEntities.Add(entity);
        }

        private bool TryReadAnimationDoneBosses(
            uint gameContext,
            HashSet<uint> doneBossEntities,
            Dictionary<uint, uint> unitConfigs)
        {
            uint entities = _memory.ReadPointer(gameContext + 0x28);
            if (entities == 0) return false;

            uint slots = _memory.ReadPointer(entities + 0x0C);
            int lastIndex = (int)_memory.ReadUInt32(new System.IntPtr(entities + 0x1C));
            if (slots == 0 || lastIndex < 0 || lastIndex > 10000) return false;

            for (int i = 0; i < lastIndex; i++)
            {
                uint entity = _memory.ReadPointer(slots + 0x10u + (uint)(i * 0x0C) + 0x08u);
                if (entity == 0) continue;

                uint components = _memory.ReadPointer(entity + 0x24);
                if (components == 0) continue;

                int componentCount = (int)_memory.ReadUInt32(new System.IntPtr(components + 0x0C));
                if (componentCount <= 391) continue;

                uint bossTraitComponent = _memory.ReadPointer(components + 0x10u + (uint)(41 * 4));
                if (bossTraitComponent == 0) continue;

                uint animationDoneComponent = _memory.ReadPointer(components + 0x10u + (uint)(11 * 4));
                if (animationDoneComponent == 0) continue;

                uint unitConfigComponent = _memory.ReadPointer(components + 0x10u + (uint)(391 * 4));
                uint unitConfig = 0;
                if (unitConfigComponent != 0)
                    unitConfig = _memory.ReadPointer(unitConfigComponent + 0x08);

                doneBossEntities.Add(entity);
                unitConfigs[entity] = unitConfig;
            }

            return true;
        }

        private void ResetBossDeathBaseline()
        {
            _knownAnimationDoneBossEntities.Clear();
            NewlyAnimationDoneBosses.Clear();
            _bossDeathBaselineReady = false;
        }

        // ============================================================
        // CONTEXT HELPERS
        // ============================================================

        private uint GetContexts()
        {
            if (
                _contextsStaticStorage == 0
            )
            {
                return 0;
            }

            return _memory.ReadPointer(
                _contextsStaticStorage
            );
        }

        private uint FindComponent(
            uint context,
            int componentIndex)
        {
            if (context == 0)
            {
                return 0;
            }

            uint entities =
                _memory.ReadPointer(
                    context + 0x28
                );

            if (entities == 0)
            {
                return 0;
            }

            uint slots =
                _memory.ReadPointer(
                    entities + 0x0C
                );

            int lastIndex =
                (int)_memory.ReadUInt32(
                    new System.IntPtr(
                        entities + 0x1C
                    )
                );

            if (
                slots == 0 ||
                lastIndex < 0 ||
                lastIndex > 10000
            )
            {
                return 0;
            }

            for (
                int i = 0;
                i < lastIndex;
                i++
            )
            {
                uint entity =
                    _memory.ReadPointer(
                        slots
                        + 0x10u
                        + (uint)(i * 0x0C)
                        + 0x08u
                    );

                if (entity == 0)
                {
                    continue;
                }

                uint components =
                    _memory.ReadPointer(
                        entity + 0x24
                    );

                if (components == 0)
                {
                    continue;
                }

                int componentCount =
                    (int)_memory.ReadUInt32(
                        new System.IntPtr(
                            components + 0x0C
                        )
                    );

                if (
                    componentIndex < 0 ||
                    componentIndex >=
                        componentCount
                )
                {
                    continue;
                }

                uint component =
                    _memory.ReadPointer(
                        components
                        + 0x10u
                        + (uint)(
                            componentIndex * 4
                        )
                    );

                if (component != 0)
                {
                    return component;
                }
            }

            return 0;
        }

        // ============================================================
        // SCENE CHANGE
        // ============================================================

        private void UpdateSceneChangeState()
        {
            HasChangeSceneCommand =
                false;

            SceneChangeProcessStarted =
                false;

            CurrentSceneName =
                null;

            TargetSceneName =
                null;

            uint contexts =
                GetContexts();

            if (contexts == 0)
            {
                return;
            }

            // Contexts.framework = +0x18
            uint frameworkContext =
                _memory.ReadPointer(
                    contexts + 0x18
                );

            // Framework ChangeSceneCommand = component 0
            uint changeSceneComponent =
                FindComponent(
                    frameworkContext,
                    0
                );

            if (changeSceneComponent == 0)
            {
                return;
            }

            // ChangeSceneCommandComponent.info = +0x08
            uint info =
                _memory.ReadPointer(
                    changeSceneComponent + 0x08
                );

            if (info == 0)
            {
                return;
            }

            HasChangeSceneCommand =
                true;

            // ChangeSceneInfo.processStarted = +0x29
            byte[] startedBytes =
                _memory.ReadBytes(
                    new System.IntPtr(
                        info + 0x29
                    ),
                    1
                );

            if (
                startedBytes != null &&
                startedBytes.Length == 1
            )
            {
                SceneChangeProcessStarted =
                    startedBytes[0] != 0;
            }

            // ChangeSceneInfo.currSceneData = +0x10
            uint currentSceneData =
                _memory.ReadPointer(
                    info + 0x10
                );

            // ChangeSceneInfo.targetSceneData = +0x14
            uint targetSceneData =
                _memory.ReadPointer(
                    info + 0x14
                );

            CurrentSceneName =
                ReadSceneName(
                    currentSceneData
                );

            TargetSceneName =
                ReadSceneName(
                    targetSceneData
                );
        }

        private string ReadSceneName(
            uint sceneData)
        {
            if (sceneData == 0)
            {
                return null;
            }

            // SceneData.sceneName = +0x18
            uint sceneName =
                _memory.ReadPointer(
                    sceneData + 0x18
                );

            return _memory.ReadMonoString(
                sceneName
            );
        }

        // ============================================================
        // SAVE LOAD PANEL
        // ============================================================

        private void UpdateSaveLoadPanelState()
        {
            SaveLoadPanelAddress =
                0;

            StartingGameMode =
                0;

            IsConfirmationPanelShowing =
                false;

            HaltNavigation =
                false;

            TrackedSaveGameIndex =
                -1;

            ScrollSelectedElementIndex =
                -1;

            LastInteractedSaveSlotState =
                0;

            SelectedSaveUsesManualSave =
                false;

            SelectedSaveGameProfileAddress =
                0;

            SelectedSaveGameDataAddress =
                0;

            SelectedSaveIsNewSave =
                false;

            uint contexts =
                GetContexts();

            if (contexts == 0)
            {
                return;
            }

            // Contexts.gUI = +0x24
            uint guiContext =
                _memory.ReadPointer(
                    contexts + 0x24
                );

            if (guiContext == 0)
            {
                return;
            }

            // SaveLoadPanel = GUI component 78
            uint saveLoadPanelComponent =
                FindComponent(
                    guiContext,
                    78
                );

            if (saveLoadPanelComponent == 0)
            {
                return;
            }

            // SaveLoadPanelComponent.value = +0x08
            uint saveLoadPanel =
                _memory.ReadPointer(
                    saveLoadPanelComponent + 0x08
                );

            if (saveLoadPanel == 0)
            {
                return;
            }

            SaveLoadPanelAddress =
                saveLoadPanel;

            // IsConfirmationPanelShowing = +0xD0
            byte[] confirmationBytes =
                _memory.ReadBytes(
                    new System.IntPtr(
                        saveLoadPanel + 0xD0
                    ),
                    1
                );

            if (
                confirmationBytes != null &&
                confirmationBytes.Length == 1
            )
            {
                IsConfirmationPanelShowing =
                    confirmationBytes[0] != 0;
            }

            // startingGameMode = +0xD4
            StartingGameMode =
                _memory.ReadUInt32(
                    new System.IntPtr(
                        saveLoadPanel + 0xD4
                    )
                );

            // cachedTrackedSaveGameIndex = +0xC0
            TrackedSaveGameIndex =
                (int)_memory.ReadUInt32(
                    new System.IntPtr(
                        saveLoadPanel + 0xC0
                    )
                );

            // cachedLastInteractedSaveSlotState = +0xC8
            LastInteractedSaveSlotState =
                _memory.ReadUInt32(
                    new System.IntPtr(
                        saveLoadPanel + 0xC8
                    )
                );

            ReadScrollSelectedIndex(
                saveLoadPanel
            );

            ReadSelectedSaveData(
                saveLoadPanel
            );

            // SaveLoadPanel.controllerPanel = +0x10
            uint controllerPanel =
                _memory.ReadPointer(
                    saveLoadPanel + 0x10
                );

            if (controllerPanel == 0)
            {
                return;
            }

            // UIControllerPanel.navigationTracker = +0x60
            uint navigationTracker =
                _memory.ReadPointer(
                    controllerPanel + 0x60
                );

            if (navigationTracker == 0)
            {
                return;
            }

            // CustomUINavigationTracker.haltNavigation = +0x30
            byte[] haltBytes =
                _memory.ReadBytes(
                    new System.IntPtr(
                        navigationTracker + 0x30
                    ),
                    1
                );

            if (
                haltBytes != null &&
                haltBytes.Length == 1
            )
            {
                HaltNavigation =
                    haltBytes[0] != 0;
            }
        }

        private void ReadScrollSelectedIndex(
            uint saveLoadPanel)
        {
            // SaveLoadPanel.scrollHandler = +0x90
            uint scrollHandler =
                _memory.ReadPointer(
                    saveLoadPanel + 0x90
                );

            if (scrollHandler == 0)
            {
                return;
            }

            // UISaveSlotsScrollHandler.selectedElementIndex = +0x68
            ScrollSelectedElementIndex =
                (int)_memory.ReadUInt32(
                    new System.IntPtr(
                        scrollHandler + 0x68
                    )
                );
        }

        private void ReadSelectedSaveData(
            uint saveLoadPanel)
        {
            int index =
                TrackedSaveGameIndex;

            if (
                index < 0 ||
                index >= 32
            )
            {
                return;
            }

            // cachedSaveGameProfileList = +0xA0
            uint profileList =
                _memory.ReadPointer(
                    saveLoadPanel + 0xA0
                );

            if (profileList == 0)
            {
                return;
            }

            // List<SaveGameProfile>._items = +0x08
            uint items =
                _memory.ReadPointer(
                    profileList + 0x08
                );

            // List<SaveGameProfile>._size = +0x0C
            int size =
                (int)_memory.ReadUInt32(
                    new System.IntPtr(
                        profileList + 0x0C
                    )
                );

            if (
                items == 0 ||
                size <= 0 ||
                index >= size
            )
            {
                return;
            }

            uint profile =
                _memory.ReadPointer(
                    items
                    + 0x10u
                    + (uint)(index * 4)
                );

            if (profile == 0)
            {
                return;
            }

            SelectedSaveGameProfileAddress =
                profile;

            uint mask =
                1u << index;

            bool useManualSave =
                (
                    LastInteractedSaveSlotState &
                    mask
                ) == 0;

            SelectedSaveUsesManualSave =
                useManualSave;

            // SaveGameProfile.manualSave = +0x08
            // SaveGameProfile.autoSave   = +0x0C
            uint selectedSaveData =
                _memory.ReadPointer(
                    profile +
                    (
                        useManualSave
                            ? 0x08u
                            : 0x0Cu
                    )
                );

            if (selectedSaveData == 0)
            {
                return;
            }

            SelectedSaveGameDataAddress =
                selectedSaveData;

            // SaveGameData.isNewSave = +0xD0
            byte[] newSaveBytes =
                _memory.ReadBytes(
                    new System.IntPtr(
                        selectedSaveData + 0xD0
                    ),
                    1
                );

            if (
                newSaveBytes != null &&
                newSaveBytes.Length == 1
            )
            {
                SelectedSaveIsNewSave =
                    newSaveBytes[0] != 0;
            }
        }

        // ============================================================
        // OVERWRITE CONFIRMATION
        //
        // MessagePanel
        // +0x70 -> onChoiceYesEvent
        //
        // Relay
        // +0x0C -> _listenersOnce
        // +0x1C -> _onceCount
        //
        // first Action
        // +0x10 -> m_target
        //
        // closure
        // +0x08 -> onConfirmationCallback
        //
        // inner Action
        // +0x08 -> method_ptr
        // ============================================================

        private void UpdateOverwriteConfirmationState()
        {
            ChoiceYesRelayOnceCount =
                0;

            ConfirmationCallbackMethodPtr =
                0;

            uint contexts =
                GetContexts();

            if (contexts == 0)
            {
                return;
            }

            uint guiContext =
                _memory.ReadPointer(
                    contexts + 0x24
                );

            if (guiContext == 0)
            {
                return;
            }

            // MessagePanel = GUI component 55
            uint messagePanelComponent =
                FindComponent(
                    guiContext,
                    55
                );

            if (messagePanelComponent == 0)
            {
                return;
            }

            // MessagePanelComponent.value = +0x08
            uint messagePanel =
                _memory.ReadPointer(
                    messagePanelComponent + 0x08
                );

            if (messagePanel == 0)
            {
                return;
            }

            // MessagePanel.onChoiceYesEvent = +0x70
            uint choiceYesRelay =
                _memory.ReadPointer(
                    messagePanel + 0x70
                );

            if (choiceYesRelay == 0)
            {
                return;
            }

            // Relay._onceCount = +0x1C
            ChoiceYesRelayOnceCount =
                _memory.ReadUInt32(
                    new System.IntPtr(
                        choiceYesRelay + 0x1C
                    )
                );

            if (ChoiceYesRelayOnceCount == 0)
            {
                return;
            }

            // Relay._listenersOnce = +0x0C
            uint listenersOnce =
                _memory.ReadPointer(
                    choiceYesRelay + 0x0C
                );

            if (listenersOnce == 0)
            {
                return;
            }

            // First element of Mono Action[]
            uint outerAction =
                _memory.ReadPointer(
                    listenersOnce + 0x10
                );

            if (outerAction == 0)
            {
                return;
            }

            // System.Delegate.m_target = +0x10
            uint closure =
                _memory.ReadPointer(
                    outerAction + 0x10
                );

            if (closure == 0)
            {
                return;
            }

            // <>c__DisplayClass30_0.onConfirmationCallback = +0x08
            uint confirmationCallback =
                _memory.ReadPointer(
                    closure + 0x08
                );

            if (confirmationCallback == 0)
            {
                return;
            }

            // System.Delegate.method_ptr = +0x08
            ConfirmationCallbackMethodPtr =
                _memory.ReadUInt32(
                    new System.IntPtr(
                        confirmationCallback + 0x08
                    )
                );
        }
    }
}