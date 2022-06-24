using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using DnD.Packets;
using Microsoft.Xna.Framework;

namespace DnD
{
    public class DNPC : GlobalNPC
    {

        public int xpModifiers;

        public override bool InstancePerEntity => true;
        public override void OnKill(NPC npc)
        {
            DNPC dnpc = npc.GetGlobalNPC<DNPC>();

            if (npc.lifeMax < 10) return;
            if (npc.friendly) return;
            if (npc.townNPC) return;


            if (ModContent.GetInstance<Common.Configs.DnDConfigs>().SharedXPToggle == true)
            {
                Player player = Array.Find(Main.player, p => p.active);
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    player = Main.LocalPlayer;
                }
                else if (Main.player[npc.target].active)
                {
                    player = Main.player[npc.target];
                }
                else
                {
                    DnDPlayer c = player.GetModPlayer<DnDPlayer>();

                    foreach (Player p in Main.player)
                        if (p != null)
                            if (p.active)
                                if (p.GetModPlayer<DnDPlayer>() != null)
                                    if (p.GetModPlayer<DnDPlayer>().playerLevel > c.playerLevel)
                                        player = p;
                }
                DnDPlayer character = player.GetModPlayer<DnDPlayer>();
                int life = npc.type == NPCID.SolarCrawltipedeTail || npc.type == NPCID.SolarCrawltipedeBody || npc.type == NPCID.SolarCrawltipedeHead
                    ? npc.lifeMax / 8
                    : npc.lifeMax;
                int defFactor = npc.defense < 0 ? 1 : npc.defense * life / (character.playerLevel + 10);
                int baseExp = Main.rand.Next((life + defFactor) / (8 + character.playerLevel)) + (life + defFactor) / (10 + character.playerLevel);
                int scaled = Main.expertMode ? (int)(baseExp * 0.5) : baseExp;

                if (!AddXPPacket.Write(npc.target, scaled))
                    character.AddXP(scaled);
            }
            else
            {
                int playerIndex;

                if (npc.lastInteraction < 255)
                {
                    Player player = Main.player[npc.lastInteraction];
                    playerIndex = npc.lastInteraction;

                    DnDPlayer character = player.GetModPlayer<DnDPlayer>();
                    int life = npc.type == NPCID.SolarCrawltipedeTail || npc.type == NPCID.SolarCrawltipedeBody || npc.type == NPCID.SolarCrawltipedeHead
                        ? npc.lifeMax / 8
                        : npc.lifeMax;
                    int defFactor = npc.defense < 0 ? 1 : npc.defense * life / (character.playerLevel + 10);
                    int baseExp = Main.rand.Next((life + defFactor) / 9) + (life + defFactor) / 10;
                    int scaled = Main.expertMode ? (int)(baseExp * 0.5) : baseExp;

                    if (!AddXPPacket.Write(playerIndex, scaled))
                        character.AddXP(scaled);
                }
            }
        }

        private int GetPlayerLevel()
        {
            try
            {
                Player player = Main.netMode == NetmodeID.Server ? Main.player[0] : Main.player[Main.myPlayer];
                return player.GetModPlayer<DnDPlayer>().playerLevel;
            }
            catch (Exception)
            {
                return 20;
            }
        }

        public override void SetDefaults(NPC npc)
        {
            if (Main.netMode == NetmodeID.SinglePlayer && !DnD.PlayerEnteredWorld)
                return;

            if (npc.boss || npc.townNPC || npc.friendly || Main.netMode == NetmodeID.MultiplayerClient)
                return;
        }
    }
}
