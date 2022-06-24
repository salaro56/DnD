using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace DnD.Items.Weapons.Rogue
{
    internal class NightBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Night's Blade");
            Tooltip.SetDefault("Shrouded in shadows" +
                "\nWhen in rogue stealth allows you to misty step");
        }

        public override void SetDefaults()
        {
            //Item stats
            Item.useAnimation = 16;
            Item.useTime = 4;
            Item.reuseDelay = 18;

            Item.damage = 38;
            Item.DamageType = DamageClass.Melee;

            Item.rare = ItemRarityID.Quest;
            Item.value = Item.sellPrice(0, 5, 35, 0);

            Item.shoot = ModContent.ProjectileType<NightProj>();
            Item.shootSpeed = 2.1f;

            //Item configs
            Item.width = 34;
            Item.height = 34;
            Item.useStyle = ItemUseStyleID.Rapier;
            Item.autoReuse = false;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.useTurn = false;
            Item.UseSound = SoundID.Item1;
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override void HoldItem(Player player)
        {

            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            if (pc.isSneaking == true)
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(5f, 5f);
                    Dust d = Dust.NewDustPerfect(new Vector2(player.Center.X + speed.X * 75, player.Center.Y + speed.Y * 75), DustID.Smoke, Vector2.Zero, Scale: 2f);
                    d.velocity *= 0.5f;
                    d.noGravity = true;
                    d.noLight = true;
                    d.alpha = 200;
                }
            }

        }

        public override bool CanUseItem(Player player)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            if (pc.isSneaking == true)
            {
                if (player.altFunctionUse == 2)
                {
                    Item.shoot = ProjectileID.None;
                    MistySteps(player);
                    return true;
                }
                else
                {
                    Item.shoot = ModContent.ProjectileType<NightProj>();
                    return true;
                }
            }
            else
            {
                if (player.altFunctionUse == 2)
                {
                    return false;
                }
                else
                {
                    Item.shoot = ModContent.ProjectileType<NightProj>();
                    return true;
                }
            }
        }


        public void MistySteps(Player player)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            Vector2 position1 = player.Center;
            Vector2 position2 = Main.MouseWorld;

            Vector2 position3 = (position1 + position2) / 2;

            if (Vector2.Distance(position1, position2) < 400 && player.whoAmI == Main.myPlayer && pc.MysticSteps > 0)
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
                pc.MysticSteps--;
                CombatText.NewText(player.getRect(), new Color(200, 200, 200), $"Teleports Left: {pc.MysticSteps}");
            }
            else if (Vector2.Distance(position1, position2) > 400 && player.whoAmI == Main.myPlayer && pc.MysticSteps > 0)
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
                pc.MysticSteps--;
                CombatText.NewText(player.getRect(), new Color(200, 200, 200), $"Teleports Left: {pc.MysticSteps}");
            }
        }


        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<ClassToken>())
                .AddIngredient(ItemID.LightsBane)
                .AddIngredient(ItemID.Muramasa)
                .AddIngredient(ItemID.BladeofGrass)
                .AddIngredient(ItemID.FieryGreatsword)
                .AddTile(ModContent.TileType<Furniture.MMTile>())
                .Register();
        }
    }

    internal class NightProj : ModProjectile
    {
        // Shortsword projectiles are handled in a special way with how they draw and damage things
        // The "hitbox" itself is closer to the player, the sprite is centered on it
        // However the interactions with the world will occur offset from this hitbox, closer to the sword's tip (CutTiles, Colliding)
        // Values chosen mostly correspond to Iron Shortword
        public const int FadeInDuration = 7;
        public const int FadeOutDuration = 4;

        public const int TotalDuration = 16;

        // The "width" of the blade
        public float CollisionWidth => 10f * Projectile.scale;

        public int Timer
        {
            get => (int)Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Night Blade");
        }

        public override void SetDefaults()
        {
            Projectile.Size = new Vector2(18); // This sets width and height to the same value (important when projectiles can rotate)
            Projectile.aiStyle = -1; // Use our own AI to customize how it behaves, if you don't want that, keep this at ProjAIStyleID.ShortSword. You would still need to use the code in SetVisualOffsets() though
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 1f;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.ownerHitCheck = true; // Prevents hits through tiles. Most melee weapons that use projectiles have this
            Projectile.extraUpdates = 1; // Update 1+extraUpdates times per tick
            Projectile.timeLeft = 360; // This value does not matter since we manually kill it earlier, it just has to be higher than the duration we use in AI
            Projectile.hide = true; // Important when used alongside player.heldProj. "Hidden" projectiles have special draw conditions
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Timer += 1;
            if (Timer >= TotalDuration)
            {
                // Kill the projectile if it reaches it's intented lifetime
                Projectile.Kill();
                return;
            }
            else
            {
                // Important so that the sprite draws "in" the player's hand and not fully infront or behind the player
                player.heldProj = Projectile.whoAmI;
            }

            // Fade in and out
            // GetLerpValue returns a value between 0f and 1f - if clamped is true - representing how far Timer got along the "distance" defined by the first two parameters
            // The first call handles the fade in, the second one the fade out.
            // Notice the second call's parameters are swapped, this means the result will be reverted
            Projectile.Opacity = Utils.GetLerpValue(0f, FadeInDuration, Timer, clamped: true) * Utils.GetLerpValue(TotalDuration, TotalDuration - FadeOutDuration, Timer, clamped: true);

            // Keep locked onto the player, but extend further based on the given velocity (Requires ShouldUpdatePosition returning false to work)
            Vector2 playerCenter = player.RotatedRelativePoint(player.MountedCenter, reverseRotation: false, addGfxOffY: false);
            Projectile.Center = playerCenter + Projectile.velocity * (Timer - 1f);

            // Set spriteDirection based on moving left or right. Left -1, right 1
            Projectile.spriteDirection = (Vector2.Dot(Projectile.velocity, Vector2.UnitX) >= 0f).ToDirectionInt();

            // Point towards where it is moving, applied offset for top right of the sprite respecting spriteDirection
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2 - MathHelper.PiOver4 * Projectile.spriteDirection;

            // The code in this method is important to align the sprite with the hitbox how we want it to
            SetVisualOffsets();
        }

        private void SetVisualOffsets()
        {
            // 32 is the sprite size (here both width and height equal)
            const int HalfSpriteWidth = 34 / 2;
            const int HalfSpriteHeight = 34 / 2;

            int HalfProjWidth = Projectile.width / 2;
            int HalfProjHeight = Projectile.height / 2;

            // Vanilla configuration for "hitbox in middle of sprite"
            DrawOriginOffsetX = 0;
            DrawOffsetX = -(HalfSpriteWidth - HalfProjWidth);
            DrawOriginOffsetY = -(HalfSpriteHeight - HalfProjHeight);

            // Vanilla configuration for "hitbox towards the end"
            //if (Projectile.spriteDirection == 1) {
            //	DrawOriginOffsetX = -(HalfProjWidth - HalfSpriteWidth);
            //	DrawOffsetX = (int)-DrawOriginOffsetX * 2;
            //	DrawOriginOffsetY = 0;
            //}
            //else {
            //	DrawOriginOffsetX = (HalfProjWidth - HalfSpriteWidth);
            //	DrawOffsetX = 0;
            //	DrawOriginOffsetY = 0;
            //}
        }

        public override bool ShouldUpdatePosition()
        {
            // Update Projectile.Center manually
            return false;
        }

        public override void CutTiles()
        {
            // "cutting tiles" refers to breaking pots, grass, queen bee larva, etc.
            DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
            Vector2 start = Projectile.Center;
            Vector2 end = start + Projectile.velocity.SafeNormalize(-Vector2.UnitY) * 10f;
            Utils.PlotTileLine(start, end, CollisionWidth, DelegateMethods.CutTiles);
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            // "Hit anything between the player and the tip of the sword"
            // shootSpeed is 2.1f for reference, so this is basically plotting 12 pixels ahead from the center
            Vector2 start = Projectile.Center;
            Vector2 end = start + Projectile.velocity * 6f;
            float collisionPoint = 0f; // Don't need that variable, but required as parameter
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, CollisionWidth, ref collisionPoint);
        }
    }
}
