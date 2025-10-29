using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// Concrete factory for creating SpeedBonus instances.
    /// Creates speed bonuses with 1.5x speed multiplier and 5000ms duration.
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
        /// <returns>A new SpeedBonus instance with 1.5x speed multiplier and 5000ms duration</returns>
        public override Bonus CreateBonus(int x, int y, int frameNumber, int frameWidth, int frameHeight)
        {
            return new SpeedBonus(x, y, frameNumber, frameWidth, frameHeight, speedMultiplier: 1.5, duration: 5000);
        }
    }
}
