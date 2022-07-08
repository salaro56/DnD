using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Localization;

namespace DnD.Items.Spells.ClericSpells.Lvl4
{
    internal class DeathWard : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault(value: "[c/FF0000:Level 4:]" +
                "\nYou touch a creature and grant it a measure of protection from death" +
                "\nThe first time a target would drop to 0 hit points, the target insteads drops to 50hp");

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
            Item.mana = 10;
            Item.noUseGraphic = true;
            Item.damage = 1;
            Item.noMelee = true;
            Item.shoot = ModContent.ProjectileType<WardProj>();
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
                .AddIngredient(ModContent.ItemType<ClassToken>())
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddCondition(NetworkText.FromLiteral("Must be of level 7 or higher"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().playerLevel >= 7)
                .AddCondition(NetworkText.FromLiteral("Must be of the Cleric Class"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().clericClass == true)
                .AddTile(ModContent.TileType<Furniture.MMTile>())
                .Register();
        }
    }

    internal class WardedDeath : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Death Ward");
            Description.SetDefault("Your life is shielded with divine magic");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (Main.rand.NextBool(5))
            {
                int dustnumber = Dust.NewDust(player.position, player.width, player.height, DustID.GoldFlame, 0f, 0f, 200, default(Color), 0.8f);
                Main.dust[dustnumber].velocity *= 0.3f;
            }
        }
    }

    internal class WardPlayer : ModPlayer
    {
        public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
        {
            Player player = Main.LocalPlayer;
            if (player.HasBuff(ModContent.BuffType<WardedDeath>()) == true && damage >= player.statLife)
            {
                CombatText.NewText(player.getRect(), Main.OurFavoriteColor, "Warded!", true);
                player.ClearBuff(ModContent.BuffType<WardedDeath>());
                player.statLife += 50;
                return false;
            }
            else return true;
        }
    }

    internal class WardProj : ModProjectile
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
                player.AddBuff(ModContent.BuffType<WardedDeath>(), 36000);
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
