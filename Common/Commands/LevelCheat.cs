using DnD;
using Terraria.ModLoader;

namespace DnD.Common.Commands
{
    public class LevelCheat : ModCommand
    {
        public override string Command => "level";

        public override string Description => "Sets your character level to the chosen value";
        public override CommandType Type => CommandType.Chat;

        public override string Usage => "/level <level>";

        public override void Action(CommandCaller caller, string input, string[] args)
        {
            DnDPlayer character = caller.Player.GetModPlayer<DnDPlayer>();
            float xp = character.experiencePoints / (float)character.XPToLevel();
            character.playerLevel = int.Parse(args[0]);
            character.experiencePoints = (int)(character.XPToLevel() * xp);
        }
    }
}