using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// Concrete factory for creating HealthBonus instances.
    /// Creates health bonuses with a default duration of 5000ms and +1 health increase.
    /// </summary>
    public class HealthBonusFactory : BonusFactory
    {
        /// <summary>
        /// Creates a new HealthBonus with predefined properties
        /// </summary>
        /// <param name="x">The x-coordinate position of the bonus</param>
        /// <param name="y">The y-coordinate position of the bonus</param>
        /// <param name="frameNumber">The frame number for sprite animation</param>
        /// <param name="frameWidth">The width of the bonus sprite frame</param>
        /// <param name="frameHeight">The height of the bonus sprite frame</param>
        /// <returns>A new HealthBonus instance with 5000ms duration and +1 health</returns>
        public override Bonus CreateBonus(int x, int y, int frameNumber, int frameWidth, int frameHeight)
        {
            return new HealthBonus(x, y, frameNumber, frameWidth, frameHeight, duration: 5000, healthIncrease: 1);
        }
    }
}
