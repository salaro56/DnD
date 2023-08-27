using DnD.Common.Players;
using DnD.Furniture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DnD.Items.Classes
{
    internal class SpellcastFocus : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Spellcasting Focus");
            /* Tooltip.SetDefault("Increases magic damage by 15%" +
                "\nA required tool for all wizards"); */
        }

        public override void SetDefaults()
        {
            //Item Stats
            Item.rare = ModContent.RarityType<Rarities.WizardRare>();
            Item.defense = 2;

            //Item Configs
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.maxStack = 1;
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
            pc.wizardClass = true;

            player.GetDamage(DamageClass.Magic) += 0.15f;
            player.statManaMax2 += 20;

            if (player.GetModPlayer<DnDPlayer>().SpellSlots == true)
            {
                player.manaRegen = 0;
                player.manaRegenCount = 0;
                player.manaRegenBonus = 0;

                if(Main.time == 0)
                {
                    player.statMana += 9999;
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(ModContent.TileType<PHBTile>())
                .Register();
        }
    }
}
