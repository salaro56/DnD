using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace DnD.NPCS.Hostile
{
    internal class WillOWisp : ModNPC
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Will O' Wisp");
            Main.npcFrameCount[NPC.type] = 5;
        }

        public override void SetDefaults()
        {
            //NPC Stats
            NPC.damage = 20;
            NPC.defense = 40;
            NPC.lifeMax = 22;

            NPC.value = 60;

            NPC.alpha = 255;

            //NPC Configs
            NPC.width = 24;
            NPC.height = 38;

            NPC.HitSound = SoundID.NPCHit36;
            NPC.DeathSound = SoundID.NPCHit36;

            NPC.aiStyle = -1;
            AnimationType = -1;

            NPC.noGravity = true;
            NPC.noTileCollide = true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if(spawnInfo.Player.ZoneForest && NPC.downedBoss1 || spawnInfo.Player.ZoneDungeon && NPC.downedBoss1)
            {
                if(NPC.AnyNPCs(Type))
                {
                    return SpawnCondition.OverworldNightMonster.Chance * 1.5f;
                }
                else
                {
                    return SpawnCondition.OverworldNightMonster.Chance * 0.05f;
                }
            }
            return 0f;
        }

        public override void AI()
        {
            int dis;
            Player player = Main.player[NPC.target];
            NPC.TargetClosest(true);
            if((player.Center - NPC.Center).Length() < 400)
            {
                dis = (int)(player.Center - NPC.Center).Length();
                Lighting.AddLight(NPC.Center, 0.1f, 0.1f, 0.1f);
            }
            else
            {
                dis = 255;
            }

            Vector2 vector = new Vector2(NPC.Center.X, NPC.Center.Y);
            float num1 = player.Center.X - vector.X;
            float num2 = player.Center.Y - vector.Y;
            float num3 = (float)Math.Sqrt(num1 * num1 + num2 * num2);
            float num4 = 12f;
            num3 = num4 / num3;
            num1 *= num3;
            num2 *= num3;
            NPC.velocity.X = (NPC.velocity.X * 100f + num1) / 101f;
            NPC.velocity.Y = (NPC.velocity.Y * 100f + num2) / 101f;
            NPC.rotation = NPC.velocity.X * 0.2f;
            NPC.position += NPC.netOffset;
            int num1445 = Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.DungeonSpirit);
            Dust dust50 = Main.dust[num1445];
            Dust dust90 = dust50;
            dust90.velocity *= 0.1f;
            Main.dust[num1445].scale = 1.3f;
            Main.dust[num1445].noGravity = true;
            NPC.position -= NPC.netOffset;

            Main.dust[num1445].alpha = dis;
            NPC.alpha = dis;
            if (NPC.alpha == 255)
            {
                NPC.dontTakeDamage = true;
            }
            else
            {
                NPC.dontTakeDamage = false;
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            target.AddBuff(BuffID.Chilled, 600);
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.spriteDirection = NPC.direction;

            int startFrame = 0;
            int finalFrame = 4;

            int frameSpeed = 5;
            NPC.frameCounter += 0.5f;
            NPC.frameCounter += NPC.velocity.Length() / 10f;

            if(NPC.frameCounter > frameSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                if(NPC.frame.Y > finalFrame * frameHeight)
                {
                    NPC.frame.Y = startFrame * frameHeight;
                }
            }
        }
    }
}
