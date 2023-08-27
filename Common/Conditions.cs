using Terraria.Localization;
using Terraria;
using Terraria.ModLoader;

namespace DnD.Common
{
    public static class Conditions
    {
        public static Condition IsWizard = new Condition("Is of Wizard Class", () => Main.LocalPlayer.GetModPlayer<DnDPlayer>().wizardClass == true);
        public static Condition IsCleric = new Condition("Is of Cleric Class", () => Main.LocalPlayer.GetModPlayer<DnDPlayer>().clericClass == true);
        public static Condition IsBarb = new Condition("Is of Barbarian Class", () => Main.LocalPlayer.GetModPlayer<DnDPlayer>().barbClass == true);
        public static Condition IsRogue = new Condition("Is of Rogue Class", () => Main.LocalPlayer.GetModPlayer<DnDPlayer>().rogueClass == true);
        public static Condition IsRanger = new Condition("Is of Ranger Class", () => Main.LocalPlayer.GetModPlayer<DnDPlayer>().rangerClass == true);
        public static Condition IsFighter = new Condition("Is of Fighter Class", () => Main.LocalPlayer.GetModPlayer<DnDPlayer>().fighterClass == true);
        public static Condition IsPaladin = new Condition("Is of Paladin Class", () => Main.LocalPlayer.GetModPlayer<DnDPlayer>().paladinClass == true);
        public static Condition IsSorc = new Condition("Is of Sorcerer Class", () => Main.LocalPlayer.GetModPlayer<DnDPlayer>().sorcClass == true);
        public static Condition IsWarlock = new Condition("Is of Warlock Class", () => Main.LocalPlayer.GetModPlayer<DnDPlayer>().warlockClass == true);

        public static Condition IsRightLevel(int requiredLevel) => new Condition($"Must be level {requiredLevel}", () => Main.LocalPlayer.GetModPlayer<DnDPlayer>().playerLevel >= requiredLevel);
    }
}
