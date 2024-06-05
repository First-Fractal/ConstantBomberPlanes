using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.DataStructures;

namespace ConstantBomberPlanes.Projectiles
{
    public class Bomb : ModProjectile
    {
        public override void SetDefaults()
        {
            //set the size of the bomb
            Projectile.width = 20;
            Projectile.height = 22;

            //give it a custom ai and ignore water
            Projectile.aiStyle = -1;
            Projectile.ignoreWater = true;

            //make it bigger and last for a while
            Projectile.scale = 1.5f;
            Projectile.timeLeft = ffFunc.TimeToTick(hours: 1);
        }

        //make it fall down into the earth 
        public override void PostAI()
        {
            Projectile.velocity = new Vector2(0, 10);
            base.PostAI();
        }

        //make it not hit the bomber planes
        public override bool? CanHitNPC(NPC target)
        {
            if (target.type == Projectile.owner)
            {
                return false;
            }
            return base.CanHitNPC(target);
        }

        //if the bomb hit a npc that isnt the plane, then kill it
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (target.type != Projectile.owner)
            {
                Projectile.Kill();
            }
            base.OnHitNPC(target, hit, damageDone);
        }

        //kill the projectile die when it hit a player
        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Projectile.Kill();
            base.OnHitPlayer(target, info);
        }


        //kill it when it hits a tile
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return base.OnTileCollide(oldVelocity);
        }

        public override void OnKill(int timeLeft)
        {
            //spawn in the explosion projectile
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position, Vector2.Zero, 
                ProjectileID.Explosives, Projectile.damage, 20);

            //configure how big the explosion is
            int explosionRadius = CBPconfig.Instance.explosionRadius;
            int bonusRadius = 6;

            //calculate how min and max size of the crater
            int minTileX = (int)(Projectile.position.X / 16f - (float)explosionRadius - bonusRadius);
            int maxTileX = (int)(Projectile.position.X / 16f + (float)explosionRadius + bonusRadius);
            int minTileY = (int)(Projectile.position.Y / 16f - (float)explosionRadius - bonusRadius);
            int maxTileY = (int)(Projectile.position.Y / 16f + (float)explosionRadius + bonusRadius);

            //clamp the values to be inside the world border
            if (minTileX < 0) minTileX = 0;
            if (maxTileX > Main.maxTilesX) maxTileX = Main.maxTilesX;
            if (minTileY < 0) minTileY = 0;
            if (maxTileY > Main.maxTilesY) maxTileY = Main.maxTilesY;

            //iter through all of the tiles for the crater
            for (int x = minTileX; x < maxTileX; x++)
            {
                for (int y = minTileY; y < maxTileY; y++)
                {
                    //get the tile
                    Tile tile = Main.tile[x, y];

                    //get the distance between the tile and the bomb
                    float dis = Vector2.Distance(Projectile.position, new Vector2(x * 16, y * 16))/16f;
                    
                    //check if the currnet tile is inside the explosion radius
                    if (dis < explosionRadius) {

                        //if the tile isnt suppose to be able to be exploded, then skip it
                        if (tile.TileType == TileID.BlueDungeonBrick || tile.TileType == TileID.GreenDungeonBrick || 
                            tile.TileType == TileID.PinkDungeonBrick
                            || tile.WallType == WallID.BlueDungeonSlabUnsafe 
                            ||  tile.WallType == WallID.BlueDungeonTileUnsafe 
                            || tile.WallType == WallID.BlueDungeonUnsafe
                            || tile.WallType == WallID.GreenDungeonSlabUnsafe || 
                            tile.WallType == WallID.GreenDungeonTileUnsafe || 
                            tile.WallType == WallID.GreenDungeonUnsafe
                            || tile.WallType == WallID.PinkDungeonSlabUnsafe || 
                            tile.WallType == WallID.PinkDungeonTileUnsafe 
                            || tile.WallType == WallID.PinkDungeonUnsafe ||
                            tile.TileType == TileID.DemonAltar || tile.TileType == TileID.LihzahrdAltar || 
                            tile.TileType == TileID.Traps || tile.TileType == 21 || tile.TileType == 467 
                            || tile.TileType == 468 
                            || tile.TileType == TileID.Dressers || tile.TileType == 470 || 
                            tile.TileType == 475 || tile.TileType == 395
                        )  break;

                        //make it unable to blow up metorite, hellstone, and hellforge in pre hardmode
                        if (!Main.hardMode && (tile.TileType == TileID.Meteorite || tile.TileType == 
                            TileID.Hellstone || tile.TileType == TileID.Hellforge)) { break; }

                        //make it unable to blow up hardmore ores if all three mechs arn't defeated
                        if ((!NPC.downedMechBoss1 && !NPC.downedMechBoss2 && !NPC.downedMechBoss3) &&
                            (tile.TileType == TileID.Cobalt || tile.TileType == TileID.Palladium ||
                            tile.TileType == TileID.Mythril || tile.TileType == TileID.Orichalcum
                            || tile.TileType == TileID.Adamantite || tile.TileType == TileID.Titanium)) break;

                        //make it unable to blow up chlorophyte if plantera isn't killed
                        if ((!NPC.downedPlantBoss) && tile.TileType == TileID.Chlorophyte) break;

                        //make it unable to destroy parts of the temple
                        if (NPC.downedGolemBoss == false && (tile.TileType == TileID.LihzahrdBrick
                            || tile.TileType == TileID.LihzahrdFurnace))  break; 

                        //don't destroy the liquid
                        bool hadLiquid = tile.LiquidAmount != 0;
                        if (hadLiquid == false)
                        {
                            //destroy the tile with a 50% chance of dropping the item
                            WorldGen.KillTile(x, y, noItem: Main.rand.NextBool(2));
                            tile.ClearEverything();
                        }
                    }
                }
            }

            //loop through each player
            foreach (Player player in Main.player)
            {
                //check if the player is active and not dead
                if (player.active && !player.dead)
                {
                    //get the distance between the player and the bomb
                    float dis = Vector2.Distance(player.position, Projectile.position);

                    //if the player is inside the radius, then instakill them
                    if (dis <= explosionRadius*16)
                    {
                        player.Hurt(PlayerDeathReason.ByProjectile(player.whoAmI, Projectile.whoAmI), 99999, -3);
                    }
                }
            }

        }
    }
}
