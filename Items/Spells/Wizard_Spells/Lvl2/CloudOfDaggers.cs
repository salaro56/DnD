using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using DnD.Furniture;
using Terraria.Localization;
using Terraria.Audio;

namespace DnD.Items.Spells.Wizard_Spells.Lvl2
{
    internal class CloudOfDaggers : ModItem
    {
        public int spellLevel = 2;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cloud of Daggers");
            Tooltip.SetDefault(value: "[c/FF0000:Level 2:]" +
                "\nYou fill the air with spinning daggers in a cube 5 feet on each side, centered on a point you choose within range." +
                "\nA creature takes 4d4 multiplied by proficiency bonus, slashing damage when it enters the spell's area" +
                "\nWhen you cast this spell using a spell level of 3rd or higher the damage increases by 2d4");
        }

        public override void SetDefaults()
        {
            //item stats
            Item.maxStack = 1;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.reuseDelay = 20;
            Item.damage = 1;
            Item.ArmorPenetration += 999;
            Item.value = Item.sellPrice(0, 0, 25, 0);

            //item configs
            Item.width = 32;
            Item.height = 32;
            Item.shoot = ModContent.ProjectileType<DaggerCloud>();
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.rare = ModContent.RarityType<Rarities.WizardRare>();
            Item.UseSound = SoundID.Item35;
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
            else if (pc.isConcentrated == true)
            {
                CombatText.NewText(player.getRect(), Color.Blue, "Concentrated");
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
                    }
                    else
                    {
                        Item.shoot = ProjectileID.None;
                        Item.UseSound = SoundID.Item4;
                        Item.mana = 0;
                        spellLevel = 2;
                        Main.NewText("Current spell level is " + spellLevel, 152, 104, 126);
                    }
                }
                else
                {
                    if (spellLevel == 3)
                    {
                        Item.shoot = ModContent.ProjectileType<DaggerCloud>();
                        Item.mana = 5 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item4;
                    }
                    if (spellLevel == 4)
                    {
                        Item.shoot = ModContent.ProjectileType<DaggerCloud>();
                        Item.mana = 6 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item4;
                    }
                    if (spellLevel == 5)
                    {
                        Item.shoot = ModContent.ProjectileType<DaggerCloud>();
                        Item.mana = 7 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item4;
                    }
                    if (spellLevel == 6)
                    {
                        Item.shoot = ModContent.ProjectileType<DaggerCloud>();
                        Item.mana = 9 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item4;
                    }
                    if (spellLevel == 7)
                    {
                        Item.shoot = ModContent.ProjectileType<DaggerCloud>();
                        Item.mana = 10 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item4;
                    }
                    if (spellLevel == 8)
                    {
                        Item.shoot = ModContent.ProjectileType<DaggerCloud>();
                        Item.mana = 11 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item4;
                    }
                    if (spellLevel == 9)
                    {
                        Item.shoot = ModContent.ProjectileType<DaggerCloud>();
                        Item.mana = 13 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item4;
                    }
                    else
                    {
                        Item.shoot = ModContent.ProjectileType<DaggerCloud>();
                        Item.mana = 3 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item4;
                    }
                }
                return true;
            }
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();

            DnDItem sItem = ModContent.GetInstance<DnDItem>();

            damage += sItem.DamageValue(minRoll: 1, maxRoll: 4, diceRolled: (spellLevel) * 2);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Item.shoot)
                {
                    Main.projectile[i].Kill();
                }
            }
            Vector2 mousePosition = Main.MouseWorld;
            Projectile.NewProjectile(source, mousePosition, velocity, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            target.defense *= 0;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.ClassToken>())
                .AddIngredient(ItemID.Glass, 10)
                .AddTile(ModContent.TileType<PHBTile>())
                .AddCondition(NetworkText.FromLiteral("Must be of level 3 or higher"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().playerLevel >= 3)
                .AddCondition(NetworkText.FromLiteral("Must be the right class"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().wizardClass == true)
                .Register();
        }
    }

    internal class DaggerCloud : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 76;
            Projectile.height = 94;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;

            Projectile.timeLeft = 360;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 25;
        }

        public override void AI()
        {  
            if (++Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            if (Projectile.active == true)
            {
                Main.player[Projectile.owner].GetModPlayer<DnDPlayer>().isConcentrated = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);

            Vector2 origin = sourceRectangle.Size() / 2f;

            float offsetX = 19f;
            origin.X = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Width - offsetX : offsetX);

            Color drawColor = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects ,0);

            return false;
        }

        public override void Kill(int timeLeft)
        {
            if (!Main.gameMenu)
            {
                SoundEngine.PlaySound(SoundID.Item34, Projectile.position);
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
        }
    }
}
