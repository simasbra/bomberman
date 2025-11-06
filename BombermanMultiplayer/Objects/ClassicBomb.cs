using System;
using System.Collections.Generic;
using System.Text;

namespace BombermanMultiplayer
{
    /// <summary>
    /// Classic (basic) bomb with standard properties
    /// Created by ClassicExplosiveFactory
    /// </summary>
    [Serializable]
    public class ClassicBomb : Bomb
    {
        public ClassicBomb(int caseLigne, int caseCol, int totalFrames, int frameWidth, int frameHeight,
                          int detonationTime, int TileWidth, int TileHeight, short proprietary)
            : base(caseLigne, caseCol, totalFrames, frameWidth, frameHeight, detonationTime, TileWidth, TileHeight, proprietary)
        {
            // Load classic bomb sprite
            this.LoadSprite(Properties.Resources.Bombe);

            // Classic bomb default properties
            this.Power = 3;
            this.DetonationTime = 2000;
            this.Exploding = false;
        }
    }
}
