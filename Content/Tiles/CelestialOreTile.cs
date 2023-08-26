using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.WorldBuilding;
using Terraria.IO;

namespace DnD.Content.Tiles
{
    internal class CelestialOreTile : ModTile
    {
		public override void SetStaticDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
			Main.tileOreFinderPriority[Type] = 410; // Metal Detector value, see https://terraria.gamepedia.com/Metal_Detector
			Main.tileShine2[Type] = true; // Modifies the draw color slightly.
			Main.tileShine[Type] = 975; // How often tiny dust appear off this tile. Larger is less frequently
			Main.tileMergeDirt[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("CelestialOre");
			AddMapEntry(new Color(152, 30, 152), name);

			DustType = DustID.PurpleCrystalShard;
			ItemDrop = ModContent.ItemType<Items.Ore.CelestialOre>();
			HitSound = SoundID.Tink;
			MineResist = 2f;
			MinPick = 100;
		}
	}

	public class CelestialOreSystem : ModSystem
	{
		public override void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
		{
			// Because world generation is like layering several images ontop of each other, we need to do some steps between the original world generation steps.

			// The first step is an Ore. Most vanilla ores are generated in a step called "Shinies", so for maximum compatibility, we will also do this.
			// First, we find out which step "Shinies" is.
			int ShiniesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));

			if (ShiniesIndex != -1)
			{
				// Next, we insert our pass directly after the original "Shinies" pass.
				// ExampleOrePass is a class seen bellow
				tasks.Insert(ShiniesIndex + 1, new CelestialOrePass("DnD Ores", 237.4298f));
			}
		}
	}

	public class CelestialOrePass : GenPass
	{
		public CelestialOrePass(string name, float loadWeight) : base(name, loadWeight)
		{
		}

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "DnD Ores";

			for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 6E-03); k++)
			{
				int x = WorldGen.genRand.Next(0, Main.maxTilesX);

				int y = WorldGen.genRand.Next(0, (int)WorldGen.worldSurfaceLow);

				Tile tile = Framing.GetTileSafely(x, y);
				if (tile.HasTile && tile.TileType == TileID.Cloud)
				{
					WorldGen.TileRunner(x, y, WorldGen.genRand.Next(2, 4), WorldGen.genRand.Next(2, 6), ModContent.TileType<Content.Tiles.CelestialOreTile>());
				}
			}
		}
	}
}
