using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Personalities;
using Terraria.DataStructures;
using System.Collections.Generic;
using ReLogic.Content;

namespace DnD.NPCS.Friendly
{
	// [AutoloadHead] and NPC.townNPC are extremely important and absolutely both necessary for any Town NPC to work at all.
	[AutoloadHead]
	public class DungeonMaster : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// DisplayName automatically assigned from localization files, but the commented line below is the normal approach.
			// DisplayName.SetDefault("Example Person");
			Main.npcFrameCount[Type] = 23; // The amount of frames the NPC has

			NPCID.Sets.ExtraFramesCount[Type] = 9; // Generally for Town NPCs, but this is how the NPC does extra things such as sitting in a chair and talking to other NPCs.
			NPCID.Sets.AttackFrameCount[Type] = 2;
			NPCID.Sets.DangerDetectRange[Type] = 700; // The amount of pixels away from the center of the npc that it tries to attack enemies.
			NPCID.Sets.AttackType[Type] = 0;
			NPCID.Sets.AttackTime[Type] = 90; // The amount of time it takes for the NPC's attack animation to be over once it starts.
			NPCID.Sets.AttackAverageChance[Type] = 30;
			NPCID.Sets.HatOffsetY[Type] = 4; // For when a party is active, the party hat spawns at a Y offset.

