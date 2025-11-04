using BombermanMultiplayer.Objects;
using BombermanMultiplayer.Strategy.Interface.BombermanMultiplayer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Strategy
{
    public class SpeedBoostEffectStrategy : IBonusEffectStrategy
    {
        public void Apply(Player player, int slotIndex, Bonus bonus)
        {
            int newSpeed = (int)(player.Vitesse * bonus.GetSpeedMultiplier());
            player.Vitesse = (byte)Math.Min(255, newSpeed);
            player.BonusSlot[slotIndex] = BonusType.SpeedBoost;
            player.BonusTimer[slotIndex] = (short)bonus.GetDuration();
        }

        public void Remove(Player player, int slotIndex)
        {
            player.Vitesse /= 2;
            player.BonusSlot[slotIndex] = BonusType.None;
        }

        public int GetDuration()
        {
            return 5000;
        }

        public BonusType GetBonusType()
        {
            return BonusType.SpeedBoost;
        }
    }
}
