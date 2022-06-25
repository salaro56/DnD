using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using DnD.Common.Players;

namespace DnD.Items.Classes
{
    internal class HolyEmblem : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Holy Emblem");
            Tooltip.SetDefault("Increases magic damage by 5%" +
                "\nIncreases summon damage by 5%" +
                "\nA focus for clerics");
        }

        public override void SetDefaults()
        {
            //Item Stats
            Item.rare = ModContent.RarityType<Rarities.ClericRare>();
            Item.defense = 5;

            //Item Configs
            Item.width = 22;
            Item.height = 30;
            Item.accessory = true;
            Item.maxStack = 1;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            DnDPlayer pc = Main.LocalPlayer.GetModPlayer<DnDPlayer>();

            TooltipLine line = new TooltipLine(Mod, "Features", "Features:");
            tooltips.Add(line);
            if (pc.playerLevel >= 2)
            {
                TooltipLine line2 = new TooltipLine(Mod, "Feat", "Channel Divinity");
                tooltips.Add(line2);
            }
        }
        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            var accSlot = ModContent.GetInstance<ClassSlot>();
            var accSlot2 = ModContent.GetInstance<ClassSlot2>();

            return slot == accSlot.Type || slot == accSlot2.Type;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            pc.clericClass = true;

            player.GetDamage(DamageClass.Magic) += 0.05f;
            player.GetDamage(DamageClass.Summon) += 0.05f;

            if (player.GetModPlayer<DnDPlayer>().SpellSlots == true)
            {
                player.manaRegen = 0;
                player.manaRegenCount = 0;
                player.manaRegenBonus = 0;

                if (Main.time == 0)
                {
                    player.statMana += 9999;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(ModContent.TileType<Furniture.PHBTile>())
                .Register();
        }
    }
}
