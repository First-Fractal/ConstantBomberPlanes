using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace ConstantBomberPlanes.Projectiles
{
    public class bomb : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 22;
            Projectile.damage = 1000;
            Projectile.aiStyle = -1;
            Projectile.scale = 1.5f;
            Projectile.ignoreWater = true;
        }

        public override void PostAI()
        {
            Projectile.velocity = new Vector2(0, 10);
            base.PostAI();
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (target.type == Projectile.owner)
            {
                return false;
            }
            return base.CanHitNPC(target);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (target.type != Projectile.owner)
            {
                Projectile.Kill();
            }
            base.OnHitNPC(target, damage, knockback, crit);
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            Projectile.Kill();
            base.OnHitPlayer(target, damage, crit);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return base.OnTileCollide(oldVelocity);
        }

        public override void Kill(int timeLeft)
        {
            int explosionRadius = 8;
            int bonusRadius = 6;

            int minTileX = (int)(Projectile.position.X / 16f - (float)explosionRadius - bonusRadius);
            int maxTileX = (int)(Projectile.position.X / 16f + (float)explosionRadius + bonusRadius);
            int minTileY = (int)(Projectile.position.Y / 16f - (float)explosionRadius - bonusRadius);
            int maxTileY = (int)(Projectile.position.Y / 16f + (float)explosionRadius + bonusRadius);


            if (minTileX < 0)
            {
                minTileX = 0;
            }
            if (maxTileX > Main.maxTilesX)
            {
                maxTileX = Main.maxTilesX;
            }
            if (minTileY < 0)
            {
                minTileY = 0;
            }
            if (maxTileY > Main.maxTilesY)
            {
                maxTileY = Main.maxTilesY;
            }

            for (int x = minTileX; x < maxTileX; x++)
            {
                for (int y = minTileY; y < maxTileY; y++)
                {
                    Tile tile = Main.tile[x, y];
                    float dis = Vector2.Distance(Projectile.position, new Vector2(x * 16, y * 16))/16f;
                    Main.NewText(dis);
                    if (dis < explosionRadius) {
                        bool hadLiquid = tile.LiquidAmount != 0;
                        if (hadLiquid == false)
                        {
                            WorldGen.KillTile(x, y, noItem: true);
                            tile.ClearEverything();
                        }
                    }
                }
            }

            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, ProjectileID.Explosives, Projectile.damage, 20);
        }
    }
}
