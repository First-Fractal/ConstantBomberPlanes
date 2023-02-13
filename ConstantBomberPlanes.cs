using Terraria;
using Terraria.ID;
using Terraria.Chat;
using Terraria.ModLoader;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using ConstantBomberPlanes.NPCs;

namespace ConstantBomberPlanes
{
	public class ConstantBomberPlanes : Mod
    {
        public void Talk(string message, Color color)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(message, color);
            }
            else
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(message), color);
            }
        }
    }

    public class ConstantBomberPlanesSystem : ModSystem
	{
        int counter = 0;
        public int spawnerCounter = 0;
        public void bomberPlaneSpawner()
        {
            counter++;
            if (counter >= 60)
            {
                spawnerCounter++;
                Main.NewText(spawnerCounter.ToString(), Color.SeaShell);
                counter = 0;
            }

            if (spawnerCounter >= 60)
            {
                Main.NewText(Language.GetTextValue("Mods.ConstantBomberPlanes.Dialogue.bomberPlane.SpawnMessage"), new Color(0f, 220f, 255f));
                NPC.NewNPC(Main.player[0].GetSource_FromThis(), 0, (Main.maxTilesY / 16) + Main.maxTilesY, ModContent.NPCType<bomberPlane>());
                spawnerCounter = 0;
            }
        }

        public override void PostUpdateEverything()
        {
            bomberPlaneSpawner();
            base.PostUpdateEverything();
        }
    }
}