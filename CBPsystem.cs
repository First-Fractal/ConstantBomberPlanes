using ConstantBomberPlanes.NPCs;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria;

namespace ConstantBomberPlanes
{
    public class CBPsystem : ModSystem
    {
        //variables for the bomber plane spawning in cooldown
        int planeCooldown = 0;
        int planeCooldownMax = ffFunc.TimeToTick(mins: CBPconfig.Instance.PlaneSpawnerCooldown);

        //function for spawning in the plane
        void PlaneSpawner()
        {
            //update the max cooldown incase it got change in the config
            planeCooldownMax = ffFunc.TimeToTick(mins: CBPconfig.Instance.PlaneSpawnerCooldown);

            //spawn in the new bomber plane
            NPC.NewNPC(Main.player[0].GetSource_FromThis(), 0, (Main.maxTilesY / 16) + Main.maxTilesY, 
                ModContent.NPCType<BomberJet>());

            //tell the world about the new bomber plane
            ffFunc.Talk(Language.GetTextValue("Mods.ConstantBomberPlanes.Dialogue.bomberPlane.SpawnMessage"), 
                new Color(0f, 220f, 255f));

        }

        //control the cooldown system after everything
        public override void PostUpdateEverything()
        {
            //make a bomber plane spawn in when it's off cooldown
            ffFunc.CooldownSystem(ref planeCooldown, ref planeCooldownMax, PlaneSpawner);
            base.PostUpdateEverything();
        }
    }
}
