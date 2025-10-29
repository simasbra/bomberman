using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Objects
{
    public class HealthBonus : Bonus
    {
        public int Duration { get; set; }
        public int HealthIncrease { get; set; }

        public HealthBonus(int x, int y, int frameNumber, int frameWidth, int frameHeight, int duration, int healthIncrease)
            : base(x, y, frameNumber, frameWidth, frameHeight, BonusType.Armor)
        {
            this.Duration = duration;
            this.HealthIncrease = healthIncrease;
        }
    }
}
