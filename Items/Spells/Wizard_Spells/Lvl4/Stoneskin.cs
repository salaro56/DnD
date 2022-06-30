using DnD.Furniture;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DnD.Items.Spells.Wizard_Spells.Lvl4
{
    internal class Stoneskin : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(value: "[c/FF0000:Level 4:]" +
                "\nThis spell turns your flesh as hard as stone. Until the spell ends you have resistance to nonmagical bludgeoning, piercing, and slashing damage");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.scale = 0.5f;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item4;
            Item.rare = ModContent.RarityType<Rarities.WizardRare>();
            Item.value = 0;
            Item.buffType = ModContent.BuffType<StoneBuff>();
            Item.buffTime = 6800;
            Item.mana = 12;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 10, 0, 0);
        }

        public override bool CanUseItem(Player player)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            if (pc.wizardClass == false || pc.isRaging == true)
            {
                return false;
            }
            else return true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(ModContent.TileType<MMTile>())
                .AddIngredient(ItemID.Diamond)
                .AddIngredient(ModContent.ItemType<Items.ClassToken>())
                .AddCondition(NetworkText.FromLiteral("Must be of level 7 or higher"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().playerLevel >= 7)
                .AddCondition(NetworkText.FromLiteral("Must be the right class"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().wizardClass == true)
                .Register();
        }
    }

    internal class StoneBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Stoneskin");
            Description.SetDefault("Your flesh is as hard as stone, you have resistance to all damage");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.endurance += 0.3f;
            if (Main.rand.NextBool(5))
            {
                int dustnumber = Dust.NewDust(player.position, player.width, player.height, DustID.Stone, 0f, 0f, 200, default(Color), 0.8f);
                Main.dust[dustnumber].velocity *= 0.3f;
            }
        }
    }
}
