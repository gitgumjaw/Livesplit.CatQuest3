using System;
using System.Windows.Forms;

namespace LiveSplit.CatQuest3
{
    public class CatQuest3Settings : UserControl
    {
        private readonly Label _startTriggerLabel;
        private readonly ComboBox _startTriggerComboBox;

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

        public CatQuest3Settings()
        {
            AutoSize = true;

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
                170;

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

            Controls.Add(
                _startTriggerLabel
            );

            Controls.Add(
                _startTriggerComboBox
            );

            Width =
                290;

            Height =
                45;
        }

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
                string displayName)
            {
                Mode = mode;

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