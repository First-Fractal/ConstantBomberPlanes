using Terraria;
using Terraria.ID;
using Terraria.Chat;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ModLoader.Config;
using Microsoft.Xna.Framework;
using ConstantBomberPlanes.NPCs;
using System.ComponentModel;
using tModPorter;

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
                counter = 0;
            }

            if (spawnerCounter >= ConstantBomberPlanesConfig.Instance.PlaneSpawnerCooldown)
            {
                ConstantBomberPlanes CBP = new ConstantBomberPlanes();
                CBP.Talk(Language.GetTextValue("Mods.ConstantBomberPlanes.Dialogue.bomberPlane.SpawnMessage"), new Color(0f, 220f, 255f));
                NPC.NewNPC(Main.player[0].GetSource_FromThis(), 0, (Main.maxTilesY / 16) + Main.maxTilesY, ModContent.NPCType<BomberJet>());
                spawnerCounter = 0;
            }
        }

        public override void PostUpdateEverything()
        {
            bomberPlaneSpawner();
            base.PostUpdateEverything();
        }
    }


    [Label("$Mods.ConstantBomberPlanes.Config.Label")]
    public class ConstantBomberPlanesConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        public static ConstantBomberPlanesConfig Instance;

        [Header("$Mods.ConstantBomberPlanes.Config.Header.GeneralOptions")]

        [Label("$Mods.ConstantBomberPlanes.Config.PlaneSpawnerCooldown.Label")]
        [Tooltip("$Mods.ConstantBomberPlanes.Config.PlaneSpawnerCooldown.Tooltip")]
        [DefaultValue(60)]
        public int PlaneSpawnerCooldown;

        [Label("$Mods.ConstantBomberPlanes.Config.BombSpawnerCooldown.Label")]
        [Tooltip("$Mods.ConstantBomberPlanes.Config.BombSpawnerCooldown.Tooltip")]
        [DefaultValue(10)]
        public int BombSpawnerCooldown;

        [Label("$Mods.ConstantBomberPlanes.Config.RandomBombSpawner.Label")]
        [Tooltip("$Mods.ConstantBomberPlanes.Config.RandomBombSpawner.Tooltip")]
        [DefaultValue(true)]
        public bool RandomBombSpawner;

        [Label("$Mods.ConstantBomberPlanes.Config.RandMin.Label")]
        [Tooltip("$Mods.ConstantBomberPlanes.Config.RandMin.Tooltip")]
        [DefaultValue(2)]
        public int RandMin;

        [Label("$Mods.ConstantBomberPlanes.Config.RandMax.Label")]
        [Tooltip("$Mods.ConstantBomberPlanes.Config.RandMax.Tooltip")]
        [DefaultValue(20)]
        public int RandMax;

        [Label("$Mods.ConstantBomberPlanes.Config.explosionRadius.Label")]
        [Tooltip("$Mods.ConstantBomberPlanes.Config.explosionRadius.Tooltip")]
        [DefaultValue(8)]
        public int explosionRadius;
    }

}