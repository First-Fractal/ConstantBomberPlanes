using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace ConstantBomberPlanes
{
    public class CBPconfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;
        public static CBPconfig Instance;

        [Header("$Mods.ConstantBomberPlanes.Config.Header.GeneralOptions")]

        [DefaultValue(5)]
        public int PlaneSpawnerCooldown;

        [DefaultValue(10)]
        public int BombSpawnerCooldown;

        [DefaultValue(true)]
        public bool RandomBombSpawner;

        [DefaultValue(2)]
        public int RandMin;

        [DefaultValue(20)]
        public int RandMax;

        [DefaultValue(8)]
        public int explosionRadius;
    }
}
