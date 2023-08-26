using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace DnD.Items.Craftables
{
    internal class CelestialBar : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Celestial Bar");
            Tooltip.SetDefault("Stars shimmer in pearlescent metal");
        }

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.maxStack = 999;
            Item.value = Item.sellPrice(0, 0, 80, 0);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Ore.CelestialOre>(), 3)
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }
}
