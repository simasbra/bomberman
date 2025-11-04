using BombermanMultiplayer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Decoratorrr
{
    public class DurationDecorator : BonusDecorator
    {
        private int _additionalDuration;

        public DurationDecorator(Bonus bonus, int additionalDuration)
            : base(bonus)
        {
            _additionalDuration = additionalDuration;
        }

        public override int GetDuration()
        {
            return _wrappedBonus.GetDuration() + _additionalDuration;
        }

        public override string GetDescription()
        {
            return $"{_wrappedBonus.GetDescription()} + ExtendedDuration(+{_additionalDuration}ms)";
        }
    }
}
