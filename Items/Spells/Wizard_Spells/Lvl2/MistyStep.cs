using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.GameContent.Creative;

namespace DnD.Items.Spells.Wizard_Spells.Lvl2
{
    internal class MistyStep : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Misty Step");
            Tooltip.SetDefault(value: "[c/FF0000:Level 2:]" +
                "\nEnvelopes yourself in silvery blue light as you teleport up to 30ft away");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void HoldItem(Player player)
        {

            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            if (pc.wizardClass == true)
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(5f, 5f);
                    Dust d = Dust.NewDustPerfect(new Vector2(player.Center.X + speed.X * 75, player.Center.Y + speed.Y * 75), DustID.ApprenticeStorm, Vector2.Zero, Scale: 2f);
                    d.velocity *= 0.5f;
                    d.noGravity = true;
                    d.color = Color.Green;
                    d.noLight = true;
                    d.alpha = 200;
                }
            }

        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.UseSound = SoundID.Item4;
            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.reuseDelay = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.rare = ModContent.RarityType<Rarities.WizardRare>();
            Item.mana = 30;
        }

        public override bool CanUseItem(Player player)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            Vector2 position1 = player.Center;
            Vector2 position2 = Main.MouseWorld;


            if (pc.wizardClass == true && pc.isRaging == false && player.whoAmI == Main.myPlayer && !Collision.SolidCollision(position2, player.width, player.height))
            {
                MistySteps(player);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void MistySteps(Player player)
        {
            Vector2 position1 = player.Center;
            Vector2 position2 = Main.MouseWorld;

            Vector2 position3 = (position1 + position2) / 2;

            if (Vector2.Distance(position1, position2) < 400 && player.whoAmI == Main.myPlayer && player.statMana >= 30)
            {
                player.LimitPointToPlayerReachableArea(ref position2);
                if (!(position2.X > 50f) || !(position2.X < (float)(Main.maxTilesX * 16 - 50)) || !(position2.Y > 50f) || !(position2.Y < (float)(Main.maxTilesY * 16 - 50)))
                {
                    return;
                }
                int num = (int)(position2.X / 16f);
                int num2 = (int)(position2.Y / 16f);
                if ((Main.tile[num, num2].WallType == 87 && (double)num2 > Main.worldSurface && !NPC.downedPlantBoss) || Collision.SolidCollision(position2, player.width, player.height))
                {
                    return;
                }
                player.Teleport(position2, 1);
                NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, player.whoAmI, position2.X, position2.Y, 1);
            }
            else if (Vector2.Distance(position1, position2) > 400 && player.whoAmI == Main.myPlayer && player.statMana >= 30)
            {
                player.LimitPointToPlayerReachableArea(ref position3);
                if (!(position3.X > 50f) || !(position3.X < (float)(Main.maxTilesX * 16 - 50)) || !(position3.Y > 50f) || !(position3.Y < (float)(Main.maxTilesY * 16 - 50)))
                {
                    return;
                }
                int num = (int)(position3.X / 16f);
                int num2 = (int)(position3.Y / 16f);
                if ((Main.tile[num, num2].WallType == 87 && (double)num2 > Main.worldSurface && !NPC.downedPlantBoss) || Collision.SolidCollision(position3, player.width, player.height))
                {
                    return;
                }
                player.Teleport(position3, 1);
                NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, player.whoAmI, position3.X, position3.Y, 1);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<Items.ClassToken>())
                .AddTile(ModContent.TileType<Furniture.PHBTile>())
                .AddCondition(NetworkText.FromLiteral("Must be of level 2 or higher"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().playerLevel >= 2)
                .AddCondition(NetworkText.FromLiteral("Must be the right class"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().wizardClass == true)
                .Register();
        }
    }
}
