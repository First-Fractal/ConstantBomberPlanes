using ConstantBomberPlanes.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ConstantBomberPlanes
{
	public class ConstantBomberPlanes : Mod
	{
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
                Main.NewText(spawnerCounter.ToString());
                counter = 0;
            }

            if (spawnerCounter >= 60)
            {
                NPC.NewNPC(Main.player[0].GetSource_FromThis(), 0, (Main.maxTilesY / 16) + Main.maxTilesY, ModContent.NPCType<bomberPlane>());
                spawnerCounter = 0;
            }
        }


        //spawn in the npcs for the server
        public override void PostUpdateEverything()
        {
            if (Main.netMode == NetmodeID.Server)
            {
                bomberPlaneSpawner();
            }
            base.PostUpdateEverything();
        }

        //spawn in the npcs for singleplayer
        public override void PostUpdateWorld()
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                bomberPlaneSpawner();
            }
            base.PostUpdateWorld();
        }
    }
}