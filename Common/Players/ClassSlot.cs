using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using DnD.Items.Classes;
using Terraria.GameInput;
using Terraria.GameContent;

namespace DnD.Common.Players
{

    internal class ClassSlotUI : ModSystem
    {
        internal static int posX;
        internal static int posY;

        public override void UpdateUI(GameTime gameTime)
        {
            if(!Main.gameMenu)
            {
                int mapH = 0;
                Main.inventoryScale = 0.85f;

                if (Main.mapEnabled && !Main.mapFullscreen && Main.mapStyle == 1)
                    mapH = 256;

                if(Main.EquipPageSelected == 2)
                {
                    if (Main.mapEnabled && (mapH + 600) > Main.screenHeight)
                        mapH = Main.screenHeight - 600;

                    posX = Main.screenWidth - 95 - (45 * 2);
                    posY = mapH + 175;

                    if (Main.netMode == NetmodeID.MultiplayerClient)
                        posX -= 50;
                }
                else
                {
                    if (Main.mapEnabled)
                    {
                        int adjustY = 600;

                        if (Main.player[Main.myPlayer].extraAccessory)
                            adjustY = 610 + PlayerInput.UsingGamepad.ToInt() * 30;

                        if ((mapH + adjustY) > Main.screenHeight)
                            mapH = Main.screenHeight - adjustY;
                    }

                    int slotCount = 7 + Main.player[Main.myPlayer].GetAmountOfExtraAccessorySlotsToShow();

                    if ((Main.screenHeight < 900) && (slotCount >= 8))
                        slotCount = 7;

                    posX = Main.screenWidth - 70 - 14 - (47 * 3) - (int)(TextureAssets.InventoryBack.Width() * Main.inventoryScale);
                    posY = (int)(mapH - 12 + 4 * 56 * Main.inventoryScale);
                }
            }
        }
    }

    public class ClassSlot : ModAccessorySlot
    {

        public override Vector2? CustomLocation => new Vector2(ClassSlotUI.posX, ClassSlotUI.posY);

        public override string FunctionalBackgroundTexture => "Terraria/Images/Inventory_Back11"; // pale blue

        public override bool DrawFunctionalSlot => Main.EquipPageSelected != 1 ? true : false;

        public override bool CanAcceptItem(Item checkItem, AccessorySlotType context)
        {
            DnDPlayer pc = Main.LocalPlayer.GetModPlayer<DnDPlayer>();

            if (checkItem.Name == "Rage Soul")
            {
                return true;
            }
            else if (checkItem.Name == "Holy Emblem")
            {
                return true;
            }
            else if (checkItem.Name == "Spellcasting Focus")
            {
                return true;
            }
            else if (checkItem.Name == "Rogue's Dagger")
            {
                return true;
            }
            else if (checkItem.Name == "Ranger's Quiver")
            {
                return true;
            }
            else return false;
        }

        public override bool ModifyDefaultSwapSlot(Item item, int accSlotToSwapTo)
        {
            if (item.Name == "Rage Soul")
            {
                return true;
            }
            else if (item.Name == "Holy Emblem")
            {
                return true;
            }
            else if (item.Name == "Spellcasting Focus")
            {
                return true;
            }
            else if (item.Name == "Rogue's Dagger")
            {
                return true;
            }
            else if (item.Name == "Ranger's Quiver")
            {
                return true;
            }
            else return false;
        }

        public override bool DrawVanitySlot => !DyeItem.IsAir;
        public override bool DrawDyeSlot => false;

        public override bool IsVisibleWhenNotEnabled()
        {
            return false; // We set to false to just not display if not Enabled. NOTE: this does not affect behavour when mod is unloaded!
        }

        public override string FunctionalTexture => "DnD/Items/Classes/SpellcastFocus";

        // Can be used to modify stuff while the Mouse is hovering over the slot.
        public override void OnMouseHover(AccessorySlotType context)
        {
            // We will modify the hover text while an item is not in the slot, so that it says "Wings".
            switch (context)
            {
                case AccessorySlotType.FunctionalSlot:
                    Main.hoverItemName = "Class";
                    break;
            }
        }
    }

    public class ClassSlot2 : ClassSlot
    {
        public override Vector2? CustomLocation => new Vector2(ClassSlotUI.posX, ClassSlotUI.posY + 50);

    }
}
