using DnD.Common.Players;
using DnD.Items.Spells.ClericSpells.Lvl4;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace DnD.Items.Classes
{
    internal class RageSoul : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Rage Soul");
            /* Tooltip.SetDefault("Increases melee damage by 15%" +
                "\nIncreases max health" +
                "\nThe soul of a barbarians"); */

            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;

            ItemID.Sets.ItemIconPulse[Item.type] = true;
        }


        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            var accSlot = ModContent.GetInstance<ClassSlot>();
            var accSlot2 = ModContent.GetInstance<ClassSlot2>();

            return slot == accSlot.Type || slot == accSlot2.Type;
        }

        public override void SetDefaults()
        {
            //Item Stats
            Item.rare = ModContent.RarityType<Rarities.BarbRare>();

            //Item Configs
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.maxStack = 1;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            pc.barbClass = true;

            if (player.armor[0].type == ItemID.None && player.armor[1].type == ItemID.None && player.armor[2].type == ItemID.None)
            {
                Item.defense = 4 + pc.playerLevel + pc.ProfBonus();
            }

            player.GetDamage(DamageClass.Melee) += 0.15f;
            if(pc.playerLevel >= 5)
            {
                player.moveSpeed += 0.1f;
            }
            player.statLifeMax2 += 40;
            if(pc.playerLevel == 20)
            {
                player.statLifeMax2 += 60;
            }

            CanRage(player, pc);

            if (player.GetModPlayer<DnDPlayer>().SpellSlots == true)
            {
                player.manaRegen = 0;
                player.manaRegenCount = 0;
                player.manaRegenBonus = 0;

                if (Main.time == 0)
                {
                    player.statMana += 9999;
                }
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            DnDPlayer pc = Main.LocalPlayer.GetModPlayer<DnDPlayer>();

            TooltipLine line = new TooltipLine(Mod, "Features", "[c/51DA5F: Features:]");
            tooltips.Add(line);
            TooltipLine line1x = new TooltipLine(Mod, "Feat", "[c/51DA5F: Rage]");
            TooltipLine line1a = new TooltipLine(Mod, "Feat", "Press the rage button to enter a rage. Can't be wearing armour while raging");
            tooltips.Add(line1x);
            tooltips.Add(line1a);
            TooltipLine line2 = new TooltipLine(Mod, "Feat", "[c/51DA5F: Unarmoured defense]");
            TooltipLine line2a = new TooltipLine(Mod, "Feat", "Increases defense based on proficiency bonus and level as long as you aren't wearing armour");
            tooltips.Add(line2);
            tooltips.Add(line2a);
            if (pc.playerLevel >= 5)
            {
                TooltipLine line3 = new TooltipLine(Mod, "Feat", "[c/51DA5F: Extra Attack]");
                TooltipLine line3a = new TooltipLine(Mod, "Feat", "Occassionally strike again dealing more damage, gain more attacks at higher levels");
                TooltipLine line4 = new TooltipLine(Mod, "Feat", "[c/51DA5F: Fast Movement]");
                TooltipLine line4a = new TooltipLine(Mod, "Feat", "Increases movement speed");
                tooltips.Add(line3); tooltips.Add(line3a);
                tooltips.Add(line4); tooltips.Add(line4a);
            }
            if(pc.playerLevel >= 9)
            {
                TooltipLine line5 = new TooltipLine(Mod, "Feat", "[c/51DA5F: Brutal Critical]");
                TooltipLine line5a = new TooltipLine(Mod, "Feat", "Increases crit chance and damage");
                tooltips.Add(line5); tooltips.Add(line5a);
            }
            if(pc.playerLevel >= 11)
            {
                TooltipLine line6 = new TooltipLine(Mod, "Feat", "[c/51DA5F: Relentless Rage]");
                TooltipLine line6a = new TooltipLine(Mod, "Feat", "Can roll to avoid taking fatal damage, difficulty increases whenever you succeed");
                tooltips.Add(line6);
                tooltips.Add(line6a);
            }
            if(pc.playerLevel >= 20)
            {
                TooltipLine line7 = new TooltipLine(Mod, "Feat", "[c/51DA5F: Primal Champion]");
                TooltipLine line7a = new TooltipLine(Mod, "Feat", "Increases max health by 100");
                tooltips.Add(line7); tooltips.Add(line7a);
            }
        }


        public static void CanRage(Player player, DnDPlayer pc)
        {
            if(player.armor[0].type == ItemID.None && player.armor[1].type == ItemID.None && player.armor[2].type == ItemID.None)
            {
                pc.canRage = true;
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(ModContent.TileType<Furniture.PHBTile>())
                .Register();
        }
    }

    internal class RageBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Rage");
            // Description.SetDefault("You fight with primal ferocity");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            pc.isRaging = true;

            player.endurance += 0.5f;
            player.GetDamage(DamageClass.Melee) += 0.05f;
            if (Main.rand.NextBool(3))
            {
                for (int i = 0; i < 4; i++)
                {
                    int dustnumber = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, DustID.Blood, 0f, 0f, 200, default(Color), 1.5f);
                    Main.dust[dustnumber].velocity -= new Vector2(Main.rand.Next(-1, 1), 3f);
                    Main.dust[dustnumber].noGravity = true;
                    Main.dust[dustnumber].color = Color.Red;
                }
            }
        }
    }

    internal class RageButton : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            DnDPlayer pc = Player.GetModPlayer<DnDPlayer>();
            if(DnDSystem.RageBuffKeybind.JustPressed && pc.canRage == true && pc.isRaging == false)
            {
                for (int i = 0; i < 20; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(3f, 3f);
                    Dust d = Dust.NewDustPerfect(Player.Center, DustID.Blood, speed * 4, Scale: 2f);
                    d.velocity *= 1f;
                    d.noGravity = true;
                }
                if (!Main.gameMenu)
                {
                    SoundEngine.PlaySound(SoundID.Item34, Player.position);
                }
                Player.AddBuff(ModContent.BuffType<RageBuff>(), 6800);
            }
        }

        public override void ResetEffects()
        {
            if (Player.armor[0].type != ItemID.None || Player.armor[1].type != ItemID.None || Player.armor[2].type != ItemID.None)
            {
                Player.ClearBuff(ModContent.BuffType<RageBuff>());
            }
        }

        public override void ModifyWeaponCrit(Item item, ref float crit)
        {
            DnDPlayer pc = Player.GetModPlayer<DnDPlayer>();
            if(pc.playerLevel >= 9 && pc.barbClass == true)
            {
                item.crit += (int)0.05f;
            }
        }

        int dcCheck = 10;
        public override void OnRespawn()
        {
            DnDPlayer pc = Player.GetModPlayer<DnDPlayer>();
            dcCheck = 10;
            if(pc.barbClass == true && pc.playerLevel >= 11)
            {
                Main.NewText( "Your saving throw has reset to " + dcCheck, 100, 255, 100);
            }
        }

        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            DnDPlayer pc = Player.GetModPlayer<DnDPlayer>();
            int dc = dcCheck;

            if (pc.barbClass == true && pc.playerLevel >= 11 && Main.rand.Next(1,20) + pc.ProfBonus() >= dc && !Main.LocalPlayer.HasBuff(ModContent.BuffType<WardedDeath>()))
            {
                Player.statLife += 60;
                dcCheck += 5;
                if (!Main.gameMenu)
                {
                    SoundEngine.PlaySound(SoundID.Item14, Player.position);
                }
                CombatText.NewText(Player.getRect(), new Color(255, 0, 0), "Defiant");
                for (int i = 0; i < 20; i++)
                {
                    Vector2 speed = Main.rand.NextVector2Circular(3f, 3f);
                    Dust d = Dust.NewDustPerfect(Player.Center, DustID.Blood, speed * 4, Scale: 2f);
                    d.velocity *= 1f;
                    d.noGravity = true;
                }

                Main.NewText("Next save is " + dcCheck, 100, 255, 100);
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
