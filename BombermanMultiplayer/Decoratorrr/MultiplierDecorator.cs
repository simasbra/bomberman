using BombermanMultiplayer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Decoratorrr
{
    public class MultiplierDecorator : BonusDecorator
    {
        private double _multiplier;

        public MultiplierDecorator(Bonus bonus, double multiplier)
            : base(bonus)
        {
            _multiplier = multiplier;
        }

        public override double GetSpeedMultiplier()
        {
            return _wrappedBonus.GetSpeedMultiplier() * _multiplier;
        }

        public override double GetPowerMultiplier()
        {
            return _wrappedBonus.GetPowerMultiplier() * _multiplier;
        }

        public override string GetDescription()
        {
            return $"{_wrappedBonus.GetDescription()} + Multiplier(x{_multiplier})";
        }
    }
}
