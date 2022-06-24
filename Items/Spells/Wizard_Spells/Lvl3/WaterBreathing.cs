using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using System.IO;

namespace DnD.Items.Spells.Wizard_Spells.Lvl3
{
    internal class WaterBreathing : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(value: "[c/FF0000:Level 3:]" +
                "\n Allows the user to breath water as if it were air");
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.scale = 0.5f;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item4;
            Item.rare = ModContent.RarityType<Rarities.WizardRare>();
            Item.value = 0;
            Item.mana = 2;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<WaterBreath>();
        }

        public override bool CanUseItem(Player player)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            if (pc.wizardClass == true)
            {
                return true;
            }
            else return false;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = Main.MouseWorld;
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
            return false;
        }


        public override void AddRecipes()
        {
            CreateRecipe()
                 .AddIngredient(ModContent.ItemType<Items.ClassToken>())
                .AddTile(ModContent.TileType<Furniture.PHBTile>())
                .AddCondition(NetworkText.FromLiteral("Must be of level 3 or higher"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().playerLevel >= 3)
                .AddCondition(NetworkText.FromLiteral("Must be the right class"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().wizardClass == true)
                .Register();
        }

    }

    internal class WaterBreath : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ShadowBeamFriendly;

        public override void SetDefaults()
        {
            //proj stats
            Projectile.penetrate = 5;

            //proj configs
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 5;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.LocalPlayer;

            if (Projectile.Hitbox.Intersects(player.Hitbox))
            {
                for (int i = 0; i < 20; i++)
                {
                    Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);
                    Dust d = Dust.NewDustPerfect(new Vector2(player.Center.X, player.Center.Y), DustID.Water, speed * 2, Scale: 2f);
                    d.noGravity = false;
                    d.color = Color.White;
                }
                player.AddBuff(BuffID.Gills, 18000);
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Vector2 position = Main.MouseWorld;
                for (int i = 0; i < 20; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(4f, 4f);
                    Dust d = Dust.NewDustPerfect(new Vector2(position.X, position.Y), DustID.Water, speed * 4, Scale: 2f);
                    d.velocity *= 0.5f;
                    d.noGravity = true;
                    d.color = Color.White;
                }
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}