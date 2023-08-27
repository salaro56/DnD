using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Audio;
using Terraria.Localization;
using DnD.Furniture;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Creative;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using DnD.Graphics;
using Terraria.Graphics;
using DnD.Common;

namespace DnD.Items.Spells.Wizard_Spells.Lvl1
{
    internal class TrueMagicMissile : ModItem
    {
        public int spellLevel = 1;

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("True Magic Missile");
            /* Tooltip.SetDefault(value: "[c/FF0000:Level 1:]" +
                "\nYou create three glowing darts of magical force. Each dart hits a creature that you can see within range" +
                "\nA dart deals 1d4 + 1 multiplied by proficiency force damage to its target" +
                "\nWhen you cast this spell using a spell slot of 2nd level or higher the number of darts increases by 1" +
                "\nRight clicking while holding changes spell level"); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void SetDefaults()
        {
            SoundStyle soundFile = new SoundStyle($"{nameof(DnD)}/Sounds/Item/MagicChant" + Main.rand.Next(1, 5));

            //item stats
            Item.maxStack = 1;
            Item.useTime = 15;
            Item.useAnimation = 15;
            Item.reuseDelay = 15;
            Item.shootSpeed = 50f;
            Item.damage = 1;
            Item.ArmorPenetration += 999;
            Item.autoReuse = true;
            Item.value = Item.sellPrice(0, 0, 10, 0);

            //item configs
            Item.width = 32;
            Item.height = 32;
            Item.shoot = ModContent.ProjectileType<MagicMissiles>();
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.rare = ModContent.RarityType<Rarities.WizardRare>();
            Item.UseSound = new SoundStyle($"{nameof(DnD)}/Sounds/Item/MagicChant" + Main.rand.Next(1, 5));
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex(x => x.Name == "Damage");
            if (index == -1)
                return;
            tooltips[index].Text = "1d4 + 1 per bolt";
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
                        Item.useTime = 5;
                        Item.useAnimation = 5;
                        Item.reuseDelay = 5;
                        spellLevel++;
                        Main.NewText("Current spell level is " + spellLevel, 152, 104, 126);
                    }
                    else
                    {
                        Item.shoot = ProjectileID.None;
                        Item.UseSound = SoundID.Item4;
                        Item.mana = 0;
                        spellLevel = 1;
                        Item.useTime = 5;
                        Item.useAnimation = 5;
                        Item.reuseDelay = 5;
                        Main.NewText("Current spell level is " + spellLevel, 152, 104, 126);
                    }

                }
                else
                {

                    if (spellLevel == 2)
                    {
                        Item.shoot = ModContent.ProjectileType<MagicMissiles>();
                        Item.mana = 3 + pc.ProfBonus();
                        Item.UseSound = new SoundStyle($"{nameof(DnD)}/Sounds/Item/MagicChant" + Main.rand.Next(1, 5));
                        Item.useTime = 15;
                        Item.useAnimation = 15;
                        Item.reuseDelay = 15;
                    }
                    else if (spellLevel == 3)
                    {
                        Item.shoot = ModContent.ProjectileType<MagicMissiles>();
                        Item.mana = 5 + pc.ProfBonus();
                        Item.UseSound = new SoundStyle($"{nameof(DnD)}/Sounds/Item/MagicChant" + Main.rand.Next(1, 5));
                        Item.useTime = 15;
                        Item.useAnimation = 15;
                        Item.reuseDelay = 15;
                    }
                    else if (spellLevel == 4)
                    {
                        Item.shoot = ModContent.ProjectileType<MagicMissiles>();
                        Item.mana = 6 + pc.ProfBonus();
                        Item.UseSound = new SoundStyle($"{nameof(DnD)}/Sounds/Item/MagicChant" + Main.rand.Next(1, 5));
                        Item.useTime = 15;
                        Item.useAnimation = 15;
                        Item.reuseDelay = 15;
                    }
                    else if (spellLevel == 5)
                    {
                        Item.shoot = ModContent.ProjectileType<MagicMissiles>();
                        Item.mana = 7 + pc.ProfBonus();
                        Item.UseSound = new SoundStyle($"{nameof(DnD)}/Sounds/Item/MagicChant" + Main.rand.Next(1, 5));
                        Item.useTime = 15;
                        Item.useAnimation = 15;
                        Item.reuseDelay = 15;
                    }
                    else if (spellLevel == 6)
                    {
                        Item.shoot = ModContent.ProjectileType<MagicMissiles>();
                        Item.mana = 9 + pc.ProfBonus();
                        Item.UseSound = new SoundStyle($"{nameof(DnD)}/Sounds/Item/MagicChant" + Main.rand.Next(1, 5));
                        Item.useTime = 15;
                        Item.useAnimation = 15;
                        Item.reuseDelay = 15;
                    }
                    else if (spellLevel == 7)
                    {
                        Item.shoot = ModContent.ProjectileType<MagicMissiles>();
                        Item.mana = 10 + pc.ProfBonus();
                        Item.UseSound = new SoundStyle($"{nameof(DnD)}/Sounds/Item/MagicChant" + Main.rand.Next(1, 5));
                        Item.useTime = 15;
                        Item.useAnimation = 15;
                        Item.reuseDelay = 15;
                    }
                    else if (spellLevel == 8)
                    {
                        Item.shoot = ModContent.ProjectileType<MagicMissiles>();
                        Item.mana = 11 + pc.ProfBonus();
                        Item.UseSound = new SoundStyle($"{nameof(DnD)}/Sounds/Item/MagicChant" + Main.rand.Next(1, 5));
                        Item.useTime = 15;
                        Item.useAnimation = 15;
                        Item.reuseDelay = 15;
                    }
                    else if (spellLevel == 9)
                    {
                        Item.shoot = ModContent.ProjectileType<MagicMissiles>();
                        Item.mana = 13 + pc.ProfBonus();
                        Item.UseSound = new SoundStyle($"{nameof(DnD)}/Sounds/Item/MagicChant" + Main.rand.Next(1, 5));
                        Item.useTime = 15;
                        Item.useAnimation = 15;
                        Item.reuseDelay = 15;
                    }
                    else
                    {
                        Item.shoot = ModContent.ProjectileType<MagicMissiles>();
                        Item.mana = 2 + pc.ProfBonus();
                        Item.UseSound = new SoundStyle($"{nameof(DnD)}/Sounds/Item/MagicChant" + Main.rand.Next(1, 5));
                        Item.useTime = 15;
                        Item.useAnimation = 15;
                        Item.reuseDelay = 15;
                    }
                }
                return true;
            }
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            damage += (Main.rand.Next(1, 5) + 1) * pc.ProfBonus();
        }

        public override void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            target.defense *= 0;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();

            float numberProjectiles = spellLevel + 2; // Takes the player level and adds 2 to match with extra darts per level

            float rotation = MathHelper.ToRadians(20);//Shoots them in a 45 degree radius. (This is technically 90 degrees because it's 45 degrees up from your cursor and 45 degrees down)
            position += Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 20f; //45 should equal whatever number you had on the previous line
            for (int i = 0; i < numberProjectiles; i++)
            {
                Vector2 perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f; // Vector for spread. Watch out for dividing by 0 if there is only 1 Projectile.
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI); //Creates a new Projectile with our new vector for spread.
            }
            return false; //makes sure it doesn't shoot the Projectile again after this
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

    internal class MagicMissiles : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 40;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            //proj stats
            Projectile.penetrate = 1;
            Projectile.coldDamage = false;

            //proj configs
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 180;
            Projectile.velocity *= 2;
            Projectile.tileCollide = false;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, default, Projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1.25f, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            default(MagicMissileDrawer).Draw(Projectile);
            return true;
        }

        public override void PostDraw(Color lightColor)
        {
            default(MagicMissileDrawer).Draw(Projectile);
        }

        public override void AI()
        {
            Projectile.localAI[0] += 1f;
            if (Projectile.localAI[0] < 3f)
            {
                for (int i = 0; i < 4; i++)
                {
                    for (int x = 0; x < 20; x++)
                    {
                        Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                        Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.YellowStarDust, speed * 4, Scale: 2f);
                        d.velocity *= 0.5f;
                        d.noGravity = true;
                    }
                }
            }

            float num132 = (float)Math.Sqrt((double)(Projectile.velocity.X * Projectile.velocity.X + Projectile.velocity.Y * Projectile.velocity.Y));
            float num133 = Projectile.localAI[0];
            if (num133 == 0f)
            {
                Projectile.localAI[0] = num132;
                num133 = num132;
            }
            float num134 = Projectile.position.X;
            float num135 = Projectile.position.Y;
            float num136 = 300f;
            bool flag3 = false;
            int num137 = 0;
            if (Projectile.ai[1] == 0f)
            {
                for (int num138 = 0; num138 < 200; num138++)
                {
                    if (Main.npc[num138].CanBeChasedBy(this, false) && (Projectile.ai[1] == 0f || Projectile.ai[1] == (float)(num138 + 1)))
                    {
                        float num139 = Main.npc[num138].position.X + (float)(Main.npc[num138].width / 2);
                        float num140 = Main.npc[num138].position.Y + (float)(Main.npc[num138].height / 2);
                        float num141 = Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num139) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num140);
                        if (num141 < num136 && Collision.CanHit(new Vector2(Projectile.position.X + (float)(Projectile.width / 2), Projectile.position.Y + (float)(Projectile.height / 2)), 1, 1, Main.npc[num138].position, Main.npc[num138].width, Main.npc[num138].height))
                        {
                            num136 = num141;
                            num134 = num139;
                            num135 = num140;
                            flag3 = true;
                            num137 = num138;
                        }
                    }
                }
                if (flag3)
                {
                    Projectile.ai[1] = (float)(num137 + 1);
                }
                flag3 = false;
            }
            if (Projectile.ai[1] > 0f)
            {
                int num142 = (int)(Projectile.ai[1] - 1f);
                if (Main.npc[num142].active && Main.npc[num142].CanBeChasedBy(this, true) && !Main.npc[num142].dontTakeDamage)
                {
                    float num143 = Main.npc[num142].position.X + (float)(Main.npc[num142].width / 2);
                    float num144 = Main.npc[num142].position.Y + (float)(Main.npc[num142].height / 2);
                    if (Math.Abs(Projectile.position.X + (float)(Projectile.width / 2) - num143) + Math.Abs(Projectile.position.Y + (float)(Projectile.height / 2) - num144) < 1000f)
                    {
                        flag3 = true;
                        num134 = Main.npc[num142].position.X + (float)(Main.npc[num142].width / 2);
                        num135 = Main.npc[num142].position.Y + (float)(Main.npc[num142].height / 2);
                    }
                }
                else
                {
                    Projectile.ai[1] = 0f;
                }
            }
            if (!Projectile.friendly)
            {
                flag3 = false;
            }
            if (flag3)
            {
                float num145 = num133;
                Vector2 vector10 = new Vector2(Projectile.position.X + (float)Projectile.width * 0.5f, Projectile.position.Y + (float)Projectile.height * 0.5f);
                float num146 = num134 - vector10.X;
                float num147 = num135 - vector10.Y;
                float num148 = (float)Math.Sqrt((double)(num146 * num146 + num147 * num147));
                num148 = num145 / num148;
                num146 *= num148;
                num147 *= num148;
                int num149 = 8;
                Projectile.velocity.X = (Projectile.velocity.X * (float)(num149 - 1) + num146) / (float)num149;
                Projectile.velocity.Y = (Projectile.velocity.Y * (float)(num149 - 1) + num147) / (float)num149;
            }
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
            Projectile.netUpdate = true;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust d = Dust.NewDustPerfect(Projectile.Center, DustID.BlueCrystalShard, speed * 4, Scale: 2f);
                d.velocity *= 0.5f;
                d.noGravity = true;
            }
            SoundEngine.PlaySound(SoundID.DD2_LightningAuraZap);
        }
    }
}

