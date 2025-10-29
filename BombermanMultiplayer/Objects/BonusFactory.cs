using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Objects
{
    public abstract class BonusFactory
    {
        public abstract Bonus CreateBonus(int x, int y, int frameNumber, int frameWidth, int frameHeight);
    }
}
