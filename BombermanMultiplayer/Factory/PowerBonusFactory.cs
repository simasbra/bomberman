using System;
using BombermanMultiplayer.Objects;

namespace BombermanMultiplayer.Factory
{
    /// <summary>
    /// Concrete factory for creating PowerBonus instances.
    /// Creates power bonuses with a default duration of 5000ms and +1 damage increase.
    /// </summary>
    public class PowerBonusFactory : BonusFactory
    {
        /// <summary>
        /// Creates a new PowerBonus with predefined properties
        /// </summary>
        /// <param name="x">The x-coordinate position of the bonus</param>
        /// <param name="y">The y-coordinate position of the bonus</param>
        /// <param name="frameNumber">The frame number for sprite animation</param>
        /// <param name="frameWidth">The width of the bonus sprite frame</param>
        /// <param name="frameHeight">The height of the bonus sprite frame</param>
        /// <returns>A new PowerBonus instance with 5000ms duration and +1 damage</returns>
        public override Bonus CreateBonus(int x, int y, int frameNumber, int frameWidth, int frameHeight)
        {
            return new PowerBonus(x, y, frameNumber, frameWidth, frameHeight, damageIncrease: 1, opponentDamageDecrease: 1);
        }
    }
}