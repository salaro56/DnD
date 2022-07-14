using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using DnD.Rarities;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Enums;
using Terraria.Audio;

namespace DnD.Items.Spells.Wizard_Spells.Lvl3
{
    internal class IceSpike : ModItem
    {
		public int minLevel = 3;
		public int spellLevel;

		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Spikes");
            Tooltip.SetDefault("[c/FF0000:Level 3:]" +
                "\nSpikes of ice sprout from the ground impailing your enemies" +
                "\nDoes 4d8 damage and increases by 1d8 for each level above 3rd");
        }

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override void SetDefaults()
        {
            Item.mana = 5;
            Item.damage = 1;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.shootSpeed = 32;
            Item.shoot = ModContent.ProjectileType<Spike>();
            Item.width = 32;
            Item.height = 32;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.rare = ModContent.RarityType<WizardRare>();
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.knockBack = 1;
            Item.value = Item.sellPrice(0, 0, 32, 0);
            Item.DamageType = DamageClass.Magic;
            Item.autoReuse = true;

			spellLevel = minLevel;
		}

		public override bool CanUseItem(Player player)
		{
			DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
			if (pc.wizardClass == false || pc.isRaging == true)
			{
				return false;
			}
			else
			{
				if (player.altFunctionUse == 2)
				{
					if (spellLevel < pc.SpellSlot())
					{
						Item.shoot = ProjectileID.None;
						Item.UseSound = SoundID.Item4;
						Item.reuseDelay = 5;
						Item.mana = 0;
						spellLevel++;
						Main.NewText("Current spell level is " + spellLevel, 152, 104, 126);
					}
					else
					{
						Item.shoot = ProjectileID.None;
						Item.UseSound = SoundID.Item4;
						Item.reuseDelay = 5;
						Item.mana = 0;
						spellLevel = minLevel;
						Main.NewText("Current spell level is " + spellLevel, 152, 104, 126);
					}
				}
				else
				{

					if (spellLevel == 3)
					{
						Item.shoot = ModContent.ProjectileType<Spike>();
						Item.mana = 5 + pc.ProfBonus();
						Item.UseSound = SoundID.Item4;
					}
					else if (spellLevel == 4)
					{
						Item.shoot = ModContent.ProjectileType<Spike>();
						Item.mana = 6 + pc.ProfBonus();
						Item.UseSound = SoundID.Item4;
					}
					else if (spellLevel == 5)
					{
						Item.shoot = ModContent.ProjectileType<Spike>();
						Item.mana = 7 + pc.ProfBonus();
						Item.UseSound = SoundID.Item4;
					}
					else if (spellLevel == 6)
					{
						Item.shoot = ModContent.ProjectileType<Spike>();
						Item.mana = 9 + pc.ProfBonus();
						Item.UseSound = SoundID.Item4;
					}
					else if (spellLevel == 7)
					{
						Item.shoot = ModContent.ProjectileType<Spike>();
						Item.mana = 10 + pc.ProfBonus();
						Item.UseSound = SoundID.Item4;
					}
					else if (spellLevel == 8)
					{
						Item.shoot = ModContent.ProjectileType<Spike>();
						Item.mana = 11 + pc.ProfBonus();
						Item.UseSound = SoundID.Item4;
					}
					else if (spellLevel == 9)
					{
						Item.shoot = ModContent.ProjectileType<Spike>();
						Item.mana = 13 + pc.ProfBonus();
						Item.UseSound = SoundID.Item4;
					}
				}
				return true;
			}
		}

