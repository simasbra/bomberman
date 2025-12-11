using System.Collections.Generic;
using BombermanMultiplayer.Objects;

namespace BombermanMultiplayer.Visitor
{
    /// <summary>
    /// Visitor that extracts data from game objects for saving.
    /// Builds SaveGameData by visiting all game objects.
    /// </summary>
    public class SaveGameVisitor : IGameObjectVisitor
    {
        private List<Bomb> bombs;
        private List<Mine> mines;
        private List<Grenade> grenades;
        private List<Player> players;
        private Tile[,] mapGrid;

        /// <summary>
        /// Initializes a new instance of SaveGameVisitor
        /// </summary>
        public SaveGameVisitor()
        {
            this.bombs = new List<Bomb>();
            this.mines = new List<Mine>();
            this.grenades = new List<Grenade>();
            this.players = new List<Player>();
        }

        /// <summary>
        /// Sets the map grid to save
        /// </summary>
        /// <param name="mapGrid">The map grid</param>
        public void SetMapGrid(Tile[,] mapGrid)
        {
            this.mapGrid = mapGrid;
        }

        /// <summary>
        /// Visits a Player object and adds it to save data
        /// </summary>
        /// <param name="player">The player to visit</param>
        public void VisitPlayer(Player player)
        {
            if (player != null)
            {
                players.Add(player);
            }
        }

        /// <summary>
        /// Visits a Bomb object and adds it to save data
        /// </summary>
        /// <param name="bomb">The bomb to visit</param>
        public void VisitBomb(Bomb bomb)
        {
            if (bomb != null && !bomb.Exploding)
            {
                bombs.Add(bomb);
            }
        }

        /// <summary>
        /// Visits a Mine object and adds it to save data
        /// </summary>
        /// <param name="mine">The mine to visit</param>
        public void VisitMine(Mine mine)
        {
            if (mine != null && !mine.Exploding)
            {
                mines.Add(mine);
            }
        }

        /// <summary>
        /// Visits a Grenade object and adds it to save data
        /// </summary>
        /// <param name="grenade">The grenade to visit</param>
        public void VisitGrenade(Grenade grenade)
        {
            if (grenade != null && !grenade.Exploding)
            {
                grenades.Add(grenade);
            }
        }

        /// <summary>
        /// Visits a Tile object (tiles are saved via map grid, not individually)
        /// </summary>
        /// <param name="tile">The tile to visit</param>
        public void VisitTile(Tile tile)
        {
            // Tiles are saved as part of the map grid, not individually
            // This method is here to satisfy the interface but doesn't need to do anything
        }

        /// <summary>
        /// Gets the save data built from visited objects
        /// </summary>
        /// <returns>SaveGameData object containing all collected data</returns>
        public SaveGameData GetSaveData()
        {
            // Convert players list to array
            Player[] playersArray = players.ToArray();

            // Create SaveGameData with bombs and map grid
            // Note: SaveGameData currently only supports bombs, not mines/grenades
            // This could be extended in the future
            return new SaveGameData(bombs, mapGrid, playersArray);
        }

        /// <summary>
        /// Resets the visitor to start a new save operation
        /// </summary>
        public void Reset()
        {
            bombs.Clear();
            mines.Clear();
            grenades.Clear();
            players.Clear();
            mapGrid = null;
        }
    }
}
