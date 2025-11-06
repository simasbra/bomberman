// filepath: BombermanMultiplayer/Objects/ClassicExplosiveFactory.cs
using BombermanMultiplayer;

namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// Concrete factory for creating classic (basic) explosive weapons
    /// Creates standard bombs, basic mines, and simple grenades
    /// </summary>
    public class ClassicExplosiveFactory : ExplosiveFactory
    {
        public override Bomb CreateBomb(int row, int col, int tileWidth, int tileHeight, short owner)
        {
            return new ClassicBomb(row, col, 8, 48, 48, 2000, tileWidth, tileHeight, owner);
        }

        public override Mine CreateMine(int row, int col, int tileWidth, int tileHeight, short owner)
        {
            return new ClassicMine(row, col, 4, 48, 48, tileWidth, tileHeight, owner)
            {
                ActivationTime = 1000,
                Power = 2
            };
        }

        public override Grenade CreateGrenade(int row, int col, int tileWidth, int tileHeight, short owner)
        {
            return new ClassicGrenade(row, col, 6, 48, 48, 1500, tileWidth, tileHeight, owner)
            {
                ThrowDistance = 3,
                Power = 2
            };
        }
    }
}