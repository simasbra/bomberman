using System;
using BombermanMultiplayer.Objects;

namespace BombermanMultiplayer.Factory
{
    /// <summary>
    /// Concrete factory for creating DefuseBonus instances.
    /// Creates defuse bonuses with a default duration of 5000ms and the ability to defuse bombs.
    /// </summary>
    public class DefuseBonusFactory : BonusFactory
    {
        /// <summary>
        /// Creates a new DefuseBonus with predefined properties
        /// </summary>
        /// <param name="x">The x-coordinate position of the bonus</param>
        /// <param name="y">The y-coordinate position of the bonus</param>
        /// <param name="frameNumber">The frame number for sprite animation</param>
        /// <param name="frameWidth">The width of the bonus sprite frame</param>
        /// <param name="frameHeight">The height of the bonus sprite frame</param>
        /// <returns>A new DefuseBonus instance with 5000ms duration and defuse capability</returns>
        public override Bonus CreateBonus(int x, int y, int frameNumber, int frameWidth, int frameHeight)
        {
            return new DefuseBonus(x, y, frameNumber, frameWidth, frameHeight, duration: 5000);
        }
    }
}