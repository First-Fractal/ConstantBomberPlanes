using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using ConstantBomberPlanes.Projectiles;
using Terraria.GameContent.Personalities;
using Microsoft.Xna.Framework.Graphics;

namespace ConstantBomberPlanes.NPCs
{
    //load in the head icon onto the map
    [AutoloadBossHead]
    public class BomberJet : ModNPC
    {
        //cooldown variables for spawning in the bomb
        public int bombCooldown = 0;
        public int bombCooldownMax = 0;

        public override void SetDefaults()
        {
            //set the width and height of the npc
            NPC.width = 160;
            NPC.height = 56;

            //make it have no knockback
            NPC.knockBackResist = 0;

            //make it impossible for the player to kill the plane
            NPC.lifeMax = 1;
            NPC.friendly = true;
            NPC.townNPC = true;
            NPC.boss = false;
            NPC.immortal = true;

            //make it immune to gravity and tiles
            NPC.noGravity = true;
            NPC.noTileCollide = true;

            //give it a custom ai
            NPC.aiStyle = -1;

            //prevent it from tp'ing into a home
            NPC.homeless = true;

            //remove it's healthbar
            NPC.BossBar = Main.BigBossProgressBar.NeverValid;

            //give it love and hate biomes (cause why not)
            NPC.Happiness
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Love)
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Hate);
            base.SetDefaults();
        }

        //function to spawn in a bomb
        void SpawnBomb()
        {
            Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.position + new Vector2(0, 100), 
                Vector2.Zero, ModContent.ProjectileType<Bomb>(), 0, 0);
        }

        public override void AI()
        {
            //permanently make it homeless
            NPC.homeless = true;

            //set the max time for the bomb to spawn in
            if (CBPconfig.Instance.RandomBombSpawner)
            {
                bombCooldownMax = Main.rand.Next(CBPconfig.Instance.RandMin, CBPconfig.Instance.RandMax);
            } else
            {
                bombCooldownMax = CBPconfig.Instance.BombSpawnerCooldown;
            }

            //slowly move the plane to the right side of the world
            NPC.position += new Vector2(8, 0);

            //if the plane is at the edge of the world, then kill it
            if (NPC.position.X/16 > Main.maxTilesX)
            {
                NPC.life = 0;
                NPC.active = false;
            }

            //make it spawn in a bomb after the cooldown goes off
            ffFunc.CooldownSystem(ref bombCooldown, ref bombCooldownMax, SpawnBomb);

            base.AI();
        }

        //get rid of the boss bar for the jet
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            NPC.BossBar = Main.BigBossProgressBar.NeverValid;
            return base.PreDraw(spriteBatch, screenPos, drawColor);
        }


        //get rid of the boss bar for the jet
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            NPC.BossBar = null;
            scale = 0.1f;
            hbPosition = 0;
            position = new Vector2(99999, 99999);
            return false;
        }

        //make it have random dialogue options when it's right click on
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
