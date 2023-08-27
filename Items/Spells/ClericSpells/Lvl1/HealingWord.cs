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
using Terraria.GameContent.Creative;
using DnD.Common;

namespace DnD.Items.Spells.ClericSpells.Lvl1
{
    internal class HealingWord : ModItem
    {
        public int spellLevel = 1;
        public override void SetStaticDefaults()
        {
            /* Tooltip.SetDefault(value: "[c/FF0000:Level 1:]" +
                "\n A creature of your choice that you can see regains hit points equal to 1d4 + proficiency bonus" +
                "\nWhen you cast this spell at higher levels, the healing increases by 1d4 per spell slot above 1st" +
                "\nRight clicking while holding changes spell level"); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Thrust;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item4;
            Item.rare = ModContent.RarityType<Rarities.ClericRare>();
            Item.value = 0;
            Item.mana = 2;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.damage = 1;
            Item.shootSpeed = 15f;
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
                            Item.shoot = ModContent.ProjectileType<HealWord>();
                            Item.mana = 3 + pc.ProfBonus();
                            Item.UseSound = SoundID.Item4;
                            Item.knockBack = 10;
                            Item.useAnimation = 20;
                            Item.useTime = 20;
                        }
                        else if (spellLevel == 3)
                        {
                            Item.shoot = ModContent.ProjectileType<HealWord>();
                            Item.mana = 5 + pc.ProfBonus();
                            Item.UseSound = SoundID.Item4;
                            Item.knockBack = 15;
                            Item.useAnimation = 20;
                            Item.useTime = 20;
                        }
                        else if (spellLevel == 4)
                        {
                            Item.shoot = ModContent.ProjectileType<HealWord>();
                            Item.mana = 6 + pc.ProfBonus();
                            Item.UseSound = SoundID.Item4;
                            Item.knockBack = 20;
                            Item.useAnimation = 20;
                            Item.useTime = 20;
                        }
                        else if (spellLevel == 5)
                        {
                            Item.shoot = ModContent.ProjectileType<HealWord>();
                            Item.mana = 7 + pc.ProfBonus();
                            Item.UseSound = SoundID.Item4;
                            Item.knockBack = 25;
                            Item.useAnimation = 20;
                            Item.useTime = 20;
                        }
                        else if (spellLevel == 6)
                        {
                            Item.shoot = ModContent.ProjectileType<HealWord>();
                            Item.mana = 9 + pc.ProfBonus();
                            Item.UseSound = SoundID.Item4;
                            Item.knockBack = 30;
                            Item.useAnimation = 20;
                            Item.useTime = 20;
                        }
                        else if (spellLevel == 7)
                        {
                            Item.shoot = ModContent.ProjectileType<HealWord>();
                            Item.mana = 10 + pc.ProfBonus();
                            Item.UseSound = SoundID.Item4;
                            Item.knockBack = 35;
                            Item.useAnimation = 20;
                            Item.useTime = 20;
                        }
                        else if (spellLevel == 8)
                        {
                            Item.shoot = ModContent.ProjectileType<HealWord>();
                            Item.mana = 11 + pc.ProfBonus();
                            Item.UseSound = SoundID.Item4;
                            Item.knockBack = 40;
                            Item.useAnimation = 20;
                            Item.useTime = 20;
                        }
                        else if (spellLevel == 9)
                        {
                            Item.shoot = ModContent.ProjectileType<HealWord>();
                            Item.mana = 13 + pc.ProfBonus();
                            Item.UseSound = SoundID.Item4;
                            Item.knockBack = 45;
                            Item.useAnimation = 20;
                            Item.useTime = 20;
                        }
                        else
                        {
                            Item.shoot = ModContent.ProjectileType<HealWord>();
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

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            DnDPlayer pc = Main.LocalPlayer.GetModPlayer<DnDPlayer>();
            switch (spellLevel)
            {
                case 1:
                    for (int i = 0; i < spellLevel; i++)
                    {
                        damage += Main.rand.Next(1, 4) + pc.ProfBonus();
                    }
                    break;
                case 2:
                    for (int i = 0; i < spellLevel; i++)
                    {
                        damage += Main.rand.Next(1, 4) + pc.ProfBonus();
                    }
                    break;
                case 3:
                    for (int i = 0; i < spellLevel; i++)
                    {
                        damage += Main.rand.Next(1, 4) + pc.ProfBonus();
                    }
                    break;
                case 4:
                    for (int i = 0; i < spellLevel; i++)
                    {
                        damage += Main.rand.Next(1, 4) + pc.ProfBonus();
                    }
                    break;
                case 5:
                    for (int i = 0; i < spellLevel; i++)
                    {
                        damage += Main.rand.Next(1, 4) + pc.ProfBonus();
                    }
                    break;
                case 6:
                    for (int i = 0; i < spellLevel; i++)
                    {
                        damage += Main.rand.Next(1, 4) + pc.ProfBonus();
                    }
                    break;
                case 7:
                    for (int i = 0; i < spellLevel; i++)
                    {
                        damage += Main.rand.Next(1, 4) + pc.ProfBonus();
                    }
                    break;
                case 8:
                    for (int i = 0; i < spellLevel; i++)
                    {
                        damage += Main.rand.Next(1, 4) + pc.ProfBonus();
                    }
                    break;
                case 9:
                    for (int i = 0; i < spellLevel; i++)
                    {
                        damage += Main.rand.Next(1, 4) + pc.ProfBonus();
                    }
                    break;
                default:
                    for (int i = 0; i < spellLevel; i++)
                    {
                        damage += Main.rand.Next(1, 4) + pc.ProfBonus();
                    }
                    break;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ClassToken>())
                .AddCondition(Conditions.IsRightLevel(1))
                .AddCondition(Conditions.IsCleric)
                .AddTile(ModContent.TileType<Furniture.PHBTile>())
                .Register();
        }
    }

    internal class HealWord : ModProjectile
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
            Projectile.timeLeft = 45;
            Projectile.tileCollide = false;
            Projectile.netImportant = true;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override void AI()
        {
            int healAmount = Projectile.damage;

            for (int i = 0; i < 255; i++)
            {
                if (i != Main.myPlayer && Main.netMode == NetmodeID.MultiplayerClient)
                {
                    if (Main.player[i].Distance(Main.LocalPlayer.Center) < 500)
                    {
                        Vector2 target = Main.player[i].Center;
                        Vector2 dir = target - Projectile.Center;
                        Vector2 vel = Vector2.Normalize(dir) * 10;

                        Projectile.velocity = vel;
                        Projectile.netUpdate = true;
                    }

                    if (Projectile.Hitbox.Distance(Main.player[i].Center) < 50 && Main.player[i] != Main.player[Projectile.owner])
                    {
                        Main.player[i].statLife += healAmount;
                        CombatText.NewText(Main.player[i].getRect(), new Color(0, 255, 0), healAmount);
                        NetMessage.SendData(MessageID.SpiritHeal, -1, -1, null, Main.player[i].whoAmI, healAmount);
                        Projectile.Kill();
                    }
                }
            }

            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] > 9f)
            {
                for (int i = 0; i < 6; i++)
                {
                    Vector2 ProjectilePosition = Projectile.Center;
                    ProjectilePosition -= Projectile.velocity * ((float)i * 0.25f);
                    Projectile.alpha = 255;
                    // Important, changed 173 to 178!
                    int dust = Dust.NewDust(ProjectilePosition, 1, 1, DustID.Clentaminator_Green, 0f, 0f, 0, default(Color), 1f);
                    Main.dust[dust].noGravity = true;
                    Main.dust[dust].position = ProjectilePosition;
                    Main.dust[dust].scale = (float)Main.rand.Next(70, 110) * 0.013f;
                    Main.dust[dust].velocity *= 0.2f;
                }
            }
            Projectile.rotation = Projectile.velocity.ToRotation();
        }

        public override void Kill(int timeLeft)
        {
            Vector2 position = Projectile.Center;
            for (int i = 0; i < 20; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(0.5f, 0.5f);
                Dust d = Dust.NewDustPerfect(new Vector2(position.X, position.Y), DustID.Clentaminator_Green, speed * 2, Scale: 2f);
                d.velocity *= 0.5f;
                d.noGravity = true;
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
