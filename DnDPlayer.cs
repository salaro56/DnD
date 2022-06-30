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
            switch (playerLevel)
            {
                case 1:
                case 2:
                    return 1;
                    
                case 3:
                case 4:
                    return 2;
                case 5:
                case 6:
                    return 3;
                case 7:
                case 8:
                    return 4;
                case 9:
                case 10:
                    return 5;
                case 11:
                case 12:
                    return 6;
                case 13:
                case 14:
                    return 7;
                case 15:
                case 16:
                    return 8;
                case 17:
                case 18:
                case 19:
                case 20:
                    return 9;

                default:
                    return 0;
            }
        }

        public int ProfBonus()
        {
            switch(playerLevel)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                    return 2;
                case 5:
                case 6:
                case 7:
                case 8:
                    return 3;
                case 9:
                case 10:
                case 11:
                case 12:
                    return 4;
                case 13:
                case 14:
                case 15:
                case 16:
                    return 5;
                case 17:
                case 18:
                case 19:
                case 20:
                    return 6;
                default:
                    return 2;
            }
        }

        public int XPToLevel()
        {
            switch(playerLevel)
            {
                case 1:
                    return 300;
                case 2:
                    return 900;
                case 3:
                    return 2700;
                case 4:
                    return 6500;
                case 5:
                    return 14000;
                case 6:
                    return 23000;
                case 7:
                    return 34000;
                case 8:
                    return 48000;
                case 9:
                    return 64000;
                case 10:
                    return 85000;
                case 11:
                    return 100000;
                case 12:
                    return 120000;
                case 13:
                    return 140000;
                case 14:
                    return 165000;
                case 15:
                    return 195000;
                case 16:
                    return 225000;
                case 17:
                    return 265000;
                case 18:
                    return 305000;
                case 19:
                    return 355000;
                case 20:
                    return 0;

                default:
                    return 0;
            }
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
