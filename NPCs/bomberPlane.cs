using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Personalities;
using Terraria.Utilities;
using Terraria.Localization;
using Terraria;

namespace ConstantBomberPlanes.NPCs
{
    [AutoloadHead]
    public class bomberPlane : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bomber Jet");
            base.SetStaticDefaults();
        }

        public override void SetDefaults()
        {
            NPC.width = 160;
            NPC.height = 56;
            NPC.knockBackResist = 1;
            NPC.lifeMax = 1;
            NPC.friendly = true;
            NPC.townNPC= true;
            NPC.immortal = true;
            NPC.noGravity = true;
            NPC.aiStyle = -1;
            NPC.noTileCollide = true;
            NPC.Happiness
                .SetBiomeAffection<ForestBiome>(AffectionLevel.Love)
                .SetBiomeAffection<UndergroundBiome>(AffectionLevel.Hate);
            base.SetDefaults();
        }

        public override void AI()
        {
            NPC.position += new Vector2(8, 0);

            //Main.NewText("Jet's X postition " + (NPC.position.X/16).ToString() + " and the max is " + Main.maxTilesX.ToString());
            if (NPC.position.X/16 > Main.maxTilesX)
            {
                NPC.life = 0;
            }
            base.AI();
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
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
            return chat;
        }
    }
}
