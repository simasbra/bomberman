using BombermanMultiplayer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Decoratorrr
{
    public abstract class BonusDecorator : Bonus
    {
        protected Bonus _wrappedBonus;

        protected BonusDecorator(Bonus bonus)
            : base(bonus.Source.X, bonus.Source.Y, bonus.totalFrames,
                   bonus.Source.Width, bonus.Source.Height, bonus.Type)
        {
            _wrappedBonus = bonus;
        }

        public override double GetSpeedMultiplier()
        {
            return _wrappedBonus.GetSpeedMultiplier();
        }

        public override int GetDuration()
        {
            return _wrappedBonus.GetDuration();
        }

        public override double GetPowerMultiplier()
        {
            return _wrappedBonus.GetPowerMultiplier();
        }

        public override string GetDescription()
        {
            return _wrappedBonus.GetDescription();
        }
    }
}
