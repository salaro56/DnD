using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace DnD.Graphics.Dusts
{
    internal class MagicRunes : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLightEmittence = true;
            dust.velocity = Vector2.Zero;
        }

        public override bool Update(Dust dust)
        {
            dust.rotation += 0.1f * (dust.dustIndex % 2 == 0 ? -1 : 1);
            dust.scale -= 0.05f;

            if (dust.scale < 0.25f)
                dust.active = false;

            return false;
        }
    }
}
