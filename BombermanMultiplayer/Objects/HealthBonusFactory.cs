using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Objects
{
    public class HealthBonusFactory : BonusFactory
    {
        public override Bonus CreateBonus(int x, int y, int frameNumber, int frameWidth, int frameHeight)
        {
            return new HealthBonus(x, y, frameNumber, frameWidth, frameHeight, duration: 5000, healthIncrease: 1);
        }
    }
}
