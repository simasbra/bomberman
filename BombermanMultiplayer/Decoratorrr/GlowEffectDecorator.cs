using BombermanMultiplayer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Decoratorrr
{
    public class GlowEffectDecorator : BonusDecorator
    {
        private string _glowColor;
        private int _glowIntensity;

        public GlowEffectDecorator(Bonus bonus, string glowColor, int glowIntensity)
            : base(bonus)
        {
            _glowColor = glowColor;
            _glowIntensity = glowIntensity;
        }

        public string GlowColor => _glowColor;

        public int GlowIntensity => _glowIntensity;

        public override string GetDescription()
        {
            return $"{_wrappedBonus.GetDescription()} + Glow({_glowColor}, {_glowIntensity}%)";
        }
    }
}
