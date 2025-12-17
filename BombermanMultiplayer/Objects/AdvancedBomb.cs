using System;
using System.Collections.Generic;
using System.Text;
using BombermanMultiplayer.Flyweight;

namespace BombermanMultiplayer
{
    /// <summary>
    /// Advanced bomb with enhanced properties
    /// Created by AdvancedExplosiveFactory
    /// Has higher power, faster detonation, and optional scattering effect
    /// </summary>
    [Serializable]
    public class AdvancedBomb : Bomb
    {
        /// <summary>
        /// If true, explosion scatters in irregular pattern
        /// </summary>
        public bool IsScattering { get; set; } = false;

        /// <summary>
        /// Additional scatter radius for advanced effect
        /// </summary>
        public int ScatterRadius { get; set; } = 0;

        public AdvancedBomb(int caseLigne, int caseCol, int totalFrames, int frameWidth, int frameHeight, 
                           int detonationTime, int TileWidth, int TileHeight, short proprietary)
            : base(caseLigne, caseCol, totalFrames, frameWidth, frameHeight, detonationTime, TileWidth, TileHeight, proprietary)
        {
            // Load advanced bomb sprite (for now using same sprite, you can change later)
            this.LoadSprite(Properties.Resources.Bombe);
            
            // Advanced bomb enhanced properties
            this.Power = 5;
            this.DetonationTime = detonationTime;
            this.IsScattering = true;
            this.ScatterRadius = 2;
        }

        /// <summary>
        /// Activate the advanced bomb's scattering effect
        /// </summary>
        public void Activate()
        {
            this.IsScattering = true;
            this.Exploding = true;
        }

        /// <summary>
        /// Execute the explosion with scattering effect
        /// Enhanced explosion that spreads in irregular pattern with increased scatter radius
        /// </summary>
        public void Execute(TileContext[,] MapGrid, Player[] players)
        {
            if (!this.IsScattering)
            {
                // If scattering is disabled, use base explosion
                this.Explosion(MapGrid, players);
                return;
            }

            // Advanced explosion with scattering effect
            int variablePosition = 0;
            bool PropagationUP = true, PropagationDOWN = true, PropagationLEFT = true, PropagationRIGHT = true;

            // Return bomb to owner and check for bonuses
            for (int i = 0; i < players.Length; i++)
            {
                if (this.Proprietary == players[i].PlayerNumero)
                {
                    players[i].BombNumb++;
                    if (players[i].BonusSlot[0] == Objects.BonusType.PowerBomb || players[i].BonusSlot[1] == Objects.BonusType.PowerBomb)
                    {
                        this.Power++;
                    }
                }
            }

            // Check if player is standing on bomb
            for (int i = 0; i < players.Length; i++)
            {
                if (this.CasePosition[0] == players[i].CasePosition[0] && this.CasePosition[1] == players[i].CasePosition[1]
                    && players[i].BonusSlot[0] != Objects.BonusType.Armor && players[i].BonusSlot[1] != Objects.BonusType.Armor)
                {
                    players[i].Dead = true;
                    players[i].LoadSprite(Properties.Resources.Blood);
                }
            }

            // Scattering propagation with additional scatter radius
            for (int i = 0; i < this.Power + this.ScatterRadius; i++)
            {
                // UP - with random scatter
                if (PropagationUP && ShouldScatterContinue())
                {
                    if ((variablePosition = this.CasePosition[0] - i) >= 0)
                    {
                        if (variablePosition <= MapGrid.GetLength(0) - 1)
                        {
                            ApplyExplosionEffect(MapGrid, variablePosition, this.CasePosition[1], ref PropagationUP);
                        }
                    }
                }

                // DOWN - with random scatter
                if (PropagationDOWN && ShouldScatterContinue())
                {
                    if ((variablePosition = this.CasePosition[0] + i) < MapGrid.GetLength(0))
                    {
                        if (variablePosition >= 0)
                        {
                            ApplyExplosionEffect(MapGrid, variablePosition, this.CasePosition[1], ref PropagationDOWN);
                        }
                    }
                }

                // LEFT - with random scatter
                if (PropagationLEFT && ShouldScatterContinue())
                {
                    if ((variablePosition = this.CasePosition[1] - i) >= 0)
                    {
                        if (variablePosition <= MapGrid.GetLength(1) - 1)
                        {
                            ApplyExplosionEffect(MapGrid, this.CasePosition[0], variablePosition, ref PropagationLEFT);
                        }
                    }
                }

                // RIGHT - with random scatter
                if (PropagationRIGHT && ShouldScatterContinue())
                {
                    if ((variablePosition = this.CasePosition[1] + i) < MapGrid.GetLength(1))
                    {
                        if (variablePosition >= 0)
                        {
                            ApplyExplosionEffect(MapGrid, this.CasePosition[0], variablePosition, ref PropagationRIGHT);
                        }
                    }
                }
            }

            MapGrid[this.CasePosition[0], this.CasePosition[1]].Occupied = false;
            MapGrid[this.CasePosition[0], this.CasePosition[1]].bomb = null;

            this.Dispose();
        }

        /// <summary>
        /// Determine if explosion should continue scattering (random chance)
        /// </summary>
        private bool ShouldScatterContinue()
        {
            // Randomly decide if scattering should continue (80% chance)
            Random random = new Random();
            return random.Next(100) < 80;
        }

        /// <summary>
        /// Apply explosion effect to a specific tile
        /// </summary>
        private void ApplyExplosionEffect(TileContext[,] MapGrid, int row, int col, ref bool propagation)
        {
            if (MapGrid[row, col].Destroyable)
            {
                MapGrid[row, col].Destroyable = false;
                MapGrid[row, col].Walkable = true;
                MapGrid[row, col].Fire = true;
                MapGrid[row, col].SpawnBonus();
                MapGrid[row, col].RebindIntrinsic(TileFlyweightFactory.GetTile(TileType.Floor, 1, MapGrid[row, col].IntrinsicTile_Source_Width, MapGrid[row, col].IntrinsicTile_Source_Height));
            }
            else if (!MapGrid[row, col].Destroyable && MapGrid[row, col].Walkable)
            {
                MapGrid[row, col].Fire = true;
            }
            else if (!MapGrid[row, col].Destroyable && !MapGrid[row, col].Walkable)
            {
                propagation = false;
            }

            // In Flyweight, we don't need to manually LoadSprite(Fire) anymore
            // as TileContext.Draw handles it based on Fire property.
        }
    }
}
