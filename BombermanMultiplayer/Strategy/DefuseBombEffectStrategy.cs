using BombermanMultiplayer.Objects;
using BombermanMultiplayer.Strategy.Interface.BombermanMultiplayer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Strategy
{
    public class DefuseBombEffectStrategy : IBonusEffectStrategy
    {
        public void Apply(Player player, int slotIndex)
        {
            player.BonusSlot[slotIndex] = BonusType.Desamorce;
            player.BonusTimer[slotIndex] = (short)GetDuration();
        }

        public void Remove(Player player, int slotIndex)
        {
            player.BonusSlot[slotIndex] = BonusType.None;
        }

        public int GetDuration()
        {
            return 10000;
        }

        public BonusType GetBonusType()
        {
            return BonusType.Desamorce;
        }
    }
}
