using DnD.Packets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace DnD.Items.Spells.ClericSpells.Lvl2
{
    internal class WardingBond : ModItem
    {
        public int spellLevel = 1;
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(value: "[c/FF0000:Level 2:]" +
                "\nCreate a bond on one creature you can touch that you share a team with" +
                "\nUntil the spell ends the creature has resistance to all damage and you take the same amount of damage they take");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.useTurn = true;
            Item.UseSound = SoundID.Item4;
            Item.rare = ModContent.RarityType<Rarities.ClericRare>();
            Item.value = 0;
            Item.mana = 2;
            Item.noUseGraphic = true;
            Item.damage = 1;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<BondProj>();
        }

        public override bool CanUseItem(Player player)
        {
            Vector2 position1 = player.Center;
            Vector2 position2 = Main.MouseWorld;
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();

            if (Vector2.Distance(position1, position2) > 70)
            {
                return false;
            }
            else return true;
        }


        public override void HoldItem(Player player)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            if (pc.clericClass == true)
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(5f, 5f);
                    Dust d = Dust.NewDustPerfect(new Vector2(player.Center.X + speed.X * 12, player.Center.Y + speed.Y * 12), DustID.GoldFlame, Vector2.Zero, Scale: 2f);
                    d.velocity *= 0.5f;
                    d.noGravity = true;
                    d.color = Color.Green;
                    d.noLight = true;
                    d.alpha = 200;
                }
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            CasterPacket.Write(Main.myPlayer);
            position = Main.MouseWorld;
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ClassToken>())
                .AddCondition(NetworkText.FromLiteral("Must be of level 2 or higher"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().playerLevel >= 1)
                .AddCondition(NetworkText.FromLiteral("Must be of the Cleric Class"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().clericClass == true)
                .AddTile(ModContent.TileType<Furniture.PHBTile>())
                .Register();
        }
    }

    internal class BondPlayer : ModPlayer
    {
        public int caster;
    }

    internal class BondBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Warding Bond");
            Description.SetDefault("You are protected by a warding bond");
        } 

        public override void Update(Player player, ref int buffIndex)
        {
            BondPlayer pc = player.GetModPlayer<BondPlayer>();
            player.endurance += 0.5f;

            for (int i = 0; i < 255; i++)
            {
                if(i != Main.myPlayer && Main.player[i].active && Main.player[i] == Main.player[pc.caster] && i == pc.caster)
                {
                    if(Vector2.Distance(player.Center, Main.player[i].Center) < 800)
                    {
                        player.hasPaladinShield = true;
                        Main.LocalPlayer.AddBuff(43, 3400);
                    }
                }
            }

            if (Main.rand.NextBool(5))
            {
                int dustnumber = Dust.NewDust(player.position, player.width, player.height, DustID.GoldFlame, 0f, 0f, 200, default(Color), 0.8f);
                Main.dust[dustnumber].velocity *= 0.3f;
            }
        }
    }

    internal class BondProj : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ShadowBeamFriendly;

        public override void SetDefaults()
        {
            //proj stats
            Projectile.penetrate = 1;

            //proj configs
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 5;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Player player = Main.LocalPlayer;

            if (Projectile.Hitbox.Intersects(player.Hitbox) && player != Main.player[Projectile.owner])
            {
                player.AddBuff(ModContent.BuffType<BondBuff>(), 3400);
                Projectile.Kill();
            }
        }

        public override void Kill(int timeLeft)
        {
            if (Projectile.owner == Main.myPlayer)
            {
                Vector2 position = Main.MouseWorld;
                for (int i = 0; i < 20; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(2.5f, 1f);
                    Dust d = Dust.NewDustPerfect(new Vector2(position.X, position.Y), DustID.GoldFlame, speed * 4, Scale: 2f);
                    d.velocity *= 0.5f;
                    d.noGravity = true;
                }
                for (int i = 0; i < 20; i++)
                {
                    Vector2 speed = Main.rand.NextVector2Circular(2.5f, 1f);
                    Dust d = Dust.NewDustPerfect(new Vector2(position.X, position.Y), DustID.GoldFlame, speed * 4, Scale: 2f);
                    d.velocity *= 0.5f;
                    d.noGravity = true;
                }
            }
        }

        public override bool? CanCutTiles()
        {
            return false;
        }
    }
}
