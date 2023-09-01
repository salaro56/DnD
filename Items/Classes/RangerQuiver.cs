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
    internal class RangerQuiver : ClassToken
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ranger's Quiver");
            /* Tooltip.SetDefault("Increases ranged damage by 15%" +
                "\nA tool for rangers"); */
        }

        public override void SetDefaults()
        {
            //Item Stats
            Item.rare = ItemRarityID.Purple;
            Item.defense = 2;

            //Item Configs
            Item.width = 36;
            Item.height = 38;
            Item.accessory = true;
            Item.maxStack = 1;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            DnDPlayer pc = Main.LocalPlayer.GetModPlayer<DnDPlayer>();

            TooltipLine line = new TooltipLine(Mod, "Features", "Features:");
            tooltips.Add(line);
            if(pc.playerLevel >= 5)
            {
                TooltipLine line2 = new TooltipLine(Mod, "Feat", "[c/51DA5F: Extra Attack]");
                tooltips.Add(line2);
                TooltipLine line2a = new TooltipLine(Mod, "Feat", "Occassionally strike again dealing more damage, gain more attacks at higher levels");
                tooltips.Add(line2a);
            }
            if(pc.playerLevel >= 8)
            {
                TooltipLine line3 = new TooltipLine(Mod, "Feat", "[c/51DA5F: Land's Stride]");
                tooltips.Add(line3);
                TooltipLine line3a = new TooltipLine(Mod, "Feat", "Ranget's have greatly increased running speed");
                tooltips.Add(line3a);
            }
            if(pc.playerLevel >= 10)
            {
                TooltipLine line4 = new TooltipLine(Mod, "Feat", "[c/51DA5F: Hide In Plain Sight]");
                tooltips.Add(line4);
                TooltipLine line4a = new TooltipLine(Mod, "Feat", "When standing still you can become camouflauged");
                tooltips.Add(line4a);
            }
            if(pc.playerLevel >= 20)
            {
                TooltipLine line5 = new TooltipLine(Mod, "Feat", "[c/51DA5F: Foe Slayer]");
                tooltips.Add(line5);
                TooltipLine line5a = new TooltipLine(Mod, "Feat", "At 20th level you are capable of adding your proficiency bonus to all damage you deal");
                tooltips.Add(line5a);
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
            pc.rangerClass = true;

            player.GetDamage(DamageClass.Ranged) += 0.15f;

            if(pc.playerLevel >= 8)
            {
                player.maxRunSpeed += 1;
            }
            if(pc.playerLevel >= 10)
            {
                player.shroomiteStealth = true;
            }
            if(pc.playerLevel >= 20)
            {
                player.GetDamage(DamageClass.Generic) += 0.1f;
            }

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
