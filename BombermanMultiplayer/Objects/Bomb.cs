using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Media;
using System.Diagnostics;
using BombermanMultiplayer.Bridge;

namespace BombermanMultiplayer
{
    /// <summary>
    /// Abstract base class for all bomb types
    /// Part of Abstract Factory pattern for creating explosive families
    /// </summary>
    [Serializable]
    public abstract class Bomb : GameObject, IDisposable
    {

        private int _DetonationTime = 2000;
        public bool Exploding = false;
        private int bombPower = 3;

        //Who drops the bomb, player 1 = 1, player 2 = 2
        public short Proprietary;

        // Bridge pattern
        protected ExplosionPattern explosionPattern;

        #region Accessors

        /// <summary>
        /// Explosion power radius
        /// </summary>
        public int Power
        {
            get { return bombPower; }
            set { bombPower = value; }
        }

        public int DetonationTime
        {
            get
            {
                return _DetonationTime;
            }

            set
            {
                if(_DetonationTime > 0)
                _DetonationTime = value;
            }
        }

     


        #endregion


  

        protected Bomb(int caseLigne, int caseCol, int totalFrames, int frameWidth, int frameHeight, int detonationTime, int TileWidth, int TileHeight, short proprietary)
            : this(caseLigne, caseCol, totalFrames, frameWidth, frameHeight, detonationTime, TileWidth, TileHeight, proprietary, null)
        {
        }

        protected Bomb(int caseLigne, int caseCol, int totalFrames, int frameWidth, int frameHeight, int detonationTime, int TileWidth, int TileHeight, short proprietary, ExplosionPattern pattern)
            : base(caseCol * TileWidth, caseLigne * TileHeight, totalFrames, frameWidth, frameHeight)
        {
            CasePosition = new int[2] { caseLigne, caseCol };

            //Define the proprietary player (who drops this bomb)
            this.Proprietary = proprietary;
            this._DetonationTime = detonationTime;

            this._frameTime = DetonationTime / 8;

            // Bridge pattern: Set explosion pattern (defaults to PlusPattern if null)
            this.explosionPattern = pattern ?? new PlusPattern();
        }

        /// <summary>
        /// Set the explosion pattern for this bomb
        /// </summary>
        public void SetExplosionPattern(ExplosionPattern pattern)
        {
            this.explosionPattern = pattern ?? new PlusPattern();
        }

        /// <summary>
        /// Get the current explosion pattern
        /// </summary>
        public ExplosionPattern GetExplosionPattern()
        {
            return explosionPattern;
        }



        public void TimingExplosion(int elsapedTime)
        {
            if (DetonationTime <= 0)
            {
                this.Exploding = true;
            }
            DetonationTime -= elsapedTime;
        }

        public void Explosion(Tile[,] MapGrid, Player[] players)
        {
            // Ensure explosion pattern is set (defaults to PlusPattern)
            if (explosionPattern == null)
            {
                explosionPattern = new PlusPattern();
            }

            // Grąžinti bombą savininkui ir patikrinti bonusą
            for (int i = 0; i < players.Length; i++)
            {
                if (Proprietary == players[i].PlayerNumero)
                {
                    players[i].BombNumb++;
                    if (players[i].BonusSlot[0] == Objects.BonusType.PowerBomb || players[i].BonusSlot[1] == Objects.BonusType.PowerBomb)
                    {
                        this.bombPower++;
                    }
                }
            }

            // Patikrinti ar žaidėjas stovi ant bombos
            for (int i = 0; i < players.Length; i++)
            {
                if (this.CasePosition[0] == players[i].CasePosition[0] && this.CasePosition[1] == players[i].CasePosition[1]
                    && players[i].BonusSlot[0] != Objects.BonusType.Armor && players[i].BonusSlot[1] != Objects.BonusType.Armor)
                {
                    players[i].Dead = true;
                    players[i].LoadSprite(Properties.Resources.Blood);
                }
            }

            // Bridge: use explosion pattern to determine affected tiles
            List<int[]> affectedTiles = explosionPattern.GetAffectedTiles(MapGrid, this.CasePosition[0], this.CasePosition[1], this.bombPower);

            // Apply explosion effects to affected tiles
            foreach (int[] tilePos in affectedTiles)
            {
                int row = tilePos[0];
                int col = tilePos[1];

                if (row >= 0 && row < MapGrid.GetLength(0) && col >= 0 && col < MapGrid.GetLength(1))
                {
                    if (MapGrid[row, col].Destroyable)
                    {
                        MapGrid[row, col].Destroyable = false;
                        MapGrid[row, col].Walkable = true;
                        MapGrid[row, col].Fire = true;
                        MapGrid[row, col].SpawnBonus();
                    }
                    else if (!MapGrid[row, col].Destroyable && MapGrid[row, col].Walkable)
                    {
                        MapGrid[row, col].Fire = true;
                    }

                    // Check if any players are on this tile
                    for (int i = 0; i < players.Length; i++)
                    {
                        if (row == players[i].CasePosition[0] && col == players[i].CasePosition[1]
                            && players[i].BonusSlot[0] != Objects.BonusType.Armor && players[i].BonusSlot[1] != Objects.BonusType.Armor)
                        {
                            players[i].Dead = true;
                            players[i].LoadSprite(Properties.Resources.Blood);
                        }
                    }
                }
            }

            MapGrid[this.CasePosition[0], this.CasePosition[1]].Occupied = false;
            MapGrid[this.CasePosition[0], this.CasePosition[1]].bomb = null;

            this.Dispose();
        }




        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).

                    this.Sprite = null;

                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Bomb() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
             GC.SuppressFinalize(this);
        }
        #endregion









    }
}
