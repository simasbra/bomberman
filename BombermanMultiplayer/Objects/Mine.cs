// filepath: BombermanMultiplayer/Objects/Mine.cs
using System;

namespace BombermanMultiplayer
{
    /// <summary>
    /// Base class for mine explosives
    /// Mines activate when player steps on them
    /// </summary>
    [Serializable]
    public abstract class Mine : GameObject, IDisposable
    {
        public int ActivationTime { get; set; } = 1000;
        public bool IsActivated { get; set; } = false;
        public bool Exploding { get; set; } = false;
        public int Power { get; set; } = 2;
        public short Proprietary { get; set; }

        protected Mine(int row, int col, int totalFrames, int frameWidth, int frameHeight, 
                      int tileWidth, int tileHeight, short owner)
            : base(col * tileWidth, row * tileHeight, totalFrames, frameWidth, frameHeight)
        {
            CasePosition = new int[2] { row, col };
            Proprietary = owner;
        }

        public abstract void Activate();
        public abstract void CheckProximity(Player[] players);

        public void Dispose()
        {
            Sprite = null;
            GC.SuppressFinalize(this);
        }
    }

    [Serializable]
    public class ClassicMine : Mine
    {
        public ClassicMine(int row, int col, int totalFrames, int frameWidth, int frameHeight,
                          int tileWidth, int tileHeight, short owner)
            : base(row, col, totalFrames, frameWidth, frameHeight, tileWidth, tileHeight, owner)
        {
            LoadSprite(Properties.Resources.Bombe); // TODO: Add mine sprite
        }

        public override void Activate()
        {
            IsActivated = true;
        }

        public override void CheckProximity(Player[] players)
        {
            // Classic mines don't check proximity
        }
    }

    [Serializable]
    public class AdvancedMine : Mine
    {
        public bool IsProximity { get; set; } = true;

        public AdvancedMine(int row, int col, int totalFrames, int frameWidth, int frameHeight,
                           int tileWidth, int tileHeight, short owner)
            : base(row, col, totalFrames, frameWidth, frameHeight, tileWidth, tileHeight, owner)
        {
            LoadSprite(Properties.Resources.Bombe); // TODO: Add advanced mine sprite
        }

        public override void Activate()
        {
            IsActivated = true;
            Exploding = true;
        }

        public override void CheckProximity(Player[] players)
        {
            if (!IsProximity || IsActivated) return;

            foreach (var player in players)
            {
                if (player.Dead) continue;

                int distance = Math.Abs(player.CasePosition[0] - CasePosition[0]) +
                              Math.Abs(player.CasePosition[1] - CasePosition[1]);

                if (distance <= 1)
                {
                    Activate();
                    break;
                }
            }
        }
    }
}