using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace DnD.Items.Consumable
{
    internal class ManaApple : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = 0;
            Item.rare = ItemRarityID.Purple;
            Item.useStyle = ItemUseStyleID.EatFood;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.UseSound = SoundID.Item2;
            Item.consumable = true;
            Item.buffType = BuffID.ManaSickness;
            Item.buffTime = 60;
        }

        public override bool ConsumeItem(Player player)
        {
            CombatText.NewText(player.getRect(), new Color(30, 30, 230), "Spellslots activated!");
            player.GetModPlayer<DnDPlayer>().SpellSlots = true;
            return true;
        }
    }

    internal class AppleInventory : ModPlayer
    {
        public override IEnumerable<Item> AddStartingItems(bool mediumCoreDeath)
        {
            return new[] {
                new Item(ModContent.ItemType<ManaApple>()),
            };
        }
    }
}
