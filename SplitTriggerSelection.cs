using System;

namespace LiveSplit.CatQuest3
{
    public enum SplitTriggerType
    {
        None = 0,
        Chest = 1,
        KeyQuestItem = 2,
        Equipment = 3,
        Boss = 4
    }

    public sealed class SplitTriggerSelection
    {
        public static readonly SplitTriggerSelection None =
            new SplitTriggerSelection(
                SplitTriggerType.None,
                string.Empty
            );

        public SplitTriggerType Type
        {
            get;
            private set;
        }

        public string Value
        {
            get;
            private set;
        }

        public SplitTriggerSelection(
            SplitTriggerType type,
            string value
        )
        {
            Type =
                type;

            Value =
                value ?? string.Empty;
        }

        public bool Matches(
            SplitTriggerType type,
            string value
        )
        {
            return
                Type == type &&
                string.Equals(
                    Value ?? string.Empty,
                    value ?? string.Empty,
                    StringComparison.OrdinalIgnoreCase
                );
        }
    }
}
