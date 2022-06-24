using DnD;
using Terraria.ModLoader;

namespace DnD.Common.Commands
{
    public class LevelReset : ModCommand
    {
        public override string Command => "reset";

        public override string Description => "Resets your level";
        public override CommandType Type => CommandType.Chat;

        public override string Usage => "/reset";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            DnDPlayer character = caller.Player.GetModPlayer<DnDPlayer>();
            character.playerLevel = 0;
            character.experiencePoints = 0;
        }
    }
}