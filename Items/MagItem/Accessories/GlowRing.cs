using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DnD.Items.MagItem.Accessories
{
    internal class GlowRing : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Glow Ring");
            /* Tooltip.SetDefault("[c/51DA5F:Uncommon Magic Item]" +
                "\nThe wearer of the ring shines with a brilliant light"); */
        }

        public override void SetDefaults()
        {
            //Item Stats
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.sellPrice(0, 5, 50, 0);

            //Item Configs
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.maxStack = 1;
        }

        public override void UpdateInventory(Player player)
        {
            Lighting.AddLight((int)(player.position.X + (float)(player.width / 2)) / 16, (int)(player.position.Y + (float)(player.height / 2)) / 16, 0.8f, 0.95f, 1f);
        }

        public override void UpdateEquip(Player player)
        {
            Lighting.AddLight((int)(player.position.X + (float)(player.width / 2)) / 16, (int)(player.position.Y + (float)(player.height / 2)) / 16, 0.8f, 0.95f, 1f);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<SmallGlowRing>())
                .AddIngredient(ItemID.HellstoneBar, 4)
                .AddTile(ModContent.TileType<Furniture.MMTile>())
                .Register();
        }
    }

    internal class SmallGlowRing : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Small Glow Ring");
            /* Tooltip.SetDefault("[c/51DA5F:Common Magic Item]" +
                "\nThe wearer of the ring shines with a dim light"); */
        }

        public override void SetDefaults()
        {
            //Item Stats
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 0, 25, 0);

            //Item Configs
            Item.width = 32;
            Item.height = 32;
            Item.accessory = true;
            Item.maxStack = 1;
        }

        public override void UpdateInventory(Player player)
        {
            Lighting.AddLight((int)(player.position.X + (float)(player.width / 2)) / 16, (int)(player.position.Y + (float)(player.height / 2)) / 16, 0.08f, 0.09f, .1f);
        }

        public override void UpdateEquip(Player player)
        {
            Lighting.AddLight((int)(player.position.X + (float)(player.width / 2)) / 16, (int)(player.position.Y + (float)(player.height / 2)) / 16, 0.08f, 0.09f, .1f);
        }
    }
}
