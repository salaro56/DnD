using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Utilities;
using Terraria.ModLoader.IO;
using System.IO;
using Terraria.DataStructures;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.ObjectModel;
using Terraria.Localization;
using DnD.Common.Structs;
using DnD.Common;

namespace DnD.Items.Spells.ClericSpells.Feats
{
    internal class TurnUndead : ModItem
    {

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Turn Undead");
            // Tooltip.SetDefault(value: "[c/FF0000:Channel Divinity:]");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.ArmorPenetration += 999;

            Item.shoot = ModContent.ProjectileType<UndeadCircle>();
            Item.DamageType = DamageClass.Magic;
            Item.useStyle = ItemUseStyleID.Swing;

            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.rare = ModContent.RarityType<Rarities.ClericRare>();
            Item.UseSound = SoundID.Item4;
            Item.width = 32;
            Item.height = 32;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            int index = tooltips.FindIndex(x => x.Name == "Damage");
            if (index == -1)
                return;
            tooltips.RemoveAt(index);

            DnDPlayer pc = Main.LocalPlayer.GetModPlayer<DnDPlayer>();

            if(pc.playerLevel >= 5)
            {
                TooltipLine itemName = new TooltipLine(Mod, "ItemName", "Destroy Undead");
                tooltips.Add(itemName);
            }
        }

        public override bool CanUseItem(Player player)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            if (pc.clericClass == true && pc.playerLevel >= 2 && pc.channelDivinity > 0)
            {
                pc.channelDivinity--;
                Main.NewText("Current channels left: " + pc.channelDivinity, 255, 215, 0);
                return true;
            }
            else return false;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(player.GetSource_FromThis(), player.Center, velocity, ModContent.ProjectileType<UndeadCircle>(), 1, knockback, player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddTile(ModContent.TileType<Furniture.PHBTile>())
                .AddCondition(Conditions.IsRightLevel(2))
                .AddCondition(Conditions.IsCleric)
                .Register();
        }
    }

    internal class UndeadCircle : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 200;
            Projectile.height = 200;
            Projectile.damage = 1;
            Projectile.friendly = true;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.scale = 2;

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 30;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            // return Color.White;
            return new Color(255, 255, 255, 0) * Projectile.Opacity;
        }

        public override void AI()
        {
            Projectile.ai[0] += 1f;

            Projectile.rotation += 0.05f * (float)Projectile.direction;

            if (++Projectile.frameCounter >= 2)
            {
                Projectile.frameCounter = 0;
                // Or more compactly Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];
                if (++Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 5;
            }

            if (Projectile.ai[0] >= 30f)
                Projectile.Kill();

            Projectile.direction = Projectile.spriteDirection = (Projectile.velocity.X > 0f) ? 1 : -1;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            // SpriteEffects helps to flip texture horizontally and vertically
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (Projectile.spriteDirection == -1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            // Getting texture of projectile
            Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

            // Calculating frameHeight and current Y pos dependence of frame
            // If texture without animation frameHeight is always texture.Height and startY is always 0
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            int startY = frameHeight * Projectile.frame;

            // Get this frame on texture
            Rectangle sourceRectangle = new Rectangle(0, startY, texture.Width, frameHeight);

            // Alternatively, you can skip defining frameHeight and startY and use this:
            // Rectangle sourceRectangle = texture.Frame(1, Main.projFrames[Projectile.type], frameY: Projectile.frame);

            Vector2 origin = sourceRectangle.Size() / 2f;

            // If sprite is vertical
            // float offsetY = 20f;
            // origin.Y = (float)(Projectile.spriteDirection == 1 ? sourceRectangle.Height - offsetY : offsetY);


            // Applying lighting and draw current frame
            Color drawColor = Projectile.GetAlpha(lightColor);
            Main.EntitySpriteDraw(texture,
                Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY),
                sourceRectangle, drawColor, Projectile.rotation, origin, Projectile.scale, spriteEffects, 0);

            // It's important to return false, otherwise we also draw the original texture.
            return false;
        }

        CreatureArrays ca = new();

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            DnDPlayer pc = Main.LocalPlayer.GetModPlayer<DnDPlayer>();
            int spellSave = 10 + pc.ProfBonus();

            if (ca.undeadNames.Any(target.FullName.Contains))
            {
                if (pc.playerLevel < 5)
                {
                    if (Main.rand.Next(1, 20) <= (spellSave))
                    {
                        target.AddBuff(BuffID.Confused, 3400);
                    }
                }
                else if (pc.playerLevel >= 5)
                {
                    if (Main.rand.Next(1, 20) <= spellSave)
                    {
                        modifiers.FinalDamage += 9999;
                    }
                }
            }
            else
            {
                modifiers.FinalDamage *= 0;
            }
        }
    }
}
