using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using DnD.Items.Spells.Wizard_Spells;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using Terraria.Audio;
using DnD.Packets;
using DnD.Items;
using DnD.Items.Spells.ClericSpells.Lvl1;
using DnD.Items.Weapons.Rogue;

namespace DnD
{
    public class DnDPlayer : ModPlayer
    {
        public int playerLevel;
        public int experiencePoints;

        public bool wizardClass;
        public bool clericClass;
        public bool sorcClass;
        public bool warlockClass;
        public bool fighterClass;
        public bool paladinClass;
        public bool barbClass;
        public bool rogueClass;

        public bool SpellSlots;

        public bool canRage = false;
        public bool isRaging = false;
        public bool isSneaking = false;

        public int channelDivinity;

        public int DailySteps = 3;
        public int MysticSteps = 3;


        public void LevelUp()
        {
            Player player = Main.LocalPlayer;
            playerLevel += 1;
            if(!Main.gameMenu)
            {
                SoundEngine.PlaySound(SoundID.Thunder, Player.position);
            }
            SyncLevelPacket.Write(Player.whoAmI, playerLevel);
            CombatText.NewText(Player.getRect(), new Color(0, 240, 10), "Level up!");
            for (int i = 0; i < 20; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(3f, 3f);
                Dust d = Dust.NewDustPerfect(Player.Center, DustID.ChlorophyteWeapon, speed * 4, Scale: 2f);
                d.velocity *= 1f;
                d.noGravity = true;
                d.color = Color.Lime;
            }
            for (int i = 0; i < 20; i++)
            {
                Vector2 speed = Main.rand.NextVector2Circular(3f, 3f);
                Dust d = Dust.NewDustPerfect(Player.Center, DustID.ChlorophyteWeapon, speed * 4, Scale: 2f);
                d.velocity *= 1f;
                d.noGravity = true;
                d.color = Color.Lime;
            }

            player.QuickSpawnItem(Player.GetSource_FromThis(), ModContent.ItemType<ClassToken>(), ProfBonus());
            Main.NewText("You have leveled up! You are now level " + playerLevel, 10, 255, 10);
            Main.NewText("Check your class features to see if you unlocked anything new!", 10, 255, 10);

            LevelAnnouncement();
        }

        private void LevelAnnouncement()
        {
            switch(playerLevel)
            {
                case 3:
                    CombatText.NewText(Player.getRect(), new Color(80, 140, 20), "Spell Level Max Increased");
                    break;
                case 5:
                    CombatText.NewText(Player.getRect(), new Color(80, 140, 20), "Spell Level Max Increased");
                    break;
                case 7:
                    CombatText.NewText(Player.getRect(), new Color(80, 140, 20), "Spell Level Max Increased");
                    break;
                case 9:
                    CombatText.NewText(Player.getRect(), new Color(80, 140, 20), "Spell Level Max Increased");
                    break;
                case 11:
                    CombatText.NewText(Player.getRect(), new Color(80, 140, 20), "Spell Level Max Increased");
                    break;
                case 13:
                    CombatText.NewText(Player.getRect(), new Color(80, 140, 20), "Spell Level Max Increased");
                    break;
                case 15:
                    CombatText.NewText(Player.getRect(), new Color(80, 140, 20), "Spell Level Max Increased");
                    break;
                case 17:
                    CombatText.NewText(Player.getRect(), new Color(80, 140, 20), "Spell Level Max Increased");
                    break;

            }
        }

        public void AddXP(int xp)
        {
            if (Main.gameMenu)
                return;
            if (xp == 0)
                return;
            experiencePoints += xp;

            Check:
            if(experiencePoints >= XPToLevel() && playerLevel < 20)
            {
                experiencePoints -= XPToLevel();
                LevelUp();
                goto Check;
            }

            CombatText.NewText(Player.getRect(), new Color(150, 10, 10), xp + " XP");
        }
        public int SpellSlot()
        {
            return playerLevel switch
            {
                1 or 2 => 1,
                3 or 4 => 2,
                5 or 6 => 3,
                7 or 8 => 4,
                9 or 10 => 5,
                11 or 12 => 6,
                13 or 14 => 7,
                15 or 16 => 8,
                17 or 18 or 19 or 20 => 9,
                _ => 0,
            };
        }

        public int ProfBonus()
        {
            return playerLevel switch
            {
                1 or 2 or 3 or 4 => 2,
                5 or 6 or 7 or 8 => 3,
                9 or 10 or 11 or 12 => 4,
                13 or 14 or 15 or 16 => 5,
                17 or 18 or 19 or 20 => 6,
                _ => 2,
            };
        }

