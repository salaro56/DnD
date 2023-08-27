using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameInput;
using DnD.Common.Players;

namespace DnD.Items.Classes
{
    internal class RogueDagger : ModItem
    {
		public bool reliableTalent = false;

        public override void SetStaticDefaults()
        {
			// DisplayName.SetDefault("Rogue's Dagger");
            /* Tooltip.SetDefault("10% Increased Melee damage" +
                "\n10% Increased Ranged damage" +
                "\nRequired for Rogues"); */
        }

        public override void SetDefaults()
        {
            //Item Stats
            Item.rare = ItemRarityID.Quest;

            //Item Configs
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.maxStack = 1;
        }

		public override bool CanEquipAccessory(Player player, int slot, bool modded)
		{
			var accSlot = ModContent.GetInstance<ClassSlot>();
			var accSlot2 = ModContent.GetInstance<ClassSlot2>();

			return slot == accSlot.Type || slot == accSlot2.Type;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            pc.rogueClass = true;


            player.GetDamage(DamageClass.Ranged) += 0.1f;
            player.GetDamage(DamageClass.Melee) += 0.1f;

			if(pc.playerLevel >= 14 && pc.rogueClass == true)
            {
				player.AddBuff(BuffID.Hunter, 1);
            }

			if(pc.playerLevel >= 11)
            {
				reliableTalent = true;
            }
			else
            {
				reliableTalent = false;
            }

            CunningAction(pc, player);

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

        public static void CunningAction(DnDPlayer pc, Player player)
        {
            if(pc.playerLevel >= 2)
            {
				player.dashType = 1;
			}
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            DnDPlayer pc = Main.LocalPlayer.GetModPlayer<DnDPlayer>();

            TooltipLine line = new TooltipLine(Mod, "Features", "[c/51DA5F: Features:]");
            tooltips.Add(line);
            TooltipLine line2 = new TooltipLine(Mod, "Feat", "[c/51DA5F: Sneak Attack]");
			TooltipLine line2a = new TooltipLine(Mod, "Feat", "Lets you enter stealth by hitting the hotkey and deal extra damage");
            tooltips.Add(line2); tooltips.Add(line2a);
            if (pc.playerLevel >= 2)
            {
                TooltipLine line3 = new TooltipLine(Mod, "Feat", "[c/51DA5F: Cunning Action]");
				TooltipLine line3a = new TooltipLine(Mod, "Feat", "Lets you dash by double tapping in a direction");
                tooltips.Add(line3); tooltips.Add(line3a);
            }
            if (pc.playerLevel >= 5)
            {
                TooltipLine line4 = new TooltipLine(Mod, "Feat", "[c/51DA5F: Uncanny Dodge]");
				TooltipLine line4a = new TooltipLine(Mod, "Feat", "Occassionally dodge and attack to take half damage");
                tooltips.Add(line4); tooltips.Add(line4a);
            }
            if (pc.playerLevel >= 11)
            {
                TooltipLine line5 = new TooltipLine(Mod, "Feat", "[c/51DA5F: Reliable Talent]");
				TooltipLine line5a = new TooltipLine(Mod, "Feat", "Increases success rate of sneak");
                tooltips.Add(line5); tooltips.Add(line5a);
            }
            if (pc.playerLevel >= 14)
            {
                TooltipLine line7 = new TooltipLine(Mod, "Feat", "[c/51DA5F: Blindsense]");
				TooltipLine line7a = new TooltipLine(Mod, "Feat", "Shows you the location of enemies near you");
                tooltips.Add(line7); tooltips.Add(line7a);
            }
        }



		public override void AddRecipes()
		{
			CreateRecipe()
				.AddTile(ModContent.TileType<Furniture.PHBTile>())
				.Register();
		}
	}

	public class SneakBuff : ModBuff
    {
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Sneak");
			// Description.SetDefault("You are hidden in the shadows");
		}

		public override void Update(Player player, ref int buffIndex)
		{
			DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
			player.invis = true;
			pc.isSneaking = true;
		}
	}

	public class RogueUpdate : GlobalItem
    {
        public override void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
        {
            if(player.GetModPlayer<DnDPlayer>().isSneaking == true)
            {
				modifiers.FinalDamage += player.GetModPlayer<Rogue>().SneakAttackDamage(player.GetModPlayer<DnDPlayer>());
            }
        }
    }

	public class RogueUpdate2 : GlobalProjectile
    {
        public override void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
        {
			if (Main.player[Main.myPlayer].GetModPlayer<DnDPlayer>().isSneaking == true)
			{
				modifiers.FinalDamage += Main.player[Main.myPlayer].GetModPlayer<Rogue>().SneakAttackDamage(Main.player[Main.myPlayer].GetModPlayer<DnDPlayer>());
			}
		}
    }

	public class Rogue : ModPlayer
	{
		// These indicate what direction is what in the timer arrays used

		public const int DashRight = 1;
		public const int DashLeft = 2;

		public const int DashCooldown = 75; // Time (frames) between starting dashes. If this is shorter than DashDuration you can start a new dash before an old one has finished
		public const int DashDuration = 30; // Duration of the dash afterimage effect in frames

		// The initial velocity.  10 velocity is about 37.5 tiles/second or 50 mph
		public const float DashVelocity = 10f;

		// The direction the player has double tapped.  Defaults to -1 for no dash double tap
		public int DashDir = -1;

		// The fields related to the dash accessory
		public bool DashAccessoryEquipped;
		public int DashDelay = 0; // frames remaining till we can dash again
		public int DashTimer = 0; // frames remaining in the dash


