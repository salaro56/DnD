using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace DnD.Items.MagItem.Accessories
{
    internal class RingOfDangersense : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ring of Dangersense");
            Tooltip.SetDefault("[c/51DA5F:Common Magic Item]" +
                "\nThe wearer of the ring is magically aware of all dangers around them");
        }

        public override void SetDefaults()
        {
            //Item Stats
            Item.rare = ItemRarityID.Green;
            Item.value = Item.sellPrice(0, 0, 15, 0);

            //Item Configs
            Item.width = 30;
            Item.height = 30;
            Item.accessory = true;
            Item.maxStack = 1;
        }

        public override void UpdateEquip(Player player)
        {
            player.AddBuff(BuffID.Dangersense, 5);
        }
    }
}
