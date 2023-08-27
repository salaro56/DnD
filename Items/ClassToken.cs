using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Creative;

namespace DnD.Items
{
    internal class ClassToken : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Class Token");
            // Tooltip.SetDefault("Used to purchase or craft spells for you class");
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
        }

        public override void SetDefaults()
        {
            Item.width = 14;
            Item.height = 16;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Purple;
        }
    }
}
