using System;
using System.Collections.Generic;

namespace LiveSplit.CatQuest3
{
    public static class KeyItemCatalog
    {
        public sealed class KeyItemEntry
        {
            public string Value { get; private set; }
            public string DisplayName { get; private set; }
            public string InternalAssetName { get; private set; }
            public string Guid { get; private set; }

            public KeyItemEntry(string value, string displayName, string internalAssetName, string guid)
            {
                Value = value; DisplayName = displayName; InternalAssetName = internalAssetName; Guid = guid;
            }
        }

        private static readonly KeyItemEntry[] _entries = new KeyItemEntry[]
        {
            new KeyItemEntry("CathulhuFang", "Cathulhu Fang", "QuestItemKey_CathulhuFang", "24eaf080746bb4d468b2c2f3b3b438bd"),
            new KeyItemEntry("DragonBone", "Dragon Bone", "QuestItemKey_DragonBone", "604f55396f7223a4d9b091d01dae3f05"),
            new KeyItemEntry("FishBlue", "Fish Blue", "QuestItemKey_FishBlue", "61c1380c5a187ed499683b5d1b9cecb2"),
            new KeyItemEntry("FishGhost", "Fish Ghost", "QuestItemKey_FishGhost", "7dcde5efc8871764d936748de7adee42"),
            new KeyItemEntry("FishLove", "Fish Love", "QuestItemKey_FishLove", "eaf6fc75f08d5ca4d90503fe4696c9da"),
            new KeyItemEntry("FishRock", "Fish Rock", "QuestItemKey_FishRock", "746abd5ad065b784b917adca4f4ae62f"),
            new KeyItemEntry("FishSunset", "Fish Sunset", "QuestItemKey_FishSunset", "47f483c2441dd0e4eb3426ee0713187b"),
            new KeyItemEntry("GoldenKeyFloor01", "Golden Key - Floor 01", "QuestItemKey_GoldenKey_Floor_01", "27f6291ec1a18b74c8de5c0943a85f97"),
            new KeyItemEntry("GoldenKeyFloor02", "Golden Key - Floor 02", "QuestItemKey_GoldenKey_Floor_02", "819e8c85c2dcf964ab347ae1a3e3ddf4"),
            new KeyItemEntry("GoldenKeyFloor03", "Golden Key - Floor 03", "QuestItemKey_GoldenKey_Floor_03", "76b76df427cf3c14490fc1bbfa5492d2"),
            new KeyItemEntry("GoldenKeyFloor04", "Golden Key - Floor 04", "QuestItemKey_GoldenKey_Floor_04", "603dfbe1386ca644ba0933922486442c"),
            new KeyItemEntry("GoldenKeyFloor05", "Golden Key - Floor 05", "QuestItemKey_GoldenKey_Floor_05", "e9ab3faa879d82c498e07130df4b06db"),
            new KeyItemEntry("InfinityKey", "Infinity Key", "QuestItemKey_InfinityKey", "f8e3efba8cd114f41902e30faa529854"),
            new KeyItemEntry("KeyAntares", "Key Antares", "QuestItemKey_KeyAntares", "5f9325f9b6b046841ad54bce22f03acf"),
            new KeyItemEntry("KeyCentauri", "Key Centauri", "QuestItemKey_KeyCentauri", "d1655f7bf60cd434f9fccfd788ddcc88"),
            new KeyItemEntry("KeyOrion", "Key Orion", "QuestItemKey_KeyOrion", "6309fd5ed37d4a748a04b0a8e6f9695b"),
            new KeyItemEntry("LostItemHammer", "Lost Item - Hammer", "QuestItemKey_LostItem_Hammer", "b802c4f9c4f08f3459c139ef9b08f8dc"),
            new KeyItemEntry("LostItemMilk", "Lost Item - Milk", "QuestItemKey_LostItem_Milk", "b64c45e70d00a6a4b89b40c1475eff43"),
            new KeyItemEntry("LostItemVoodooDoll", "Lost Item - Voodoo Doll", "QuestItemKey_LostItem_VoodooDoll", "bfab845ea26109d43aeb743da9dbe5ad"),
            new KeyItemEntry("LovepurrBookOne", "Lovepurr Chronicles: Book One", "QuestKey_WorldQuest_LovepurrCastle_Book01_Obtained", "462f3c9dceb819143b792e42f667fc18"),
            new KeyItemEntry("LovepurrBookThree", "Lovepurr Chronicles: Book Three", "QuestKey_WorldQuest_LovepurrCastle_Book03_Obtained", "7d5083421c8ee1c4b95166042675fc50"),
            new KeyItemEntry("LovepurrBookTwo", "Lovepurr Chronicles: Book Two", "QuestKey_WorldQuest_LovepurrCastle_Book02_Obtained", "435ba3e8d8e7d7341a8b35b48e9617e1"),
            new KeyItemEntry("MailBlueStarfish", "Mail - For Blue Starfish", "QuestItemKey_Mail_BlueStarfish", "c5923be04af0345488fbfb153743830a"),
            new KeyItemEntry("MailBoarb", "Mail - For Boarb", "QuestItemKey_Mail_Boarb", "877a02bea53041745813cf7df07db5bf"),
            new KeyItemEntry("MailBoarb02", "Mail - For Boarb 02", "QuestItemKey_Mail_Boarb_02", "5119bbbec3e43134fbaa0f167801cd06"),
            new KeyItemEntry("MailBonehead", "Mail - For Bonehead", "QuestItemKey_Mail_Bonehead", "6fc3fee8693ddd14984d0934fc953914"),
            new KeyItemEntry("MailCharon", "Mail - For Charon", "QuestItemKey_Mail_Charon", "19f1d57e325f5ea479b8d2f715cea13d"),
            new KeyItemEntry("MailOinkerChief", "Mail - For Oinker Chief", "QuestItemKey_Mail_OinkerChief", "badcc36b6c079c046b27a5306153275c"),
            new KeyItemEntry("MailPinkStarfish", "Mail - For Pink Starfish", "QuestItemKey_Mail_PinkStarfish", "7771242b2657cb04fa901b3a990d8cb1"),
            new KeyItemEntry("MailPostmutt", "Mail - For Postmutt", "QuestItemKey_Mail_Postmutt", "51cac8a4d76263b4d8419811af2dadad"),
            new KeyItemEntry("MailRatFloat", "Mail - For Rat Float", "QuestItemKey_Mail_RatFloat", "414e26181fb26df49a796d7c80dbca6e"),
            new KeyItemEntry("MewtallicaConcertTicket", "Mewtallica Concert Ticket", "QuestItemKey_MewtallicaConcertTicket", "d2b0bc1943cab874c8aa6117573c24bf"),
            new KeyItemEntry("NorthStarEssence", "North Star Essence", "QuestItemKey_NorthStarEssence", "fb47a7fb818f58948851fab78dbaf8fa"),
            new KeyItemEntry("ShellPhone", "Shell Phone", "QuestItemKey_ShellPhone", "eee9e83cd3d4bcc4ba250b9db30741cd"),
            new KeyItemEntry("ShipKey", "Ship Key", "QuestItemKey_ShipKey", "5d16ca25d9411a744b61d54265287cad"),
            new KeyItemEntry("Tentakey1", "Tentakey 1", "GateKey_SpicySquid_01", "0481a65323ed4e542b2580d4a19e96f5"),
            new KeyItemEntry("Tentakey2", "Tentakey 2", "GateKey_SpicySquid_02", "02ed53aab186b5e44b63ec454ae7bc06"),
            new KeyItemEntry("Tentakey3", "Tentakey 3", "GateKey_SpicySquid_03", "1b9c24906af485547b9c79c0eba57cab"),
            new KeyItemEntry("TwinCastleBossRoomKey", "Twin Castle - Boss Room Key", "QuestItemKey_TwinCastle_BossRoomKey", "02fc8a29140e9744ab49765e65ab0f1a"),
            new KeyItemEntry("TwinCastleDiningHallKey", "Twin Castle - Dining Hall Key", "QuestItemKey_TwinCastle_DiningHallKey", "c64c1ed3bb511344e9e8177dfe6dd9b6"),
            new KeyItemEntry("TwinCastleMainEntranceKey", "Twin Castle - Main Entrance Key", "QuestItemKey_TwinCastle_MainEntranceKey", "7500a1cc93ca4ae4d8e02ec8e3ee100c"),
            new KeyItemEntry("TwinCastleMasterKey", "Twin Castle - Master Key", "QuestItemKey_TwinCastle_MasterKey", "538b10bb1ce566945bcfe3217f0c6292")
        };

        public static IEnumerable<KeyItemEntry> Entries { get { return _entries; } }

        public static bool TryGetByValue(string value, out KeyItemEntry entry)
        {
            foreach (KeyItemEntry candidate in _entries)
            {
                if (string.Equals(candidate.Value, value, StringComparison.OrdinalIgnoreCase))
                { entry = candidate; return true; }
            }
            entry = null; return false;
        }

        public static string GetDisplayNameByGuid(string guid)
        {
            foreach (KeyItemEntry candidate in _entries)
            {
                if (string.Equals(candidate.Guid, guid, StringComparison.OrdinalIgnoreCase)) return candidate.DisplayName;
            }
            return guid ?? string.Empty;
        }
    }
}
