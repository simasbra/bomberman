// filepath: BombermanMultiplayer/Objects/Mine.cs
using System;
using System.Drawing;
using BombermanMultiplayer.Composite;
using BombermanMultiplayer.Visitor;

namespace BombermanMultiplayer
{
    /// <summary>
    /// Base class for mine explosives
    /// Mines activate when player steps on them
    /// Implements IExplosive for Composite pattern
    /// Implements IVisitable for Visitor pattern
    /// </summary>
    [Serializable]
    public abstract class Mine : GameObject, IDisposable, IExplosive, IVisitable
    {
        public int ActivationTime { get; set; } = 1000;
        public bool IsActivated { get; set; } = false;
        public bool Exploding { get; set; } = false;
        public int Power { get; set; } = 2;
        public short Proprietary { get; set; }
        
        public int InitialDelay { get; set; } = 1000;
        public bool PlayerHasLeftMine { get; set; } = false;

        protected Mine(int row, int col, int totalFrames, int frameWidth, int frameHeight, 
                      int tileWidth, int tileHeight, short owner)
            : base(col * tileWidth, row * tileHeight, totalFrames, frameWidth, frameHeight)
        {
            CasePosition = new int[2] { row, col };
            Proprietary = owner;
            InitialDelay = 1000;
            PlayerHasLeftMine = false;
        }

        public abstract void Activate();
        public abstract void CheckProximity(Player[] players);

        /// <summary>
        /// Handle mine timing and activation logic
        /// </summary>
        public void TimingExplosion(int elapsedTime)
        {
            if (IsActivated)
            {
                ActivationTime -= elapsedTime;
                if (ActivationTime <= 0)
                {
                    Exploding = true;
                }
            }
        }

        /// <summary>
        /// Mine explosion - similar logic to bomb but with mine-specific behavior
        /// </summary>
        public void Explosion(Tile[,] MapGrid, Player[] players)
        {
            int variablePosition = 0;
            bool PropagationUP = true, PropagationDOWN = true, PropagationLEFT = true, PropagationRIGHT = true;

            // Return mine to owner
            for (int i = 0; i < players.Length; i++)
            {
                if (Proprietary == players[i].PlayerNumero)
                {
                    if (players[i].BonusSlot[0] == Objects.BonusType.PowerBomb || players[i].BonusSlot[1] == Objects.BonusType.PowerBomb)
                    {
                        this.Power++;
                    }
                }
            }

            // Check if player is on the mine
            for (int i = 0; i < players.Length; i++)
            {
                if (this.CasePosition[0] == players[i].CasePosition[0] && this.CasePosition[1] == players[i].CasePosition[1]
                    && players[i].BonusSlot[0] != Objects.BonusType.Armor && players[i].BonusSlot[1] != Objects.BonusType.Armor)
                {
                    players[i].Dead = true;
                    players[i].LoadSprite(Properties.Resources.Blood);
                }
            }

            // Explosion propagation
            for (int i = 0; i < this.Power; i++)
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
            this.Dispose();
        }

        public void Dispose()
        {
            Sprite = null;
            GC.SuppressFinalize(this);
        }

        #region IExplosive Implementation (Composite Pattern)

        /// <summary>
        /// Updates the mine's state (IExplosive interface)
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since last update</param>
        /// <param name="mapGrid">The game map grid</param>
        /// <param name="players">Array of players</param>
        public void Update(int elapsedTime, Tile[,] mapGrid, Player[] players)
        {
            if (players != null)
            {
                CheckProximity(players);
            }
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
        /// Checks if the mine is exploding (IExplosive interface)
        /// </summary>
        /// <returns>True if exploding</returns>
        bool IExplosive.IsExploding()
        {
            return Exploding;
        }

        /// <summary>
        /// Gets the position of the mine (IExplosive interface)
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
            visitor?.VisitMine(this);
        }

        #endregion
    }

    [Serializable]
    public class ClassicMine : Mine
    {
        public ClassicMine(int row, int col, int totalFrames, int frameWidth, int frameHeight,
                          int tileWidth, int tileHeight, short owner)
            : base(row, col, totalFrames, frameWidth, frameHeight, tileWidth, tileHeight, owner)
        {
            LoadSprite(Properties.Resources.Mine); // TODO: Add mine sprite
        }

        public override void Activate()
        {
            IsActivated = true;
        }

        public override void CheckProximity(Player[] players)
        {
            if (InitialDelay > 0)
            {
                InitialDelay -= 40; // Subtract one logic frame (40ms)
                
                // While waiting, check if player has LEFT the mine
                bool playerOnMine = false;
                foreach (var player in players)
                {
                    if (!player.Dead && 
                        player.CasePosition[0] == this.CasePosition[0] && 
                        player.CasePosition[1] == this.CasePosition[1])
                    {
                        playerOnMine = true;
                        break;
                    }
                }
                
                // If player was on mine but is gone now, mark as LEFT
                if (!playerOnMine && InitialDelay > 0)
                {
                    PlayerHasLeftMine = true;
                }
                
                return;
            }

            foreach (var player in players)
            {
                if (player.Dead) continue;

                bool playerOnMine = (player.CasePosition[0] == this.CasePosition[0] && 
                                     player.CasePosition[1] == this.CasePosition[1]);

                if (playerOnMine)
                {
                    // Only activate if player left first, then came back
                    if (PlayerHasLeftMine)
                    {
                        Activate();
                        break;
                    }
                }
                else
                {
                    // Player is not on mine anymore - they can come back
                    if (!PlayerHasLeftMine)
                    {
                        PlayerHasLeftMine = true;
                    }
                }
            }
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
            LoadSprite(Properties.Resources.Mine); // TODO: Add advanced mine sprite
        }

        public override void Activate()
        {
            IsActivated = true;
            Exploding = true;
        }

        public override void CheckProximity(Player[] players)
        {
            if (InitialDelay > 0)
            {
                InitialDelay -= 40;
                
                bool playerNearMine = false;
                foreach (var player in players)
                {
                    if (!player.Dead)
                    {
                        int distance = Math.Abs(player.CasePosition[0] - CasePosition[0]) +
                                      Math.Abs(player.CasePosition[1] - CasePosition[1]);
                        if (distance <= 1)
                        {
                            playerNearMine = true;
                            break;
                        }
                    }
                }
                
                if (!playerNearMine && InitialDelay > 0)
                {
                    PlayerHasLeftMine = true;
                }
                
                return;
            }

            if (!IsProximity || IsActivated) return;

            foreach (var player in players)
            {
                if (player.Dead) continue;

                int distance = Math.Abs(player.CasePosition[0] - CasePosition[0]) +
                              Math.Abs(player.CasePosition[1] - CasePosition[1]);

                if (distance <= 1 && PlayerHasLeftMine)
                {
                    Activate();
                    break;
                }
                
                if (distance > 1 && !PlayerHasLeftMine)
                {
                    PlayerHasLeftMine = true;
                }
            }
        }
    }
}