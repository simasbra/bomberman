// filepath: BombermanMultiplayer/Objects/AdvancedExplosiveFactory.cs
using BombermanMultiplayer;

namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// Concrete factory for creating advanced explosive weapons
    /// Creates powerful bombs, proximity mines, and throwable grenades
    /// Used when player collects advanced arsenal bonus
    /// </summary>
    public class AdvancedExplosiveFactory : ExplosiveFactory
    {
        public override Bomb CreateBomb(int row, int col, int tileWidth, int tileHeight, short owner)
        {
            return new AdvancedBomb(row, col, 8, 48, 48, 1500, tileWidth, tileHeight, owner)
            {
                DetonationTime = 1500,
                Power = 5,
                IsScattering = true,
                ScatterRadius = 2
            };
        }

        public override Mine CreateMine(int row, int col, int tileWidth, int tileHeight, short owner)
        {
            return new AdvancedMine(row, col, 4, 48, 48, tileWidth, tileHeight, owner)
            {
                ActivationTime = 500,
                Power = 4,
                IsProximity = true
            };
        }

        public override Grenade CreateGrenade(int row, int col, int tileWidth, int tileHeight, short owner)
        {
            return new AdvancedGrenade(row, col, 6, 48, 48, 1000, tileWidth, tileHeight, owner)
            {
                ThrowDistance = 5,
                Power = 4,
                IsBouncing = true
            };
        }
    }
}