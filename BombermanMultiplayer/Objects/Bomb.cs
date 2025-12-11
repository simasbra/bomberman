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
using BombermanMultiplayer.Composite;
using BombermanMultiplayer.Visitor;

namespace BombermanMultiplayer
{
    /// <summary>
    /// Abstract base class for all bomb types
    /// Part of Abstract Factory pattern for creating explosive families
    /// Implements IExplosive for Composite pattern
    /// Implements IVisitable for Visitor pattern
    /// </summary>
    [Serializable]
    public abstract class Bomb : GameObject, IDisposable, IExplosive, IVisitable
    {

        private int _DetonationTime = 2000;
        public bool Exploding = false;
        private int bombPower = 3;

        //Who drops the bomb, player 1 = 1, player 2 = 2
        public short Proprietary;

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
            : base(caseCol * TileWidth, caseLigne * TileHeight, totalFrames, frameWidth, frameHeight)
        {
            CasePosition = new int[2] { caseLigne, caseCol };

            //Define the proprietary player (who drops this bomb)
            this.Proprietary = proprietary;
            this._DetonationTime = detonationTime;

            this._frameTime = DetonationTime / 8;
        }



        public void TimingExplosion(int elsapedTime)
        {
            if (DetonationTime <= 0)
            {
                this.Exploding = true;
            }
            DetonationTime -= elsapedTime;
        }

        #region IExplosive Implementation (Composite Pattern)

        /// <summary>
        /// Updates the bomb's state (IExplosive interface)
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since last update</param>
        /// <param name="mapGrid">The game map grid</param>
        /// <param name="players">Array of players</param>
        public void Update(int elapsedTime, Tile[,] mapGrid, Player[] players)
        {
            UpdateFrame(elapsedTime);
            TimingExplosion(elapsedTime);
        }

        /// <summary>
        /// Triggers the explosion (IExplosive interface)
        /// </summary>
        /// <param name="mapGrid">The game map grid</param>
        /// <param name="players">Array of players</param>
        public void Explode(Tile[,] mapGrid, Player[] players)
        {
            if (Exploding && mapGrid != null && players != null)
            {
                Explosion(mapGrid, players);
            }
        }

        /// <summary>
        /// Checks if the bomb is exploding (IExplosive interface)
        /// </summary>
        /// <returns>True if exploding</returns>
        bool IExplosive.IsExploding()
        {
            return Exploding;
        }

        /// <summary>
        /// Gets the position of the bomb (IExplosive interface)
        /// </summary>
        /// <returns>Point representing the position</returns>
        public Point GetPosition()
        {
            if (CasePosition != null && CasePosition.Length >= 2)
            {
                return new Point(CasePosition[1], CasePosition[0]);
            }
            return new Point(0, 0);
        }

        #endregion

        #region IVisitable Implementation (Visitor Pattern)

        /// <summary>
        /// Accepts a visitor (Visitor pattern)
        /// </summary>
        /// <param name="visitor">The visitor to accept</param>
        public void Accept(IGameObjectVisitor visitor)
        {
            visitor?.VisitBomb(this);
        }

        #endregion

