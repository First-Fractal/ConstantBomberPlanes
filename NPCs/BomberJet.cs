using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using ConstantBomberPlanes.Projectiles;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;

namespace ConstantBomberPlanes.NPCs
{
    [AutoloadBossHead]
    public class BomberJet : ModNPC
    {
        int counter = 0;
        public int spawnerCounter = 0;
        public int timer = 0; 
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            NPC.width = 160;
            NPC.height = 56;
            NPC.knockBackResist = 1;
            NPC.lifeMax = 1;
            NPC.friendly = true;
            NPC.townNPC = true;
            NPC.boss = false;
            NPC.immortal = true;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.homeless = true;
            NPC.BossBar = Main.BigBossProgressBar.NeverValid;
            NPC.Happiness
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Love)
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Hate);
            base.SetDefaults();
        }

        public override void AI()
        {
            NPC.homeless = true;
            if (ConstantBomberPlanesConfig.Instance.RandomBombSpawner)
            {
                timer = Main.rand.Next(ConstantBomberPlanesConfig.Instance.RandMin, ConstantBomberPlanesConfig.Instance.RandMax);
            } else
            {
                timer = ConstantBomberPlanesConfig.Instance.BombSpawnerCooldown;
            }

            NPC.position += new Vector2(8, 0);

            if (NPC.position.X/16 > Main.maxTilesX)
            {
                NPC.life = 0;
            }

            counter++;
            if (counter >= 60)
            {
                spawnerCounter++;
                counter = 0;
            }

            if (spawnerCounter >= timer)
            {
                Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position + new Vector2(0, 100), Vector2.Zero, ModContent.ProjectileType<bomb>(), 0, 0);
                spawnerCounter = 0;
            }

            base.AI();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC.BossBar = Main.BigBossProgressBar.NeverValid;
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            NPC.BossBar = null;
            scale = 0.1f;
            hbPosition = 0;
            position = new Vector2(99999, 99999);
            return false;
        }

        public override string GetChat()
        {
            WeightedRandom<string> chat = new WeightedRandom<string>();
            chat.Add(Language.GetTextValue("Mods.ConstantBomberPlanes.Dialogue.bomberPlane.dialogue1"));
            chat.Add(Language.GetTextValue("Mods.ConstantBomberPlanes.Dialogue.bomberPlane.dialogue2"));
            chat.Add(Language.GetTextValue("Mods.ConstantBomberPlanes.Dialogue.bomberPlane.dialogue3"));
            chat.Add(Language.GetTextValue("Mods.ConstantBomberPlanes.Dialogue.bomberPlane.dialogue4"));
            chat.Add(Language.GetTextValue("Mods.ConstantBomberPlanes.Dialogue.bomberPlane.dialogue5"));
            chat.Add(Language.GetTextValue("Mods.ConstantBomberPlanes.Dialogue.bomberPlane.dialogue6"));
            return chat;
        }
    }
}
