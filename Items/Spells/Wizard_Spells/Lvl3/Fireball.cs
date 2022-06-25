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
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using DnD.Rarities;
using Terraria.Graphics;

namespace DnD.Items.Spells.Wizard_Spells.Lvl3
{
    internal class Fireball : ModItem
    {
        public int minLevel = 3;
        public int spellLevel;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fireball");
            Tooltip.SetDefault(value: "[c/FF0000:Level 3:]" +
                "\nA bright streak flashes from your pointing finger to a point you choose within range and then blossoms with a low roar into an explosion of flame." +
                "\nA target takes 8d6 x proficiency bonus fire damage" +
                "\nWhen you cast this spell using a spell slot of 4th level or higher the damage increases by 1d6 for each level");
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }



        public override void SetDefaults()
        {
            //item stats
            Item.maxStack = 1;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.shootSpeed = 5f;
            Item.damage = 1;
            Item.ArmorPenetration += 999;

            //item configs
            Item.width = 32;
            Item.height = 32;
            Item.shoot = ModContent.ProjectileType<BallFire>();
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.rare = ModContent.RarityType<WizardRare>();
            Item.UseSound = SoundID.Item20;

            spellLevel = minLevel;
        }


        public override bool CanUseItem(Player player)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            if (pc.wizardClass == false || pc.isRaging == true)
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
                        Item.reuseDelay = 5;
                        Item.mana = 0;
                        spellLevel++;
                        Main.NewText("Current spell level is " + spellLevel, 152, 104, 126);
                    }
                    else
                    {
                        Item.shoot = ProjectileID.None;
                        Item.UseSound = SoundID.Item4;
                        Item.reuseDelay = 5;
                        Item.mana = 0;
                        spellLevel = minLevel;
                        Main.NewText("Current spell level is " + spellLevel, 152, 104, 126);
                    }
                }
                else
                {

                    if (spellLevel == 3)
                    {
                        Item.shoot = ModContent.ProjectileType<BallFire>();
                        Item.mana = 5 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item14;
                        Item.reuseDelay = 25;
                    }
                    else if (spellLevel == 4)
                    {
                        Item.shoot = ModContent.ProjectileType<BallFire>();
                        Item.mana = 6 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item14;
                        Item.reuseDelay = 25;
                    }
                    else if (spellLevel == 5)
                    {
                        Item.shoot = ModContent.ProjectileType<BallFire>();
                        Item.mana = 7 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item14;
                        Item.reuseDelay = 25;
                    }
                    else if (spellLevel == 6)
                    {
                        Item.shoot = ModContent.ProjectileType<BallFire>();
                        Item.mana = 9 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item14;
                        Item.reuseDelay = 25;
                    }
                    else if (spellLevel == 7)
                    {
                        Item.shoot = ModContent.ProjectileType<BallFire>();
                        Item.mana = 10 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item14;
                        Item.reuseDelay = 25;
                    }
                    else if (spellLevel == 8)
                    {
                        Item.shoot = ModContent.ProjectileType<BallFire>();
                        Item.mana = 11 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item14;
                        Item.reuseDelay = 25;
                    }
                    else if (spellLevel == 9)
                    {
                        Item.shoot = ModContent.ProjectileType<BallFire>();
                        Item.mana = 13 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item14;
                        Item.reuseDelay = 25;
                    }
                }
                return true;
            }
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();

            DnDItem sItem = ModContent.GetInstance<DnDItem>();

            damage += sItem.DamageValue(minRoll: 1, maxRoll: 6, diceRolled: spellLevel + 7);
        }
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            target.defense *= 0;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.ClassToken>())
                .AddIngredient(ModContent.ItemType<SpellComponents.Sulfur>())
                .AddTile(ModContent.TileType<PHBTile>())
                .AddCondition(NetworkText.FromLiteral("Must be of level 5 or higher"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().playerLevel >= 5)
                .AddCondition(NetworkText.FromLiteral("Must be the right class"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().wizardClass == true)
                .Register();
        }

        internal class BallFire : ModProjectile
        {
            public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ShadowBeamFriendly;
            public override void SetStaticDefaults()
            {
                ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
                ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            }

            public Vector2 target;

            public override void SetDefaults()
            {
                //proj stats
                Projectile.penetrate = 1;

                //proj configs
                Projectile.width = 4;
                Projectile.height = 4;
                Projectile.friendly = true;
                Projectile.DamageType = DamageClass.Magic;
                Projectile.extraUpdates = 4;
                Projectile.timeLeft = 180;
                Projectile.tileCollide = true;
                target = Main.MouseWorld;
            }

            public override bool? CanHitNPC(NPC target)
            {
                return false;
            }

            public override bool PreDraw(ref Color lightColor)
            {
                default(FlameLashDrawer).Draw(Projectile);
                return true;
            }

            public override void AI()
            {
                Projectile.localAI[0] += 1f;
                /*if (Projectile.localAI[0] > 3f)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector2 projectilePosition = Projectile.position;
                        projectilePosition -= Projectile.velocity * ((float)i * 0.25f);
                        Projectile.alpha = 255;
                        // Important, changed 173 to 178!
                        int dust = Dust.NewDust(projectilePosition, 1, 1, DustID.FlameBurst, 0f, 0f, 0, default(Color), 1f);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].position = projectilePosition;
                        Main.dust[dust].scale = (float)Main.rand.Next(70, 110) * 0.013f;
                        Main.dust[dust].velocity *= 0.2f;
                    }
                }*/

                Rectangle targetPosition = new Rectangle((int)target.X, (int)target.Y, 8, 8);
                if (Projectile.Hitbox.Intersects(targetPosition))
                {
                    Projectile.Kill();
                    return;
                }
                if (Projectile.owner == Main.myPlayer)
                {
                    Vector2 targetDirection = new Vector2(target.X, target.Y) - Projectile.Center;
                    Projectile.velocity = Vector2.Normalize(targetDirection) * 5f;
                }
                Projectile.netUpdate = true;
            }

            public override void Kill(int timeLeft)
            {
                for (int i = 0; i < 20; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.Smoke, speed * 4, Scale: 2f);
                    d.velocity *= 0.5f;
                    d.noGravity = false;
                    d.color = Color.White;
                }
                for (int i = 0; i < 20; i++)
                {
                    Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.Smoke, speed * 4, Scale: 2f);
                    d.velocity *= 0.5f;
                    d.noGravity = false;
                    d.color = Color.White;
                }
                for (int i = 0; i < 100; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(.5f, 1f);
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.FlameBurst, speed * 14, Scale: 1.5f);
                    d.noGravity = true;
                    d.velocity *= -2;
                    d.color = Color.OrangeRed;
                }
                for (int i = 0; i < 100; i++)
                {
                    Vector2 speed = Main.rand.NextVector2Circular(.5f, 1f);
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.FlameBurst, speed * 14, Scale: 1.5f);
                    d.noGravity = true;
                    d.velocity *= -2;
                    d.color = Color.OrangeRed;
                }
                for (int i = 0; i < 100; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.GoldFlame, speed * 8, Scale: 1.5f);
                    d.noGravity = true;
                    d.color = Color.Red;
                }
                for (int i = 0; i < 100; i++)
                {
                    Vector2 speed = Main.rand.NextVector2Circular(1f, 1f);
                    Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.GoldFlame, speed * 8, Scale: 1.5f);
                    d.noGravity = true;
                    d.color = Color.Red;
                }

                for (int g = 0; g < 1; g++)
                {
                    int goreIndex = Gore.NewGore(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].scale = 1f;
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.2f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.2f;
                    goreIndex = Gore.NewGore(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].scale = 1f;
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.2f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y + 1.2f;
                    goreIndex = Gore.NewGore(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].scale = 1f;
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X + 1.2f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.2f;
                    goreIndex = Gore.NewGore(Projectile.GetSource_FromThis(), new Vector2(Projectile.position.X + (float)(Projectile.width / 2) - 24f, Projectile.position.Y + (float)(Projectile.height / 2) - 24f), default(Vector2), Main.rand.Next(61, 64), 1f);
                    Main.gore[goreIndex].scale = 1f;
                    Main.gore[goreIndex].velocity.X = Main.gore[goreIndex].velocity.X - 1.2f;
                    Main.gore[goreIndex].velocity.Y = Main.gore[goreIndex].velocity.Y - 1.2f;
                }

                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<fireExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
            }
        }
    }

    internal class fireExplosion : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ShadowBeamFriendly;
        public override void SetDefaults()
        {
            //proj stats
            Projectile.penetrate = -1;

            //proj configs
            Projectile.width = 250;
            Projectile.height = 250;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 5;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(1, 21) > 16)
            {
                target.AddBuff(BuffID.OnFire, 60);
            }
        }
    }
}
