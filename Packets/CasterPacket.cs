using System.Collections.Generic;
using System.IO;
using DnD.Enums;
using DnD.Items.Spells.ClericSpells.Lvl2;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DnD.Packets
{
    internal class CasterPacket
    {
        public static void Read(BinaryReader reader)
        {
            if(Main.netMode == NetmodeID.MultiplayerClient)
            {
                BondPlayer player = Main.LocalPlayer.GetModPlayer<BondPlayer>();
                player.caster = reader.ReadInt32();
            }
        }

        public static void Write(int caster)
        {
            if(Main.netMode == NetmodeID.Server)
            {
                ModPacket packet = DnD.Mod.GetPacket();
                packet.Write((byte)Message.Caster);
                packet.Write(caster);
                packet.Send();
            }
        }
    }
}
