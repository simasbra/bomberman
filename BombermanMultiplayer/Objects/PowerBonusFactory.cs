using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Objects
{
    public class PowerBonusFactory : BonusFactory
    {
        public override Bonus CreateBonus(int x, int y, int frameNumber, int frameWidth, int frameHeight)
        {
            return new PowerBonus(x, y, frameNumber, frameWidth, frameHeight, damageIncrease: 1, opponentDamageDecrease: 1);
        }
    }
}
