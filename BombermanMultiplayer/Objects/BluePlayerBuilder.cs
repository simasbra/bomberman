using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// Concrete builder for creating Blue team players
    /// </summary>
    public class BluePlayerBuilder : IBuilder
    {
        private byte PlayerNumber;
        private string Name = "Blue Player";
        private byte BombCount = 2;
        private byte Lives = 1;
        private Bomb ExtraBonusSlot;
        private int TileWidth;
        private int TileHeight;
        private int MapSize;

        /// <summary>
        /// Initializes a new instance of the BluePlayerBuilder class
        /// </summary>
        /// <param name="tileWidth">The width of a tile in pixels</param>
        /// <param name="tileHeight">The height of a tile in pixels</param>
        /// <param name="mapSize">The size of the map grid (for positioning at opposite corner)</param>
        public BluePlayerBuilder(int tileWidth, int tileHeight, int mapSize)
        {
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            MapSize = mapSize;
        }

        /// <summary>
        /// Sets the player number identifier
        /// </summary>
        /// <param name="number">The player number</param>
        /// <returns>The builder instance for method chaining</returns>
        public IBuilder SetNumber(byte number)
        {
            PlayerNumber = number;
            return this;
        }

        /// <summary>
        /// Sets the player's display name
        /// </summary>
        /// <param name="name">The name to display for the player</param>
        /// <returns>The builder instance for method chaining</returns>
        public IBuilder SetName(string name)
        {
            Name = name;
            return this;
        }

        /// <summary>
        /// Sets the number of bombs the player can place
        /// </summary>
        /// <param name="bombCount">The initial bomb count</param>
        /// <returns>The builder instance for method chaining</returns>
        public IBuilder SetNumBombs(byte bombCount)
        {
            BombCount = bombCount;
            return this;
        }

        /// <summary>
        /// Sets the number of lives for the player
        /// </summary>
        /// <param name="lives">The initial number of lives</param>
        /// <returns>The builder instance for method chaining</returns>
        public IBuilder SetLived(byte lives)
        {
            Lives = lives;
            return this;
        }

        /// <summary>
        /// Adds an extra bonus bomb to the player
        /// </summary>
        /// <returns>The builder instance for method chaining</returns>
        public IBuilder AddExtra()
        {
            BombCount++;
            return this;
        }

        /// <summary>
        /// Builds and returns the configured Blue Player object
        /// </summary>
        /// <returns>The constructed BluePlayer instance positioned at the blue spawn point (opposite corner from red)</returns>
        public Player Build()
        {
            int spawnPosition = MapSize - 2;

            BluePlayer player = new BluePlayer(
                Lives,
                totalFrames: 3,
                frameWidth: 48,
                frameHeight: 48,
                caseligne: spawnPosition,
                casecolonne: spawnPosition,
                TileWidth,
                TileHeight,
                frameTime: 125,
                PlayerNumber
            );

            player.Name = Name;
            player.BombNumb = BombCount;

            return player;
        }
    }
}
