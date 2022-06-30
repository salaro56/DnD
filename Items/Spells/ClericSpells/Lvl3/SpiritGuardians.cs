using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using DnD.Graphics.Dusts;

namespace DnD.Items.Spells.ClericSpells.Lvl3
{
    internal class SpiritGuardians : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
        }

        public override void HoldItem(Player player)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            if (pc.clericClass == true)
            {
                for (int i = 0; i < 5; i++)
                {
                    Vector2 speed = Main.rand.NextVector2CircularEdge(5f, 5f);
                    Dust d = Dust.NewDustPerfect(new Vector2(player.Center.X + speed.X * 75, player.Center.Y + speed.Y * 75), ModContent.DustType<MagicRunes>(), Vector2.Zero, Scale: 3f);
                    d.velocity *= 0.5f;
                    d.noGravity = true;
                    d.color = Color.LightGoldenrodYellow;
                    d.noLight = true;
                    d.alpha = 100;
                }
            }
        }
    }
}