			// Influences how the NPC looks in the Bestiary
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Velocity = 1f, // Draws the NPC in the bestiary as if its walking +1 tiles in the x direction
				Direction = 1 // -1 is left and 1 is right. NPCs are drawn facing the left by default but ExamplePerson will be drawn facing the right
							  // Rotation = MathHelper.ToRadians(180) // You can also change the rotation of an NPC. Rotation is measured in radians
							  // If you want to see an example of manually modifying these when the NPC is drawn, see PreDraw
			};

			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);

			// Set Example Person's biome and neighbor preferences with the NPCHappiness hook. You can add happiness text and remarks with localization (See an example in ExampleMod/Localization/en-US.lang).
			// NOTE: The following code uses chaining - a style that works due to the fact that the SetXAffection methods return the same NPCHappiness instance they're called on.
			NPC.Happiness
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like) // Example Person prefers the forest.
				.SetBiomeAffection<SnowBiome>(AffectionLevel.Dislike) // Example Person dislikes the snow.
				.SetBiomeAffection<HallowBiome>(AffectionLevel.Love) // Example Person loves the Hallow Biome
				.SetNPCAffection(NPCID.DD2Bartender, AffectionLevel.Love) // Loves living near the tavernkeep.
				.SetNPCAffection(NPCID.Wizard, AffectionLevel.Like) // Likes living near the Wizard.
				.SetNPCAffection(NPCID.Merchant, AffectionLevel.Dislike) // Dislikes living near the merchant.
				.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate) // Hates living near the demolitionist.
			; // < Mind the semicolon!
		}

		public override void SetDefaults()
		{
			NPC.townNPC = true; // Sets NPC to be a Town NPC
			NPC.friendly = true; // NPC Will not attack player
			NPC.width = 40;
			NPC.height = 62;
			NPC.aiStyle = 7;
			NPC.damage = 10;
			NPC.defense = 15;
			NPC.lifeMax = 250;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0.5f;

			AnimationType = NPCID.Wizard;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				// Sets the preferred biomes of this town NPC listed in the bestiary.
				// With Town NPCs, you usually set this to what biome it likes the most in regards to NPC happiness.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				// Sets your NPC's flavor text in the bestiary.
				new FlavorTextBestiaryInfoElement("An all powerful entity that is here to help guide you through the world"),
			});
		}

		// The PreDraw hook is useful for drawing things before our sprite is drawn or running code before the sprite is drawn
		// Returning false will allow you to manually draw your NPC
		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			// This code slowly rotates the NPC in the bestiary
			// (simply checking NPC.IsABestiaryIconDummy and incrementing NPC.Rotation won't work here as it gets overridden by drawModifiers.Rotation each tick)
			if (NPCID.Sets.NPCBestiaryDrawOffset.TryGetValue(Type, out NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers))
			{
				drawModifiers.Rotation += 0.001f;

				// Replace the existing NPCBestiaryDrawModifiers with our new one with an adjusted rotation
				NPCID.Sets.NPCBestiaryDrawOffset.Remove(Type);
				NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
			}

			return true;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			int num = NPC.life > 0 ? 1 : 5;

			for (int k = 0; k < num; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GoldFlame);
			}
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{ // Requirements for the town NPC to spawn.
			for (int k = 0; k < 255; k++)
			{
				Player player = Main.player[k];
				if (!player.active)
				{
					continue;
				}

				// Player has to have either an ExampleItem or an ExampleBlock in order for the NPC to spawn
				if (player.inventory.Any(item => item.type == ModContent.ItemType<Items.ClassToken>() || item.type == ModContent.ItemType<Furniture.PHB>()))
				{
					return true;
				}
				else if (player.GetModPlayer<DnDPlayer>().playerLevel >= 1)
                {
					return true;
                }
			}

			return false;
		}

		public override ITownNPCProfile TownNPCProfile()
		{
			return new DungeonMasterProfile();
		}

		public override List<string> SetNPCNameList()
		{
			return new List<string>() {
				"Dungeon Master",
				"DM",
				"Salaro",
				"Matt",
				"Brennan"
			};
		}

		public override string GetChat()
		{
			WeightedRandom<string> chat = new WeightedRandom<string>();

			int wizard = NPC.FindFirstNPC(NPCID.Wizard);
			if (wizard >= 0 && Main.rand.NextBool(4))
			{
				chat.Add("Can you please tell " + Main.npc[wizard].GivenName + " to stop casting fireball to solve all his problems");
			}
			// These are things that the NPC has a chance of telling you when you talk to it.
			chat.Add("Have you crafted your Player's Handbook yet?");
			chat.Add("You can ask me anything about your classes");
			chat.Add("Don't forget to roll for initiative!");
			chat.Add("I am your Dungeon Master, your guide in the realms of Dungeons & Dragons", 5.0);
			chat.Add("No you can't seduce the dragon!", 0.1);
			return chat; // chat is implicitly cast to a string.
		}

		public override void SetChatButtons(ref string button, ref string button2)
		{ // What the chat buttons are when you open up the chat UI
			button = "Help";
		}

		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				int num = Main.rand.Next(8);
				switch(num)
                {
					case 0:
						Main.npcChatText = "You can craft a player's handbook by using a mana crystal";
						break;
					case 1:
						Main.npcChatText = "As a wizard or cleric you can craft spells using a class token";
						break;
					case 2:
						Main.npcChatText = "Make sure to set your keybinds in the controls to use your abilities";
						break;
					case 3:
						Main.npcChatText = "You can't rage? Make sure you aren't wearing any armour as a barbarian!";
						break;
					case 4:
						Main.npcChatText = "How do you get class tokens? Well you gotta level up silly!";
						break;
					case 5:
						Main.npcChatText = "Where do you get a class? You make it at the Player's Handbook";
						break;
					case 6:
						Main.npcChatText = "How do I get my mana back? You need to take a long rest, it will restore at the turn of the day cycle";
						break;
					case 7:
						Main.npcChatText = "You can increase your spell's level by right clicking while holding the spell if the spell allows it!";
						break;
				}
			}
		}	

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ItemID.CelestialSigil));
		}

		// Make this Town NPC teleport to the King and/or Queen statue when triggered.
		public override bool CanGoToStatue(bool toKingStatue) => true;

		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 20;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 30;
			randExtraCooldown = 30;
		}

		 //todo: implement
		 public override void TownNPCAttackProj(ref int projType, ref int attackDelay) {
		 	projType = ProjectileID.BallofFire;
		 	attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 12f;
			randomOffset = 2f;
		}
	}

	public class DungeonMasterProfile : ITownNPCProfile
	{
		public int RollVariation() => 0;
		public string GetNameForVariant(NPC npc) => npc.getNewNPCName();

		public Asset<Texture2D> GetTextureNPCShouldUse(NPC npc)
		{
			if (npc.IsABestiaryIconDummy && !npc.ForcePartyHatOn)
				return ModContent.Request<Texture2D>("DnD/NPCS/Friendly/DungeonMaster");

			if (npc.altTexture == 1)
				return ModContent.Request<Texture2D>("DnD/NPCS/Friendly/DungeonMaster");

			return ModContent.Request<Texture2D>("DnD/NPCS/Friendly/DungeonMaster");
		}

		public int GetHeadTextureIndex(NPC npc) => ModContent.GetModHeadSlot("DnD/NPCS/Friendly/DungeonMaster_Head");
	}
}