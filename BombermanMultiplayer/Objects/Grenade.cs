// filepath: BombermanMultiplayer/Objects/Grenade.cs
using System;
using BombermanMultiplayer.Flyweight;

namespace BombermanMultiplayer
{
    /// <summary>
    /// Base class for grenade explosives
    /// Grenades are projectiles that can be thrown
    /// </summary>
    [Serializable]
    public abstract class Grenade : GameObject, IDisposable
    {
        public int ThrowDistance { get; set; } = 3;
        public int DetonationTime { get; set; } = 1500;
        public bool Exploding { get; set; } = false;
        public int Power { get; set; } = 2;
        public short Proprietary { get; set; }
        
        public int Direction { get; set; } = 0; // 0=not thrown, 1=UP, 2=DOWN, 3=LEFT, 4=RIGHT
        public int DistanceTraveled { get; set; } = 0;

        protected Grenade(int row, int col, int totalFrames, int frameWidth, int frameHeight,
                         int detonationTime, int tileWidth, int tileHeight, short owner)
            : base(col * tileWidth, row * tileHeight, totalFrames, frameWidth, frameHeight)
        {
            CasePosition = new int[2] { row, col };
            DetonationTime = detonationTime;
            Proprietary = owner;
        }

        /// <summary>
        /// Throw grenade in a direction
        /// </summary>
        public abstract void Throw(int targetRow, int targetCol);

        /// <summary>
        /// Move grenade projectile
        /// </summary>
        public void MoveGrenade(TileContext[,] mapGrid)
        {
            if (Direction == 0) return; // Not thrown yet

            // Grenades clear their current tile when moving
            mapGrid[CasePosition[0], CasePosition[1]].Occupied = false;

            if (CasePosition != null && mapGrid[CasePosition[0], CasePosition[1]].Occupied)
            {
                mapGrid[CasePosition[0], CasePosition[1]].Occupied = false;
            }
            
            if (DistanceTraveled < ThrowDistance)
            {
                bool canContinue = false;

                switch (Direction)
                {
                    case 1: // UP
                        if (CasePosition[0] - 1 >= 0 && mapGrid[CasePosition[0] - 1, CasePosition[1]].Walkable)
                        {
                            CasePosition[0]--;
                            DistanceTraveled++;
                            canContinue = true;
                        }
                        break;
                    case 2: // DOWN
                        if (CasePosition[0] + 1 < mapGrid.GetLength(0) && mapGrid[CasePosition[0] + 1, CasePosition[1]].Walkable)
                        {
                            CasePosition[0]++;
                            DistanceTraveled++;
                            canContinue = true;
                        }
                        break;
                    case 3: // LEFT
                        if (CasePosition[1] - 1 >= 0 && mapGrid[CasePosition[0], CasePosition[1] - 1].Walkable)
                        {
                            CasePosition[1]--;
                            DistanceTraveled++;
                            canContinue = true;
                        }
                        break;
                    case 4: // RIGHT
                        if (CasePosition[1] + 1 < mapGrid.GetLength(1) && mapGrid[CasePosition[0], CasePosition[1] + 1].Walkable)
                        {
                            CasePosition[1]++;
                            DistanceTraveled++;
                            canContinue = true;
                        }
                        break;
                }
                this.ChangeLocation(this.CasePosition[1] * this.Source.Width,
                                     this.CasePosition[0] * this.Source.Height);
                
                mapGrid[CasePosition[0], CasePosition[1]].Occupied = true;

                
                // If grenade hit obstacle or max distance, stop throwing
                if (!canContinue)
                {
                    Direction = 0;
                }
            }
            else
            {
                Direction = 0; // Stop throwing after max distance
            }
        }

        public void TimingExplosion(int elapsedTime)
        {
            DetonationTime -= elapsedTime;
            if (DetonationTime <= 0)
            {
                Exploding = true;
            }
        }

        /// <summary>
        /// Grenade explosion - similar to bomb but typically with wider radius
        /// </summary>
        public void Explosion(TileContext[,] MapGrid, Player[] players)
        {
            int variablePosition = 0;
            bool PropagationUP = true, PropagationDOWN = true, PropagationLEFT = true, PropagationRIGHT = true;

            // Return grenade to owner
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

            // Check if player is on the grenade
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
                                MapGrid[variablePosition, this.CasePosition[1]].RebindIntrinsic(TileFlyweightFactory.GetTile(TileType.Floor, 1, MapGrid[variablePosition, this.CasePosition[1]].IntrinsicTile_Source_Width, MapGrid[variablePosition, this.CasePosition[1]].IntrinsicTile_Source_Height));
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
                                MapGrid[variablePosition, this.CasePosition[1]].RebindIntrinsic(TileFlyweightFactory.GetTile(TileType.Floor, 1, MapGrid[variablePosition, this.CasePosition[1]].IntrinsicTile_Source_Width, MapGrid[variablePosition, this.CasePosition[1]].IntrinsicTile_Source_Height));
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
                                MapGrid[this.CasePosition[0], variablePosition].RebindIntrinsic(TileFlyweightFactory.GetTile(TileType.Floor, 1, MapGrid[this.CasePosition[0], variablePosition].IntrinsicTile_Source_Width, MapGrid[this.CasePosition[0], variablePosition].IntrinsicTile_Source_Height));
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
                                MapGrid[this.CasePosition[0], variablePosition].RebindIntrinsic(TileFlyweightFactory.GetTile(TileType.Floor, 1, MapGrid[this.CasePosition[0], variablePosition].IntrinsicTile_Source_Width, MapGrid[this.CasePosition[0], variablePosition].IntrinsicTile_Source_Height));
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
    }

    [Serializable]
    public class ClassicGrenade : Grenade
    {
        public ClassicGrenade(int row, int col, int totalFrames, int frameWidth, int frameHeight,
                             int detonationTime, int tileWidth, int tileHeight, short owner)
            : base(row, col, totalFrames, frameWidth, frameHeight, detonationTime, tileWidth, tileHeight, owner)
        {
            Power = 2;  
            ThrowDistance = 3; 
            DetonationTime = 1500; 
            
            LoadSprite(Properties.Resources.Grenade); // TODO: Add grenade sprite
        }

        public override void Throw(int targetRow, int targetCol)
        {
            // Simple throw - determine direction based on target
            int rowDiff = targetRow - CasePosition[0];
            int colDiff = targetCol - CasePosition[1];
            
            if (Math.Abs(rowDiff) > Math.Abs(colDiff))
            {
                // Throw vertically
                Direction = rowDiff > 0 ? 2 : 1; // 2=DOWN, 1=UP
            }
            else
            {
                // Throw horizontally
                Direction = colDiff > 0 ? 4 : 3; // 4=RIGHT, 3=LEFT
            }
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
            Power = 4;
            ThrowDistance = 5;  
            DetonationTime = 1000;     
            
            LoadSprite(Properties.Resources.Grenade); // TODO: Add advanced grenade sprite
        }

        public override void Throw(int targetRow, int targetCol)
        {
            int rowDiff = targetRow - CasePosition[0];
            int colDiff = targetCol - CasePosition[1];
            
            if (Math.Abs(rowDiff) > Math.Abs(colDiff))
            {
                Direction = rowDiff > 0 ? 2 : 1;
            }
            else
            {
                Direction = colDiff > 0 ? 4 : 3;
            }
        }
    }
}