using System.Collections.Generic;
using System.IO;
using DnD.Enums;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DnD.Packets
{
    public static class AddXPPacket
    {
        public static void Read(BinaryReader reader)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                //Player player = Main.player[Main.myPlayer];
                //if (Vector2.Distance(player.Center, Main.npc[(int)tags[DataTag.npcId]].Center) > 1024)
                //    break;
                //character.AddXp((int) tags[DataTag.Amount]);
                if(ModContent.GetInstance<Common.Configs.DnDConfigs>().SharedXPToggle == true)
                {
                    int target = reader.ReadInt32();
                    DnDPlayer character = Main.LocalPlayer.GetModPlayer<DnDPlayer>();
                    character.AddXP((int)reader.ReadInt32());
                }
                else
                {
                    Player player = Main.player[reader.ReadInt32()];
                    DnDPlayer character = player.GetModPlayer<DnDPlayer>();
                    character.AddXP((int)reader.ReadInt32());
                }
            }
        }

        public static bool Write(int target, int scaled)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket packet = DnD.Mod.GetPacket();
                packet.Write((byte)Message.AddXp);
                packet.Write(target);
                packet.Write(scaled);
                // todo this seems extra....
                packet.Send();
                return true;
            }

            return false;
        }
    }
}