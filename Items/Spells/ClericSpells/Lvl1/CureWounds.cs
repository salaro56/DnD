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
using Terraria.GameContent.Creative;

namespace DnD.Items.Spells.ClericSpells.Lvl1
{
    internal class CureWounds : ModItem
    {
        public int spellLevel = 1;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(value: "[c/FF0000:Level 1:]" +
                "\n A creature you touch regains a number of hit points equal to 1d8 + your Proficiency bonus" +
                "\nWhen you cast this spell at higher levels, the healing increases by 1d8 per spell slot above 1st" +
                "\nRight clicking while holding changes spell level");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item4;
            Item.rare = ModContent.RarityType<Rarities.ClericRare>();
            Item.value = 0;
            Item.mana = 2;
            Item.noUseGraphic = true;
            Item.damage = 1;
            Item.noMelee = true;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex(x => x.Name == "Damage");
            if (index == -1)
                return;
            tooltips[index].Text = "1d8 + proficiency bonus per spell level";
        }

        public override void HoldItem(Player player)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            if (pc.clericClass == true)
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(5f, 5f);
                    Dust d = Dust.NewDustPerfect(new Vector2(player.Center.X + speed.X * 12, player.Center.Y + speed.Y * 12), DustID.GreenTorch, Vector2.Zero, Scale: 2f);
                    d.velocity *= 0.5f;
                    d.noGravity = true;
                    d.color = Color.Green;
                    d.noLight = true;
                    d.alpha = 200;
                }
            }
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            Vector2 position1 = player.Center;
            Vector2 position2 = Main.MouseWorld;
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();

            if (Vector2.Distance(position1, position2) > 70)
            {
                return false;
            }
            else
            {
                if (pc.clericClass == false || pc.isRaging == true)
                {
                    return false;
                }
                else
                {

                    if (player.altFunctionUse == 2)
                    {
                        if (spellLevel < pc.SpellSlot())
                        {
                            Item.shoot = ProjectileID.None;
                            Item.UseSound = SoundID.Item4;
                            Item.mana = 0;
                            spellLevel++;
                            Main.NewText("Current spell level is " + spellLevel, 152, 104, 126);
                            Item.useAnimation = 5;
                            Item.useTime = 5;
                        }
                        else
                        {
                            Item.shoot = ProjectileID.None;
                            Item.UseSound = SoundID.Item4;
                            Item.mana = 0;
                            spellLevel = 1;
                            Main.NewText("Current spell level is " + spellLevel, 152, 104, 126);
                            Item.useAnimation = 5;
                            Item.useTime = 5;
                        }

                    }
                    else
                    {

                        if (spellLevel == 2)
                        {
                            Item.shoot = ModContent.ProjectileType<HealWounds>();
                            Item.mana = 3 + pc.ProfBonus();
                            Item.UseSound = SoundID.Item4;
                            Item.knockBack = 10;
                            Item.useAnimation = 20;
                            Item.useTime = 20;
                        }
                        else if (spellLevel == 3)
                        {
                            Item.shoot = ModContent.ProjectileType<HealWounds>();
                            Item.mana = 5 + pc.ProfBonus();
                            Item.UseSound = SoundID.Item4;
                            Item.knockBack = 15;
                            Item.useAnimation = 20;
                            Item.useTime = 20;
                        }
                        else if (spellLevel == 4)
                        {
                            Item.shoot = ModContent.ProjectileType<HealWounds>();
                            Item.mana = 6 + pc.ProfBonus();
                            Item.UseSound = SoundID.Item4;
                            Item.knockBack = 20;
                            Item.useAnimation = 20;
                            Item.useTime = 20;
                        }
                        else if (spellLevel == 5)
                        {
                            Item.shoot = ModContent.ProjectileType<HealWounds>();
                            Item.mana = 7 + pc.ProfBonus();
                            Item.UseSound = SoundID.Item4;
                            Item.knockBack = 25;
                            Item.useAnimation = 20;
                            Item.useTime = 20;
                        }
                        else if (spellLevel == 6)
                        {
                            Item.shoot = ModContent.ProjectileType<HealWounds>();
                            Item.mana = 9 + pc.ProfBonus();
                            Item.UseSound = SoundID.Item4;
                            Item.knockBack = 30;
                            Item.useAnimation = 20;
                            Item.useTime = 20;
                        }
                        else if (spellLevel == 7)
                        {
                            Item.shoot = ModContent.ProjectileType<HealWounds>();
                            Item.mana = 10 + pc.ProfBonus();
                            Item.UseSound = SoundID.Item4;
                            Item.knockBack = 35;
                            Item.useAnimation = 20;
                            Item.useTime = 20;
                        }
                        else if (spellLevel == 8)
                        {
                            Item.shoot = ModContent.ProjectileType<HealWounds>();
                            Item.mana = 11 + pc.ProfBonus();
                            Item.UseSound = SoundID.Item4;
                            Item.knockBack = 40;
                            Item.useAnimation = 20;
                            Item.useTime = 20;
                        }
                        else if (spellLevel == 9)
                        {
                            Item.shoot = ModContent.ProjectileType<HealWounds>();
                            Item.mana = 13 + pc.ProfBonus();
                            Item.UseSound = SoundID.Item4;
                            Item.knockBack = 45;
                            Item.useAnimation = 20;
                            Item.useTime = 20;
                        }
                        else
                        {
                            Item.shoot = ModContent.ProjectileType<HealWounds>();
                            Item.mana = 2 + pc.ProfBonus();
                            Item.UseSound = SoundID.Item4;
                            Item.knockBack = 5;
                            Item.useAnimation = 20;
                            Item.useTime = 20;
                        }
                    }
                    return true;
                }
            }


        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            DnDPlayer pc = Main.LocalPlayer.GetModPlayer<DnDPlayer>();
            switch (spellLevel)
            {
                case 1:
                    for (int i = 0; i < spellLevel; i++)
                    {
                        damage += Main.rand.Next(1, 9) + pc.ProfBonus();
                    }
                    break;
                case 2:
                    for (int i = 0; i < spellLevel; i++)
                    {
                        damage += Main.rand.Next(1, 9) + pc.ProfBonus();
                    }
                    break;
                case 3:
                    for (int i = 0; i < spellLevel; i++)
                    {
                        damage += Main.rand.Next(1, 9) + pc.ProfBonus();
                    }
                    break;
                case 4:
                    for (int i = 0; i < spellLevel; i++)
                    {
                        damage += Main.rand.Next(1, 9) + pc.ProfBonus();
                    }
                    break;
                case 5:
                    for (int i = 0; i < spellLevel; i++)
                    {
                        damage += Main.rand.Next(1, 9) + pc.ProfBonus();
                    }
                    break;
                case 6:
                    for (int i = 0; i < spellLevel; i++)
                    {
                        damage += Main.rand.Next(1, 9) + pc.ProfBonus();
                    }
                    break;
                case 7:
                    for (int i = 0; i < spellLevel; i++)
                    {
                        damage += Main.rand.Next(1, 9) + pc.ProfBonus();
                    }
                    break;
                case 8:
                    for (int i = 0; i < spellLevel; i++)
                    {
                        damage += Main.rand.Next(1, 9) + pc.ProfBonus();
                    }
                    break;
                case 9:
                    for (int i = 0; i < spellLevel; i++)
                    {
                        damage += Main.rand.Next(1, 9) + pc.ProfBonus();
                    }
                    break;
                default:
                    for (int i = 0; i < spellLevel; i++)
                    {
                        damage += Main.rand.Next(1, 9) + pc.ProfBonus();
                    }
                    break;
            }
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
                .AddIngredient(ModContent.ItemType<ClassToken>())
                .AddCondition(NetworkText.FromLiteral("Must be of level 1 or higher"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().playerLevel >= 1)
                .AddCondition(NetworkText.FromLiteral("Must be of the Cleric Class"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().clericClass == true)
                .AddTile(ModContent.TileType<Furniture.PHBTile>())
                .Register();
        }
    }

    internal class HealWounds : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ShadowBeamFriendly;

        public override void SetDefaults()
        {
            //proj stats
            Projectile.penetrate = 1;

            //proj configs
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 15;
            Projectile.tileCollide = false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override void AI()
        {
            Player player = Main.LocalPlayer;
            int healAmount = Projectile.damage;

            if (Projectile.Hitbox.Intersects(player.Hitbox))
            {
                for (int i = 0; i < 20; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(3f, 1f);
                    Dust d = Dust.NewDustPerfect(new Vector2(player.Center.X, player.Center.Y + 35), DustID.Clentaminator_Green, speed * 3, Scale: 2f);
                    d.velocity *= 0.5f;
                    d.noGravity = true;
                    d.color = Color.Green;
                }
                player.statLife += healAmount;
                CombatText.NewText(player.getRect(), new Color(0, 255, 0), healAmount);
                NetMessage.SendData(MessageID.SpiritHeal, -1, -1, null, player.whoAmI, healAmount);
                Projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Vector2 position = Main.MouseWorld;
                for (int i = 0; i < 20; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(0.5f, 0.5f);
                    Dust d = Dust.NewDustPerfect(new Vector2(position.X, position.Y), DustID.Clentaminator_Green, speed * 2, Scale: 2f);
                    d.velocity *= 0.5f;
                    d.noGravity = true;
                }
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
