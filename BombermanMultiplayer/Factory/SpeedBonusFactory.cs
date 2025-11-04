using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BombermanMultiplayer.Objects;

namespace BombermanMultiplayer.Factory
{
    /// <summary>
    /// Concrete factory for creating SpeedBonus instances.
    /// Creates speed bonuses with a default duration of 5000ms and +5 speed increase.
    /// </summary>
    public class SpeedBonusFactory : BonusFactory
    {
        /// <summary>
        /// Creates a new SpeedBonus with predefined properties
        /// </summary>
        /// <param name="x">The x-coordinate position of the bonus</param>
        /// <param name="y">The y-coordinate position of the bonus</param>
        /// <param name="frameNumber">The frame number for sprite animation</param>
        /// <param name="frameWidth">The width of the bonus sprite frame</param>
        /// <param name="frameHeight">The height of the bonus sprite frame</param>
        /// <returns>A new SpeedBonus instance with 5000ms duration and +5 speed</returns>
        public override Bonus CreateBonus(int x, int y, int frameNumber, int frameWidth, int frameHeight)
        {
            return new SpeedBonus(x, y, frameNumber, frameWidth, frameHeight, duration: 5000, speedIncrease: 5);
        }
    }
}
