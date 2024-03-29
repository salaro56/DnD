﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;
using Terraria.ObjectData;
using Terraria.DataStructures;
using Terraria.Enums;
using Microsoft.Xna.Framework;

namespace DnD.Furniture
{
    internal class PHB : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Player's Handbook");
            // Tooltip.SetDefault("An incredibly powerful book with the tools to develop your character");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.createTile = ModContent.TileType<Furniture.PHBTile>();

            Item.width = 28;
            Item.height = 30;

            Item.useTurn = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 10;

            Item.maxStack = 99;
            Item.consumable = true;
            Item.value = Item.sellPrice(0, 0, 0, 50);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
            .AddIngredient(ItemID.ManaCrystal)
            .Register();
        }
    }

    public class PHBTile : ModTile
    {
            public override void SetStaticDefaults()
            {
                // Properties
                Main.tileTable[Type] = false;
                Main.tileSolidTop[Type] = false;
                Main.tileNoAttach[Type] = true;
                Main.tileLavaDeath[Type] = true;
                Main.tileFrameImportant[Type] = true;
                TileID.Sets.DisableSmartCursor[Type] = true;
                TileID.Sets.IgnoredByNpcStepUp[Type] = true; // This line makes NPCs not try to step up this tile during their movement. Only use this for furniture with solid tops.


                // Placement
                TileObjectData.newTile.CopyFrom(TileObjectData.Style2x1);
                TileObjectData.newTile.CoordinateHeights = new[] { 18 };
                TileObjectData.addTile(Type);

                AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);

                // Etc
                LocalizedText name = CreateMapEntryName();
                // name.SetDefault("Player's Handbook");
                AddMapEntry(new Color(200, 200, 200), name);
            }

            public override void NumDust(int x, int y, bool fail, ref int num)
            {
                num = fail ? 1 : 3;
            }

            public override void KillMultiTile(int x, int y, int frameX, int frameY)
            {
                Item.NewItem(new EntitySource_TileBreak(x, y), x * 16, y * 16, 32, 16, ModContent.ItemType<PHB>());
            }
    }
}
