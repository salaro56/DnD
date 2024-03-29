﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using DnD.Rarities;
using DnD.Furniture;
using Terraria.Localization;
using DnD.Common.Structs;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using DnD.Common;

namespace DnD.Items.Spells.ClericSpells.Cantrips
{
    internal class SacredFlame : ModItem
    {
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault(value: "[c/FF0000:Cantrip:]" +
                "\nDoes 1d8 x proficiency bonus radiant damage" +
                "\nFlame like radiance descends on a creature that you can see"); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            //item stats
            Item.maxStack = 1;
            Item.useTime = 30;
            Item.useAnimation = 30;
            Item.reuseDelay = 10;
            Item.shootSpeed = 20f;
            Item.damage = 1;
            Item.ArmorPenetration += 999;
            Item.value = Item.sellPrice(0, 0, 3, 50);

            //item configs
            Item.width = 32;
            Item.height = 32;
            Item.shoot = ModContent.ProjectileType<HolyFlame>();
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.rare = ModContent.RarityType<ClericRare>();
            Item.UseSound = SoundID.Item27;
        }

        int level = 0;
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            DnDPlayer pc = Main.LocalPlayer.GetModPlayer<DnDPlayer>();
            string[] damageVal = { "1d8", "2d8", "3d8", "4d8" };
            switch (pc.playerLevel)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    level = 0;
                    break;
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                    level = 1;
                    break;
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                    level = 2;
                        break;
                case 17:
                case 18:
                case 19:
                case 20:
                    level = 3;
                    break;
            }


            int index = tooltips.FindIndex(x => x.Name == "Damage");
            if (index == -1)
                return;
            tooltips[index].Text = damageVal[level];
        }

        public override bool CanUseItem(Player player)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            if (pc.clericClass == false || pc.isRaging)
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

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 target = Main.MouseWorld;
            Vector2 pos = new Vector2(target.X, target.Y - 100);

            Vector2 dir = target - pos;
            dir.Normalize();
            dir *= Item.shootSpeed;

            Projectile.NewProjectile(source, pos, dir, type, damage, knockback, player.whoAmI);
            return false;
        }


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.ClassToken>())
                .AddTile(ModContent.TileType<PHBTile>())
                .AddCondition(Conditions.IsRightLevel(1))
                .AddCondition(Conditions.IsCleric)
                .Register();
        }
    }

    internal class HolyFlame : ModProjectile
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
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 120;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] <= 3)
            {
                for (int i = 0; i < 20; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(1f * Projectile.localAI[0], 0.5f);
                    Dust d = Dust.NewDustPerfect(new Vector2(Projectile.position.X, Projectile.position.Y), DustID.GoldFlame, speed * 2, Scale: 2f);
                    d.velocity *= 0.5f;
                    d.noGravity = true;
                }
            }
            if (Projectile.localAI[0] > 5f)
            {
                for (int i = 0; i < 3; i++)
                {
                    Vector2 projectilePosition = Projectile.position;
                    projectilePosition -= Projectile.velocity * ((float)i * 0.25f);
                    Projectile.alpha = 255;
                    // Important, changed 173 to 178!
                    int dust = Dust.NewDust(projectilePosition, 1, 1, DustID.GoldFlame, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].position = projectilePosition;
                    Main.dust[dust].scale = (float)Main.rand.Next(70, 110) * 0.02f;
                    Main.dust[dust].velocity *= 0.2f;
                    Main.dust[dust].noLight = false;
                }
                Projectile.tileCollide = true;
            }
            Projectile.netUpdate = true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(3f, 0.2f);
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.GoldFlame, speed * 4, Scale: 2f);
                d.velocity *= 0.5f;
                d.noGravity = true;
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            CreatureArrays ca = new();
            if (ca.undeadNames.Any(target.FullName.Contains))
            {
                modifiers.FinalDamage *= 2;
            }
        }
    }
}
