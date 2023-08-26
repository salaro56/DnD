using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;

namespace DnD.Items.Weapons.Ranged
{
    internal class LightCrossbow : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Light Crossbow");
        }

        public override void SetDefaults()
        {
            //item stats
            Item.damage = 18;

            Item.rare = ItemRarityID.Green;

            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.reuseDelay = Item.useAnimation + 4;

            //item configs
            Item.width = 56;
            Item.height = 24;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = false;
            Item.UseSound = SoundID.Item5;

            Item.DamageType = DamageClass.Ranged;
            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 18;
            Item.useAmmo = AmmoID.Arrow;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.WoodenBow)
                .AddIngredient(ItemID.WhiteString)
                .AddRecipeGroup(RecipeGroupID.IronBar, 4)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            velocity = velocity.RotatedByRandom(MathHelper.ToRadians(5));
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6f, -2f);
        }
    }
}
