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
using Terraria.Localization;
using DnD.Furniture;
using Terraria.GameContent.Creative;
using DnD.Common;

namespace DnD.Items.Spells.Wizard_Spells.Lvl1
{
    internal class RayOfSickness : ModItem
    {
        public int spellLevel = 1;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Ray of Sickness");
            /* Tooltip.SetDefault(value: "[c/FF0000:Level 1:]" +
                "\nDoes 2d8 poison damage + 1d8 for each level above 1st multiplied by proficiency bonus" +
                "\nA ray of sickening greenish energy lashes out toward a creature within range" +
                "\nOn a failed save the target is also poisoned for 3 seconds" +
                "\nRight clicking while holding changes spell level"); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
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
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.rare = ModContent.RarityType<Rarities.WizardRare>();
            Item.UseSound = SoundID.Item16;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex(x => x.Name == "Damage");
            if (index == -1)
                return;
            tooltips[index].Text = "2d8 + 1d8 per level above 1st";
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
                        Item.shoot = ModContent.ProjectileType<SickRay>();
                        Item.mana = 3 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item16;
                    }
                    else if (spellLevel == 3)
                    {
                        Item.shoot = ModContent.ProjectileType<SickRay>();
                        Item.mana = 5 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item16;
                    }
                    else if (spellLevel == 4)
                    {
                        Item.shoot = ModContent.ProjectileType<SickRay>();
                        Item.mana = 6 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item16;
                    }
                    else if (spellLevel == 5)
                    {
                        Item.shoot = ModContent.ProjectileType<SickRay>();
                        Item.mana = 7 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item16;
                    }
                    else if (spellLevel == 6)
                    {
                        Item.shoot = ModContent.ProjectileType<SickRay>();
                        Item.mana = 9 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item16;
                    }
                    else if (spellLevel == 7)
                    {
                        Item.shoot = ModContent.ProjectileType<SickRay>();
                        Item.mana = 10 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item16;
                    }
                    else if (spellLevel == 8)
                    {
                        Item.shoot = ModContent.ProjectileType<SickRay>();
                        Item.mana = 11 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item16;
                    }
                    else if (spellLevel == 9)
                    {
                        Item.shoot = ModContent.ProjectileType<SickRay>();
                        Item.mana = 13 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item16;
                    }
                    else
                    {
                        Item.shoot = ModContent.ProjectileType<SickRay>();
                        Item.mana = 2 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item16;
                    }
                }
                return true;
            }
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();

            DnDItem sItem = new();

            damage += sItem.DamageValue(minRoll: 1, maxRoll: 8, diceRolled: spellLevel + 1);
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

        internal class SickRay : ModProjectile
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
                Projectile.coldDamage = false;

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
                        int dust = Dust.NewDust(projectilePosition, 1, 1, DustID.GreenFairy, 0f, 0f, 0, default(Color), 1f);
                        Main.dust[dust].noGravity = true;
                        Main.dust[dust].position = projectilePosition;
                        Main.dust[dust].scale = (float)Main.rand.Next(70, 110) * 0.013f;
                        Main.dust[dust].velocity *= 0.2f;
                    }
                }
                Projectile.netUpdate = true;
            }

            public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
            {
                if (Main.rand.Next(1, 21) < 16)
                {
                    target.AddBuff(BuffID.Poisoned, 180);
                }
            }
        }
    }
}
