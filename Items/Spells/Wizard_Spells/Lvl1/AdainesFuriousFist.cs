using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using DnD.Furniture;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DnD.Common;

namespace DnD.Items.Spells.Wizard_Spells.Lvl1
{
    internal class AdainesFuriousFist : ModItem
    {
        public int spellLevel = 1;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Adaine's Furious Fist");
            /* Tooltip.SetDefault(value: "[c/FF0000:Level 1:]" +
                "\nYour fist gleams with a silvery arcane light" +
                "\nOn a failed save the target is also knocked back 5 feet" +
                "\nRight clicking while holding changes spell level"); */
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
            Item.value = Item.sellPrice(0, 0, 10, 0);

            //item configs
            Item.width = 32;
            Item.height = 32;
            Item.shoot = ModContent.ProjectileType<FuriousFist>();
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.rare = ModContent.RarityType<Rarities.WizardRare>();
            Item.UseSound = SoundID.Item10;
            Item.knockBack = 5;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex(x => x.Name == "Damage");
            if (index == -1)
                return;
            tooltips[index].Text = "2d8 + 2d8 per spell level above 1st level";
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
                        Item.mana = 0;
                        spellLevel++;
                        Main.NewText("Current spell level is " + spellLevel, 152, 104, 126);
                    }
                    else
                    {
                        Item.shoot = ProjectileID.None;
                        Item.UseSound = SoundID.Item4;
                        Item.mana = 0;
                        spellLevel = 1;
                        Main.NewText("Current spell level is " + spellLevel, 152, 104, 126);
                    }

                }
                else
                {

                    if (spellLevel == 2)
                    {
                        Item.shoot = ModContent.ProjectileType<FuriousFist>();
                        Item.mana = 3 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item10;
                        Item.knockBack = 10;
                    }
                    else if (spellLevel == 3)
                    {
                        Item.shoot = ModContent.ProjectileType<FuriousFist>();
                        Item.mana = 5 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item10;
                        Item.knockBack = 15;
                    }
                    else if (spellLevel == 4)
                    {
                        Item.shoot = ModContent.ProjectileType<FuriousFist>();
                        Item.mana = 6 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item10;
                        Item.knockBack = 20;
                    }
                    else if (spellLevel == 5)
                    {
                        Item.shoot = ModContent.ProjectileType<FuriousFist>();
                        Item.mana = 7 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item10;
                        Item.knockBack = 25;
                    }
                    else if (spellLevel == 6)
                    {
                        Item.shoot = ModContent.ProjectileType<FuriousFist>();
                        Item.mana = 9 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item10;
                        Item.knockBack = 30;
                    }
                    else if (spellLevel == 7)
                    {
                        Item.shoot = ModContent.ProjectileType<FuriousFist>();
                        Item.mana = 10 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item10;
                        Item.knockBack = 35;
                    }
                    else if (spellLevel == 8)
                    {
                        Item.shoot = ModContent.ProjectileType<FuriousFist>();
                        Item.mana = 11 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item10;
                        Item.knockBack = 40;
                    }
                    else if (spellLevel == 9)
                    {
                        Item.shoot = ModContent.ProjectileType<FuriousFist>();
                        Item.mana = 13 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item10;
                        Item.knockBack = 45;
                    }
                    else
                    {
                        Item.shoot = ModContent.ProjectileType<FuriousFist>();
                        Item.mana = 2 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item10;
                        Item.knockBack = 5;
                    }
                }
                return true;
            }
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();

            DnDItem sItem = new();

            damage += sItem.DamageValue( minRoll: 1, maxRoll: 10, diceRolled: spellLevel * 2); 
        }
        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            target.defense *= 0;
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

    internal class FuriousFist : ModProjectile
    {
        public const int FadeInDuration = 7;
        public const int FadeOutDuration = 4;

        public const int TotalDuration = 16;

        public float CollisionWidth => 10f * Projectile.scale;


        public int Timer
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(40); // This sets width and height to the same value (important when projectiles can rotate)
            Projectile.aiStyle = -1;

            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.ownerHitCheck = true;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 360;

            Projectile.alpha = 200;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return new Color(255, 255, 255, 0) * Projectile.Opacity;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Timer += 1;
            if (Timer >= TotalDuration)
            {
                // Kill the projectile if it reaches it's intented lifetime
                Projectile.Kill();
                return;
            }
            else
            {
                // Important so that the sprite draws "in" the player's hand and not fully infront or behind the player
                player.heldProj = Projectile.whoAmI;
            }

            Lighting.AddLight(Projectile.Center, 0.1f, 0.1f, 0.8f);

            // Fade in and out
            // GetLerpValue returns a value between 0f and 1f - if clamped is true - representing how far Timer got along the "distance" defined by the first two parameters
            // The first call handles the fade in, the second one the fade out.
            // Notice the second call's parameters are swapped, this means the result will be reverted
            Projectile.Opacity = Utils.GetLerpValue(0f, FadeInDuration, Timer, clamped: true) * Utils.GetLerpValue(TotalDuration, TotalDuration - FadeOutDuration, Timer, clamped: true);

            // Keep locked onto the player, but extend further based on the given velocity (Requires ShouldUpdatePosition returning false to work)
            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false);
            Projectile.Center = playerCenter + Projectile.velocity * (Timer - 1f);

            // Set spriteDirection based on moving left or right. Left -1, right 1
            Projectile.spriteDirection = (Vector2.Dot(Projectile.velocity, Vector2.UnitX) >= 0f).ToDirectionInt();


            Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 0f) ? 1 : -1;

            Projectile.rotation = Projectile.velocity.ToRotation();

            if(Projectile.spriteDirection == -1)
            {
                Projectile.rotation += MathHelper.Pi;
            }

            SetVisualOffsets();
        }

        private void SetVisualOffsets()
        {
            // 32 is the sprite size (here both width and height equal)
            const int HalfSpriteWidth = 32 / 2;
            const int HalfSpriteHeight = 32 / 2;

            int HalfProjWidth = Projectile.width / 2;
            int HalfProjHeight = Projectile.height / 2;

            // Vanilla configuration for "hitbox in middle of sprite"
            DrawOriginOffsetX = 0;
            DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
            DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);

            // Vanilla configuration for "hitbox towards the end"
            //if (Projectile.spriteDirection == 1) {
            //	DrawOriginOffsetX = -(HalfProjWidth - HalfSpriteWidth);
            //	DrawOffsetX = (int)-DrawOriginOffsetX * 2;
            //	DrawOriginOffsetY = 0;
            //}
            //else {
            //	DrawOriginOffsetX = (HalfProjWidth - HalfSpriteWidth);
            //	DrawOffsetX = 0;
            //	DrawOriginOffsetY = 0;
            //}
        }

        public override bool ShouldUpdatePosition()
        {
            // Update Projectile.Center manually
            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // "Hit anything between the player and the tip of the sword"
            // shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
            Vector2 start = Projectile.Center;
            Vector2 end = start + Projectile.velocity * 6f;
            float collisionPoint = 0f; // Don't need that variable, but required as parameter
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, CollisionWidth, ref collisionPoint);
        } 
    }
}
