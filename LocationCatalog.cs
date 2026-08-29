using System;
using System.Collections.Generic;

namespace LiveSplit.CatQuest3
{
    /// <summary>
    /// Curated list of meaningful Cat Quest III locations for Enter/Exit
    /// split triggers. Display names are intentionally user-facing rather
    /// than copied blindly from the game's internal scene names.
    /// </summary>
    public static class LocationCatalog
    {
        public sealed class LocationEntry
        {
            public string Value { get; private set; }
            public string DisplayName { get; private set; }
            public string SceneName { get; private set; }

            public LocationEntry(string value, string displayName, string sceneName)
            {
                Value = value;
                DisplayName = displayName;
                SceneName = sceneName;
            }

            public bool MatchesSceneName(string sceneName)
            {
                return string.Equals(SceneName, sceneName, StringComparison.Ordinal);
            }
        }

        private static readonly LocationEntry[] _entries =
        {
            new LocationEntry("AntaresRuins", "Antares Ruins", "Ruins_Antares"),
            new LocationEntry("BootyCave", "Booty Cave", "Cave_BootyCave"),
            new LocationEntry("CentauriRuins", "Centauri Ruins", "Ruins_Centauri"),
            new LocationEntry("EightBitDungeon", "Eight Bit Dungeon", "Castle_8BitDungeon"),
            new LocationEntry("FirePiratHideout", "Fire Pi-rat Hideout", "Castle_FirePiratesHideout"),
            new LocationEntry("FurrlornCave", "Furrlorn Cave", "Cave_FurrlornCave"),
            new LocationEntry("FurstCave", "Furst Cave", "Cave_FurstCave"),
            new LocationEntry("GoldenTower", "Golden Tower", "Castle_GoldenTower"),
            new LocationEntry("HeartpurreakCave", "Heartpurreak Cave", "Cave_HeartpurreakCave"),
            new LocationEntry("IcePiratHideout", "Ice Pi-rat Hideout", "Castle_IcePiratesHideout"),
            new LocationEntry("InfinityTower", "Infinity Tower", "InfinityTower"),
            new LocationEntry("KiddCatsSmithy", "Kidd Cat's Smithy", "Interior_KiddCatSmithy"),
            new LocationEntry("LonelyCave", "Lonely Cave", "Cave_LonelyCave"),
            new LocationEntry("LovepurrCastle", "Lovepurr Castle", "Castle_LovepurrCastle"),
            new LocationEntry("LovepurrCave", "Lovepurr Cave", "Cave_LovepurrCave"),
            new LocationEntry("MeowtallikasConcert", "Meowtallika's Concert", "Interior_MewtallicaStage"),
            new LocationEntry("OinkerMaze", "Oinker Maze", "Cave_OinkerCave"),
            new LocationEntry("OrionRuins", "Orion Ruins", "Ruins_Orion"),
            new LocationEntry("PiratKingHideout", "Pi-rat King Hideout", "Interior_PirateKingHideout"),
            new LocationEntry("PolarisRuins", "Polaris Ruins", "Ruins_Polaris"),
            new LocationEntry("RuinedCastle", "Ruined Castle", "Castle_RuinedCastle"),
            new LocationEntry("SpicyCave", "Spicy Cave", "Cave_SquidCave"),
            new LocationEntry("SqueakyCave", "Squeaky Cave", "Cave_SqueakyCave"),
            new LocationEntry("TavernTalesBoarArena", "Tavern Tales - Boar Arena", "TavernTalesArena_BoarBoss"),
            new LocationEntry("TavernTalesGentlebrosArena", "Tavern Tales - Gentlebros Arena", "Interior_GentlebrosArena"),
            new LocationEntry("TavernTalesMeowtallikaArena", "Tavern Tales - Meowtallika Arena", "TavernTalesArena_MewtallicaBoss"),
            new LocationEntry("TavernTalesPiratKingArena", "Tavern Tales - Pi-rat King Arena", "TavernTalesArena_PirateKing"),
            new LocationEntry("TavernTalesTakomeowkiArena", "Tavern Tales - Takomeowki Arena", "TavernTalesArena_SpicySquidTakomeowkiBoss"),
            new LocationEntry("TheMagicBone", "The Magic Bone", "Interior_MageShop"),
            new LocationEntry("TheMilkyBarrel", "The Milky Barrel", "Interior_Tavern"),
            new LocationEntry("ThePurribean", "The Purribean", "MainOverworld"),
            new LocationEntry("TitleScreen", "Title Screen", "TitleScene"),
            new LocationEntry("TwinCastleDark", "Twin Castle (Dark)", "Castle_TwinCastle_02"),
            new LocationEntry("TwinCastleLight", "Twin Castle (Light)", "Castle_TwinCastle_01"),
            new LocationEntry("UnknownCaveDoggyJones", "Unknown Cave - Doggy Jones", "Cave_UnknownCave"),
            new LocationEntry("VolcanoMountain", "Volcano Mountain", "Cave_Volcano"),
            new LocationEntry("ZeroDimension", "Zero Dimension", "Interior_ZeroDimension_FinalDungeon"),
            new LocationEntry("ZeroDimensionCloister", "Zero Dimension - Cloister of the North Star", "Interior_ZeroDimension_FinalBoss"),
        };

        public static IEnumerable<LocationEntry> Entries
        {
            get { return _entries; }
        }

        public static bool TryGetByValue(string value, out LocationEntry entry)
        {
            foreach (LocationEntry candidate in _entries)
            {
                if (string.Equals(candidate.Value, value, StringComparison.Ordinal))
                {
                    entry = candidate;
                    return true;
                }
            }

            entry = null;
            return false;
        }
    }
}
