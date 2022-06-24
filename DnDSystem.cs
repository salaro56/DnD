using DnD.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace DnD
{
    internal class DnDSystem : ModSystem
    {
		internal UserInterface experienceBarUI;
		internal ExperienceBar ExperienceBar;

		public override void UpdateUI(GameTime gameTime)
        {
            experienceBarUI?.Update(gameTime);
        }

		public override void Load()
		{
			if (!Main.dedServ)
			{
				ExperienceBar = new ExperienceBar();
				experienceBarUI = new UserInterface();
				experienceBarUI.SetState(ExperienceBar);
			}

			RageBuffKeybind = KeybindLoader.RegisterKeybind(Mod, "Rage buff", "P");
			SneakBuffKeybind = KeybindLoader.RegisterKeybind(Mod, "Sneak buff", "X");
		}

		public override void Unload()
		{
			// Not required if your AssemblyLoadContext is unloading properly, but nulling out static fields can help you figure out what's keeping it loaded.
			RageBuffKeybind = null;
			SneakBuffKeybind = null;
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
			if (resourceBarIndex != -1)
			{
				layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
					"DnD: Experience Bar",
					delegate {
						experienceBarUI.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}

		public static ModKeybind RageBuffKeybind { get; private set; }
		public static ModKeybind SneakBuffKeybind { get; private set; }
	}
}
