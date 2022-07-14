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
        public override string Texture => "Terraria/Images/Item_" + ItemID.ShadowbeamStaff;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Chromatic Dragon");
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
            Item.damage = 70;
=======
            Item.damage = 140;
>>>>>>> Stashed changes
=======
            Item.damage = 140;
>>>>>>> Stashed changes
=======
            Item.damage = 140;
>>>>>>> Stashed changes
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 0;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<Drag1>();
            Item.channel = true;
            Item.mana = 20;
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
            int p6 = Projectile.NewProjectile(source, position, velocity, ModContent.ProjectileType<Drag4>(), damage, knockback, player.whoAmI, p5);
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
<<<<<<< Updated upstream
=======
            Projectile.alpha = 255;
            Projectile.DamageType = DamageClass.Magic;
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes

            botPoint = new Vector2(Projectile.Bottom.X, Projectile.Bottom.Y);
            topPoint = new Vector2(Projectile.Center.X, Projectile.Top.Y);
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

            if (Projectile.type == parts[0])
            {
                Vector2 target = Main.MouseWorld;
                Vector2 dir = target - Projectile.Center;
                (dir.X > 0f).ToDirectionInt();
                (dir.Y < 0f).ToDirectionInt();
                float scaleFactor = 0.4f;
                if (dir.Length() > 50)
                {
                    Projectile.velocity += Vector2.Normalize(dir) * scaleFactor * 1.5f;
                    if (Vector2.Dot(Projectile.velocity, dir) < 0.25f)
                    {
                        Projectile.velocity *= 0.8f;
                    }
                }
                float num22 = 30f;
                if (Projectile.velocity.Length() > num22)
                {
                    Projectile.velocity = Vector2.Normalize(Projectile.velocity) * num22;
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
                    Projectile.alpha -= 42;
                    if (Projectile.alpha < 0)
                    {
                        Projectile.alpha = 0;
                    }
                }

                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    Vector2 path = npc.Center - Projectile.Center;
                    Vector2 vel = Vector2.Normalize(path) * 3;

<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
                    if (Projectile.Distance(npc.Center) < 150 && !npc.friendly && Main.player[Projectile.owner].CanHit(npc))
=======
                    if (Projectile.Distance(npc.Center) < 150 && !npc.friendly && Main.player[Projectile.owner].CanHit(npc) && npc.lifeMax > 0 && npc.active)
>>>>>>> Stashed changes
=======
                    if (Projectile.Distance(npc.Center) < 150 && !npc.friendly && Main.player[Projectile.owner].CanHit(npc) && npc.lifeMax > 0 && npc.active)
>>>>>>> Stashed changes
=======
                    if (Projectile.Distance(npc.Center) < 150 && !npc.friendly && Main.player[Projectile.owner].CanHit(npc) && npc.lifeMax > 0 && npc.active)
>>>>>>> Stashed changes
                    {
                        int flame = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, vel, ProjectileID.Flames, (int)(Projectile.damage * 0.3f), Projectile.knockBack, Projectile.owner);
                        Main.projectile[flame].friendly = true;
                        Main.projectile[flame].DamageType = DamageClass.Magic;
                    }
                }
            }
            else
            {
                Vector2 value = Vector2.Zero;
                Vector2 _ = Vector2.Zero;
                float num14 = 0f;
                float scaleFactor2 = 0f;
                float scaleFactor3 = 1f;
                int byUUID = Projectile.GetByUUID(Projectile.owner, (int)Projectile.ai[0]);
                if (Main.projectile.IndexInRange(byUUID))
                {
                    Projectile projectile = Main.projectile[byUUID];
                    if (projectile.active && projectile.owner == Main.myPlayer && (projectile.type == parts[0] || projectile.type == parts[1] || projectile.type == parts[2]))
                    {
                        value = projectile.Center;
                        _ = projectile.velocity;
                        num14 = projectile.rotation;
                        scaleFactor2 = 16f;
                        scaleFactor3 = MathHelper.Clamp(projectile.scale, 0f, 50f);
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