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
            Projectile.NewProjectile(source, player.Center.X, player.Center.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
            Projectile.NewProjectile(source, player.Center.X, player.Center.Y, velocity.X, velocity.Y, ModContent.ProjectileType<BreakerSwing>(), damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ClassToken>())
                .AddIngredient(ItemID.Feather, 5)
                .AddIngredient(ItemID.SilverBar, 12)
                .AddTile(ModContent.TileType<Furniture.PHBTile>())
                .Register();
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

        private void UpdateAim(Vector2 source, float speed)
        {
            // Get the player's current aiming direction as a normalized vector.
            Vector2 aim = Vector2.Normalize(Main.MouseWorld - source);
            if (aim.HasNaNs())
            {
                aim = -Vector2.UnitY;
            }

        }
        private void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
        {
            // Place the Prism directly into the player's hand at all times.
            Projectile.Center = playerHandPos;
            // The beams emit from the tip of the Prism, not the side. As such, rotate the sprite by pi/2 (90 degrees).
            Projectile.spriteDirection = Projectile.direction;

            // The Prism is a holdout Projectile, so change the player's variables to reflect that.
            // Constantly resetting player.itemTime and player.itemAnimation prevents the player from switching items or doing anything else.
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            // If you do not multiply by Projectile.direction, the player's hand will point the wrong direction while facing left.
            player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, false);
            double deg = (double)Projectile.ai[1] * 5;
            double rad = deg * (Math.PI / -90);
            double dist = 100f;
            float Rot = (player.Center - Projectile.Center).ToRotation() - 15.0625f;
            Projectile.rotation = Rot;
            if (player.direction == -1)
            {
                rad = deg * (Math.PI / -90);
            }
            Projectile.position.X = player.Center.X + (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
            Projectile.position.Y = player.Center.Y + (int)(Math.Sin(rad) * dist) - Projectile.height / 2;
            Projectile.ai[1] += 1f;
            if (Projectile.owner == Main.myPlayer)
            {
                bool stillInUse = player.channel && !player.noItems && !player.CCed;
                if (stillInUse)
                {
                    Player.CompositeArmStretchAmount stretch = default;
                    player.GetFrontHandPosition(stretch, 10f);
                    UpdateAim(rrp, player.HeldItem.shootSpeed);
                    Projectile.rotation = Rot;
                }
                else if (!stillInUse)
                {
                    ;
                    SoundEngine.PlaySound(SoundID.NPCHit2);
                    Projectile.Kill();
                }
            }
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

        private void UpdateAim(Vector2 source, float speed)
        {
            // Get the player's current aiming direction as a normalized vector.
            Vector2 aim = Vector2.Normalize(Main.MouseWorld - source);
            if (aim.HasNaNs())
            {
                aim = -Vector2.UnitY;
            }

        }
        private void UpdatePlayerVisuals(Player player, Vector2 playerHandPos)
        {
            // Place the Prism directly into the player's hand at all times.
            Projectile.Center = playerHandPos;
            // The beams emit from the tip of the Prism, not the side. As such, rotate the sprite by pi/2 (90 degrees).
            Projectile.spriteDirection = Projectile.direction;

            // The Prism is a holdout Projectile, so change the player's variables to reflect that.
            // Constantly resetting player.itemTime and player.itemAnimation prevents the player from switching items or doing anything else.
            player.ChangeDir(Projectile.direction);
            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            // If you do not multiply by Projectile.direction, the player's hand will point the wrong direction while facing left.
            player.itemRotation = (Projectile.velocity * Projectile.direction).ToRotation();
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Vector2 rrp = player.RotatedRelativePoint(player.MountedCenter, false);
            double deg = (double)Projectile.ai[1] * 5;
            double rad = deg * (Math.PI / -90);
            double dist = 300f;
            float Rot = (player.Center - Projectile.Center).ToRotation() - 15.0625f;
            Projectile.rotation = Rot;
            if (player.direction == -1)
            {
                rad = deg * (Math.PI / -90);
            }
            Projectile.position.X = player.Center.X + (int)(Math.Cos(rad) * dist) - Projectile.width / 2;
            Projectile.position.Y = player.Center.Y + (int)(Math.Sin(rad) * dist) - Projectile.height / 2;
            Projectile.ai[1] += 1f;

            if (Projectile.owner == Main.myPlayer)
            {
                bool stillInUse = player.channel && !player.noItems && !player.CCed;
                if (stillInUse)
                {
                    Player.CompositeArmStretchAmount stretch = default;
                    player.GetFrontHandPosition(stretch, 10f);
                    UpdateAim(rrp, player.HeldItem.shootSpeed);
                    Projectile.rotation = Rot;
                }
                else if (!stillInUse)
                {
                    ;
                    SoundEngine.PlaySound(SoundID.NPCHit2);
                    Projectile.Kill();
                }
            }
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
