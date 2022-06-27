using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using Terraria.ID;
using Terraria.ModLoader;

namespace DnD.Items.Weapons.Swords
{
    internal class Aegis : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Aegis Sword");
            Tooltip.SetDefault("A weapon of celestial origin");
        }

        public override void SetDefaults()
        {
            //Item stats
            Item.damage = 30;
            Item.DamageType = DamageClass.Melee;

            Item.useTime = 25;
            Item.useAnimation = 25;

            Item.rare = ItemRarityID.Red;
            Item.value = Item.sellPrice(0, 3, 75, 0);

            Item.shoot = ModContent.ProjectileType<AegisSwing>();
            Item.shootSpeed = 0;

            //Item configs
            Item.width = 64;
            Item.height = 64;

            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = false;

            Item.useTurn = false;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, player.Center.X, player.Center.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
            return false;
        }

    }

    internal class AegisSwing : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.damage = 30;

            Projectile.tileCollide = false;
            Projectile.width = 192;
            Projectile.height = 192;
            Projectile.penetrate = -1;

            Projectile.DamageType = DamageClass.Melee;
            Projectile.friendly = true;

            Projectile.alpha = 255;
        }

        public override void AI()
        {
            FadeInAndOut();

            Projectile.localAI[0] += 1;

            Player player = Main.LocalPlayer;


            float x = 0;
            if(player.direction == 1)
            {
                x = (player.Center.X - 240) + (MathF.Cos((Projectile.localAI[0] - 20) * 0.1f) * 200);

                if (Main.rand.NextBool(2))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        int d = Dust.NewDust(Position: new Vector2((player.Center.X - 200) + (MathF.Cos((Projectile.localAI[0] - 20) * 0.1f) * 200), (player.Center.Y - 50) + (MathF.Sin((Projectile.localAI[0] - 20) * 0.157f) * 125)), Projectile.width, Projectile.height, DustID.Enchanted_Gold, Projectile.velocity.X, Projectile.velocity.Y, default);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].noLight = false;
                    }
                }
            }
            else
            {
                x = (player.Center.X - 140) + (MathF.Cos(Projectile.localAI[0] * 0.1f) * 200);

                if (Main.rand.NextBool(2))
                {
                    for (int i = 0; i < 10; i++)
                    {
                        int d = Dust.NewDust(Position: new Vector2((player.Center.X - 120) + (MathF.Cos(Projectile.localAI[0] * 0.1f) * 200), (player.Center.Y - 50) + (MathF.Sin((Projectile.localAI[0] - 20) * 0.157f) * 125)), Projectile.width, Projectile.height, DustID.Enchanted_Gold, Projectile.velocity.X, Projectile.velocity.Y, default);
                        Main.dust[d].noGravity = true;
                        Main.dust[d].noLight = false;
                    }
                }
            }
            float y = (player.Center.Y - 100) + (MathF.Sin((Projectile.localAI[0] - 20) * 0.157f) * 125);
            Projectile.position = new Vector2(x, y);

            Projectile.rotation = player.itemRotation;

            Projectile.spriteDirection = player.direction;

            if (Projectile.localAI[0] >= 20)
            {
                Projectile.Kill();
            }
        }

        private void FadeInAndOut()
        {
            // If last less than 50 ticks — fade in, than more — fade out
            if (Projectile.ai[0] <= 10f)
            {
                // Fade in
                Projectile.alpha -= 50;
                // Cap alpha before timer reaches 50 ticks
                if (Projectile.alpha < 150)
                    Projectile.alpha = 150;
                return;
            }
            else if (Projectile.ai[0] >= 15)
            {
                // Fade out
                Projectile.alpha += 55;
                // Cap alpha to the maximum 255(complete transparent)
                if (Projectile.alpha > 255)
                    Projectile.alpha = 255;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
            particleOrchestraSettings.PositionInWorld = target.Center;
            ParticleOrchestraSettings settings = particleOrchestraSettings;

            ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.RainbowRodHit, settings);
        }

    }
}
