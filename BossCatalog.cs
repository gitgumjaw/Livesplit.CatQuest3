using System;
using System.Collections.Generic;

namespace LiveSplit.CatQuest3
{
    /// <summary>
    /// One source of truth for BossKill trigger names and runtime UnitConfig.unitName aliases.
    ///
    /// Value is the stable string saved in LiveSplit settings.
    /// DisplayName is what appears in the BossKill dropdown.
    /// UnitNames contains every runtime unitName that should satisfy that logical boss trigger.
    /// </summary>
    public static class BossCatalog
    {
        public sealed class BossEntry
        {
            public string Value { get; private set; }
            public string DisplayName { get; private set; }
            public string[] UnitNames { get; private set; }

            public BossEntry(
                string value,
                string displayName,
                params string[] unitNames)
            {
                Value = value ?? string.Empty;
                DisplayName = displayName ?? string.Empty;
                UnitNames = unitNames ?? new string[0];
            }

            public bool MatchesUnitName(
                string unitName)
            {
                if (string.IsNullOrEmpty(unitName))
                {
                    return false;
                }

                foreach (string alias in UnitNames)
                {
                    if (
                        string.Equals(
                            alias,
                            unitName,
                            StringComparison.OrdinalIgnoreCase
                        )
                    )
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        private static readonly BossEntry[] _entries =
        {
            new BossEntry("CaptainMeowtallika", "Captain Meowtallika", "Captain Meowtallika"),
            new BossEntry("CaptainTakomeowki", "Captain Takomeowki", "Captain Takomeowki"),
            new BossEntry("Clawford", "Clawford", "Clawford"),
            new BossEntry("Dratcula", "Dratcula", "Dratcula"),
            new BossEntry("DuckOfDoom", "Duck of Doom", "Duck of Doom"),
            new BossEntry("FirePiratCaptain", "Fire Pi-rat Captain", "Fire Pi-rat Captain"),
            new BossEntry("IcePiratCaptain", "Ice Pi-rat Captain", "Ice Pi-rat Captain"),
            new BossEntry("MachoDog", "Macho Dog", "Macho Dog"),
            new BossEntry("MisterClean", "Mister Clean", "Mister Clean"),
            new BossEntry(
                "OinkerChief",
                "Oinker Chief",
                "Oinker Chief, the Pigment of Enlightenment"
            ),
            new BossEntry(
                "SeekerAntares",
                "Seeker Antares",
                "Seeker of Antares, Warden of the North Star"
            ),
            new BossEntry(
                "SeekerOrion",
                "Seeker Orion",
                "Seeker of Orion",
                "Seeker Orion, Warden of the North Star"
            ),
            new BossEntry("BoarKing", "Boar King", "The Boar King"),
            new BossEntry("Cathulhu", "Cathulhu", "The Cathulhu"),
            new BossEntry("DragonBoar", "Dragon Boar", "The Dragon Boar"),
            new BossEntry("Meowkoyakuza", "Meowkoyakuza", "The Meowkoyakuza"),
            new BossEntry("Meowtallicurse", "Meowtallicurse", "The Meowtallicurse"),
            new BossEntry("Necromouser", "Necromouser", "The Necromouser"),
            new BossEntry("NorthStar", "North Star", "The North Star"),
            new BossEntry("PiratKing", "Pi-rat King", "The Pi-rat King"),
            new BossEntry("TheUndying", "The Undying", "The Undying")
        };

        public static IEnumerable<BossEntry> Entries
        {
            get { return _entries; }
        }

        public static bool TryGetByValue(
            string value,
            out BossEntry entry)
        {
            foreach (BossEntry candidate in _entries)
            {
                if (
                    string.Equals(
                        candidate.Value,
                        value,
                        StringComparison.OrdinalIgnoreCase
                    )
                )
                {
                    entry = candidate;
                    return true;
                }
            }

            entry = null;
            return false;
        }

        public static string GetDisplayName(
            string value)
        {
            BossEntry entry;

            if (TryGetByValue(value, out entry))
            {
                return entry.DisplayName;
            }

            return value ?? string.Empty;
        }
    }
}
