using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DnD.Furniture;
using Terraria.Localization;
using Terraria.GameContent.Creative;
using DnD.Rarities;
using DnD.Common;

namespace DnD.Items.Spells.Wizard_Spells.Cantrips
{
    internal class RayOfFrost : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ray of Frost");
            /* Tooltip.SetDefault(value: "[c/FF0000:Cantrip:]" +
                "\nDoes 1d8 x proficiency bonus cold damage" +
                "\nA frigid beam of blue-white light streaks toward a creature within range." +
                "\nDoes an additional d8 at level 5, 11, and 17"); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }
        public override void SetDefaults()
        {
            //item stats
            Item.maxStack = 1;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.reuseDelay = 10;
            Item.shootSpeed = 5f;
            Item.damage = 1;
            Item.value = Item.sellPrice(0, 0, 3, 50);
            Item.ArmorPenetration += 999;

            //item configs
            Item.width = 32;
            Item.height = 32;
            Item.shoot = ModContent.ProjectileType<FrostRay>();
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.rare = ModContent.RarityType<WizardRare>();
            Item.UseSound = SoundID.Item27;
        }

        public override bool CanUseItem(Player player)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            if (pc.wizardClass == false || pc.isRaging)
            {
                return false;
            }
            else return true;
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            if (pc.playerLevel < 5)
            {
                int dice = 1;
                for (int i = 0; i < dice; i++)
                {
                    damage += Main.rand.Next(1, 8) * pc.ProfBonus();
                }
            }
            else if (pc.playerLevel >= 5)
            {
                int dice = 2;
                for (int i = 0; i < dice; i++)
                {
                    damage += Main.rand.Next(1, 8) * pc.ProfBonus();
                }
            }
            else if (pc.playerLevel >= 11)
            {
                int dice = 3;
                for (int i = 0; i < dice; i++)
                {
                    damage += Main.rand.Next(1, 8) * pc.ProfBonus();
                }
            }
            else if (pc.playerLevel >= 17)
            {
                int dice = 4;
                for (int i = 0; i < dice; i++)
                {
                    damage += Main.rand.Next(1, 8) * pc.ProfBonus();
                }
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.ClassToken>())
                .AddTile(ModContent.TileType<PHBTile>())
                .AddCondition(Conditions.IsWizard)
                .AddCondition(Conditions.IsRightLevel(1))
                .Register();
        }
    }

    internal class FrostRay : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ShadowBeamFriendly;
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            //proj stats
            Projectile.penetrate = 1;
            Projectile.coldDamage = true;

            //proj configs
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.extraUpdates = 20;
            Projectile.timeLeft = 100;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 9f)
            {
                for (int i = 0; i < 1; i++)
                {
                    Vector2 projectilePosition = Projectile.position;
                    projectilePosition -= Projectile.velocity * ((float)i * 0.25f);
                    Projectile.alpha = 255;
                    // Important, changed 173 to 178!
                    int dust = Dust.NewDust(projectilePosition, 1, 1, DustID.BlueCrystalShard, 0f, 0f, 0, default(Color), 1f);
                    int dust2 = Dust.NewDust(projectilePosition, 1, 1, DustID.Snow, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].position = projectilePosition;
                    Main.dust[dust].scale = (float)Main.rand.Next(70, 110) * 0.013f;
                    Main.dust[dust].velocity *= 0.2f;
                    Main.dust[dust].noLight = false;
                    Main.dust[dust2].noGravity = true;
                    Main.dust[dust2].position = projectilePosition;
                    Main.dust[dust2].scale = (float)Main.rand.Next(70, 110) * 0.013f;
                    Main.dust[dust2].velocity *= 0.2f;
                    Main.dust[dust2].noLight = false;
                }
            }
            Projectile.netUpdate = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Wet, 120);
            target.AddBuff(BuffID.Slow, 180);
        }
    }
}
