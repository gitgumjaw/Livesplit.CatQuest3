using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LiveSplit.CatQuest3
{
    public class CatQuest3Settings : UserControl
    {
        private readonly LiveSplitState _state;

        private readonly Label _startTriggerLabel;
        private readonly ComboBox _startTriggerComboBox;

        private readonly Label _splitTriggersLabel;
        private readonly Panel _splitScrollPanel;
        private readonly TableLayoutPanel _splitTable;

        private readonly Dictionary<int, SplitTriggerSelection>
            _splitTriggers =
                new Dictionary<int, SplitTriggerSelection>();

        private readonly Dictionary<int, ComboBox>
            _splitTypeComboBoxes =
                new Dictionary<int, ComboBox>();

        private readonly Dictionary<int, ComboBox>
            _splitSpecificComboBoxes =
                new Dictionary<int, ComboBox>();

        private readonly List<string> _splitNamesSnapshot =
            new List<string>();

        public StartTriggerMode StartTrigger
        {
            get
            {
                if (
                    _startTriggerComboBox.SelectedItem
                    is StartTriggerOption option
                )
                {
                    return option.Mode;
                }

                return StartTriggerMode.Manual;
            }

            set
            {
                for (
                    int i = 0;
                    i < _startTriggerComboBox.Items.Count;
                    i++
                )
                {
                    StartTriggerOption option =
                        _startTriggerComboBox.Items[i]
                        as StartTriggerOption;

                    if (
                        option != null &&
                        option.Mode == value
                    )
                    {
                        _startTriggerComboBox.SelectedIndex =
                            i;

                        return;
                    }
                }

                _startTriggerComboBox.SelectedIndex =
                    0;
            }
        }

        public CatQuest3Settings(
            LiveSplitState state
        )
        {
            _state =
                state;

            AutoSize =
                false;

            Width =
                475;

            Height =
                430;

            _startTriggerLabel =
                new Label();

            _startTriggerLabel.Text =
                "Start Trigger:";

            _startTriggerLabel.AutoSize =
                true;

            _startTriggerLabel.Left =
                10;

            _startTriggerLabel.Top =
                14;

            _startTriggerComboBox =
                new ComboBox();

            _startTriggerComboBox.DropDownStyle =
                ComboBoxStyle.DropDownList;

            _startTriggerComboBox.Left =
                100;

            _startTriggerComboBox.Top =
                10;

            _startTriggerComboBox.Width =
                190;

            _startTriggerComboBox.Items.Add(
                new StartTriggerOption(
                    StartTriggerMode.Manual,
                    "Manual Start"
                )
            );

            _startTriggerComboBox.Items.Add(
                new StartTriggerOption(
                    StartTriggerMode.Overwrite,
                    "Overwrite Start"
                )
            );

            _startTriggerComboBox.Items.Add(
                new StartTriggerOption(
                    StartTriggerMode.EmptySlot,
                    "Empty-Slot Start"
                )
            );

            _startTriggerComboBox.Items.Add(
                new StartTriggerOption(
                    StartTriggerMode.Continue,
                    "Continue Start"
                )
            );

            _startTriggerComboBox.Items.Add(
                new StartTriggerOption(
                    StartTriggerMode.Any,
                    "Any Start"
                )
            );

            _startTriggerComboBox.SelectedIndex =
                0;

            _splitTriggersLabel =
                new Label();

            _splitTriggersLabel.Text =
                "Split Triggers:";

            _splitTriggersLabel.AutoSize =
                true;

            _splitTriggersLabel.Left =
                10;

            _splitTriggersLabel.Top =
                52;

            _splitScrollPanel =
                new Panel();

            _splitScrollPanel.Left =
                10;

            _splitScrollPanel.Top =
                75;

            _splitScrollPanel.Width =
                455;

            _splitScrollPanel.Height =
                340;

            _splitScrollPanel.AutoScroll =
                true;

            _splitTable =
                new TableLayoutPanel();

            _splitTable.AutoSize =
                true;

            _splitTable.AutoSizeMode =
                AutoSizeMode.GrowAndShrink;

            _splitTable.ColumnCount =
                3;

            _splitTable.GrowStyle =
                TableLayoutPanelGrowStyle.AddRows;

            _splitTable.Padding =
                new Padding(
                    0,
                    0,
                    15,
                    0
                );

            _splitTable.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Absolute,
                    125
                )
            );

            _splitTable.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Absolute,
                    85
                )
            );

            _splitTable.ColumnStyles.Add(
                new ColumnStyle(
                    SizeType.Absolute,
                    225
                )
            );

            _splitScrollPanel.Controls.Add(
                _splitTable
            );

            Controls.Add(
                _startTriggerLabel
            );

            Controls.Add(
                _startTriggerComboBox
            );

            Controls.Add(
                _splitTriggersLabel
            );

            Controls.Add(
                _splitScrollPanel
            );

            RefreshSplitRows();
        }

        // ============================================================
        // PUBLIC SPLIT-TRIGGER API
        // ============================================================

        public SplitTriggerSelection GetSplitTrigger(
            int splitIndex
        )
        {
            SplitTriggerSelection selection;

            if (
                _splitTriggers.TryGetValue(
                    splitIndex,
                    out selection
                )
            )
            {
                return selection;
            }

            return SplitTriggerSelection.None;
        }

        public void SetSplitTrigger(
            int splitIndex,
            SplitTriggerType type,
            string value
        )
        {
            if (splitIndex < 0)
            {
                return;
            }

            SplitTriggerSelection selection =
                new SplitTriggerSelection(
                    type,
                    value
                );

            if (
                type == SplitTriggerType.None
            )
            {
                _splitTriggers.Remove(
                    splitIndex
                );
            }
            else
            {
                _splitTriggers[splitIndex] =
                    selection;
            }

            ApplySelectionToRow(
                splitIndex,
                selection
            );
        }

        public void ClearSplitTriggers()
        {
            _splitTriggers.Clear();

            foreach (
                KeyValuePair<int, ComboBox> pair
                in _splitTypeComboBoxes
            )
            {
                SelectType(
                    pair.Value,
                    SplitTriggerType.None
                );
            }

            foreach (
                KeyValuePair<int, ComboBox> pair
                in _splitSpecificComboBoxes
            )
            {
                PopulateSpecificOptions(
                    pair.Key,
                    SplitTriggerType.None,
                    string.Empty
                );
            }
        }

        public IEnumerable<KeyValuePair<int, SplitTriggerSelection>>
            GetConfiguredSplitTriggers()
        {
            for (
                int splitIndex = 0;
                splitIndex < _state.Run.Count;
                splitIndex++
            )
            {
                SplitTriggerSelection selection =
                    GetSplitTrigger(
                        splitIndex
                    );

                if (
                    selection.Type !=
                    SplitTriggerType.None
                )
                {
                    yield return
                        new KeyValuePair<int, SplitTriggerSelection>(
                            splitIndex,
                            selection
                        );
                }
            }
        }

        public void RefreshSplitRows()
        {
            if (
                SplitNamesAreCurrent()
            )
            {
                return;
            }

            _splitNamesSnapshot.Clear();

            for (
                int i = 0;
                i < _state.Run.Count;
                i++
            )
            {
                _splitNamesSnapshot.Add(
                    _state.Run[i].Name
                );
            }

            RebuildSplitRows();
        }

        // ============================================================
        // SPLIT ROWS
        // ============================================================

        private bool SplitNamesAreCurrent()
        {
            if (
                _splitNamesSnapshot.Count !=
                _state.Run.Count
            )
            {
                return false;
            }

            for (
                int i = 0;
                i < _state.Run.Count;
                i++
            )
            {
                if (
                    _splitNamesSnapshot[i] !=
                    _state.Run[i].Name
                )
                {
                    return false;
                }
            }

            return true;
        }

        private void RebuildSplitRows()
        {
            _splitTable.SuspendLayout();

            _splitTable.Controls.Clear();

            _splitTable.RowStyles.Clear();

            _splitTypeComboBoxes.Clear();

            _splitSpecificComboBoxes.Clear();

            AddHeaderRow();

            for (
                int splitIndex = 0;
                splitIndex < _state.Run.Count;
                splitIndex++
            )
            {
                AddSplitRow(
                    splitIndex,
                    _state.Run[splitIndex].Name
                );
            }

            _splitTable.ResumeLayout(
                true
            );
        }

        private void AddHeaderRow()
        {
            AddHeaderLabel(
                "Split",
                0
            );

            AddHeaderLabel(
                "Trigger Type",
                1
            );

            AddHeaderLabel(
                "Specific Trigger",
                2
            );
        }

        private void AddHeaderLabel(
            string text,
            int column
        )
        {
            Label header =
                new Label();

            header.Text =
                text;

            header.Font =
                new Font(
                    header.Font,
                    FontStyle.Bold
                );

            header.AutoSize =
                true;

            header.Margin =
                new Padding(
                    3,
                    5,
                    3,
                    6
                );

            _splitTable.Controls.Add(
                header,
                column,
                0
            );
        }

        private void AddSplitRow(
            int splitIndex,
            string splitName
        )
        {
            int rowIndex =
                splitIndex + 1;

            Label splitLabel =
                new Label();

            splitLabel.Text =
                (splitIndex + 1).ToString() +
                ". " +
                splitName;

            splitLabel.AutoEllipsis =
                true;

            splitLabel.Width =
                120;

            splitLabel.Height =
                24;

            splitLabel.TextAlign =
                ContentAlignment.MiddleLeft;

            ComboBox typeComboBox =
                new ComboBox();

            typeComboBox.DropDownStyle =
                ComboBoxStyle.DropDownList;

            typeComboBox.Width =
                80;

            AddTypeOptions(
                typeComboBox
            );

            ComboBox specificComboBox =
                new ComboBox();

            specificComboBox.DropDownStyle =
                ComboBoxStyle.DropDownList;

            specificComboBox.Width =
                220;

            specificComboBox.MaxDropDownItems =
                20;

            _splitTypeComboBoxes[splitIndex] =
                typeComboBox;

            _splitSpecificComboBoxes[splitIndex] =
                specificComboBox;

            SplitTriggerSelection selection =
                GetSplitTrigger(
                    splitIndex
                );

            SelectType(
                typeComboBox,
                selection.Type
            );

            PopulateSpecificOptions(
                splitIndex,
                selection.Type,
                selection.Value
            );

            int capturedSplitIndex =
                splitIndex;

            typeComboBox.SelectedIndexChanged +=
                delegate
                {
                    TriggerTypeOption typeOption =
                        typeComboBox.SelectedItem
                        as TriggerTypeOption;

                    if (typeOption == null)
                    {
                        return;
                    }

                    SplitTriggerType newType =
                        typeOption.Type;

                    if (
                        newType ==
                        SplitTriggerType.None
                    )
                    {
                        _splitTriggers.Remove(
                            capturedSplitIndex
                        );

                        PopulateSpecificOptions(
                            capturedSplitIndex,
                            SplitTriggerType.None,
                            string.Empty
                        );

                        return;
                    }

                    PopulateSpecificOptions(
                        capturedSplitIndex,
                        newType,
                        string.Empty
                    );

                    SpecificTriggerOption firstOption =
                        specificComboBox.SelectedItem
                        as SpecificTriggerOption;

                    if (firstOption != null)
                    {
                        _splitTriggers[capturedSplitIndex] =
                            new SplitTriggerSelection(
                                newType,
                                firstOption.Value
                            );
                    }
                    else
                    {
                        _splitTriggers[capturedSplitIndex] =
                            new SplitTriggerSelection(
                                newType,
                                string.Empty
                            );
                    }
                };

            specificComboBox.SelectedIndexChanged +=
                delegate
                {
                    TriggerTypeOption typeOption =
                        typeComboBox.SelectedItem
                        as TriggerTypeOption;

                    SpecificTriggerOption specificOption =
                        specificComboBox.SelectedItem
                        as SpecificTriggerOption;

                    if (
                        typeOption == null ||
                        typeOption.Type ==
                            SplitTriggerType.None ||
                        specificOption == null
                    )
                    {
                        return;
                    }

                    _splitTriggers[capturedSplitIndex] =
                        new SplitTriggerSelection(
                            typeOption.Type,
                            specificOption.Value
                        );
                };

            _splitTable.Controls.Add(
                splitLabel,
                0,
                rowIndex
            );

            _splitTable.Controls.Add(
                typeComboBox,
                1,
                rowIndex
            );

            _splitTable.Controls.Add(
                specificComboBox,
                2,
                rowIndex
            );
        }

        private void ApplySelectionToRow(
            int splitIndex,
            SplitTriggerSelection selection
        )
        {
            ComboBox typeComboBox;

            if (
                !_splitTypeComboBoxes.TryGetValue(
                    splitIndex,
                    out typeComboBox
                )
            )
            {
                return;
            }

            SelectType(
                typeComboBox,
                selection.Type
            );

            PopulateSpecificOptions(
                splitIndex,
                selection.Type,
                selection.Value
            );
        }

        // ============================================================
        // TYPE DROPDOWN
        // ============================================================

        private void AddTypeOptions(
            ComboBox comboBox
        )
        {
            comboBox.Items.Add(
                new TriggerTypeOption(
                    SplitTriggerType.None,
                    "None"
                )
            );

            comboBox.Items.Add(
                new TriggerTypeOption(
                    SplitTriggerType.Chest,
                    "Chest"
                )
            );

            comboBox.Items.Add(
                new TriggerTypeOption(
                    SplitTriggerType.Enter,
                    "Enter"
                )
            );

            comboBox.Items.Add(
                new TriggerTypeOption(
                    SplitTriggerType.Exit,
                    "Exit"
                )
            );

            comboBox.Items.Add(
                new TriggerTypeOption(
                    SplitTriggerType.KeyQuestItem,
                    "KeyItem"
                )
            );

            comboBox.Items.Add(
                new TriggerTypeOption(
                    SplitTriggerType.Equipment,
                    "Equipment"
                )
            );

            comboBox.Items.Add(
                new TriggerTypeOption(
                    SplitTriggerType.Boss,
                    "BossKill"
                )
            );
        }

        private void SelectType(
            ComboBox comboBox,
            SplitTriggerType type
        )
        {
            for (
                int i = 0;
                i < comboBox.Items.Count;
                i++
            )
            {
                TriggerTypeOption option =
                    comboBox.Items[i]
                    as TriggerTypeOption;

                if (
                    option != null &&
                    option.Type == type
                )
                {
                    comboBox.SelectedIndex =
                        i;

                    return;
                }
            }

            if (
                comboBox.Items.Count > 0
            )
            {
                comboBox.SelectedIndex =
                    0;
            }
        }

        // ============================================================
        // SPECIFIC-TRIGGER DROPDOWN
        // ============================================================

        private void PopulateSpecificOptions(
            int splitIndex,
            SplitTriggerType type,
            string selectedValue
        )
        {
            ComboBox comboBox;

            if (
                !_splitSpecificComboBoxes.TryGetValue(
                    splitIndex,
                    out comboBox
                )
            )
            {
                return;
            }

            comboBox.BeginUpdate();

            comboBox.Items.Clear();

            comboBox.Enabled =
                type != SplitTriggerType.None;

            if (
                type == SplitTriggerType.Chest
            )
            {
                foreach (
                    ChestCatalog.ChestEntry chest
                    in ChestCatalog.Entries
                )
                {
                    comboBox.Items.Add(
                        new SpecificTriggerOption(
                            chest.Value,
                            chest.DisplayName
                        )
                    );
                }
            }
            else if (
                type == SplitTriggerType.Enter ||
                type == SplitTriggerType.Exit
            )
            {
                foreach (
                    LocationCatalog.LocationEntry location
                    in LocationCatalog.Entries
                )
                {
                    comboBox.Items.Add(
                        new SpecificTriggerOption(
                            location.Value,
                            location.DisplayName
                        )
                    );
                }
            }
            else if (
                type == SplitTriggerType.KeyQuestItem
            )
            {
                foreach (
                    KeyItemCatalog.KeyItemEntry keyItem
                    in KeyItemCatalog.Entries
                )
                {
                    comboBox.Items.Add(
                        new SpecificTriggerOption(
                            keyItem.Value,
                            keyItem.DisplayName
                        )
                    );
                }
            }
            else if (
                type == SplitTriggerType.Equipment
            )
            {
                foreach (
                    EquipmentCatalog.EquipmentEntry equipment
                    in EquipmentCatalog.Entries
                )
                {
                    comboBox.Items.Add(
                        new SpecificTriggerOption(
                            equipment.Value,
                            equipment.DisplayName
                        )
                    );
                }
            }
            else if (
                type == SplitTriggerType.Boss
            )
            {
                foreach (
                    BossCatalog.BossEntry boss
                    in BossCatalog.Entries
                )
                {
                    comboBox.Items.Add(
                        new SpecificTriggerOption(
                            boss.Value,
                            boss.DisplayName
                        )
                    );
                }
            }

            SelectSpecificValue(
                comboBox,
                type,
                selectedValue
            );

            comboBox.EndUpdate();
        }

        private void SelectSpecificValue(
            ComboBox comboBox,
            SplitTriggerType type,
            string selectedValue
        )
        {
            for (
                int i = 0;
                i < comboBox.Items.Count;
                i++
            )
            {
                SpecificTriggerOption option =
                    comboBox.Items[i]
                    as SpecificTriggerOption;

                if (
                    option != null &&
                    string.Equals(
                        option.Value ?? string.Empty,
                        selectedValue ?? string.Empty,
                        StringComparison.OrdinalIgnoreCase
                    )
                )
                {
                    comboBox.SelectedIndex =
                        i;

                    return;
                }
            }

            if (
                !string.IsNullOrEmpty(
                    selectedValue
                )
            )
            {
                SpecificTriggerOption unknownOption =
                    new SpecificTriggerOption(
                        selectedValue,
                        "Unknown " +
                        GetTypeDisplayName(
                            type
                        ) +
                        " — " +
                        selectedValue
                    );

                comboBox.Items.Add(
                    unknownOption
                );

                comboBox.SelectedItem =
                    unknownOption;

                return;
            }

            if (
                comboBox.Items.Count > 0
            )
            {
                comboBox.SelectedIndex =
                    0;
            }
        }

        private string GetTypeDisplayName(
            SplitTriggerType type
        )
        {
            switch (type)
            {
                case SplitTriggerType.Chest:
                    return "Chest";

                case SplitTriggerType.Enter:
                    return "Enter";

                case SplitTriggerType.Exit:
                    return "Exit";

                case SplitTriggerType.KeyQuestItem:
                    return "Key / Quest Item";

                case SplitTriggerType.Equipment:
                    return "Equipment";

                case SplitTriggerType.Boss:
                    return "Boss";

                default:
                    return "Trigger";
            }
        }

        // ============================================================
        // DISPLAY OPTION CLASSES
        // ============================================================

        private class StartTriggerOption
        {
            public StartTriggerMode Mode
            {
                get;
                private set;
            }

            private readonly string _displayName;

            public StartTriggerOption(
                StartTriggerMode mode,
                string displayName
            )
            {
                Mode =
                    mode;

                _displayName =
                    displayName;
            }

            public override string ToString()
            {
                return _displayName;
            }
        }

        private class TriggerTypeOption
        {
            public SplitTriggerType Type
            {
                get;
                private set;
            }

            private readonly string _displayName;

            public TriggerTypeOption(
                SplitTriggerType type,
                string displayName
            )
            {
                Type =
                    type;

                _displayName =
                    displayName;
            }

            public override string ToString()
            {
                return _displayName;
            }
        }

        private class SpecificTriggerOption
        {
            public string Value
            {
                get;
                private set;
            }

            private readonly string _displayName;

            public SpecificTriggerOption(
                string value,
                string displayName
            )
            {
                Value =
                    value ?? string.Empty;

                _displayName =
                    displayName;
            }

            public override string ToString()
            {
                return _displayName;
            }
        }
    }
}
