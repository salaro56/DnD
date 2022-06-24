using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.GameContent.Dyes;
using Terraria.GameContent.UI;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using DnD.Enums;
using DnD.Packets;
using DnD.UI;
using DnD;

namespace DnD
{
	public class DnD : Mod
	{
		public DnD()
        {
			Mod = this;
        }
		public static DnD Mod { get; set; }

        public static bool PlayerEnteredWorld { get; set; } = false;

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            Message msg = (Message)reader.ReadByte();

            switch (msg)
            {
                case Message.SyncLevel:
                    SyncLevelPacket.Read(reader);
                    DnD.PlayerEnteredWorld = true;
                    break;
                case Message.AddXp:
                    AddXPPacket.Read(reader);
                    break;
                case Message.Caster:
                    CasterPacket.Read(reader);
                    break;
            }
        }

    }
}