		public int SneakAttackDamage(DnDPlayer pc)
		{
			int damage = 0;
			switch (pc.playerLevel)
			{
				case 1:
				case 2:
					damage += Main.rand.Next(1, 6);
					return damage;
				case 3:
				case 4:
					for (int i = 0; i < 2; i++)
					{
						damage += Main.rand.Next(1, 6);
					}
					return damage;
				case 5:
				case 6:
					for (int i = 0; i < 3; i++)
					{
						damage += Main.rand.Next(1, 6);
					}
					return damage;
				case 7:
				case 8:
					for (int i = 0; i < 4; i++)
					{
						damage += Main.rand.Next(1, 6);
					}
					return damage;
				case 9:
				case 10:
					for (int i = 0; i < 5; i++)
					{
						damage += Main.rand.Next(1, 6);
					}
					return damage;
				case 11:
				case 12:
					for (int i = 0; i < 6; i++)
					{
						damage += Main.rand.Next(1, 6);
					}
					return damage;
				case 13:
				case 14:
					for (int i = 0; i < 7; i++)
					{
						damage += Main.rand.Next(1, 6);
					}
					return damage;
				case 15:
				case 16:
					for (int i = 0; i < 8; i++)
					{
						damage += Main.rand.Next(1, 6);
					}
					return damage;
				case 17:
				case 18:
					for (int i = 0; i < 9; i++)
					{
						damage += Main.rand.Next(1, 6);
					}
					return damage;
				case 19:
				case 20:
					for (int i = 0; i < 10; i++)
					{
						damage += Main.rand.Next(1, 6);
					}
					return damage;
				default:
					damage += 1;
					return damage;
			}
		}

        public override void OnHitAnything(float x, float y, Entity victim)
        {
			Player.ClearBuff(ModContent.BuffType<SneakBuff>());
        }

        public override void ModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers)
        {
			DnDPlayer pc = Player.GetModPlayer<DnDPlayer>();
			if (pc.playerLevel >= 5 && pc.rogueClass == true && Main.rand.Next(1, 20) >= 16)
            {
				modifiers.FinalDamage *= (int)0.5f;
				CombatText.NewText(Player.getRect(), new Color(100, 20, 20), "Dodged");
            }
        }

        public override void ModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers)
        {
			DnDPlayer pc = Player.GetModPlayer<DnDPlayer>();
			if (pc.playerLevel >= 5 && pc.rogueClass == true && Main.rand.Next(1, 20) >= 16)
			{
				modifiers.FinalDamage *= (int)0.5f;
				CombatText.NewText(Player.getRect(), new Color(100, 20, 20), "Dodged");
			}
		}

        public override void ProcessTriggers(TriggersSet triggersSet)
		{
			RogueDagger rogue = ModContent.GetInstance<RogueDagger>();
			DnDPlayer pc = Player.GetModPlayer<DnDPlayer>();
			if (rogue.reliableTalent == true)
			{
				if (DnDSystem.SneakBuffKeybind.JustPressed && Main.rand.Next(10, 21) + pc.ProfBonus() >= 15 && pc.rogueClass == true && pc.isSneaking == false)
				{
					for (int i = 0; i < 20; i++)
					{
						Vector2 speed = Main.rand.NextVector2Circular(3f, 3f);
						Dust d = Dust.NewDustPerfect(Player.Center, DustID.Smoke, speed * 4, Scale: 2f);
						d.velocity *= 1f;
						d.noGravity = true;
					}
					if (!Main.gameMenu)
					{
						SoundEngine.PlaySound(SoundID.Item8, Player.position);
					}
					Player.AddBuff(ModContent.BuffType<SneakBuff>(), 6800);
				}
				else if (DnDSystem.SneakBuffKeybind.JustPressed && Main.rand.Next(10, 21) + pc.ProfBonus() < 15 && pc.rogueClass == true)
				{
					if (!Main.gameMenu)
					{
						SoundEngine.PlaySound(SoundID.Item16, Player.position);
					}
					CombatText.NewText(Player.getRect(), new Color(100, 100, 100), "Failed Stealth");
				}
			}
			else
			{
				if (DnDSystem.SneakBuffKeybind.JustPressed && Main.rand.Next(1, 20) + pc.ProfBonus() >= 15 && pc.rogueClass == true && pc.isSneaking == false)
				{
					for (int i = 0; i < 20; i++)
					{
						Vector2 speed = Main.rand.NextVector2Circular(3f, 3f);
						Dust d = Dust.NewDustPerfect(Player.Center, DustID.Smoke, speed * 4, Scale: 2f);
						d.velocity *= 1f;
						d.noGravity = true;
					}
					if (!Main.gameMenu)
					{
						SoundEngine.PlaySound(SoundID.Item8, Player.position);
					}
					Player.AddBuff(ModContent.BuffType<SneakBuff>(), 6800);
				}
				else if (DnDSystem.SneakBuffKeybind.JustPressed && Main.rand.Next(1, 20) + pc.ProfBonus() < 15 && pc.rogueClass == true)
				{
					if (!Main.gameMenu)
					{
						SoundEngine.PlaySound(SoundID.Item16, Player.position);
					}
					CombatText.NewText(Player.getRect(), new Color(100, 100, 100), "Failed Stealth");
				}
			}
		}

		public override void ResetEffects()
		{

			RogueDagger rogue = ModContent.GetInstance<RogueDagger>();
			rogue.reliableTalent = false;

			// Reset our equipped flag. If the accessory is equipped somewhere, ExampleShield.UpdateAccessory will be called and set the flag before PreUpdateMovement
			DashAccessoryEquipped = false;
		}
	}
}