        public int XPToLevel()
        {
            return playerLevel switch
            {
                1 => 300,
                2 => 900,
                3 => 2700,
                4 => 6500,
                5 => 14000,
                6 => 23000,
                7 => 34000,
                8 => 48000,
                9 => 64000,
                10 => 85000,
                11 => 100000,
                12 => 120000,
                13 => 140000,
                14 => 165000,
                15 => 195000,
                16 => 225000,
                17 => 265000,
                18 => 305000,
                19 => 355000,
                20 => 0,
                _ => 0,
            };
        }

        public override void ResetEffects()
        {
            clericClass = false;
            wizardClass = false;
            barbClass = false;
            isRaging = false;
            canRage = false;
            rogueClass = false;
            isSneaking = false;

            NightBlade jumps = ModContent.GetInstance<NightBlade>();

            if (SpellSlots == true)
            {
                Player player = Main.LocalPlayer;
                player.manaRegen = 0;
                player.manaRegenCount = 0;
                player.manaRegenBonus = 0;
                player.manaRegenCount = 1000;
                player.manaRegenDelay = 1000;

                if (Main.time == 0)
                {
                    player.statMana += 9999;
                    CombatText.NewText(player.getRect(), Color.LightBlue, "Spell Slots Restored!");
                }
            }
            if(Main.time == 0)
            {
                channelDivinity = ProfBonus();
                MysticSteps = DailySteps;
                Main.NewText("Daily Features restored", 255, 215, 0);
            }

        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            DnDPlayer pc = Player.GetModPlayer<DnDPlayer>();
            for (int i = 0; i < ExtraAttack(); i++)
            {
                if (pc.barbClass == true && Main.rand.Next(1, 20) + pc.ProfBonus() >= 17 && proj.DamageType != DamageClass.Magic || pc.fighterClass == true && Main.rand.Next(1, 20) + pc.ProfBonus() >= 17 && proj.DamageType != DamageClass.Magic)
                {
                    damage *= 2;
                    CombatText.NewText(Main.LocalPlayer.getRect(), Color.Red, "Extra Attack!");
                }
            }
        }

        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            DnDPlayer pc = Player.GetModPlayer<DnDPlayer>();
            for (int i = 0; i < ExtraAttack(); i++)
            {
                if (pc.barbClass == true && Main.rand.Next(1, 20) + pc.ProfBonus() >= 17 && item.DamageType != DamageClass.Magic || pc.fighterClass == true && Main.rand.Next(1, 20) + pc.ProfBonus() >= 17 && item.DamageType != DamageClass.Magic)
                {
                    damage *= 2;
                    CombatText.NewText(Main.LocalPlayer.getRect(), Color.Red, "Extra Attack!");
                }
            }
        }

        public int ExtraAttack()
        {
            DnDPlayer pc = Player.GetModPlayer<DnDPlayer>();
            return pc.playerLevel switch
            {
                5 or 6 or 7 or 8 or 9 or 10 => 1,
                11 or 12 or 13 or 14 or 15 or 16 or 17 or 18 or 19 => 2,
                20 => 3,
                _ => 0,
            };
        }


        public override void PlayerConnect(Player player)
        {
            SyncLevelPacket.Write(player.whoAmI, playerLevel, true);
        }

        public override void PostUpdate()
        {
            switch (Main.netMode)
            {
                case 2:
                case 1 when Main.myPlayer != Player.whoAmI:
                    return;
            }
        }

        public override void OnEnterWorld(Player player)
        {
            DnD.PlayerEnteredWorld = true;
            Main.NewText("Your current level is " + playerLevel);
        }

        public override void Initialize()
        {
            playerLevel = 0;
            experiencePoints = 0;
            channelDivinity = ProfBonus();
            SpellSlots = false;
        }

        public override void LoadData(TagCompound tag)
        {
            playerLevel = tag.GetInt("playerLevel");
            experiencePoints = tag.GetInt("experience");
            channelDivinity = tag.GetInt("divinity");
            SpellSlots = tag.GetBool("spellSlots");
        }

        public override void SaveData(TagCompound tag)
        {
            tag["playerLevel"] = playerLevel;
            tag["experience"] = experiencePoints;
            tag["divinity"] = channelDivinity;
            tag["spellSlots"] = SpellSlots;
        }
    }
}
