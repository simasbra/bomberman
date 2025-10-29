using BombermanMultiplayer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Strategy.Interface
{
    namespace BombermanMultiplayer.Objects
    {
        public interface IBonusEffectStrategy
        {
            void Apply(Player player, int slotIndex);
            void Remove(Player player, int slotIndex);
            int GetDuration();
            BonusType GetBonusType();
        }
    }
}
