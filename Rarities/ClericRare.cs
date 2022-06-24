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
    internal class ClericRare : ModRarity
    {
        public override Color RarityColor => new Color(Main.DiscoB, Main.DiscoB * 2, Main.DiscoB / 2);
    }
}
