using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using DnD.Rarities;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace DnD.Items.Weapons.Swords
{
    internal class BerzerkersAxe : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Berzerker's Axe");
            Tooltip.SetDefault("Your blood boils just by holding this" +
                "\nDoes 5d12 damage" +
                "\nIngnites body when enraged");
        }

        public override void SetDefaults()
        {
            Item.width = 86;
            Item.height = 86;

            Item.damage = 1;
            Item.knockBack = 5;

            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = true;

            Item.useTurn = true;
            Item.rare = ModContent.RarityType<BarbRare>();
        }

        public override void HoldItem(Player player)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            if(pc.isRaging == true)
            {
                player.AddBuff(BuffID.Inferno, 10);
            }
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();

            DnDItem sItem = new();

            damage += sItem.DamageValue(minRoll: 1, maxRoll: 13, diceRolled: 5);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ClassToken>())
                .AddIngredient(ItemID.HellstoneBar, 12)
                .AddIngredient(ItemID.SoulofFright, 3)
                .AddTile(ModContent.TileType<Furniture.MMTile>())
                .Register();
        }
    }
}
