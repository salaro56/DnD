using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using DnD.Rarities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.DataStructures;
using Terraria.GameContent.Drawing;

namespace DnD.Items.Spells.Wizard_Spells.Lvl8
{
    internal class SwordSwarm : ModItem
    {
        int minLevel = 8;
        int spellLevel;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sword Dance");
            Tooltip.SetDefault("[c/FF0000:Level 8:]" +
                "Call upon the might of the Empress");
        }

        public override void SetDefaults()
        {
            //item stats
            Item.maxStack = 1;
            Item.useTime = 4;
            Item.useAnimation = 12;
            Item.shootSpeed = 5f;
            Item.damage = 1;
            Item.ArmorPenetration += 999;
            Item.knockBack = 6;
            Item.value = Item.sellPrice(0, 42, 0, 0);
            Item.reuseDelay = Item.useAnimation + 6;

            //item configs
            Item.width = 32;
            Item.height = 32;
            Item.shoot = ModContent.ProjectileType<Swarm>();
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.rare = ModContent.RarityType<WizardRare>();
            Item.UseSound = SoundID.Item9;
            Item.autoReuse = true;

            spellLevel = minLevel;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            if (pc.wizardClass == false || pc.isRaging == true)
            {
                return false;
            }
            else return true;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();

            DnDItem sItem = ModContent.GetInstance<DnDItem>();

            damage += sItem.DamageValue(minRoll: 1, maxRoll: 13, diceRolled: 2 + spellLevel);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 2; i++)
            {
                Projectile.NewProjectile(source, player.Center.X + Main.rand.Next(-50, 50), player.Center.Y + Main.rand.Next(-50, 50), velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
            }
            return false;
        }

    }

    internal class Swarm : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.FairyQueenLance;

        public override void SetDefaults()
        {
            //proj stats
            Projectile.penetrate = 1;

            //proj configs
            Projectile.width = 24;
            Projectile.height = 120;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 4;
            Projectile.timeLeft = 180;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1;
            if (Projectile.localAI[0] <= 3)
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(0.5f, 0.5f);
                    Dust d = Dust.NewDustPerfect(new Vector2(Projectile.Center.X, Projectile.Center.Y), DustID.BlueCrystalShard, speed * 2, Scale: 2f);
                    d.velocity *= 0.5f;
                    d.noGravity = true;
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void Kill(int timeLeft)
        {
                ParticleOrchestraSettings particleOrchestraSettings = default(ParticleOrchestraSettings);
                particleOrchestraSettings.PositionInWorld = Projectile.Center;
                ParticleOrchestraSettings settings = particleOrchestraSettings;

                ParticleOrchestrator.RequestParticleSpawn(clientOnly: false, ParticleOrchestraType.RainbowRodHit, settings);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Projectile proj = Projectile;
            float num12 = Main.rand.NextFloat();
            float num13 = Utils.GetLerpValue(0f, 0.3f, num12, clamped: true) * Utils.GetLerpValue(1f, 0.5f, num12, clamped: true);
            Color fairyQueenWeaponsColor = proj.GetFairyQueenWeaponsColor(0.25f, 0f, (Main.rand.NextFloat() * 0.33f + Main.GlobalTimeWrappedHourly) % 1f);
            Vector2 vector = proj.Center - Main.screenPosition;
            Texture2D value = TextureAssets.Projectile[proj.type].Value;
            Vector2 origin = value.Frame().Size() / 2f;
            Color color2 = fairyQueenWeaponsColor;
            color2 *= num13 * 0.5f;
            Color value4 = Color.White * num13;
            value4.A /= 2;
            Color color3 = value4 * 0.5f;

            float num = MathHelper.Lerp(0.7f, 1f, Utils.GetLerpValue(0f, 5f, proj.ai[0], clamped: true));
            float opacity = proj.Opacity;
            if (opacity > 0f)
            {
                float lerpValue = Utils.GetLerpValue(0f, 1f, proj.velocity.Length(), clamped: true);
                for (float num2 = 0f; num2 < 1f; num2 += 1f / 6f)
                {
                    Vector2 value2 = proj.rotation.ToRotationVector2() * -120f * num2 * lerpValue;
                    Main.spriteBatch.Draw(value, vector + value2, null, color2, proj.rotation, origin, num, SpriteEffects.None, 0f);
                }
                for (float num3 = 0f; num3 < 1f; num3 += 0.25f)
                {
                    Vector2 value3 = (num3 * ((float)Math.PI * 2f) + proj.rotation).ToRotationVector2() * 4f * num;
                    Main.spriteBatch.Draw(value, vector + value3, null, color3, proj.rotation, origin, num, SpriteEffects.None, 0f);
                }
            }
            return false;
        }
    }
}
