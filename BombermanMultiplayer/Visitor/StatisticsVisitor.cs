using System.Collections.Generic;
using BombermanMultiplayer.Objects;

namespace BombermanMultiplayer.Visitor
{
    /// <summary>
    /// Visitor that collects game statistics from game objects.
    /// </summary>
    public class StatisticsVisitor : IGameObjectVisitor
    {
        private GameStatistics stats;

        /// <summary>
        /// Initializes a new instance of StatisticsVisitor
        /// </summary>
        public StatisticsVisitor()
        {
            this.stats = new GameStatistics();
        }

        /// <summary>
        /// Visits a Player object and collects statistics
        /// </summary>
        /// <param name="player">The player to visit</param>
        public void VisitPlayer(Player player)
        {
            if (player == null) return;

            if (!player.Dead)
            {
                stats.PlayersAlive++;
            }
        }

        /// <summary>
        /// Visits a Bomb object and collects statistics
        /// </summary>
        /// <param name="bomb">The bomb to visit</param>
        public void VisitBomb(Bomb bomb)
        {
            if (bomb == null) return;

            stats.BombsPlaced++;
        }

        /// <summary>
        /// Visits a Mine object and collects statistics
        /// </summary>
        /// <param name="mine">The mine to visit</param>
        public void VisitMine(Mine mine)
        {
            if (mine == null) return;

            stats.MinesPlaced++;
        }

        /// <summary>
        /// Visits a Grenade object and collects statistics
        /// </summary>
        /// <param name="grenade">The grenade to visit</param>
        public void VisitGrenade(Grenade grenade)
        {
            if (grenade == null) return;

            stats.GrenadesThrown++;
        }

        /// <summary>
        /// Visits a Tile object and collects statistics
        /// </summary>
        /// <param name="tile">The tile to visit</param>
        public void VisitTile(Tile tile)
        {
            if (tile == null) return;

            // Count destroyed tiles (walkable but was destroyable)
            if (tile.Walkable && !tile.Destroyable)
            {
                stats.TilesDestroyed++;
            }
        }

        /// <summary>
        /// Gets the collected statistics
        /// </summary>
        /// <returns>GameStatistics object containing collected data</returns>
        public GameStatistics GetStatistics()
        {
            return stats;
        }

        /// <summary>
        /// Resets all statistics
        /// </summary>
        public void Reset()
        {
            stats = new GameStatistics();
        }
    }

    /// <summary>
    /// Data class for game statistics
    /// </summary>
    public class GameStatistics
    {
        public int BombsPlaced { get; set; } = 0;
        public int TilesDestroyed { get; set; } = 0;
        public int PlayersAlive { get; set; } = 0;
        public int MinesPlaced { get; set; } = 0;
        public int GrenadesThrown { get; set; } = 0;
    }
}
