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
using DnD.Common.Structs;

namespace DnD.Items.Pets
{
    internal class EternalToken : ModItem
    {
        PatreonNames names = new();

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Eternal Token");
            /* Tooltip.SetDefault("My gratitude for those who support me is eternal" +
                "\nOnly usable by patreon supporters!"); */

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override bool CanUseItem(Player player)
        {
            if(names.patreonNames.Contains(player.name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool CanRightClick()
        {
            if (names.patreonNames.Contains(Main.LocalPlayer.name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            if (names.patreonNames.Contains(player.name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.SkeletronPetItem);

            Item.shoot = ModContent.ProjectileType<D20>();
            Item.buffType = ModContent.BuffType<PatreonPet>();
        }

        public override void UseStyle(Player player, Rectangle heldItemFrame)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0 && names.patreonNames.Contains(player.name))
            {
                player.AddBuff(Item.buffType, 3600);
            }
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(TileID.WorkBenches)
                .Register();
        }
    }

    internal class D20 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Lucky Die");

            Main.projPet[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.SkeletronPet); // Copy the stats of the Zephyr Fish
            Projectile.scale = 0.75f;

            AIType = ProjectileID.ZephyrFish; // Copy the AI of the Zephyr Fish.
        }

        public override bool PreAI()
        {
            Player player = Main.player[Projectile.owner];

            player.zephyrfish = false; // Relic from aiType

            return true;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.spriteDirection = 1;

            // Keep the projectile from disappearing as long as the player isn't dead and has the pet buff.
            if (!player.dead && player.HasBuff(ModContent.BuffType<PatreonPet>()))
            {
                Projectile.timeLeft = 2;
            }
        }
    }

    internal class PatreonPet : ModBuff
    {
        PatreonNames names = new();

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Lucky D20");
            // Description.SetDefault("Roll true patrons");

            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        { // This method gets called every frame your buff is active on your player.
            player.buffTime[buffIndex] = 18000;

            int projType = ModContent.ProjectileType<D20>();

            // If the player is local, and there hasn't been a pet projectile spawned yet - spawn it.
            if (player.whoAmI == Main.myPlayer && player.ownedProjectileCounts[projType] <= 0 && names.patreonNames.Contains(player.name))
            {
                var entitySource = player.GetSource_Buff(buffIndex);

                Projectile.NewProjectile(entitySource, player.Center, Vector2.Zero, projType, 0, 0f, player.whoAmI);
            }
        }
    }
}
