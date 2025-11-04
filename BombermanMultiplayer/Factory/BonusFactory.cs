using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BombermanMultiplayer.Objects;

namespace BombermanMultiplayer.Factory
{
    /// <summary>
    /// Abstract factory class for creating different types of bonus objects.
    /// Implements the Factory Method pattern to allow subclasses to define specific bonus creation logic.
    /// </summary>
    public abstract class BonusFactory
    {
        /// <summary>
        /// Creates a specific type of bonus with the given parameters.
        /// Must be implemented by concrete factory classes.
        /// </summary>
        /// <param name="x">The x-coordinate position of the bonus</param>
        /// <param name="y">The y-coordinate position of the bonus</param>
        /// <param name="frameNumber">The frame number for sprite animation</param>
        /// <param name="frameWidth">The width of the bonus sprite frame</param>
        /// <param name="frameHeight">The height of the bonus sprite frame</param>
        /// <returns>A new instance of a Bonus or its derived type</returns>
        public abstract Bonus CreateBonus(int x, int y, int frameNumber, int frameWidth, int frameHeight);
    }
}