		public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
		{
			DnDPlayer pc = player.GetModPlayer<DnDPlayer>();

			DnDItem sItem = ModContent.GetInstance<DnDItem>();

			damage += sItem.DamageValue(minRoll: 1, maxRoll: 8, diceRolled: spellLevel + 1);
		}


		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 pointPoisition2 = Main.MouseWorld;
            //player.LimitPointToPlayerReachableArea(ref pointPoisition2);
            Vector2 vector15 = pointPoisition2 + Main.rand.NextVector2Circular(8f, 8f);
            Vector2 value25 = FindSpikeSpot(vector15).ToWorldCoordinates(Main.rand.Next(17), Main.rand.Next(17));
            Vector2 vector16 = (vector15 - value25).SafeNormalize(-Vector2.UnitY) * 16f;
            Projectile.NewProjectile(source, value25.X, value25.Y, vector16.X, vector16.Y, type, damage, knockback, player.whoAmI, 0f, Main.rand.NextFloat() * 0.5f + 0.5f);
            return false;
        }

        private Point FindSpikeSpot(Vector2 targetSpot)
        {
			Player player = Main.player[Main.myPlayer];
			Point point = targetSpot.ToTileCoordinates();
			Vector2 center = player.Center;
			Vector2 endPoint = targetSpot;
			int samplesToTake = 3;
			float samplingWidth = 4f;
			Collision.AimingLaserScan(center, endPoint, samplingWidth, samplesToTake, out var vectorTowardsTarget, out var samples);
			float num = float.PositiveInfinity;
			for (int i = 0; i < samples.Length; i++)
			{
				if (samples[i] < num)
				{
					num = samples[i];
				}
			}
			targetSpot = center + vectorTowardsTarget.SafeNormalize(Vector2.Zero) * num;
			point = targetSpot.ToTileCoordinates();
			Rectangle value = new Rectangle(point.X, point.Y, 1, 1);
			value.Inflate(6, 16);
			Rectangle value2 = new Rectangle(0, 0, Main.maxTilesX, Main.maxTilesY);
			value2.Inflate(-40, -40);
			value = Rectangle.Intersect(value, value2);
			List<Point> list = new List<Point>();
			List<Point> list2 = new List<Point>();
			for (int j = value.Left; j <= value.Right; j++)
			{
				for (int k = value.Top; k <= value.Bottom; k++)
				{
					if (!WorldGen.SolidTile(j, k))
					{
						continue;
					}
					Vector2 value3 = new Vector2(j * 16 + 8, k * 16 + 8);
					if (!(Vector2.Distance(targetSpot, value3) > 200f))
					{
						if (FindSpikeOpening(j, k, j > point.X, j < point.X, k > point.Y, k < point.Y))
						{
							list.Add(new Point(j, k));
						}
						else
						{
							list2.Add(new Point(j, k));
						}
					}
				}
			}
			if (list.Count == 0 && list2.Count == 0)
			{
				list.Add((player.Center.ToTileCoordinates().ToVector2() + Main.rand.NextVector2Square(-2f, 2f)).ToPoint());
			}
			List<Point> list3 = list;
			if (list3.Count == 0)
			{
				list3 = list2;
			}
			int index = Main.rand.Next(list3.Count);
			return list3[index];
		}

		private bool FindSpikeOpening(int x, int y, bool acceptLeft, bool acceptRight, bool acceptUp, bool acceptDown)
        {
			if (acceptLeft && !WorldGen.SolidTile(x - 1, y))
			{
				return true;
			}
			if (acceptRight && !WorldGen.SolidTile(x + 1, y))
			{
				return true;
			}
			if (acceptUp && !WorldGen.SolidTile(x, y - 1))
			{
				return true;
			}
			if (acceptDown && !WorldGen.SolidTile(x, y + 1))
			{
				return true;
			}
			return false;
		}

    }

    internal class Spike : ModProjectile
    {
        public override void SetDefaults()
        {
			Projectile.width = 32;
			Projectile.height = 32;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.alpha = 255;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.penetrate = 3;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 25;
		}

        public override void AI()
        {
			int num = 16;
			float scaleFactor = 1f;
			int num2 = 10;
			int num3 = 10;
			int num4 = 2;
			int num5 = 2;
			int num6 = 20;
			int num7 = 20;
			int num8 = 30;
			int maxValue = 6;
			bool flag = Projectile.ai[0] < (float)num6;
			bool flag2 = Projectile.ai[0] >= (float)num7;
			bool flag3 = Projectile.ai[0] >= (float)num8;
			Projectile.ai[0] += 1f;
			if (Projectile.localAI[0] == 0f)
			{
				Projectile.localAI[0] = 1f;
				Projectile.rotation = Projectile.velocity.ToRotation();
				for (int i = 0; i < num2; i++)
				{
					Dust dust = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(24f, 24f), num, Projectile.velocity * scaleFactor * MathHelper.Lerp(0.2f, 0.7f, Main.rand.NextFloat()));
					dust.velocity += Main.rand.NextVector2Circular(0.5f, 0.5f);
					dust.scale = 0.8f + Main.rand.NextFloat() * 0.5f;
				}
				for (int j = 0; j < num3; j++)
				{
					Dust dust2 = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(24f, 24f), num, Main.rand.NextVector2Circular(2f, 2f) + Projectile.velocity * scaleFactor * MathHelper.Lerp(0.2f, 0.5f, Main.rand.NextFloat()));
					dust2.velocity += Main.rand.NextVector2Circular(0.5f, 0.5f);
					dust2.scale = 0.8f + Main.rand.NextFloat() * 0.5f;
					dust2.fadeIn = 1f;
				}
				SoundEngine.PlaySound(SoundID.DeerclopsIceAttack, Projectile.Center);
			}
			if (flag)
			{
				Projectile.Opacity += 0.1f;
				Projectile.scale = Projectile.Opacity * Projectile.ai[1];
				for (int k = 0; k < num4; k++)
				{
					Dust dust3 = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(16f, 16f), num, Projectile.velocity * scaleFactor * MathHelper.Lerp(0.2f, 0.5f, Main.rand.NextFloat()));
					dust3.velocity += Main.rand.NextVector2Circular(0.5f, 0.5f);
					dust3.velocity *= 0.5f;
					dust3.scale = 0.8f + Main.rand.NextFloat() * 0.5f;
				}
			}
			if (flag2)
			{
				Projectile.Opacity -= 0.2f;
				for (int l = 0; l < num5; l++)
				{
					Dust dust4 = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(16f, 16f), num, Projectile.velocity * scaleFactor * MathHelper.Lerp(0.2f, 0.5f, Main.rand.NextFloat()));
					dust4.velocity += Main.rand.NextVector2Circular(0.5f, 0.5f);
					dust4.velocity *= 0.5f;
					dust4.scale = 0.8f + Main.rand.NextFloat() * 0.5f;
				}
			}
			if (flag3)
			{
				Projectile.Kill();
			}
			Lighting.AddLight(Projectile.Center, new Vector3(0.1f, 0.1f, 0.5f) * Projectile.scale);
			Projectile.velocity *= 0.2f;
		}

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
			float collisionPoint7 = 0f;
			if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + Projectile.velocity.SafeNormalize(-Vector2.UnitY) * 200f * Projectile.scale, 22f * Projectile.scale, ref collisionPoint7))
			{
				return true;
			}
			return false;
		}

        public override bool PreDraw(ref Color lightColor)
        {
			SpriteEffects spriteEffects = SpriteEffects.None;
			Color color31 = Color.LightBlue;
			Texture2D value242 = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle value181 = value242.Frame(1, 1, 0);
			Vector2 origin11 = new Vector2(16f, value181.Height / 2);
			Color alpha14 = Projectile.GetAlpha(color31);
			Vector2 scale31 = new Vector2(Projectile.scale);
			float lerpValue4 = Utils.GetLerpValue(60f, 30f, Projectile.ai[0], clamped: true);
			scale31.Y *= lerpValue4;
			Vector4 vector30 = color31.ToVector4();
			Vector4 vector31 = new Color(17, 17, 67).ToVector4();
			vector31 *= vector30;
			Main.EntitySpriteDraw(TextureAssets.Extra[98].Value, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) - Projectile.velocity * Projectile.scale * 0.5f, null, Projectile.GetAlpha(new Color(vector31.X, vector31.Y, vector31.Z, vector31.W)) * 1f, Projectile.rotation + (float)Math.PI / 2f, TextureAssets.Extra[98].Value.Size() / 2f, Projectile.scale * 0.9f, spriteEffects, 0);
			Main.EntitySpriteDraw(value242, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), value181, alpha14, Projectile.rotation, origin11, scale31, spriteEffects, 0);
			return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			if(Projectile.penetrate == 1)
            {
				Projectile.damage = 0;
				Projectile.penetrate = -1;
            }
        }
    }
}
