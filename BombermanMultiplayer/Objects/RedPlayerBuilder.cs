using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// Concrete builder for creating Red team players
    /// </summary>
    public class RedPlayerBuilder : IBuilder
    {
        private byte PlayerNumber;
        private string Name = "Red Player";
        private byte BombCount = 2;
        private byte Lives = 1;
        private List<BonusType> Bonuses = new List<BonusType>();
        private int TileWidth;
        private int TileHeight;

        /// <summary>
        /// Initializes a new instance of the RedPlayerBuilder class
        /// </summary>
        /// <param name="tileWidth">The width of a tile in pixels</param>
        /// <param name="tileHeight">The height of a tile in pixels</param>
        public RedPlayerBuilder(int tileWidth, int tileHeight)
        {
            TileWidth = tileWidth;
            TileHeight = tileHeight;
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
        public IBuilder SetLives(byte lives)
        {
            Lives = lives;
            return this;
        }

        /// <summary>
        /// Adds a bonus to the player's bonus slot
        /// </summary>
        /// <param name="bonusType">The type of bonus to add</param>
        /// <returns>The builder instance for method chaining</returns>
        public IBuilder AddBonus(BonusType bonusType)
        {
            if (Bonuses.Count < 2)
            {
                Bonuses.Add(bonusType);
            }
            return this;
        }

        /// <summary>
        /// Builds and returns the configured Red Player object
        /// </summary>
        /// <returns>The constructed RedPlayer instance positioned at the red spawn point (1,1)</returns>
        public Player Build()
        {
            RedPlayer player = new RedPlayer(
                Lives,
                totalFrames: 3,
                frameWidth: 48,
                frameHeight: 48,
                caseligne: 1,
                casecolonne: 1,
                TileWidth,
                TileHeight,
                frameTime: 125,
                PlayerNumber
            );

            player.Name = Name;
            player.BombNumb = BombCount;

            for (int i = 0; i < Bonuses.Count && i < 2; i++)
            {
                player.BonusSlot[i] = Bonuses[i];
            }

            return player;
        }
    }
}
