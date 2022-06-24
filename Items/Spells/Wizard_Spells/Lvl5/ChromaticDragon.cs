using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace DnD.Items.Spells.Wizard_Spells.Lvl5
{
    internal class ChromaticDragon : ModItem
    {
        public override string Texture => "Terraria/Images/Item_" + ItemID.ShadowbeamStaff;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chromatic Dragon");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.damage = 70;
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 0;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<Drag1>();
            Item.channel = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Item.shoot || Main.projectile[i].type == ModContent.ProjectileType<Drag2>() || Main.projectile[i].type == ModContent.ProjectileType<Drag3>() || Main.projectile[i].type == ModContent.ProjectileType<Drag4>())
                {
                    Main.projectile[i].Kill();
                }
            }

            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Drag2>(), damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Drag3>(), damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Drag4>(), damage, knockback, player.whoAmI);
            return true;
        }
    }

    internal class Dragon : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ShadowBeamFriendly;

        public Vector2 botPoint { get; set; }

        int[] parts = { ModContent.ProjectileType<Drag1>(), ModContent.ProjectileType<Drag2>(), ModContent.ProjectileType<Drag3>(), ModContent.ProjectileType<Drag4>() };

        bool spawnOnce = true;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chromatic Dragon");
        }

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.width = 12;
            Projectile.height = 28;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;

            botPoint = new Vector2(Projectile.Bottom.X, Projectile.Bottom.Y);
        }

        public override void AI()
        {
            Player player = Main.LocalPlayer;
            for (int i = 0; i < 1000; i++)
            {
                if (Main.player[Projectile.owner].dead || !player.channel)
                {
                    Projectile.Kill();
                }
            }

            for (int i = 1; i < 1000; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == parts[i])
                {
                    for (int x = 1; x < parts.Length; x++)
                    {

                    }
                }
            }
        }
    }

    internal class Drag1 : Dragon
    {
        Player player = Main.player[Main.myPlayer];

        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.StardustDragon1;
        public override void AI()
        { 
            if (Main.player[Projectile.owner].dead || !player.channel)
            {
                Projectile.Kill();
            }

            Vector2 target = Main.MouseWorld;
            Vector2 dir = target - Projectile.Center;
            Vector2 vel = Vector2.Normalize(dir) * 10;

            Projectile.velocity = vel;
            Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2;

            Projectile.direction = (Projectile.spriteDirection = ((Projectile.velocity.X > 0f) ? 1 : (-1)));
        }
    }

    internal class Drag2 : Dragon
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.StardustDragon2;

    }

    internal class Drag3 : Dragon
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.StardustDragon3;

    }

    internal class Drag4 : Dragon
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.StardustDragon4;
    }
}