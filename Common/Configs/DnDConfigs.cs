using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace DnD.Common.Configs
{
    internal class DnDConfigs : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("$Configs")] // Headers are like titles in a config. You only need to declare a header on the item it should appear over, not every item in the category.
        [Label("$Shared EXP")] // A label is the text displayed next to the option. This should usually be a short description of what it does.
        [Tooltip("$Toggles shared xp or solo xp")] // A tooltip is a description showed when you hover your mouse over the option. It can be used as a more in-depth explanation of the option.
        [DefaultValue(true)] // This sets the configs default value.
        [ReloadRequired] // Marking it with [ReloadRequired] makes tModLoader force a mod reload if the option is changed. It should be used for things like item toggles, which only take effect during mod loading
        public bool SharedXPToggle; // To see the implementation of this option, see ExampleWings.cs

        [Label("$Screen Shake")]
        [Tooltip("$Turns off screenshake effect")]
        [DefaultValue(true)]
        public bool ScreenShake;
    }
}
