using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Objects
{
    public class SpeedBonusFactory : BonusFactory
    {
        public override Bonus CreateBonus(int x, int y, int frameNumber, int frameWidth, int frameHeight)
        {
            return new SpeedBonus(x, y, frameNumber, frameWidth, frameHeight, speedMultiplier: 1.5, duration: 5000);
        }
    }
}
