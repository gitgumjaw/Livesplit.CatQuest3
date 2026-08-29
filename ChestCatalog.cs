using System.Collections.Generic;

namespace LiveSplit.CatQuest3
{
    /// <summary>
    /// One source of truth for chest GUIDs and their approved display names.
    /// To rename a chest, Ctrl+F its GUID, internal asset name, or current
    /// display name and edit the DisplayName value in the matching entry.
    /// </summary>
    public static class ChestCatalog
    {
        public sealed class ChestEntry
        {
            public string Guid { get; private set; }
            public string DisplayName { get; private set; }
            public string InternalAssetName { get; private set; }

            public ChestEntry(
                string guid,
                string displayName,
                string internalAssetName
            )
            {
                Guid = guid;
                DisplayName = displayName;
                InternalAssetName = internalAssetName;
            }
        }

        private static readonly ChestEntry[] _entries =
        {
            new ChestEntry(
                "650a3b3f9dc251d418a1210095932574",
                "8 Bit Dungeon — 6F Boss Room Chest",
                "ChestID_Castle_8BitDungeon_ChestTrigger_01"
            ),
            new ChestEntry(
                "934c21a2d35eb7f4288f21e7a3541757",
                "8 Bit Dungeon — 4F Painting Reward Chest",
                "ChestID_Castle_8BitDungeon_ChestTrigger_02"
            ),
            new ChestEntry(
                "a5a38691eb6971f49836b0a3a7110eee",
                "8 Bit Dungeon — 4F Jailed Chest",
                "ChestID_Castle_8BitDungeon_ChestTrigger_03"
            ),
            new ChestEntry(
                "05cc6497a3dfe114ea26d790975672dc",
                "Fire Pirate Hideout — 1F Chest Near Entrance",
                "ChestID_Castle_FirePirateHideout_ChestTrigger_01"
            ),
            new ChestEntry(
                "6b9f59a8637366d43ad1ef3da5caba52",
                "Fire Pirate Hideout — 1F Obstacle Room Chest",
                "ChestID_Castle_FirePirateHideout_ChestTrigger_02"
            ),
            new ChestEntry(
                "4da727baf755b6149b9fa363c77a3f4a",
                "Fire Pirate Hideout — 3F Storage - Chest",
                "ChestID_Castle_FirePirateHideout_ChestTrigger_03"
            ),
            new ChestEntry(
                "98f31902d9a7e6346a695d0cde7e63df",
                "Fire Pirate Hideout — 3F Storage - Right Crate",
                "ChestID_Castle_FirePirateHideout_ChestTrigger_04"
            ),
            new ChestEntry(
                "2256b9efebb70a24ca2ea71f473ebdbd",
                "Fire Pirate Hideout — 3F Storage - Left Crate",
                "ChestID_Castle_FirePirateHideout_ChestTrigger_05"
            ),
            new ChestEntry(
                "d4b443db593fa614b8d31364c618c701",
                "Fire Pirate Hideout — 1F Silver Boss Chest",
                "ChestID_Castle_FirePirateHideout_ChestTrigger_FireTreasure"
            ),
            new ChestEntry(
                "93bde94f3b424c141a03e022e73db2f8",
                "Golden Tower — 2F Chest",
                "ChestID_Castle_GoldenTower_ChestTrigger_Floor_02"
            ),
            new ChestEntry(
                "fc6ac86f93a489a4ea033cdf2b0aea7f",
                "Golden Tower — 3F Chest",
                "ChestID_Castle_GoldenTower_ChestTrigger_Floor_03"
            ),
            new ChestEntry(
                "267e1fb1ecce1db44b74ce076b888dd5",
                "Golden Tower — 4F Chest",
                "ChestID_Castle_GoldenTower_ChestTrigger_Floor_04"
            ),
            new ChestEntry(
                "39f16566266b9a54eabd82b9ff09c04a",
                "Golden Tower — 5F Chest",
                "ChestID_Castle_GoldenTower_ChestTrigger_Floor_05"
            ),
            new ChestEntry(
                "0ae7048c4f28e274789aa9067f8be419",
                "Golden Tower — Golden Treasure Chest",
                "ChestID_Castle_GoldenTower_ChestTrigger_GoldenTreasure"
            ),
            new ChestEntry(
                "29c566636e6b4584fabfca543bf92884",
                "Ice Pirate Hideout — 3F Storage - Crate",
                "ChestID_Castle_IcePirateHideout_ChestTrigger_01"
            ),
            new ChestEntry(
                "db68361cf021927468cb1e8bbae943ea",
                "Ice Pirate Hideout — 2F Side Room - Chest",
                "ChestID_Castle_IcePirateHideout_ChestTrigger_02"
            ),
            new ChestEntry(
                "c0f90f4d9db2e384e95d523203d2cb26",
                "Ice Pirate Hideout — 3F Secret Boss Crate",
                "ChestID_Castle_IcePirateHideout_ChestTrigger_03"
            ),
            new ChestEntry(
                "6a9906e7eaa875e40af268898cc5d319",
                "Ice Pirate Hideout — 3F Storage - Secret Chest",
                "ChestID_Castle_IcePirateHideout_ChestTrigger_04"
            ),
            new ChestEntry(
                "7ffc3cfad5dc7fd4dbb797725534b476",
                "Ice Pirate Hideout — 3F Silver Boss Chest",
                "ChestID_Castle_IcePirateHideout_ChestTrigger_IceTreasure"
            ),
            new ChestEntry(
                "f8a072b39ec682441909c974658f50ec",
                "Lovepurr Castle — Boss Room Silver Chest",
                "ChestID_Castle_LovepurrCastle_ChestTrigger_01"
            ),
            new ChestEntry(
                "6af9706a9de86e94196493c0d53a0a15",
                "Lovepurr Castle — 1st Book Puzzle Chest",
                "ChestID_Castle_LovepurrCastle_ChestTrigger_02"
            ),
            new ChestEntry(
                "0298dd4cc39e1da4eabc8543b62c3aad",
                "Lovepurr Castle — 2nd Book Puzzle Chest",
                "ChestID_Castle_LovepurrCastle_ChestTrigger_03"
            ),
            new ChestEntry(
                "e522995d163539e44a6e1254027f390d",
                "Lovepurr Castle — 3rd Book Puzzle Chest",
                "ChestID_Castle_LovepurrCastle_ChestTrigger_04"
            ),
            new ChestEntry(
                "3f889d945ebc9b64bb1fec6ce9d958ce",
                "Lovepurr Castle — Good Clawford Silver Chest",
                "ChestID_Castle_LovepurrCastle_ChestTrigger_MainTreasure"
            ),
            new ChestEntry(
                "8b18fe868c67f4746bd8650cbbc938d6",
                "Ruined Castle — 1F Hidden Chest",
                "ChestID_Castle_RuinedCastle_ChestTrigger_01"
            ),
            new ChestEntry(
                "1a2f714ee2ac91642b3a10ca439b1dca",
                "Ruined Castle — 2F Crate",
                "ChestID_Castle_RuinedCastle_ChestTrigger_02"
            ),
            new ChestEntry(
                "54e4b9b58f104ac41991f57702a842ec",
                "Ruined Castle — 2F Silver Chest",
                "ChestID_Castle_RuinedCastle_ChestTrigger_MainTreasure"
            ),
            new ChestEntry(
                "d173c2366ee964c49b2d532cbd6edd25",
                "Twin Castle (Light) - 1F Ladder Room Secret Chest",
                "ChestID_Castle_TwinCastle01_ChestTrigger_01"
            ),
            new ChestEntry(
                "f1870b21c3481b347b67772f5040dc72",
                "Twin Castle (Light) - 2F Dining Hall Chest",
                "ChestID_Castle_TwinCastle01_ChestTrigger_02"
            ),
            new ChestEntry(
                "b72d69482c7757848af2dce623a7e515",
                "Twin Castle (Light) - 1F Crate near entrance",
                "ChestID_Castle_TwinCastle01_ChestTrigger_03"
            ),
            new ChestEntry(
                "18eb29576852a0a49ab53dcbec35f5c0",
                "Twin Castle (Light) - 3F Master Room Chest",
                "ChestID_Castle_TwinCastle01_ChestTrigger_04"
            ),
            new ChestEntry(
                "793ef98daa1cbb84199b3f510bef7973",
                "Twin Castle (Dark) - 1F Ladder Room Secret Crate",
                "ChestID_Castle_TwinCastle02_ChestTrigger_01"
            ),
            new ChestEntry(
                "5c8c69b2d010ab34d9ff927087c1f8ce",
                "Twin Castle (Dark) - 2F Dining Hall Center Chest",
                "ChestID_Castle_TwinCastle02_ChestTrigger_02"
            ),
            new ChestEntry(
                "a6b238a85756cee4e99e777f69adcb46",
                "Twin Castle (Dark) - 2F Dining Hall East Chest",
                "ChestID_Castle_TwinCastle02_ChestTrigger_03"
            ),
            new ChestEntry(
                "f9ef4f4f11165884a90e2716603ff0f5",
                "Twin Castle (Dark) - 3F Silver Boss Chest",
                "ChestID_Castle_TwinCastle02_ChestTrigger_Necromouser"
            ),
            new ChestEntry(
                "97c655d0c52d71f449bac4027cc88162",
                "Booty Cave — Northwest Crate",
                "ChestID_Caves_BootyCave_ChestTrigger_01"
            ),
            new ChestEntry(
                "710b17d5f4483634db7334baa449f877",
                "Booty Cave — Southwest Crate",
                "ChestID_Caves_BootyCave_ChestTrigger_02"
            ),
            new ChestEntry(
                "2d7f47bd4c1c53c4ab176ca074865522",
                "Booty Cave — Southeast Crate",
                "ChestID_Caves_BootyCave_ChestTrigger_03"
            ),
            new ChestEntry(
                "f0e0c67d26a5a544a8deee091b2caabc",
                "Booty Cave — Southeast Chest",
                "ChestID_Caves_BootyCave_ChestTrigger_04"
            ),
            new ChestEntry(
                "705337889a942bd448bf9775204670d9",
                "Booty Cave — Southeast Silver Boss Chest",
                "ChestID_Caves_BootyCave_ChestTrigger_PatchyTreasure"
            ),
            new ChestEntry(
                "dfc3fbac9afec104694dd7b01c6350ee",
                "Furrlorn Cave — East Crate",
                "ChestID_Caves_FurrlornCave_ChestTrigger_01"
            ),
            new ChestEntry(
                "6d13f4ec1fca4d845a89adb064acb25f",
                "Furrlorn Cave — South Chest",
                "ChestID_Caves_FurrlornCave_ChestTrigger_02"
            ),
            new ChestEntry(
                "6ea2dee80b528214584104c3852b98cf",
                "Furrlorn Cave — West Chest",
                "ChestID_Caves_FurrlornCave_ChestTrigger_03"
            ),
            new ChestEntry(
                "260010564cd4f98439a9c737e10b0f57",
                "Furst Cave — South Crate",
                "ChestID_Caves_FurstCave_ChestTrigger_01"
            ),
            new ChestEntry(
                "4d3ce29d1c2ff6c4cb43911c3441f93c",
                "Furst Cave — East Silver Chest",
                "ChestID_Caves_FurstCave_ChestTrigger_FurstBooty"
            ),
            new ChestEntry(
                "30a5eace858b19f49a0b891553ba7449",
                "Heartpurreak Cave — East Chest",
                "ChestID_Caves_HeartpurreakCave_ChestTrigger_01"
            ),
            new ChestEntry(
                "fd3c4288b8589a245a1f0c4a485df86a",
                "Heartpurreak Cave — West Chest",
                "ChestID_Caves_HeartpurreakCave_ChestTrigger_02"
            ),
            new ChestEntry(
                "eec25a62ba70cd041b84ba7c24435edb",
                "Lonely Cave — Dragon Boar Treasure",
                "ChestID_Caves_LonelyCave_ChestTrigger_DragonBoarTreasure"
            ),
            new ChestEntry(
                "ce0de41c4a8f54f4eba73810b1f3b92f",
                "Lovepurr Cave — Northeast Chest",
                "ChestID_Caves_LovepurrCave_ChestTrigger_01"
            ),
            new ChestEntry(
                "5b556b74e37a53c42a78456d9e8d5df2",
                "Lovepurr Cave — Southeast Crate",
                "ChestID_Caves_LovepurrCave_ChestTrigger_02"
            ),
            new ChestEntry(
                "34c936f5885617b4e8b0aefd996d0b66",
                "Oinker Maze — Northwest Crate",
                "ChestID_Caves_OinkerCave_ChestTrigger_01"
            ),
            new ChestEntry(
                "6933e153a18611b4aa2e200b61ffbe98",
                "Oinker Maze — Northeast Crate",
                "ChestID_Caves_OinkerCave_ChestTrigger_02"
            ),
            new ChestEntry(
                "19e8ffe66f2256a4c940b760da49f6c0",
                "Oinker Maze — Midsouth Crate",
                "ChestID_Caves_OinkerCave_ChestTrigger_03"
            ),
            new ChestEntry(
                "5e50b5c9ff27f65489de1179dcbc509c",
                "Oinker Maze — South Silver Chest",
                "ChestID_Caves_OinkerCave_ChestTrigger_OinkerTreasure"
            ),
            new ChestEntry(
                "cffd7dd91ce2f0047b4147b98e1de519",
                "Squeaky Cave — Southeast Crate",
                "ChestID_Caves_SqueakyCave_ChestTrigger_01"
            ),
            new ChestEntry(
                "3c6c9743645e41f43a350a2eac957339",
                "Squeaky Cave — East Chest",
                "ChestID_Caves_SqueakyCave_ChestTrigger_02"
            ),
            new ChestEntry(
                "9c3ba62a7e041964f827d060b6da0396",
                "Squeaky Cave — Midsouth Crate",
                "ChestID_Caves_SqueakyCave_ChestTrigger_03"
            ),
            new ChestEntry(
                "ca2deab51e68ef948958a2602a55d96e",
                "Squeaky Cave — Stash Upper Crate",
                "ChestID_Caves_SqueakyCave_ChestTrigger_04"
            ),
            new ChestEntry(
                "02a45c5c5c713e242bfc87cd00842473",
                "Squeaky Cave — Stash Middle Crate",
                "ChestID_Caves_SqueakyCave_ChestTrigger_05"
            ),
            new ChestEntry(
                "ab9934d4c98574c4c933285db0f02ba9",
                "Squeaky Cave — Stash Lower Crate",
                "ChestID_Caves_SqueakyCave_ChestTrigger_06"
            ),
            new ChestEntry(
                "999a297bce7a7a04c8ca97ef179e32ae",
                "Squeaky Cave — Silver Chest",
                "ChestID_Caves_SqueakyCave_ChestTrigger_SqueakyTreasure"
            ),
            new ChestEntry(
                "638408a12bed38444b384f0cde8c9aef",
                "Spicy Cave — Waterway Crate",
                "ChestID_Caves_SquidCave_ChestTrigger_01"
            ),
            new ChestEntry(
                "261ce9cb52222074d9b16b8fde8d5ff9",
                "Spicy Cave — Waterway Chest",
                "ChestID_Caves_SquidCave_ChestTrigger_02"
            ),
            new ChestEntry(
                "fdef68233b096cf4d8700a2779f50c6c",
                "Spicy Cave — Arena Silver Chest",
                "ChestID_Caves_SquidCave_ChestTrigger_SquidTreasure"
            ),
            new ChestEntry(
                "33fee15cffb16fd49b79610468f4c162",
                "Doggy Jones Silver Chest",
                "ChestID_Caves_UnknownCave_ChestTrigger_DoggyJones"
            ),
            new ChestEntry(
                "b2499a3555b56eb4daae8b1cc6b9cd8c",
                "Volcano — Boss Chest",
                "ChestID_Caves_Volcano_ChestTrigger_01"
            ),
            new ChestEntry(
                "5a6c2d1d4f6e7c14e809e3974d205402",
                "Hidden Chest — Furggy Rock",
                "ChestID_HiddenItem_ChestTrigger_Furggy_Rock"
            ),
            new ChestEntry(
                "45f01605765638645a88115d012de54b",
                "Hidden Chest — Furtigua Rock",
                "ChestID_HiddenItem_ChestTrigger_Furtigua_Rock"
            ),
            new ChestEntry(
                "42952e286c6845e4292ff22aee9e5112",
                "Hidden Chest — Purvanna Bush",
                "ChestID_HiddenItem_ChestTrigger_Purvanna_Bush"
            ),
            new ChestEntry(
                "412ae0f105665914682393c44afb6bff",
                "Hidden Chest — Purvanna Starfish",
                "ChestID_HiddenItem_ChestTrigger_Purvanna_Starfish"
            ),
            new ChestEntry(
                "0cc22180e8fc0954892d30d279091438",
                "Hidden Chest — Rogue Bandana",
                "ChestID_HiddenItem_ChestTrigger_SandyIsle_Starfish"
            ),
            new ChestEntry(
                "ab89e4e54f9044643933cc44377e4e34",
                "Hidden Chest  — Sunset Maze Starfish",
                "ChestID_HiddenItem_ChestTrigger_SunsetMaze_Starfish"
            ),
            new ChestEntry(
                "6a455d30b4b3f7c4ba9706cecd14c9bc",
                "Mewtallica Stage — Boss Chest",
                "ChestID_MewtallicaStage_ChestTrigger_MewtallicaTreasure"
            ),
            new ChestEntry(
                "7790326666aad054e8775e3759d4a28a",
                "Catuga — West Chest",
                "ChestID_Overworld_Catuga_ChestTrigger_01"
            ),
            new ChestEntry(
                "0294cd0074d1d204db0bac9eab555e55",
                "Catuga — North Crate",
                "ChestID_Overworld_Catuga_ChestTrigger_02"
            ),
            new ChestEntry(
                "b3c5d4e55fb2a914b8a547680be68a78",
                "Catuga — East Crate",
                "ChestID_Overworld_Catuga_ChestTrigger_03"
            ),
            new ChestEntry(
                "ff059a94849ec604fa0b4e9ab8e1bfa7",
                "Code Isle — Puzzle Chest",
                "ChestID_Overworld_CodeIsle_ChestTrigger_PuzzleStone"
            ),
            new ChestEntry(
                "fe3fe14f11edeae41900d430c0b50e00",
                "Furggy Island — Lower Chest",
                "ChestID_Overworld_FurggyIsland_ChestTrigger_01"
            ),
            new ChestEntry(
                "ade0153040edc094ab6f92579b203996",
                "Furggy Island — Upper Chest",
                "ChestID_Overworld_FurggyIsland_ChestTrigger_02"
            ),
            new ChestEntry(
                "5103bd10c70b02f478a6847bf1a0532d",
                "Furggy Island — Pirate King Chest",
                "ChestID_Overworld_FurggyIsland_ChestTrigger_PirateKing"
            ),
            new ChestEntry(
                "1bfdcdccecc9d5644b3a28a2ae65152c",
                "Furggy Island — Meowgus Puzzle Chest",
                "ChestID_Overworld_FurggyIsland_ChestTrigger_PuzzleStone_Monolith"
            ),
            new ChestEntry(
                "d984358aec0bbd54eaf49469b714b7ed",
                "Furggy Island — Running Coin Chest",
                "ChestID_Overworld_FurggyIsland_ChestTrigger_RunningGold_Furggy"
            ),
            new ChestEntry(
                "7eef18bdcb68fd8488b9abbc8b7cd06c",
                "Furtigua —Running Coin Chest",
                "ChestID_Overworld_Furtigua_ChestTrigger_RunningGold_Furtigua"
            ),
            new ChestEntry(
                "77de935239d827f47bac48e3eabaf703",
                "Lonely Island — Silver Chest",
                "ChestID_Overworld_Lonely_ChestTrigger_01"
            ),
            new ChestEntry(
                "1751eee047777704abdcf4a8850d2f1c",
                "Long Island — Starfish Beach Crate",
                "ChestID_Overworld_LongIsland_ChestTrigger_01"
            ),
            new ChestEntry(
                "f6c19dda60222654295850e763940365",
                "Long Island — Inside Fort Crate",
                "ChestID_Overworld_LongIsland_ChestTrigger_02"
            ),
            new ChestEntry(
                "18923644a48006e4cb913d6659dfc679",
                "Long Island — Inside Fort Silver Chest",
                "ChestID_Overworld_LongIsland_ChestTrigger_FortTail"
            ),
            new ChestEntry(
                "6dd3d666854199043accdeb7db7ee6ef",
                "Long Island — Puzzle Chest",
                "ChestID_Overworld_LongIsland_ChestTrigger_PuzzleStone"
            ),
            new ChestEntry(
                "71b038b5c49de574db6f980e8d47314b",
                "Lovepurr Island — East Chest",
                "ChestID_Overworld_Lovepurr_ChestTrigger_01"
            ),
            new ChestEntry(
                "c1430ab86288b0e429895078cdb462d1",
                "Lovepurr Island — North Crate",
                "ChestID_Overworld_Lovepurr_ChestTrigger_02"
            ),
            new ChestEntry(
                "c98d45fadc3efbc46ae8038998bc8506",
                "Lovepurr Island — East Crate",
                "ChestID_Overworld_Lovepurr_ChestTrigger_03"
            ),
            new ChestEntry(
                "967f5e72fc001c04c8b9f47141da8647",
                "Purricade Island — West Crate",
                "ChestID_Overworld_PurricadeIsland_ChestTrigger_01"
            ),
            new ChestEntry(
                "393fe718501adc647937573afd7d7b8b",
                "Purricade Island — East Crate",
                "ChestID_Overworld_PurricadeIsland_ChestTrigger_02"
            ),
            new ChestEntry(
                "38cd4cd2c9fb18b4b866669b8d6af7ec",
                "Purricade Island — Silver Chest",
                "ChestID_Overworld_PurricadeIsland_ChestTrigger_PurricadeTreasure"
            ),
            new ChestEntry(
                "1407f568697db4f45993938fec140271",
                "Purvanna — Northwest Chest behind Puzzle Mage",
                "ChestID_Overworld_Purvanna_ChestTrigger_01"
            ),
            new ChestEntry(
                "70a652125ad35114eb07f8ca03e9ca4d",
                "Purvanna — Crate behind Postmutt",
                "ChestID_Overworld_Purvanna_ChestTrigger_02"
            ),
            new ChestEntry(
                "aa443439d999ea8448ecca0a70766c21",
                "Purvanna — Beach Starguide Crate",
                "ChestID_Overworld_Purvanna_ChestTrigger_03"
            ),
            new ChestEntry(
                "dba40d9696029314a9cdc8bce592f234",
                "Purvanna — East Boar Field Chest",
                "ChestID_Overworld_Purvanna_ChestTrigger_04"
            ),
            new ChestEntry(
                "561abb47fa18a4a4ea7454655357eb02",
                "Purvanna — South Polaris Field Chest",
                "ChestID_Overworld_Purvanna_ChestTrigger_05"
            ),
            new ChestEntry(
                "cf416388ba105e04e87e442c364d5904",
                "Purvanna — Crate between Kidd/Bonnie",
                "ChestID_Overworld_Purvanna_ChestTrigger_06"
            ),
            new ChestEntry(
                "26f49bbe109e19649bf24720fa8f49dd",
                "Purvanna — Long West Crate",
                "ChestID_Overworld_Purvanna_ChestTrigger_07"
            ),
            new ChestEntry(
                "7dd48d87bef43b8449cf059db65c44ff",
                "Purvanna — Fort Silver Chest",
                "ChestID_Overworld_Purvanna_ChestTrigger_FortWreck"
            ),
            new ChestEntry(
                "4f5cf9c5150ebeb42b05fbb05984cc50",
                "Purvanna — Northeast Ambush Chest",
                "ChestID_Overworld_Purvanna_ChestTrigger_MewtallicaAmbush"
            ),
            new ChestEntry(
                "5da65f809499be340acffeb3c50a481d",
                "Purvanna — Postmutt Inc. Chest",
                "ChestID_Overworld_Purvanna_ChestTrigger_Postman"
            ),
            new ChestEntry(
                "2c72187e064a83b40b4d8b572d687709",
                "Purvanna — Puzzle Chest Purvanna Beach",
                "ChestID_Overworld_Purvanna_ChestTrigger_PuzzleStone_PurvannaBeach"
            ),
            new ChestEntry(
                "f82978ff672a3f242a70538d55a51ea1",
                "Purvanna — Puzzle Chest Purvanna Tower",
                "ChestID_Overworld_Purvanna_ChestTrigger_PuzzleStone_PurvannaTower"
            ),
            new ChestEntry(
                "9c818583d272d6c4890e5b4624a28f1b",
                "Purvanna — Running Coin Chest",
                "ChestID_Overworld_Purvanna_ChestTrigger_RunningGold_Purvanna"
            ),
            new ChestEntry(
                "8fd8d1ca6974b4049b195d297b1e4df9",
                "Purvanna — Monster Researcher Chest",
                "ChestID_Overworld_Purvanna_MonsterResearcher_ChestTrigger_01"
            ),
            new ChestEntry(
                "422de585444f27a44880337447009e8a",
                "Purvanna — Strait Watchtower Chest",
                "ChestID_Overworld_Purvanna_StraitsWatchtower_ChestTrigger_01"
            ),
            new ChestEntry(
                "bfa78bb6bcac0a647ac487b438aa30bb",
                "Sandy Isle — Puzzle Chest",
                "ChestID_Overworld_SandyIsle_ChestTrigger_PuzzleStone"
            ),
            new ChestEntry(
                "61d8bcfe28f108f4cb40ad4609a78cd7",
                "Furst Island — Chest",
                "ChestID_Overworld_SkinnyIsland_ChestTrigger_01"
            ),
            new ChestEntry(
                "141696e640d364516a3b7c54d5e633e9",
                "Gentlebros Treasure Chest",
                "ChestID_Overworld_SQ_Gentlebros_ChestTrigger_GentlebrosTreasure"
            ),
            new ChestEntry(
                "6474872b10eb23a4f80db2ab474c9efb",
                "Straits Island — Crate by Barrels",
                "ChestID_Overworld_StraitsIsland_ChestTrigger_01"
            ),
            new ChestEntry(
                "6c490b335ded9a14d89c9c18c6ad8b03",
                "Straits Island — Puzzle Chest",
                "ChestID_Overworld_StraitsIsland_ChestTrigger_PuzzleStone"
            ),
            new ChestEntry(
                "6173ff30115e9ac4f97d975afcbc05a5",
                "Sunset Squid Isles — West of Center Crate",
                "ChestID_Overworld_Sunset_ChestTrigger_01"
            ),
            new ChestEntry(
                "d107fb7f357514a43a557593c8a1b66c",
                "Sunset Squid Isles — East Voodoo Island Crate",
                "ChestID_Overworld_Sunset_ChestTrigger_02"
            ),
            new ChestEntry(
                "0acc33a4a5424af40a37a7e5c066d25e",
                "Sunset Squid Isles — East of Center Chest",
                "ChestID_Overworld_Sunset_ChestTrigger_03"
            ),
            new ChestEntry(
                "183796bc91c4b2d438f459521eeea9d6",
                "Sunset Squid Isles — Middle Squid Key Chest",
                "ChestID_Overworld_Sunset_ChestTrigger_Key01"
            ),
            new ChestEntry(
                "8404bf3e4082dcd47b7a49f72abbe4c5",
                "Sunset Squid Isles — Southwest Squid Key Chest",
                "ChestID_Overworld_Sunset_ChestTrigger_Key02"
            ),
            new ChestEntry(
                "f787f84550d5dab4d91336a61439d3fd",
                "Sunset Squid Isles — Purrmaid Purrison Chest",
                "ChestID_Overworld_Sunset_ChestTrigger_Purrison"
            ),
            new ChestEntry(
                "45a66d30b0778ff4c9460f3697c200ae",
                "Sunset Squid Isles — Puzzle Chest Volcano",
                "ChestID_Overworld_Sunset_ChestTrigger_PuzzleStone_Sunset"
            ),
            new ChestEntry(
                "15f6a3b8da064434ebf03f22ed5a9ba6",
                "Sunset Maze — Puzzle Chest Sunset Maze",
                "ChestID_Overworld_Sunset_ChestTrigger_PuzzleStone_SunsetMaze"
            ),
            new ChestEntry(
                "cfde168fd11e6a14ebd25dfc688c4569",
                "Sunset Maze — Running Coin Chest",
                "ChestID_Overworld_Sunset_ChestTrigger_RunningGold_Sunset"
            ),
            new ChestEntry(
                "c5ec6f52e8dbb9c4da7b757521618417",
                "Sunset Maze — Middle Crate",
                "ChestID_Overworld_SunsetMaze_ChestTrigger_01"
            ),
            new ChestEntry(
                "c5135337e4fcd3148842b15dc6173d89",
                "Sunset Maze — Southwest Crate",
                "ChestID_Overworld_SunsetMaze_ChestTrigger_02"
            ),
            new ChestEntry(
                "b8705aa0b55653945832585e5490180b",
                "Macho Dog Chest",
                "ChestID_Overworld_SunsetMaze_ChestTrigger_MachoDog"
            ),
            new ChestEntry(
                "92f3c5e9cd46a684ea99a273b11a3507",
                "Twilight Isles — Running Coin Chest",
                "ChestID_Overworld_Twilight_ChestTrigger_RunningGold_Twilight"
            ),
            new ChestEntry(
                "adf7097a005347a4db661064c9080f27",
                "Twlight Isles — Crate before Barricade",
                "ChestID_Overworld_Twlight_ChestTrigger_01"
            ),
            new ChestEntry(
                "b864f5c7b30f6784d97fed7e2acffb28",
                "Twlight Isles — Urchin-field Crate",
                "ChestID_Overworld_Twlight_ChestTrigger_02"
            ),
            new ChestEntry(
                "8998202d86997af459daeaa646fdca1b",
                "Twlight Isles — West Chest",
                "ChestID_Overworld_Twlight_ChestTrigger_03"
            ),
            new ChestEntry(
                "a5a766e09d1501d4294a69afa881c923",
                "Twlight Isles — Oinker Field Bush Chest",
                "ChestID_Overworld_Twlight_ChestTrigger_04"
            ),
            new ChestEntry(
                "68b2cc3e2778b814a88f462a694205ae",
                "Purrgatory Silver Chest - Purrvana",
                "ChestID_Quests_FearsofthePawst_ChestTrigger_Purrgatory_01"
            ),
            new ChestEntry(
                "b8310d164078e92438d3ff599c2930cd",
                "Purrgatory Silver Chest - Twilight Isles",
                "ChestID_Quests_FearsofthePawst_ChestTrigger_Purrgatory_02"
            ),
            new ChestEntry(
                "a4a0ff1717a25354a9fea38bb192c7a0",
                "Purrgatory Silver Chest - Sunset Isles",
                "ChestID_Quests_FearsofthePawst_ChestTrigger_Purrgatory_03"
            ),
            new ChestEntry(
                "673c1ba89895c1044a67380b86e1aa93",
                "Fishercat — Fish Blue Chest",
                "ChestID_Quests_Fishercat_ChestTrigger_FishBlue"
            ),
            new ChestEntry(
                "29c1fe796bb21604cb38c8490b9403a7",
                "Fishercat — Fish Ghost Chest",
                "ChestID_Quests_Fishercat_ChestTrigger_FishGhost"
            ),
            new ChestEntry(
                "b272609e38d2da641b02478112f3dc2a",
                "Fishercat — Fish Love Chest",
                "ChestID_Quests_Fishercat_ChestTrigger_FishLove"
            ),
            new ChestEntry(
                "41e8fc856247df649b91d4feab8ab5ea",
                "Fishercat — Fish Rock Chest",
                "ChestID_Quests_Fishercat_ChestTrigger_FishRock"
            ),
            new ChestEntry(
                "1a560e2024cc4544b9088b89d3438ceb",
                "Fishercat — Fish Sunset Chest",
                "ChestID_Quests_Fishercat_ChestTrigger_FishSunset"
            ),
            new ChestEntry(
                "90477425058ebba4fab00aa1874f8cb6",
                "Fishercat — Treasure Chest",
                "ChestID_Quests_Fishercat_ChestTrigger_Treasure"
            ),
            new ChestEntry(
                "a7b8e9e0c3078d94a80d95b1a2212d18",
                "Purrseidon Treasure Chest",
                "ChestID_Quests_PurrseidonsTrident_ChestTrigger_PurrseidonTreasure"
            ),
            new ChestEntry(
                "0ee1b826db5619c49820c97e1f40a603",
                "Sibling Rivalry — Heirloom Treasure Chest",
                "ChestID_Quests_SiblingRivalry_ChestTrigger_HeirloomTreasure"
            ),
            new ChestEntry(
                "538666e2a65e6a541b5f32698dd4242c",
                "Sibling Rivalry — Starfish Chest",
                "ChestID_Quests_SiblingRivalry_ChestTrigger_Starfish"
            ),
            new ChestEntry(
                "351906c663d633a479e75b7e8e041ec7",
                "Ruins Antares — 1F Chest",
                "ChestID_Ruins_RuinsAntares_ChestTrigger_01"
            ),
            new ChestEntry(
                "b00f38fee899baa47ad8bbc0ee8b87ea",
                "Ruins Antares — 2F Guarded Chest",
                "ChestID_Ruins_RuinsAntares_ChestTrigger_02"
            ),
            new ChestEntry(
                "82a4dc6f4295d1340b424cfbbcf73e9f",
                "Ruins Antares — 2F Hallway Crate",
                "ChestID_Ruins_RuinsAntares_ChestTrigger_03"
            ),
            new ChestEntry(
                "4f768de2337b01f4f84b7b8c39bbce95",
                "Ruins Antares — 2F Silver Boss Chest",
                "ChestID_Ruins_RuinsAntares_ChestTrigger_AntaresTreasure"
            ),
            new ChestEntry(
                "4eb17e845d7483c4cacf4abcb0638589",
                "Ruins Antares — 3F Silver Boss Chest",
                "ChestID_Ruins_RuinsAntares_ChestTrigger_BossChest"
            ),
            new ChestEntry(
                "5d340ab81b0589e499200e3798c33e94",
                "Ruins Centauri — Stairway Chest",
                "ChestID_Ruins_RuinsCentauri_ChestTrigger_01"
            ),
            new ChestEntry(
                "c2930180ed192db4ebf640ce951a0326",
                "Ruins Centauri — Exit Gate Chest",
                "ChestID_Ruins_RuinsCentauri_ChestTrigger_02"
            ),
            new ChestEntry(
                "15623d4fa9a260e49924e8eaadc94da4",
                "Ruins Centauri — Silver Boss Chest",
                "ChestID_Ruins_RuinsCentauri_ChestTrigger_CentauriTreasure"
            ),
            new ChestEntry(
                "483118acd52fb634096af9c84c13459f",
                "Ruins Orion — 1F Crate",
                "ChestID_Ruins_RuinsOrion_ChestTrigger_01"
            ),
            new ChestEntry(
                "0f2feba36548bd7418abb20cf712f29a",
                "Ruins Orion — 2F Hallway Chest",
                "ChestID_Ruins_RuinsOrion_ChestTrigger_02"
            ),
            new ChestEntry(
                "5d63f0b6fac9f184c942a0d0bde6bc32",
                "Ruins Orion — 3F Middle Chest",
                "ChestID_Ruins_RuinsOrion_ChestTrigger_03"
            ),
            new ChestEntry(
                "6fbf13274c1400f49873c0364a291cf5",
                "Ruins Orion — 3F Southwest Chest",
                "ChestID_Ruins_RuinsOrion_ChestTrigger_04"
            ),
            new ChestEntry(
                "58fa6214e5a835c4e9892081f1985237",
                "Ruins Orion — 3F Northeast Crate",
                "ChestID_Ruins_RuinsOrion_ChestTrigger_05"
            ),
            new ChestEntry(
                "22e3c82ce535c2a4ebc2c53e50a5e28f",
                "Ruins Orion — 4F Boss Silver Chest",
                "ChestID_Ruins_RuinsOrion_ChestTrigger_BossChest"
            ),
            new ChestEntry(
                "3ea8ee2370c612e438a1a87573659360",
                "Ruins Orion — 2F Jailed Silver Chest",
                "ChestID_Ruins_RuinsOrion_ChestTrigger_OrionTreasure"
            ),
            new ChestEntry(
                "f486e3102443a1247a74a86c4e20eb3d",
                "Ruins Polaris — 1F Puzzle Crate",
                "ChestID_Ruins_RuinsPolaris_ChestTrigger_01"
            ),
            new ChestEntry(
                "e4a19150ff4a0b148a88dfc1177ec0ad",
                "Ruins Polaris — 2F Puzzle Crate",
                "ChestID_Ruins_RuinsPolaris_ChestTrigger_02"
            ),
            new ChestEntry(
                "f0ae325c8aebd1f498f65f6cca2f3ce2",
                "Ruins Polaris — 3F Puzzle Chest",
                "ChestID_Ruins_RuinsPolaris_ChestTrigger_03"
            ),
            new ChestEntry(
                "c8ce1e3d1398ebb40b24504a83f8a9b6",
                "Ruins Polaris — 3F Crate",
                "ChestID_Ruins_RuinsPolaris_ChestTrigger_04"
            ),
            new ChestEntry(
                "59dbcf423b7833645930a13b9c08fa59",
                "Ruins Polaris — 1F Arena Chest",
                "ChestID_Ruins_RuinsPolaris_ChestTrigger_PolarisTreasure"
            ),
            new ChestEntry(
                "ad374b5039a7e65428b67123e6eb0241",
                "Smithy — Kidd Cat's Treasure Chest",
                "ChestID_Smithy_ChestTrigger_KiddCat"
            ),
            new ChestEntry(
                "fe61992df5a3b424d9d54d570f469efc",
                "Cathulu Treasure Chest",
                "ChestID_Sunset_SQ_Cathulu_ChestTrigger_CathuluTreasure"
            ),
            new ChestEntry(
                "b8f61f3e00ecea04681c4da63a125264",
                "Tavern — Mama Milka's Treasure Chest",
                "ChestID_Tavern_ChestTrigger_MamaMilka"
            ),
            new ChestEntry(
                "a3a76bece3af81b4a9fda9683e142135",
                "Wanted Poster Boar Boss Chest",
                "ChestID_Tavern_WantedPoster_BoarBoss"
            ),
            new ChestEntry(
                "cec4cb43c43ff1947a098e33eafdbd1a",
                "Wanted Poster Clawford Chest",
                "ChestID_Tavern_WantedPoster_Clawford"
            ),
            new ChestEntry(
                "d17953ae3c25506468127f9303ecc6af",
                "Wanted Poster Dratcula Chest",
                "ChestID_Tavern_WantedPoster_Dratcula"
            ),
            new ChestEntry(
                "1593c8645e241fd4985755ec1b3893ee",
                "Wanted Poster Fire Captain Chest",
                "ChestID_Tavern_WantedPoster_FireMageBoss"
            ),
            new ChestEntry(
                "3c6679dc4786d1e488cf3eb788de02d1",
                "Wanted Poster Ice Captain Chest",
                "ChestID_Tavern_WantedPoster_IceMageBoss"
            ),
            new ChestEntry(
                "77715414f8e354bcda5c757d2c9fd052",
                "Wanted Poster Lonely Boar Chest",
                "ChestID_Tavern_WantedPoster_LonelyBoar"
            ),
            new ChestEntry(
                "713ca6ef34e68488e869322864fff567",
                "Wanted Poster Macho Dog Chest",
                "ChestID_Tavern_WantedPoster_MachoDog"
            ),
            new ChestEntry(
                "64a38d13326f05d48aaed811df73ea14",
                "Wanted Poster Mewtallica Chest",
                "ChestID_Tavern_WantedPoster_Mewtallica"
            ),
            new ChestEntry(
                "7f60a487de20f3442ad04cfd731d525f",
                "Wanted Poster Necromouser Chest",
                "ChestID_Tavern_WantedPoster_Necromouser"
            ),
            new ChestEntry(
                "45b6bcffd0fe2a040866e875c5bc1091",
                "Wanted Poster Pig Boss Chest",
                "ChestID_Tavern_WantedPoster_PigBoss"
            ),
            new ChestEntry(
                "3932ca5ac640623479aad9802c072acb",
                "Wanted Poster Pirate King Chest",
                "ChestID_Tavern_WantedPoster_PirateKing"
            ),
            new ChestEntry(
                "2886586db97f4844185d51a93607616a",
                "Wanted Poster Rubber Duck Chest",
                "ChestID_Tavern_WantedPoster_RubberDuck"
            ),
            new ChestEntry(
                "a35cc2876b24ad644a56fb48e53b83dc",
                "Wanted Poster Takomeowki Chest",
                "ChestID_Tavern_WantedPoster_Takomeowki"
            ),
            new ChestEntry(
                "31efb466be90b0c48a78e21acf18f41a",
                "Zero Dimension — Midwest Silver Chest",
                "ChestID_ZeroDimension_ChestTrigger_01"
            ),
            new ChestEntry(
                "55277548c718ace4383d3069933a9a90",
                "Zero Dimension — East Silver Chest",
                "ChestID_ZeroDimension_ChestTrigger_02"
            ),
            new ChestEntry(
                "5beccabce2ecdc942b327dddd868b1b6",
                "Zero Dimension — Northwest Silver Chest",
                "ChestID_ZeroDimension_ChestTrigger_03"
            ),
        };

        private static readonly Dictionary<string, ChestEntry> _byGuid =
            BuildGuidLookup();

        public static IEnumerable<ChestEntry> Entries
        {
            get { return _entries; }
        }

        public static bool TryGetByGuid(
            string guid,
            out ChestEntry entry
        )
        {
            if (string.IsNullOrEmpty(guid))
            {
                entry = null;
                return false;
            }

            return _byGuid.TryGetValue(
                guid,
                out entry
            );
        }

        public static string GetDisplayName(
            string guid
        )
        {
            ChestEntry entry;

            if (
                TryGetByGuid(
                    guid,
                    out entry
                )
            )
            {
                return entry.DisplayName;
            }

            return guid;
        }

        private static Dictionary<string, ChestEntry> BuildGuidLookup()
        {
            Dictionary<string, ChestEntry> lookup =
                new Dictionary<string, ChestEntry>();

            foreach (ChestEntry entry in _entries)
            {
                lookup.Add(
                    entry.Guid,
                    entry
                );
            }

            return lookup;
        }
    }
}
