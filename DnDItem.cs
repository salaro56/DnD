using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace DnD
{
    internal class DnDItem : GlobalItem
    {
        public override bool CanUseItem(Item item, Player player)
        {
            DnDPlayer pc = player.GetModPlayer<DnDPlayer>();
            if (item.CountsAsClass(DamageClass.Magic) == true && pc.isRaging)
            {
                return false;
            }
            else return true;
        }


        public int DamageValue(int minRoll, int maxRoll, int diceRolled)
        {
            DnDPlayer pc = Main.LocalPlayer.GetModPlayer<DnDPlayer>();
            int damage = 0;

            for (int i = 0; i < diceRolled; i++)
            {
                damage += Main.rand.Next(minRoll, maxRoll) * pc.ProfBonus();
            }

            return damage;
        }
    }
}