using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace DnD.Rarities
{
    internal class BarbRare : ModRarity
    {
        public override Color RarityColor => new Color(Main.DiscoR + ColorRotate(), 40, 4);

        private int ColorRotate()
        {
            int cRed = 150;
            if(cRed >= 200)
            {
                cRed = 150;
            }
            else
            {
                cRed++;
            }
            return cRed;
        }
    }
}
