using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace DnD.Items.Spells.ClericSpells.Lvl2
{
    internal class PoisonProtection : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Protection from Poison");
            Tooltip.SetDefault("[c/FF0000:Level 2:]" +
                "\nYou touch a creature and neutralize poison" +
                "\nFor the duration of the spell they are also immune to future poisons");
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
            Item.rare = ModContent.RarityType<Rarities.ClericRare>();
            Item.value = 0;
            Item.mana = 12;
            Item.noUseGraphic = true;
            Item.value = Item.sellPrice(0, 10, 0, 0);
            Item.shoot = ModContent.ProjectileType<PoisonShield>();
        }

        public override void HoldItem(Player player)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            if (pc.clericClass == true)
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(5f, 5f);
                    Dust d = Dust.NewDustPerfect(new Vector2(player.Center.X + speed.X * 12, player.Center.Y + speed.Y * 12), DustID.GreenTorch, Vector2.Zero, Scale: 2f);
                    d.velocity *= 0.5f;
                    d.noGravity = true;
                    d.color = Color.Green;
                    d.noLight = true;
                    d.alpha = 200;
                }
            }
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
            else
            {
                if (pc.clericClass == false || pc.isRaging == true)
                {
                    return false;
                }
                else return true;
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            position = Main.MouseWorld;
            Projectile.NewProjectile(source, position.X, position.Y, velocity.X, velocity.Y, type, damage, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(ModContent.TileType<Furniture.MMTile>())
                .AddIngredient(ItemID.JungleSpores, 4)
                .AddIngredient(ModContent.ItemType<Items.ClassToken>())
                .AddCondition(NetworkText.FromLiteral("Must be of level 3 or higher"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().playerLevel >= 3)
                .AddCondition(NetworkText.FromLiteral("Must be the right class"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().clericClass == true)
                .Register();
        }
    }

    internal class PoisonBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Protection from Poison");
            Description.SetDefault("You are immune to the poison debuff");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffImmune[BuffID.Poisoned] = true;
        }
    }

    internal class PoisonShield : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.ShadowBeamFriendly;

        public override void SetDefaults()
        {
            //proj stats
            Projectile.penetrate = 1;

            //proj configs
            Projectile.width = 4;
            Projectile.height = 4;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.timeLeft = 15;
            Projectile.tileCollide = false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override void AI()
        {
            Player player = Main.LocalPlayer;
            int healAmount = Projectile.damage;

            if (Projectile.Hitbox.Intersects(player.Hitbox))
            {
                for (int i = 0; i < 20; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(3f, 1f);
                    Dust d = Dust.NewDustPerfect(new Vector2(player.Center.X, player.Center.Y + 35), DustID.GoldFlame, speed * 3, Scale: 2f);
                    d.velocity *= 0.5f;
                    d.noGravity = true;
                    d.color = Color.Green;
                }
                player.AddBuff(ModContent.BuffType<PoisonBuff>(), 6800);
                if(player.HasBuff(BuffID.Poisoned))
                {
                    player.ClearBuff(BuffID.Poisoned);
                }
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
                    Vector2 speed = Main.rand.NextVector2CircularEdge(0.5f, 0.5f);
                    Dust d = Dust.NewDustPerfect(new Vector2(position.X, position.Y), DustID.GoldFlame, speed * 2, Scale: 2f);
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
