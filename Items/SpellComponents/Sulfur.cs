using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.ItemDropRules;

namespace DnD.Items.SpellComponents
{
    internal class Sulfur : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sulfurous Compound");
            Tooltip.SetDefault("A mixture of nitric acid and sulfur");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(0, 0, 18, 0);
            Item.rare = ItemRarityID.Green;
            Item.maxStack = 999;
        }
    }

    internal class SulfurDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            int[] batType = { NPCID.CaveBat, NPCID.GiantBat, NPCID.IceBat, NPCID.IlluminantBat, NPCID.JungleBat, NPCID.SporeBat, NPCID.VampireBat, NPCID.Hellbat, NPCID.Lavabat };
                if(npc.type == batType[0] || npc.type == batType[1] || npc.type == batType[2] || npc.type == batType[3] || npc.type == batType[4] || npc.type == batType[5] || npc.type == batType[6] || npc.type == batType[7] || npc.type == batType[8])
                {
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Sulfur>(), 25));
                }
        }
    }
}
