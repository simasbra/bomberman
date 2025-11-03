using BombermanMultiplayer;

namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// Abstract Factory interface for creating families of explosive objects
    /// Problem: When player picks up advanced bonus, their entire arsenal should upgrade
    /// Solution: Factory creates consistent family of weapons (bombs, mines, grenades)
    /// </summary>
    public abstract class ExplosiveFactory
    {
        public abstract Bomb CreateBomb(int row, int col, int tileWidth, int tileHeight, short owner);
        public abstract Mine CreateMine(int row, int col, int tileWidth, int tileHeight, short owner);
        public abstract Grenade CreateGrenade(int row, int col, int tileWidth, int tileHeight, short owner);
    }
}