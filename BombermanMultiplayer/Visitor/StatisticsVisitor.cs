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
        private HashSet<Bomb> countedBombs;
        private HashSet<Mine> countedMines;
        private HashSet<Grenade> countedGrenades;
        private HashSet<Tile> countedDestroyedTiles;

        /// <summary>
        /// Initializes a new instance of StatisticsVisitor
        /// </summary>
        public StatisticsVisitor()
        {
            this.stats = new GameStatistics();
            this.countedBombs = new HashSet<Bomb>();
            this.countedMines = new HashSet<Mine>();
            this.countedGrenades = new HashSet<Grenade>();
            this.countedDestroyedTiles = new HashSet<Tile>();
        }

        /// <summary>
        /// Visits a Player object and collects statistics.
        /// PlayersAlive is current state (not cumulative), so it's recalculated each time.
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
        /// Resets only the current state statistics (like PlayersAlive), 
        /// but keeps cumulative statistics (bombs, mines, grenades, tiles).
        /// </summary>
        public void ResetCurrentState()
        {
            stats.PlayersAlive = 0;
        }

        /// <summary>
        /// Visits a Bomb object and collects statistics.
        /// Only counts each bomb once (cumulative history).
        /// </summary>
        /// <param name="bomb">The bomb to visit</param>
        public void VisitBomb(Bomb bomb)
        {
            if (bomb == null) return;

            // Only count if we haven't seen this bomb before
            if (!countedBombs.Contains(bomb))
            {
                countedBombs.Add(bomb);
                stats.BombsPlaced++;
            }
        }

        /// <summary>
        /// Visits a Mine object and collects statistics.
        /// Only counts each mine once (cumulative history).
        /// </summary>
        /// <param name="mine">The mine to visit</param>
        public void VisitMine(Mine mine)
        {
            if (mine == null) return;

            // Only count if we haven't seen this mine before (track history)
            if (!countedMines.Contains(mine))
            {
                countedMines.Add(mine);
                stats.MinesPlaced++;
            }
        }

        /// <summary>
        /// Visits a Grenade object and collects statistics.
        /// Only counts each grenade once (cumulative history).
        /// </summary>
        /// <param name="grenade">The grenade to visit</param>
        public void VisitGrenade(Grenade grenade)
        {
            if (grenade == null) return;

            // Only count if we haven't seen this grenade before
            if (!countedGrenades.Contains(grenade))
            {
                countedGrenades.Add(grenade);
                stats.GrenadesThrown++;
            }
        }

        /// <summary>
        /// Visits a Tile object and collects statistics.
        /// Only counts each destroyed tile once (cumulative history).
        /// </summary>
        /// <param name="tile">The tile to visit</param>
        public void VisitTile(Tile tile)
        {
            if (tile == null) return;

            // Count destroyed tiles (walkable but was destroyable) - only once per tile
            if (tile.Walkable && !tile.Destroyable)
            {
                if (!countedDestroyedTiles.Contains(tile))
                {
                    countedDestroyedTiles.Add(tile);
                    stats.TilesDestroyed++;
                }
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
        /// Resets all statistics and tracking sets
        /// </summary>
        public void Reset()
        {
            stats = new GameStatistics();
            countedBombs.Clear();
            countedMines.Clear();
            countedGrenades.Clear();
            countedDestroyedTiles.Clear();
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
