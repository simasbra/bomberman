using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Objects
{
    public class SpeedBonus : Bonus
    {
        public double SpeedMultiplier { get; set; }
        public int Duration { get; set; }

        public SpeedBonus(int x, int y, int frameNumber, int frameWidth, int frameHeight, double speedMultiplier, int duration)
            : base(x, y, frameNumber, frameWidth, frameHeight, BonusType.SpeedBoost)
        {
            this.SpeedMultiplier = speedMultiplier;
            this.Duration = duration;
        }
    }
}
