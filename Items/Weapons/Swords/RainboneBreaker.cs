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
using Terraria.GameContent.Drawing;
using DnD.Rarities;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics;
using Terraria.Audio;

namespace DnD.Items.Weapons.Swords
{
    internal class RainboneBreaker : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rainbow Breaker");
            Tooltip.SetDefault("Crush light itself");
        }

        public override void SetDefaults()
        {
            //Item stats
            Item.damage = 100;
            Item.DamageType = DamageClass.Melee;
            Item.knockBack = 7;

            Item.useTime = 35;
            Item.useAnimation = 35;

            Item.rare = ModContent.RarityType<BarbRare>();
            Item.value = Item.sellPrice(0, 42, 0, 0);

            Item.shoot = ModContent.ProjectileType<BreakerStreak>();
            Item.shootSpeed = 0;

            //Item configs
            Item.width = 106;
            Item.height = 106;

            Item.channel = true;

            Item.UseSound = SoundID.Item1;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.autoReuse = false;

            Item.useTurn = false;

            Item.noUseGraphic = true;
            Item.noMelee = true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Item.shoot || Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == ModContent.ProjectileType<BreakerSwing>())
                {
                    Main.projectile[i].Kill();
                }
            }

            Projectile.NewProjectile(source, player.Center.X, player.Center.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, player.Center.X, player.Center.Y, velocity.X, velocity.Y, ModContent.ProjectileType<BreakerSwing>(), damage, knockback, player.whoAmI);
            return false;
        }
    }

    internal class BreakerStreak : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rainbow Breaker");
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.height = 106;
            Projectile.width = 106;
            Projectile.timeLeft = int.MaxValue;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.manualDirectionChange = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.penetrate = -1;
            Projectile.light = .25f;
            Projectile.owner = Main.myPlayer;
        }
        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public override bool ShouldUpdatePosition()
        {
            // Update Projectile.Center manually
            return false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, false);
            double deg = (double)Projectile.ai[1] * 5;
            double rad = deg * (Math.PI / (-90 * player.direction));
            double dist = 100f;
            float Rot = (player.Center - Projectile.Center).ToRotation() - MathHelper.ToRadians(125);
            Projectile.rotation = Rot;

            rad = deg * (Math.PI / (90 * player.direction));

            Projectile.position.X = (player.Center.X - ((int)(Math.Cos(rad) * dist) * player.direction) - Projectile.width / 2);
            Projectile.position.Y = (player.Center.Y - ((int)(Math.Sin(rad) * dist) * player.direction) - Projectile.height / 2);
            Projectile.ai[1] += 1f;


            if (Projectile.owner == Main.myPlayer)
            {
                bool stillInUse = player.channel && !player.noItems && !player.CCed;
                if (stillInUse)
                {
                    Player.CompositeArmStretchAmount stretch = default;
                    player.GetFrontHandPosition(stretch, 10f);
                }
                else if (!stillInUse)
                {
                    ;
                    SoundEngine.PlaySound(SoundID.Item1);
                    Projectile.Kill();
                }
            }
            Projectile.spriteDirection = Projectile.direction;
            Projectile.netUpdate = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            if (Projectile.direction == -1)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            }
            Color global = Color.DeepPink;
            Vector2 vector46 = Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
            Texture2D value111 = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Color color71 = global;
            Vector2 origin19 = new Vector2(value111.Width, value111.Height) / 2f;
            float num299 = Projectile.rotation;
            Vector2 vector47 = Vector2.One * Projectile.scale;
            Rectangle? sourceRectangle2 = null;
            Vector2 scale17 = vector47;
            Vector2 spinningpoint8 = new Vector2(2f * scale17.X + (float)Math.Cos(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 4f)) * 0.4f, 0f).RotatedBy(num299 + Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f));
            for (float num307 = 0f; num307 < 1f; num307 += 1f / 6f)
            {
                Color color78 = global;
                color78.A = 0;
                Main.EntitySpriteDraw(value111, vector46 + spinningpoint8.RotatedBy(num307 * ((float)Math.PI * 2f)), null, color78, num299, origin19, scale17 * 1.125f, SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(value111, vector46, null, color71, num299, origin19, vector47, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            default(Graphics.RainbowRodDrawer).Draw(Projectile);
            return true;
        }
    }

    internal class BreakerSwing : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rainbow Breaker");
        }
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.height = 200;
            Projectile.width = 200;
            Projectile.timeLeft = int.MaxValue;
            Projectile.hostile = false;
            Projectile.friendly = true;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.manualDirectionChange = true;
            Projectile.localNPCHitCooldown = 15;
            Projectile.penetrate = -1;
            Projectile.light = .25f;
            Projectile.alpha = 100;
        }
        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public override bool ShouldUpdatePosition()
        {
            // Update Projectile.Center manually
            return false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, false);
            double deg = (double)Projectile.ai[1] * 5;
            double rad = deg * (Math.PI / (-90 * player.direction));
            double dist = 300f;
            float Rot = (player.Center - Projectile.Center).ToRotation() - MathHelper.ToRadians(75);
            Projectile.rotation = Rot;
            Projectile.spriteDirection = player.direction;

            rad = deg * (Math.PI / (90 * player.direction));

            Projectile.position.X = (player.Center.X - ((int)(Math.Cos(rad) * dist) * player.direction) - Projectile.width / 2);
            Projectile.position.Y = (player.Center.Y - ((int)(Math.Sin(rad) * dist) * player.direction) - Projectile.height / 2);
            Projectile.ai[1] += 1f;



            if (Projectile.owner == Main.myPlayer)
            {
                bool stillInUse = player.channel && !player.noItems && !player.CCed;
                if (stillInUse)
                {
                    Player.CompositeArmStretchAmount stretch = default;
                    player.GetFrontHandPosition(stretch, 10f);
                }
                else if (!stillInUse)
                {
                    ;
                    Projectile.Kill();
                }
            }
            Projectile.spriteDirection = Projectile.direction;
            Projectile.netUpdate = true;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            if (Projectile.direction == -1)
            {
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver4;

            }
            Color global = Color.LightBlue;
            Vector2 vector46 = Projectile.position + new Vector2(Projectile.width, Projectile.height) / 2f + Vector2.UnitY * Projectile.gfxOffY - Main.screenPosition;
            Texture2D value111 = (Texture2D)TextureAssets.Projectile[Projectile.type];
            Color color71 = global;
            Vector2 origin19 = new Vector2(value111.Width, value111.Height) / 2f;
            float num299 = Projectile.rotation;
            Vector2 vector47 = Vector2.One * Projectile.scale;
            Rectangle? sourceRectangle2 = null;
            Vector2 scale17 = vector47;
            Vector2 spinningpoint8 = new Vector2(2f * scale17.X + (float)Math.Cos(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 4f)) * 0.4f, 0f).RotatedBy(num299 + Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f));
            for (float num307 = 0f; num307 < 1f; num307 += 1f / 6f)
            {
                Color color78 = global;
                color78.A = 0;
                Main.EntitySpriteDraw(value111, vector46 + spinningpoint8.RotatedBy(num307 * ((float)Math.PI * 2f)), null, color78, num299, origin19, scale17 * 1.125f, SpriteEffects.None, 0);
            }
            Main.EntitySpriteDraw(value111, vector46, null, color71, num299, origin19, vector47, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
            default(RainbowRodDrawer).Draw(Projectile);
            return false;
        }
    }
}
