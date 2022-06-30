using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;

namespace DnD.Graphics
{
    internal class GoldFlameDrawer
    {
		private static VertexStrip _vertexStrip = new VertexStrip();

		private float transitToDark;

		public void Draw(Projectile proj)
		{
			this.transitToDark = Utils.GetLerpValue(0f, 6f, proj.localAI[0], clamped: true);
			MiscShaderData miscShaderData = GameShaders.Misc["FlameLash"];
			miscShaderData.UseSaturation(-2f);
			miscShaderData.UseOpacity(MathHelper.Lerp(4f, 8f, this.transitToDark));
			miscShaderData.Apply();
			GoldFlameDrawer._vertexStrip.PrepareStripWithProceduralPadding(proj.oldPos, proj.oldRot, StripColors, StripWidth, -Main.screenPosition + proj.Size / 2f);
			GoldFlameDrawer._vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}

		private Color StripColors(float progressOnStrip)
		{
			float lerpValue = Utils.GetLerpValue(0f - 0.1f * this.transitToDark, 0.7f - 0.2f * this.transitToDark, progressOnStrip, clamped: true);
			Color result = Color.Lerp(Color.Lerp(Color.White, Color.Gold, this.transitToDark * 0.5f), Main.OurFavoriteColor, lerpValue) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
			result.A /= 8;
			return result;
		}

		private float StripWidth(float progressOnStrip)
		{
			float lerpValue = Utils.GetLerpValue(0f, 0.06f + this.transitToDark * 0.01f, progressOnStrip, clamped: true);
			lerpValue = 1f - (1f - lerpValue) * (1f - lerpValue);
			return MathHelper.Lerp(24f + this.transitToDark * 16f, 8f, Utils.GetLerpValue(0f, 1f, progressOnStrip, clamped: true)) * lerpValue;
		}
	}
}