        public void Explosion(Tile[,] MapGrid, Player[] players)
        {
            int variablePosition = 0;

            bool PropagationUP = true, PropagationDOWN = true, PropagationLEFT = true, PropagationRIGHT = true;

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

            for (int i = 0; i < this.bombPower; i++)
            {
                // UP
                if (PropagationUP)
                {
                    if ((variablePosition = this.CasePosition[0] - i) >= 0)
                    {
                        if (variablePosition <= MapGrid.GetLength(0) - 1)
                        {
                            if (MapGrid[variablePosition, this.CasePosition[1]].Destroyable)
                            {
                                MapGrid[variablePosition, this.CasePosition[1]].Destroyable = false;
                                MapGrid[variablePosition, this.CasePosition[1]].Walkable = true;
                                MapGrid[variablePosition, this.CasePosition[1]].Fire = true;
                                MapGrid[variablePosition, this.CasePosition[1]].SpawnBonus();
                            }
                            else if (!MapGrid[variablePosition, this.CasePosition[1]].Destroyable && MapGrid[variablePosition, this.CasePosition[1]].Walkable)
                            {
                                MapGrid[variablePosition, this.CasePosition[1]].Fire = true;
                            }
                            else if (!MapGrid[variablePosition, this.CasePosition[1]].Destroyable && !MapGrid[variablePosition, this.CasePosition[1]].Walkable)
                            {
                                PropagationUP = false;
                            }
                        }
                    }
                }

                // DOWN
                if (PropagationDOWN)
                {
                    if ((variablePosition = this.CasePosition[0] + i) < MapGrid.GetLength(0))
                    {
                        if (variablePosition >= 0)
                        {
                            if (MapGrid[variablePosition, this.CasePosition[1]].Destroyable)
                            {
                                MapGrid[variablePosition, this.CasePosition[1]].Destroyable = false;
                                MapGrid[variablePosition, this.CasePosition[1]].Walkable = true;
                                MapGrid[variablePosition, this.CasePosition[1]].Fire = true;
                                MapGrid[variablePosition, this.CasePosition[1]].SpawnBonus();
                            }
                            else if (!MapGrid[variablePosition, this.CasePosition[1]].Destroyable && MapGrid[variablePosition, this.CasePosition[1]].Walkable)
                            {
                                MapGrid[variablePosition, this.CasePosition[1]].Fire = true;
                            }
                            else if (!MapGrid[variablePosition, this.CasePosition[1]].Destroyable && !MapGrid[variablePosition, this.CasePosition[1]].Walkable)
                            {
                                PropagationDOWN = false;
                            }
                        }
                    }
                }

                // LEFT
                if (PropagationLEFT)
                {
                    if ((variablePosition = this.CasePosition[1] - i) >= 0)
                    {
                        if (variablePosition <= MapGrid.GetLength(1) - 1)
                        {
                            if (MapGrid[this.CasePosition[0], variablePosition].Destroyable)
                            {
                                MapGrid[this.CasePosition[0], variablePosition].Destroyable = false;
                                MapGrid[this.CasePosition[0], variablePosition].Walkable = true;
                                MapGrid[this.CasePosition[0], variablePosition].Fire = true;
                                MapGrid[this.CasePosition[0], variablePosition].SpawnBonus();
                            }
                            else if (!MapGrid[this.CasePosition[0], variablePosition].Destroyable && MapGrid[this.CasePosition[0], variablePosition].Walkable)
                            {
                                MapGrid[this.CasePosition[0], variablePosition].Fire = true;
                            }
                            else if (!MapGrid[this.CasePosition[0], variablePosition].Destroyable && !MapGrid[this.CasePosition[0], variablePosition].Walkable)
                            {
                                PropagationLEFT = false;
                            }
                        }
                    }
                }

                // RIGHT
                if (PropagationRIGHT)
                {
                    if ((variablePosition = this.CasePosition[1] + i) < MapGrid.GetLength(1))
                    {
                        if (variablePosition >= 0)
                        {
                            if (MapGrid[this.CasePosition[0], variablePosition].Destroyable)
                            {
                                MapGrid[this.CasePosition[0], variablePosition].Destroyable = false;
                                MapGrid[this.CasePosition[0], variablePosition].Walkable = true;
                                MapGrid[this.CasePosition[0], variablePosition].Fire = true;
                                MapGrid[this.CasePosition[0], variablePosition].SpawnBonus();
                            }
                            else if (!MapGrid[this.CasePosition[0], variablePosition].Destroyable && MapGrid[this.CasePosition[0], variablePosition].Walkable)
                            {
                                MapGrid[this.CasePosition[0], variablePosition].Fire = true;
                            }
                            else if (!MapGrid[this.CasePosition[0], variablePosition].Destroyable && !MapGrid[this.CasePosition[0], variablePosition].Walkable)
                            {
                                PropagationRIGHT = false;
                            }
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
