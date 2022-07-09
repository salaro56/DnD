using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.Localization;

namespace DnD.Items.Spells.Wizard_Spells.Lvl9
{
    internal class ChromaticDragon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chromatic Dragon");
            Tooltip.SetDefault(value: "[c/FF0000:Level 9:]" +
            "\nCommand a mighty dragon spirit to maul your foes");
            ItemID.Sets.ItemIconPulse[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.damage = 100;
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 0;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<Drag1>();
            Item.channel = true;
            Item.mana = 20;
            Item.UseSound = SoundID.Item4;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == Item.shoot || Main.projectile[i].type == ModContent.ProjectileType<Drag2>() || Main.projectile[i].type == ModContent.ProjectileType<Drag3>() || Main.projectile[i].type == ModContent.ProjectileType<Drag4>())
                {
                    Main.projectile[i].Kill();
                }
            }
            int p1 = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Drag1>(), damage, knockback, player.whoAmI);
            int p2 = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Drag2>(), damage, knockback, player.whoAmI, p1);
            int p3 = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Drag3>(), damage, knockback, player.whoAmI, p2);
            int p4 = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Drag2>(), damage, knockback, player.whoAmI, p3);
            int p5 = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Drag3>(), damage, knockback, player.whoAmI, p4);
            int p6 = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Drag2>(), damage, knockback, player.whoAmI, p5);
            int p7 = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Drag3>(), damage, knockback, player.whoAmI, p6);
            int p8 = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Drag4>(), damage, knockback, player.whoAmI, p7);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(ModContent.TileType<Furniture.DMGTile>())
                .AddIngredient(ItemID.FlaskofFire)
                .AddIngredient(ItemID.SoulofMight, 5)
                .AddIngredient(ModContent.ItemType<Items.ClassToken>())
                .AddCondition(NetworkText.FromLiteral("Must be of level 17 or higher"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().playerLevel >= 17)
                .AddCondition(NetworkText.FromLiteral("Must be the right class"), r => Main.LocalPlayer.GetModPlayer<DnDPlayer>().wizardClass == true)
                .Register();
        }
    }

    internal class Dragon : ModProjectile
    {
        public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.StardustDragon1;

        public Vector2 botPoint { get; set; }
        public Vector2 topPoint { get; set; }


        public int[] parts = { ModContent.ProjectileType<Drag1>(), ModContent.ProjectileType<Drag2>(), ModContent.ProjectileType<Drag3>(), ModContent.ProjectileType<Drag3>(), ModContent.ProjectileType<Drag4>() };

        public override void SetStaticDefaults() => DisplayName.SetDefault("Chromatic Dragon");

        public override void SetDefaults()
        {
            Projectile.friendly = true;
            Projectile.width = 12;
            Projectile.height = 28;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 20;
            Projectile.aiStyle = -1;
            Projectile.alpha = 255;

            botPoint = new Vector2(Projectile.Bottom.X, Projectile.Bottom.Y);
            topPoint = new Vector2(Projectile.Center.X, Projectile.Top.Y);
        }

        private Projectile FindDragonHead()
        {
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile projectile = Main.projectile[i];
                if(projectile.active && projectile.owner == Projectile.owner && projectile.type == parts[0])
                {
                    return projectile;
                }
            }
            return null;
        }

        public override void AI()
        {
            if (Main.player[Projectile.owner].dead || !Main.player[Projectile.owner].channel)
            {
                Projectile.Kill();
            }
            else
            {
                Projectile.timeLeft = 100;
            }

            Projectile.alpha -= 42;
            if (Projectile.alpha < 0)
            {
                Projectile.alpha = 0;
            }

            if (Projectile.type == parts[0])
            {
                Vector2 target = Main.MouseWorld;
                Vector2 dir = target - Projectile.Center;
                Vector2 vel = Vector2.Normalize(dir) * 15;
                
                Projectile.velocity = vel;
                if(Projectile.Distance(target) < 200)
                {
                    Projectile.velocity *= 0.6f;
                }
                if(Projectile.Distance(target) < 30)
                {
                    Projectile.velocity *= 0.01f;
                }

                Projectile.rotation = Projectile.velocity.ToRotation() + (float)Math.PI / 2f;
                int num24 = Projectile.direction;
                Projectile.direction = Projectile.spriteDirection = Projectile.velocity.X > 0f ? 1 : -1;
                if (num24 != Projectile.direction)
                {
                    Projectile.netUpdate = true;
                }
                float num12 = MathHelper.Clamp(Projectile.localAI[0], 0f, 50f);
                Projectile.position = Projectile.Center;
                Projectile.scale = 1f + num12 * 0.01f;
                Projectile.width = Projectile.height = (int)(10 * Projectile.scale);
                Projectile.Center = Projectile.position;
                if (Projectile.alpha > 0)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        int num13 = Dust.NewDust(new Vector2(Projectile.position.X, Projectile.position.Y), Projectile.width, Projectile.height, 135, 0f, 0f, 100, default, 2f);
                        Main.dust[num13].noGravity = true;
                        Main.dust[num13].noLight = true;
                    }
                }

                // fire breath
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    Vector2 path = npc.Center - Projectile.Center;
                    Vector2 vel2 = Vector2.Normalize(path) * 3;

                    if (Projectile.Distance(npc.Center) < 150 && !npc.friendly && Main.player[Projectile.owner].CanHit(npc) && npc.lifeMax > 0)
                    {
                        int flame = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel2, ProjectileID.Flames, (int)(Projectile.damage * 0.2f), Projectile.knockBack, Projectile.owner);
                        Main.projectile[flame].friendly = true;
                    }
                }
            }
            else
            {
                bool flag2 = false;
                Vector2 value = Vector2.Zero;
                _ = Vector2.Zero;
                float num14 = 0f;
                float scaleFactor2 = 0f;
                float scaleFactor3 = 1f;
                if (Projectile.ai[1] == 1f)
                {
                    Projectile.ai[1] = 0f;
                    Projectile.netUpdate = true;
                }
                int byUUID = Projectile.GetByUUID(Projectile.owner, (int)Projectile.ai[0]);
                if (Main.projectile.IndexInRange(byUUID))
                {
                    Projectile projectile = Main.projectile[byUUID];
                    if (projectile.active && projectile.owner == Main.myPlayer && (projectile.type == parts[0] || projectile.type == parts[1] || projectile.type == parts[2]))
                    {
                        flag2 = true;
                        value = projectile.Center;
                        _ = projectile.velocity;
                        num14 = projectile.rotation;
                        scaleFactor2 = 16f;
                        scaleFactor3 = MathHelper.Clamp(projectile.scale, 0f, 50f);
                        _ = projectile.alpha;
                        projectile.localAI[0] = Projectile.localAI[0] + 1f;
                        if (projectile.type != 625)
                        {
                            projectile.localAI[1] = Projectile.whoAmI;
                        }
                        if (Projectile.owner == Main.myPlayer && Projectile.type == parts[3] && projectile.type == parts[0])
                        {
                            projectile.Kill();
                            Projectile.Kill();
                            return;
                        }
                    }
                }
                if (!flag2)
                {
                    for (int k = 0; k < 1000; k++)
                    {
                        Projectile projectile2 = Main.projectile[k];
                        if (projectile2.active && projectile2.owner == Projectile.owner && ProjectileID.Sets.StardustDragon[projectile2.type] && projectile2.localAI[1] == Projectile.ai[0])
                        {
                            Projectile.ai[0] = projectile2.projUUID;
                            projectile2.localAI[1] = Projectile.whoAmI;
                            Projectile.netUpdate = true;
                        }
                    }
                    return;
                }
                Projectile.velocity = Vector2.Zero;
                Vector2 vector3 = value - Projectile.Center;
                if (num14 != Projectile.rotation)
                {
                    float num16 = MathHelper.WrapAngle(num14 - Projectile.rotation);
                    vector3 = vector3.RotatedBy(num16 * 0.1f);
                }
                Projectile.rotation = vector3.ToRotation() + (float)Math.PI / 2f;
                Projectile.position = Projectile.Center;
                Projectile.scale = scaleFactor3;
                Projectile.width = Projectile.height = (int)(10 * Projectile.scale);
                Projectile.Center = Projectile.position;
                if (vector3 != Vector2.Zero)
                {
                    Projectile.Center = value - Vector2.Normalize(vector3) * scaleFactor2 * scaleFactor3;
                }
                Projectile.spriteDirection = vector3.X > 0f ? 1 : -1;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.FlameBurst, Projectile.velocity.X, Projectile.velocity.Y, 0, default);
                Main.dust[d].noGravity = true;
                Main.dust[d].noLight = false;
                Main.dust[d].velocity *= 0.3f;
            }
        }
    }

    internal class Drag1 : Dragon
    {
        public override string Texture => "DnD/Items/Spells/Wizard_Spells/Lvl9/Drag1";

    }

    internal class Drag2 : Dragon
    {
        public override string Texture => "DnD/Items/Spells/Wizard_Spells/Lvl9/Drag2";

    }

    internal class Drag3 : Dragon
    {
        public override string Texture => "DnD/Items/Spells/Wizard_Spells/Lvl9/Drag3";

    }

    internal class Drag4 : Dragon
    {
        public override string Texture => "DnD/Items/Spells/Wizard_Spells/Lvl9/Drag4";
    }
}