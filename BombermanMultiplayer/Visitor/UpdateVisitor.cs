using System;
using System.Drawing;
using BombermanMultiplayer.Objects;

namespace BombermanMultiplayer.Visitor
{
    /// <summary>
    /// Visitor that updates game objects.
    /// Replaces scattered update logic (BombsLogic, MinesLogic, GrenadesLogic) with a unified approach.
    /// </summary>
    public class UpdateVisitor : IGameObjectVisitor
    {
        private int elapsedTime;
        private World world;
        private Player[] players;

        /// <summary>
        /// Initializes a new instance of UpdateVisitor
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since last update in milliseconds</param>
        /// <param name="world">The game world</param>
        /// <param name="players">Array of players in the game</param>
        public UpdateVisitor(int elapsedTime, World world, Player[] players)
        {
            this.elapsedTime = elapsedTime;
            this.world = world;
            this.players = players;
        }

        /// <summary>
        /// Sets the elapsed time for updates
        /// </summary>
        /// <param name="time">Time elapsed in milliseconds</param>
        public void SetElapsedTime(int time)
        {
            this.elapsedTime = time;
        }

        /// <summary>
        /// Sets the map grid for updates
        /// </summary>
        /// <param name="mapGrid">The map grid</param>
        public void SetMapGrid(Tile[,] mapGrid)
        {
            if (world != null)
            {
                world.MapGrid = mapGrid;
            }
        }

        /// <summary>
        /// Sets the players array for updates
        /// </summary>
        /// <param name="players">Array of players</param>
        public void SetPlayers(Player[] players)
        {
            this.players = players;
        }

        /// <summary>
        /// Visits and updates a Player object
        /// </summary>
        /// <param name="player">The player to update</param>
        public void VisitPlayer(Player player)
        {
            if (player == null || player.Dead) return;

            // Update player location
            player.LocationCheck(48, 48);

            // Update player movement and animation
            if (player.Orientation != Player.MovementDirection.NONE)
            {
                player.Move();
                player.UpdateFrame(elapsedTime);
            }
            else
            {
                player.frameindex = 1;
            }
        }

        /// <summary>
        /// Visits and updates a Bomb object
        /// </summary>
        /// <param name="bomb">The bomb to update</param>
        public void VisitBomb(Bomb bomb)
        {
            if (bomb == null) return;

            bomb.UpdateFrame(elapsedTime);
            bomb.TimingExplosion(elapsedTime);

            // If bomb is exploding, trigger explosion
            if (bomb.Exploding && world != null && world.MapGrid != null && players != null)
            {
                bomb.Explosion(world.MapGrid, players);
            }
        }

        /// <summary>
        /// Visits and updates a Mine object
        /// </summary>
        /// <param name="mine">The mine to update</param>
        public void VisitMine(Mine mine)
        {
            if (mine == null) return;

            // Check proximity to players
            if (players != null)
            {
                mine.CheckProximity(players);
            }

            mine.UpdateFrame(elapsedTime);
            mine.TimingExplosion(elapsedTime);

            // If mine is exploding, trigger explosion
            if (mine.Exploding && world != null && world.MapGrid != null && players != null)
            {
                mine.Explosion(world.MapGrid, players);
            }
        }

        /// <summary>
        /// Visits and updates a Grenade object
        /// </summary>
        /// <param name="grenade">The grenade to update</param>
        public void VisitGrenade(Grenade grenade)
        {
            if (grenade == null) return;

            // Move grenade if it's being thrown
            if (world != null && world.MapGrid != null)
            {
                grenade.MoveGrenade(world.MapGrid);
            }

            grenade.UpdateFrame(elapsedTime);
            grenade.TimingExplosion(elapsedTime);

            // If grenade is exploding, trigger explosion
            if (grenade.Exploding && world != null && world.MapGrid != null && players != null)
            {
                grenade.Explosion(world.MapGrid, players);
            }
        }

        /// <summary>
        /// Visits and updates a Tile object
        /// </summary>
        /// <param name="tile">The tile to update</param>
        public void VisitTile(Tile tile)
        {
            if (tile == null) return;

            // Update fire on tile
            if (tile.Fire)
            {
                if (tile.FireTime <= 0)
                {
                    tile.Fire = false;
                    tile.FireTime = 500;
                }
                else
                {
                    tile.FireTime -= elapsedTime;
                }
            }
        }
    }
}
