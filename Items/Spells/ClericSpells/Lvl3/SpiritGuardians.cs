using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using DnD.Graphics.Dusts;
using Terraria.DataStructures;
using DnD.Common.Structs;
using Terraria.Localization;

namespace DnD.Items.Spells.ClericSpells.Lvl3
{
    internal class SpiritGuardians : ModItem
    {
        int spellLevel;
        int minLevel = 3;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spirit Guardians");

            Tooltip.SetDefault(value: "[c/FF0000:Level 3:]" +
            "\nYou call forth spirits to protect you" +
            "\nDoes 3d8 radiant damage and an additional 1d8 for each level above 3rd");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item4;
            Item.rare = ModContent.RarityType<Rarities.ClericRare>();
            Item.value = 0;

            Item.damage = 1;
            Item.DamageType = DamageClass.Summon;

            Item.buffType = ModContent.BuffType<SpiritGuard>();
            Item.buffTime = 6800;

            Item.shoot = ModContent.ProjectileType<Guard>();

            Item.mana = 12;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 10, 0, 0);

            spellLevel = minLevel;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
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
                        Item.buffType = 0;
                        Item.buffTime = 1;
                        Item.shoot = ProjectileID.None;
                        Item.UseSound = SoundID.Item4;
                        Item.reuseDelay = 5;
                        Item.mana = 0;
                        spellLevel++;
                        Main.NewText("Current spell level is " + spellLevel, 152, 104, 126);
                    }
                    else
                    {
                        Item.buffType = 0;
                        Item.buffTime = 1;
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
                        Item.buffType = ModContent.BuffType<SpiritGuard>();
                        Item.buffTime = 6800;
                        Item.shoot = ModContent.ProjectileType<Guard>();
                        Item.mana = 5 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item14;
                        Item.reuseDelay = 25;
                    }
                    else if (spellLevel == 4)
                    {
                        Item.buffType = ModContent.BuffType<SpiritGuard>();
                        Item.buffTime = 6800;
                        Item.shoot = ModContent.ProjectileType<Guard>();
                        Item.mana = 6 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item14;
                        Item.reuseDelay = 25;
                    }
                    else if (spellLevel == 5)
                    {
                        Item.buffType = ModContent.BuffType<SpiritGuard>();
                        Item.buffTime = 6800;
                        Item.shoot = ModContent.ProjectileType<Guard>();
                        Item.mana = 7 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item14;
                        Item.reuseDelay = 25;
                    }
                    else if (spellLevel == 6)
                    {
                        Item.buffType = ModContent.BuffType<SpiritGuard>();
                        Item.buffTime = 6800;
                        Item.shoot = ModContent.ProjectileType<Guard>();
                        Item.mana = 9 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item14;
                        Item.reuseDelay = 25;
                    }
                    else if (spellLevel == 7)
                    {
                        Item.buffType = ModContent.BuffType<SpiritGuard>();
                        Item.buffTime = 6800;
                        Item.shoot = ModContent.ProjectileType<Guard>();
                        Item.mana = 10 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item14;
                        Item.reuseDelay = 25;
                    }
                    else if (spellLevel == 8)
                    {
                        Item.buffType = ModContent.BuffType<SpiritGuard>();
                        Item.buffTime = 6800;
                        Item.shoot = ModContent.ProjectileType<Guard>();
                        Item.mana = 11 + pc.ProfBonus();
                        Item.UseSound = SoundID.Item14;
                        Item.reuseDelay = 25;
                    }
                    else if (spellLevel == 9)
                    {
                        Item.buffType = ModContent.BuffType<SpiritGuard>();
                        Item.buffTime = 6800;
                        Item.shoot = ModContent.ProjectileType<Guard>();
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

            damage += sItem.DamageValue(minRoll: 1, maxRoll: 8, diceRolled: spellLevel);
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

            // This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
            player.AddBuff(Item.buffType, 6800);

            // Minions have to be spawned manually, then have originalDamage assigned to the damage of the summon item
            var projectile = Projectile.NewProjectileDirect(source, player.Center, velocity, type, damage, knockback, player.whoAmI);
            projectile.originalDamage = Item.damage;

            // Since we spawned the projectile manually already, we do not need the game to spawn it for ourselves anymore, so return false
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.ClassToken>())
                .AddTile(ModContent.TileType<Furniture.PHBTile>())
                .AddCondition(NetworkText.FromLiteral("Must be of level 5 or higher"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().playerLevel >= 5)
                .AddCondition(NetworkText.FromLiteral("Must be the right class"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().clericClass == true)
                .Register();
        }
    }

    internal class SpiritGuard : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Spirit Guardian");
            Description.SetDefault("You are protected by the spirits");

            Main.buffNoSave[Type] = true; // This buff won't save when you exit the world
        }

        public override void Update(Player player, ref int buffIndex)
        {
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                NPC npc = Main.npc[i];
                if(Vector2.Distance(player.Center, npc.Center) <= 200)
                {
                    npc.velocity *= 0.5f;
                }
            }
        }
    }

    internal class Guard : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 600;
            Projectile.height = 600;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 100;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.damage = 1;
            Projectile.friendly = true;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 40;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.Center = player.Center;

            Projectile.rotation += 0.05f;

            if (player.HasBuff(ModContent.BuffType<SpiritGuard>()))
                Projectile.timeLeft = 2;


            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            if (pc.wizardClass == true)
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(4f, 4f);
                    Dust d = Dust.NewDustPerfect(new Vector2(player.Center.X + speed.X * 75, player.Center.Y + speed.Y * 75), ModContent.DustType<MagicRunes>(), Vector2.Zero, Scale: 2f);
                    d.velocity *= 0.5f;
                    d.noGravity = true;
                    d.color = Color.Gold;
                    d.noLight = true;
                    d.alpha = 100;
                }
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            CreatureArrays ca = new();
            if (ca.undeadNames.Any(target.FullName.Contains))
            {
                damage *= 2;
            }
        }
    }
}
