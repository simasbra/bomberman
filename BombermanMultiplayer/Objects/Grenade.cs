// filepath: BombermanMultiplayer/Objects/Grenade.cs
using System;

namespace BombermanMultiplayer
{
    /// <summary>
    /// Base class for grenade explosives
    /// Grenades can be thrown over obstacles
    /// </summary>
    [Serializable]
    public abstract class Grenade : GameObject, IDisposable
    {
        public int ThrowDistance { get; set; } = 3;
        public int DetonationTime { get; set; } = 1500;
        public bool Exploding { get; set; } = false;
        public int Power { get; set; } = 2;
        public short Proprietary { get; set; }

        protected Grenade(int row, int col, int totalFrames, int frameWidth, int frameHeight,
                         int detonationTime, int tileWidth, int tileHeight, short owner)
            : base(col * tileWidth, row * tileHeight, totalFrames, frameWidth, frameHeight)
        {
            CasePosition = new int[2] { row, col };
            DetonationTime = detonationTime;
            Proprietary = owner;
        }

        public abstract void Throw(int targetRow, int targetCol);

        public void TimingExplosion(int elapsedTime)
        {
            DetonationTime -= elapsedTime;
            if (DetonationTime <= 0)
            {
                Exploding = true;
            }
        }

        public void Dispose()
        {
            Sprite = null;
            GC.SuppressFinalize(this);
        }
    }

    [Serializable]
    public class ClassicGrenade : Grenade
    {
        public ClassicGrenade(int row, int col, int totalFrames, int frameWidth, int frameHeight,
                             int detonationTime, int tileWidth, int tileHeight, short owner)
            : base(row, col, totalFrames, frameWidth, frameHeight, detonationTime, tileWidth, tileHeight, owner)
        {
            LoadSprite(Properties.Resources.Bombe); // TODO: Add grenade sprite
        }

        public override void Throw(int targetRow, int targetCol)
        {
            // Simple throw - direct path
            CasePosition[0] = targetRow;
            CasePosition[1] = targetCol;
        }
    }

    [Serializable]
    public class AdvancedGrenade : Grenade
    {
        public bool IsBouncing { get; set; } = true;

        public AdvancedGrenade(int row, int col, int totalFrames, int frameWidth, int frameHeight,
                              int detonationTime, int tileWidth, int tileHeight, short owner)
            : base(row, col, totalFrames, frameWidth, frameHeight, detonationTime, tileWidth, tileHeight, owner)
        {
            LoadSprite(Properties.Resources.Bombe); // TODO: Add advanced grenade sprite
        }

        public override void Throw(int targetRow, int targetCol)
        {
            // Advanced throw with bouncing
            CasePosition[0] = targetRow;
            CasePosition[1] = targetCol;
            // TODO: Implement bouncing logic
        }
    }
}