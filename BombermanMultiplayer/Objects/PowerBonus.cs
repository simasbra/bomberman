using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Objects
{
    public class PowerBonus : Bonus
    {
        public int DamageIncrease { get; set; }
        public int OpponentDamageDecrease { get; set; }

        public PowerBonus(int x, int y, int frameNumber, int frameWidth, int frameHeight, int damageIncrease, int opponentDamageDecrease)
            : base(x, y, frameNumber, frameWidth, frameHeight, BonusType.PowerBomb)
        {
            this.DamageIncrease = damageIncrease;
            this.OpponentDamageDecrease = opponentDamageDecrease;
        }
    }
}
