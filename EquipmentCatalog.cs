using System;
using System.Collections.Generic;

namespace LiveSplit.CatQuest3
{
    public static class EquipmentCatalog
    {
        public sealed class EquipmentEntry
        {
            public string Value { get; private set; }
            public string DisplayName { get; private set; }
            public string InternalAssetName { get; private set; }
            public string Guid { get; private set; }

            public EquipmentEntry(string value, string displayName, string internalAssetName, string guid)
            {
                Value = value; DisplayName = displayName; InternalAssetName = internalAssetName; Guid = guid;
            }
        }

        private static readonly EquipmentEntry[] _entries = new EquipmentEntry[]
        {
            new EquipmentEntry("AntaresBody", "Antares' Armor", "Equipment_Antares_Body", "2a19ecdd4b825354296c219bbd4fe166"),
            new EquipmentEntry("AntaresHead", "Antares' Cavalier", "Equipment_Antares_Head", "315e51fe6a3095b42b303215e83fe805"),
            new EquipmentEntry("AntaresWand", "Stave of Antares", "Equipment_Antares_Wand", "2b23dd80b39373c47802ab20782ebcb2"),
            new EquipmentEntry("ClawfordBody", "Clawford's Vest", "Equipment_Clawford_Body", "72e52ed580d288b4c86778032adf010d"),
            new EquipmentEntry("ClawfordHead", "Clawford's Hat", "Equipment_Clawford_Head", "6f745808351b0b94da3e17cfecb5b09a"),
            new EquipmentEntry("ClawfordShieldHeartbreaker", "Heartbreaker", "Equipment_Clawford_Shield_Heartbreaker", "0e5106fb19ef49c41b39d6cef53c1efe"),
            new EquipmentEntry("ClawfordShieldHeartmender", "Heartmender", "Equipment_Clawford_Shield_Heartmender", "a8b1b5a198a523242afdbef645fbc878"),
            new EquipmentEntry("DragonboneClaws", "The Dragonbone", "Equipment_Dragonbone_Claws", "05a3052309998477998604ae09e04138"),
            new EquipmentEntry("DratculaBody", "Dratcula's Coat", "Equipment_Dratcula_Body", "50e881e98d4643747aa91f034b3d08b6"),
            new EquipmentEntry("DratculaHead", "Dratcula's Topper", "Equipment_Dratcula_Head", "191f43b910b9f90439fbda4606d93575"),
            new EquipmentEntry("DratculaSword", "Bloodsucker", "Equipment_Dratcula_Sword", "5bc5771112e39724da71a8acb6b1ac64"),
            new EquipmentEntry("FishercatBody", "Fishercat Attire", "Equipment_Fishercat_Body", "7b91d604cdaea5041a23fa0f960eff3e"),
            new EquipmentEntry("FishercatFishingGun", "The Son's Rod", "Equipment_Fishercat_FishingGun", "68200e67071238644bfcb12ab7f25d62"),
            new EquipmentEntry("FishercatHead", "Fishercat Hat", "Equipment_Fishercat_Head", "5e7fae7d2a4801241958b53f5dfd0950"),
            new EquipmentEntry("GentlebroBody", "Gentlebro Tuxedo", "Equipment_Gentlebro_Body", "d8c33e8f52e4f4b8eb8aa88824833351"),
            new EquipmentEntry("GentlebroHead", "Gentlebro Hat", "Equipment_Gentlebro_Head", "0cdedd7af543b4f9d8a3467e2a04e994"),
            new EquipmentEntry("GentlebroWand", "The Architect", "Equipment_Gentlebro_Wand", "6802dc76bfc0c43138904cf31e3a135b"),
            new EquipmentEntry("GoldenBody", "Golden Armor", "Equipment_Golden_Body", "265e6d948988ee74c8028e3a0c991710"),
            new EquipmentEntry("GoldenHead", "Golden Helm", "Equipment_Golden_Head", "79d36535bbeafbf44a4e6f38efe46f10"),
            new EquipmentEntry("GoldenShield", "Golden Shield", "Equipment_Golden_Shield", "e323a5b66b62ddb4d8eabe6a7f978442"),
            new EquipmentEntry("GunnerBlunderpuss", "Blunderpuss", "Equipment_Gunner_Blunderpuss", "be1a97b87da865e4ba66bd2a2e67b3f9"),
            new EquipmentEntry("GunnerBody", "Gunner Vest", "Equipment_Gunner_Body", "b8cdf4f95ea10f545bd6c1aa6c0c9a2c"),
            new EquipmentEntry("GunnerFlintlock", "Furlintlock", "Equipment_Gunner_Flintlock", "deceac1753640df498d791d9f5f0c730"),
            new EquipmentEntry("GunnerHead", "Gunner Eyepatch", "Equipment_Gunner_Head", "a89e219720277d445aa44c1ecf883231"),
            new EquipmentEntry("KnightBody", "Knight Armor", "Equipment_Knight_Body", "25d8454650e86dd44a477eea9e98fed1"),
            new EquipmentEntry("KnightHead", "Knight Hat", "Equipment_Knight_Head", "b7785028aedfb724dac830ed9e28f18c"),
            new EquipmentEntry("KnightShield", "Knight Shield", "Equipment_Knight_Shield", "4de760602ad941d4da69d4de7b965a34"),
            new EquipmentEntry("MachoBody", "Macho Suit", "Equipment_Macho_Body", "0b20e35ff4a848a439f46136742053f8"),
            new EquipmentEntry("MageArcaneBody", "Arcane Mage Vest", "Equipment_MageArcane_Body", "fc70ba5ad13881747838751bfa015f93"),
            new EquipmentEntry("MageArcaneHead", "Arcane Mage Hat", "Equipment_MageArcane_Head", "372460f0f3ceea74785f1b1317778bfa"),
            new EquipmentEntry("MageArcaneWand", "Arcane Wand", "Equipment_MageArcane_Wand", "da085ee77836af34fa74c9289db12ae2"),
            new EquipmentEntry("MageFireBody", "Fire Mage Vest", "Equipment_MageFire_Body", "ea1bd2467b130094490bcc54a0fd45f9"),
            new EquipmentEntry("MageFireHead", "Fire Mage Hat", "Equipment_MageFire_Head", "3a6541654bd805b48acae446af1c8f41"),
            new EquipmentEntry("MageFireWand", "Fire Wand", "Equipment_MageFire_Wand", "511e00ecb4b224442a822c5d193a608e"),
            new EquipmentEntry("MageIceBody", "Ice Mage Vest", "Equipment_MageIce_Body", "2a1e5ebb40871634385970ccca6ab09d"),
            new EquipmentEntry("MageIceHead", "Ice Mage Hat", "Equipment_MageIce_Head", "afe3168cc9e6dfb4d9494ed8e585bf99"),
            new EquipmentEntry("MageIceWand", "Ice Wand", "Equipment_MageIce_Wand", "1d1e375b82aa5e54c9d95580c261104d"),
            new EquipmentEntry("MewtallicaAxe", "Meowtallika Axe", "Equipment_Mewtallica_Axe", "7194c5e8666e60349a53faa508626aa6"),
            new EquipmentEntry("MewtallicaBody", "Meowtallika Shirt", "Equipment_Mewtallica_Body", "7577162b107ea814dac7bc94a5bc73bb"),
            new EquipmentEntry("MewtallicaHead", "Meowtallika Helm", "Equipment_Mewtallica_Head", "bc5d04eda9a275140b45d43ec560c9bb"),
            new EquipmentEntry("MewtallicaBossBody", "Meowtallika's Jacket", "Equipment_MewtallicaBoss_Body", "b42a74af9127a1546af227630f195521"),
            new EquipmentEntry("MewtallicaBossHead", "Meowtallika's Tricorne", "Equipment_MewtallicaBoss_Head", "cb1e055b9f101d94d849f098e0c767fa"),
            new EquipmentEntry("MewtallicaBossRocker", "The Rocker", "Equipment_MewtallicaBoss_Rocker", "50acd59b67faf194dbbf08ee7d181aa3"),
            new EquipmentEntry("NorthStarBody", "North Star Coat", "Equipment_NorthStar_Body", "e80d3aaa2222c4cb8adf4e90b5294381"),
            new EquipmentEntry("NorthStarClaws", "The Guiding Claw", "Equipment_NorthStar_Claws", "10dc602349d724047a814545e0f23ad8"),
            new EquipmentEntry("NorthStarGun", "The Guiding Light", "Equipment_NorthStar_Gun", "b6bb6a7a321f74d3fbf158fb9e6da379"),
            new EquipmentEntry("NorthStarHead", "North Star Bicorne", "Equipment_NorthStar_Head", "9e7287eff0140485394ef479b49431aa"),
            new EquipmentEntry("NorthStarShield", "The Guiding Wall", "Equipment_NorthStar_Shield", "d2955e7c2ddc742e1991c0f65cfe47ae"),
            new EquipmentEntry("NorthStarSword", "The Guiding Blade", "Equipment_NorthStar_Sword", "3a3d9eb918bec4250afa91e79ab9c4d8"),
            new EquipmentEntry("NorthStarWand", "The Guiding Star", "Equipment_NorthStar_Wand", "8f96b8fd5ba4e4eef88c56304c467d0f"),
            new EquipmentEntry("OinkerBody", "Oinker Garments", "Equipment_Oinker_Body", "cc5bb1210428f466ca01fff6bba14a40"),
            new EquipmentEntry("OinkerHead", "Oinker Headdress", "Equipment_Oinker_Head", "5219457213ff34fc38f86f28e343d37c"),
            new EquipmentEntry("OinkerShield", "Oinker Shield", "Equipment_Oinker_Shield", "7297468d864ec4ae587bb8b15587d63d"),
            new EquipmentEntry("OrionBody", "Orion's Armor", "Equipment_Orion_Body", "db1335b11a1572c4caf632f8fc06c935"),
            new EquipmentEntry("OrionHead", "Orion's Cavalier", "Equipment_Orion_Head", "dd2394eb14ca08f4587611deef3ad431"),
            new EquipmentEntry("OrionSword", "Blade of Orion", "Equipment_Orion_Sword", "ad14fa34366d8a24593ddec86b5685e2"),
            new EquipmentEntry("PatchyBody", "Patchy's Coat", "Equipment_Patchy_Body", "35b1114715ace1e4287c943e28c47af8"),
            new EquipmentEntry("PatchyClaws", "Patchy's Claws", "Equipment_Patchy_Claws", "2c80b4f47c565924c8b254fe9e19ddc4"),
            new EquipmentEntry("PatchyHead", "Patchy's Hood", "Equipment_Patchy_Head", "06d833928ae2247429482ca8860399c8"),
            new EquipmentEntry("PirateBody", "Pirate Shirt", "Equipment_Pirate_Body", "a9a12b1d652518c4c833750e965ad108"),
            new EquipmentEntry("PirateHead", "Pirate Bandana", "Equipment_Pirate_Head", "996c2d9486433f74cafe4879a88a16f2"),
            new EquipmentEntry("PirateSword", "Pirate Cutlass", "Equipment_Pirate_Sword", "f14e4bbc80f3e6f4f8c6992049be567e"),
            new EquipmentEntry("PirateKingBody", "Pi-rat King's Coat", "Equipment_PirateKing_Body", "d2dfe7bfeadd52949acd59f25acafd34"),
            new EquipmentEntry("PirateKingClaws", "Death's Hook", "Equipment_PirateKing_Claws", "e5022c29d9917bd4abaa23319d34d0e0"),
            new EquipmentEntry("PirateKingHead", "Pi-rat King's Bicorne", "Equipment_PirateKing_Head", "8cfd849e295cf2d4a95d6f4f2e0aeaf2"),
            new EquipmentEntry("PrivateerBody", "Purrivateer Vest", "Equipment_Privateer_Body", "83801aa69466a4f469081e4cf7aa5214"),
            new EquipmentEntry("PrivateerHead", "Purrivateer Hat", "Equipment_Privateer_Head", "e41ef77460893a846bc2e55d5bb55aab"),
            new EquipmentEntry("PrivateerMachineGun", "Meowchine Gun", "Equipment_Privateer_MachineGun", "3887830cfdb07bf48ad72b4eaddd72fc"),
            new EquipmentEntry("PurccaneerBody", "Purccaneer Shirt", "Equipment_Purccaneer_Body", "4e6ec354a142b46a28584ba49a12f11c"),
            new EquipmentEntry("PurccaneerHead", "Purccaneer Bandana", "Equipment_Purccaneer_Head", "2a77b2ebec8fe45ef82139e8131e0a4d"),
            new EquipmentEntry("PurccaneerSword", "Purccaneer Cutlass", "Equipment_Purccaneer_Sword", "6f5c149052fb745bda2cd8f255c595d6"),
            new EquipmentEntry("PurrgatoryHead", "The Mask of Purrgatory", "Equipment_Purrgatory_Head", "4f68b5975ee4a5b4bae55c66ce80dc98"),
            new EquipmentEntry("PurrmaidBody", "Purrmaid Vest", "Equipment_Purrmaid_Body", "c351f25f947081b4e800a28c00837a39"),
            new EquipmentEntry("PurrmaidClaws", "Tri-Claws", "Equipment_Purrmaid_Claws", "fe839ed1e2e988545a0f40899439a213"),
            new EquipmentEntry("PurrmaidHead", "Purrmaid Helm", "Equipment_Purrmaid_Head", "26958f850750eb0479a3a8bc1852deaf"),
            new EquipmentEntry("PurrseidonTrident", "Purrseidon's Trident", "Equipment_Purrseidon_Trident", "a84918523ed6a2f44937469e9658704d"),
            new EquipmentEntry("RogueBody", "Rogue Vest", "Equipment_Rogue_Body", "5505a271cff345949a5853a64812a63f"),
            new EquipmentEntry("RogueClaws", "Rogue Claws", "Equipment_Rogue_Claws", "60707e1a9eae5ef40ae67003fe152d2d"),
            new EquipmentEntry("RogueHead", "Rogue Bandana", "Equipment_Rogue_Head", "db71318237afdd94f9cc735465ba4d2a"),
            new EquipmentEntry("SqueakyHead", "Squeaky's Mop", "Equipment_Squeaky_Head", "915f26a96425b264c83a9a40f23ec0ef"),
            new EquipmentEntry("SquidBody", "Spicy Vest", "Equipment_Squid_Body", "14e768b983d6e444ca2e6bed3eaac492"),
            new EquipmentEntry("SquidHead", "Spicy Bandana", "Equipment_Squid_Head", "476e7ec69b0c83c42ab943af38723942"),
            new EquipmentEntry("SquidSquiderpuss", "Squiderpuss", "Equipment_Squid_Squiderpuss", "4acebff9ed392764b9841ae7acbc5b47"),
            new EquipmentEntry("TakomeowkiBody", "Takomeowki's Coat", "Equipment_Takomeowki_Body", "da33a185d3f67534eac9f8dce133fbff"),
            new EquipmentEntry("TakomeowkiHead", "Takomeowki's Bicorne", "Equipment_Takomeowki_Head", "3bda770fe9f979a499b9dc4c1ae4bf10"),
            new EquipmentEntry("TakomeowkiTsurai", "Tsurai", "Equipment_Takomeowki_Tsurai", "23240f77bf616924d92c8f9b3d5d9177"),
            new EquipmentEntry("TrinketBirdPoop", "Bird Poop", "Equipment_Trinket_BirdPoop", "321ebd701eff1264eb7f7296d033831c"),
            new EquipmentEntry("TrinketBoarTusk", "Boar Tusk", "Equipment_Trinket_BoarTusk", "55629125b5625984ca727b181c9a87b4"),
            new EquipmentEntry("TrinketBombSatchel", "Bomb Satchel", "Equipment_Trinket_BombSatchel", "2a9ad2fe7abf0124a8ca3d72e91fe6a4"),
            new EquipmentEntry("TrinketChimeowraClaw", "Chimeowra Claw", "Equipment_Trinket_ChimeowraClaw", "ac7868a7350644454af106341d1ab15f"),
            new EquipmentEntry("TrinketDoggyJonesLocket", "Doggy Jones Locket", "Equipment_Trinket_DoggyJonesLocket", "5ccbc4e4af1b0d74aa9d295226a4f8c6"),
            new EquipmentEntry("TrinketDragonBoarBall", "Dragon Boar Ball", "Equipment_Trinket_DragonBoarBall", "d1f9a6d20248e3f4e8678d2bf828dc5e"),
            new EquipmentEntry("TrinketElectricCircuit", "Electric Circuit", "Equipment_Trinket_ElectricCircuit", "b39ec733282179049a39f0ce28245c73"),
            new EquipmentEntry("TrinketFlameCrystal", "Flame Crystal", "Equipment_Trinket_FlameCrystal", "4af61a79efc84e94385da5cda75eef83"),
            new EquipmentEntry("TrinketIceCrystal", "Ice Crystal", "Equipment_Trinket_IceCrystal", "e08d84b88c3622a439496c2c4cacae0f"),
            new EquipmentEntry("TrinketLousyBoot", "Lousy Boot", "Equipment_Trinket_LousyBoot", "f5c57488a278cb9428f39fb4a99157a1"),
            new EquipmentEntry("TrinketLousyGloves", "Lousy Gloves", "Equipment_Trinket_LousyGloves", "e27eced6ee3f7f644b4338224a3de2cb"),
            new EquipmentEntry("TrinketMagicalGlove", "Magical Glove", "Equipment_Trinket_MagicalGlove", "109f7faaf9c9e403fa18bb8b281f4bf9"),
            new EquipmentEntry("TrinketMagicalPunchingBag", "Magical Punching Bag", "Equipment_Trinket_MagicalPunchingBag", "d1f63b68407984a7aacaf1dd4d6c27fc"),
            new EquipmentEntry("TrinketMagicalPurse", "Magical Purse", "Equipment_Trinket_MagicalPurse", "ac1c4311cf0474e1cbab1f1406a7e375"),
            new EquipmentEntry("TrinketMail", "The Furst Mail", "Equipment_Trinket_Mail", "4abccf991e5427b4ca02c44e08925c2d"),
            new EquipmentEntry("TrinketMewtallicasPick", "Meowtallika's Pick", "Equipment_Trinket_Mewtallica'sPick", "9e84470ed00c12d44befdadb7653b606"),
            new EquipmentEntry("TrinketMilkHearty", "Hearty Milk", "Equipment_Trinket_MilkHearty", "087db2a1ab2cc46ddb65cf498b40850e"),
            new EquipmentEntry("TrinketMilkMagic", "Magic Milk", "Equipment_Trinket_MilkMagic", "0eea9849b2f2c4acd8c88a54739f7edc"),
            new EquipmentEntry("TrinketMilkPawer", "Pawer Milk", "Equipment_Trinket_MilkPawer", "a2b5671b1eda82d43835e71eb2db25e9"),
            new EquipmentEntry("TrinketMilkSpecial", "Mama Milka's Special", "Equipment_Trinket_MilkSpecial", "c3a8bf3256d4d524199c03256b382e24"),
            new EquipmentEntry("TrinketNecropawmicon", "The Necropawmicon", "Equipment_Trinket_Necropawmicon", "11f1df91d1233e347b28fd752ea56bef"),
            new EquipmentEntry("TrinketOinkerNecklace", "Oinker Necklace", "Equipment_Trinket_OinkerNecklace", "1b40fbbaf162b374e995e4f1baeb9d57"),
            new EquipmentEntry("TrinketOinkerStarAmulet", "Oinker Star Amulet", "Equipment_Trinket_OinkerStarAmulet", "4a1f3c31bb068a049a497f4b921b5321"),
            new EquipmentEntry("TrinketWarriorsBraid", "Warrior's Braid", "Equipment_Trinket_Warrior'sBraid", "485f8e9efe7b88b439833e66e0b7b4aa"),
            new EquipmentEntry("TrinketWarriorsBraid02", "Warrior's Braid II", "Equipment_Trinket_Warrior'sBraid_02", "a2a53c60e7f309842b7f98e743d488ca"),
            new EquipmentEntry("TrinketWeirdMushroom", "Weird Mushroom", "Equipment_Trinket_WeirdMushroom", "5a2435316df0c480ab7b6eb569761ec4")
        };

        public static IEnumerable<EquipmentEntry> Entries { get { return _entries; } }

        public static bool TryGetByValue(string value, out EquipmentEntry entry)
        {
            foreach (EquipmentEntry candidate in _entries)
            {
                if (string.Equals(candidate.Value, value, StringComparison.OrdinalIgnoreCase))
                { entry = candidate; return true; }
            }
            entry = null; return false;
        }

        public static string GetDisplayNameByGuid(string guid)
        {
            foreach (EquipmentEntry candidate in _entries)
            {
                if (string.Equals(candidate.Guid, guid, StringComparison.OrdinalIgnoreCase)) return candidate.DisplayName;
            }
            return guid ?? string.Empty;
        }
    }
}
