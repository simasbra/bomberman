using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// Defines the interface for building Player objects
    /// </summary>
    public interface IBuilder
    {
        /// <summary>
        /// Sets the player number identifier
        /// </summary>
        /// <param name="number">The player number (typically 0 or 1)</param>
        /// <returns>The builder instance for method chaining</returns>
        IBuilder SetNumber(byte number);

        /// <summary>
        /// Sets the player's display name
        /// </summary>
        /// <param name="name">The name to display for the player</param>
        /// <returns>The builder instance for method chaining</returns>
        IBuilder SetName(string name);

        /// <summary>
        /// Sets the number of bombs the player can place
        /// </summary>
        /// <param name="bombCount">The initial bomb count</param>
        /// <returns>The builder instance for method chaining</returns>
        IBuilder SetNumBombs(byte bombCount);

        /// <summary>
        /// Sets the number of lives for the player
        /// </summary>
        /// <param name="lives">The initial number of lives</param>
        /// <returns>The builder instance for method chaining</returns>
        IBuilder SetLives(byte lives);

        /// <summary>
        /// Adds a bonus to the player's bonus slot
        /// </summary>
        /// <param name="bonusType">The type of bonus to add</param>
        /// <returns>The builder instance for method chaining</returns>
        IBuilder AddBonus(BonusType bonusType);

        /// <summary>
        /// Builds and returns the configured Player object
        /// </summary>
        /// <returns>The constructed Player instance</returns>
        Player Build();
    }
}
