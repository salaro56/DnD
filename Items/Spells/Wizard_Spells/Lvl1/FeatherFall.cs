﻿using System;
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
using DnD.Common;

namespace DnD.Items.Spells.Wizard_Spells.Lvl1
{
    internal class FeatherFall : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault(value: "[c/FF0000:Level 1:]" +
                "\n Provides slowfall for the duration of the spell"); */
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
            Item.value = Item.sellPrice(0, 0, 10, 0);
            Item.mana = 2;
            Item.noUseGraphic = true;
            Item.shoot = ModContent.ProjectileType<FeatherBurst>();
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
                .AddCondition(Conditions.IsWizard)
                .AddCondition(Conditions.IsRightLevel(1))
                .Register();
        }

    }

    internal class FeatherBurst : ModProjectile
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
                player.AddBuff(BuffID.Featherfall, 3400);
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Vector2 position = Main.MouseWorld;
                for (int i = 0; i < 20; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(2.5f, 1f);
                    Dust d = Dust.NewDustPerfect(new Vector2(position.X, position.Y), DustID.Smoke, speed * 4, Scale: 2f);
                    d.velocity *= 0.5f;
                    d.noGravity = false;
                    d.color = Color.White;
                }
                for (int i = 0; i < 20; i++)
                {
                    Vector2 speed = Main.rand.NextVector2Circular(2.5f, 1f);
                    Dust d = Dust.NewDustPerfect(new Vector2(position.X, position.Y), DustID.Smoke, speed * 4, Scale: 2f);
                    d.velocity *= 0.5f;
                    d.noGravity = false;
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
