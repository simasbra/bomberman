using System;
using System.Collections.Generic;
using BombermanMultiplayer.Objects;

namespace BombermanMultiplayer.Bridge
{
    /// <summary>
    /// Abstract base class for explosion patterns (Bridge)
    /// Defines the interface for different explosion behaviors
    /// </summary>
    [Serializable]
    public abstract class ExplosionPattern
    {
        /// <summary>
        /// Get the list of affected tile coordinates [row, col] for the explosion
        /// </summary>
        /// <param name="mapGrid">The game map grid</param>
        /// <param name="originRow">Row index of explosion origin</param>
        /// <param name="originCol">Column index of explosion origin</param>
        /// <param name="power">Explosion power/radius</param>
        /// <returns>List of affected tile coordinates [row, col]</returns>
        public abstract List<int[]> GetAffectedTiles(Tile[,] mapGrid, int originRow, int originCol, int power);
    }

    /// <summary>
    /// Plus pattern: explosion spreads in all four directions (up, down, left, right)
    /// </summary>
    [Serializable]
    public class PlusPattern : ExplosionPattern
    {
        public override List<int[]> GetAffectedTiles(Tile[,] mapGrid, int originRow, int originCol, int power)
        {
            List<int[]> affectedTiles = new List<int[]>();
            bool propagationUP = true, propagationDOWN = true, propagationLEFT = true, propagationRIGHT = true;

            // Always include the origin tile
            affectedTiles.Add(new int[] { originRow, originCol });

            for (int i = 1; i <= power; i++)
            {
                // UP
                if (propagationUP)
                {
                    int row = originRow - i;
                    if (row >= 0 && row < mapGrid.GetLength(0))
                    {
                        affectedTiles.Add(new int[] { row, originCol });
                        if (!mapGrid[row, originCol].Walkable && !mapGrid[row, originCol].Destroyable)
                        {
                            propagationUP = false;
                        }
                    }
                    else
                    {
                        propagationUP = false;
                    }
                }

                // DOWN
                if (propagationDOWN)
                {
                    int row = originRow + i;
                    if (row >= 0 && row < mapGrid.GetLength(0))
                    {
                        affectedTiles.Add(new int[] { row, originCol });
                        if (!mapGrid[row, originCol].Walkable && !mapGrid[row, originCol].Destroyable)
                        {
                            propagationDOWN = false;
                        }
                    }
                    else
                    {
                        propagationDOWN = false;
                    }
                }

                // LEFT
                if (propagationLEFT)
                {
                    int col = originCol - i;
                    if (col >= 0 && col < mapGrid.GetLength(1))
                    {
                        affectedTiles.Add(new int[] { originRow, col });
                        if (!mapGrid[originRow, col].Walkable && !mapGrid[originRow, col].Destroyable)
                        {
                            propagationLEFT = false;
                        }
                    }
                    else
                    {
                        propagationLEFT = false;
                    }
                }

                // RIGHT
                if (propagationRIGHT)
                {
                    int col = originCol + i;
                    if (col >= 0 && col < mapGrid.GetLength(1))
                    {
                        affectedTiles.Add(new int[] { originRow, col });
                        if (!mapGrid[originRow, col].Walkable && !mapGrid[originRow, col].Destroyable)
                        {
                            propagationRIGHT = false;
                        }
                    }
                    else
                    {
                        propagationRIGHT = false;
                    }
                }
            }
            return affectedTiles;
        }
    }

    /// <summary>
    /// Horizontal pattern: explosion spreads only horizontally (left and right)
    /// </summary>
    [Serializable]
    public class HorizontalPattern : ExplosionPattern
    {
        public override List<int[]> GetAffectedTiles(Tile[,] mapGrid, int originRow, int originCol, int power)
        {
            List<int[]> affectedTiles = new List<int[]>();
            bool propagationLEFT = true, propagationRIGHT = true;

            // Always include the origin tile
            affectedTiles.Add(new int[] { originRow, originCol });

            for (int i = 1; i <= power; i++)
            {
                // LEFT
                if (propagationLEFT)
                {
                    int col = originCol - i;
                    if (col >= 0 && col < mapGrid.GetLength(1))
                    {
                        affectedTiles.Add(new int[] { originRow, col });
                        if (!mapGrid[originRow, col].Walkable && !mapGrid[originRow, col].Destroyable)
                        {
                            propagationLEFT = false;
                        }
                    }
                    else
                    {
                        propagationLEFT = false;
                    }
                }

                // RIGHT
                if (propagationRIGHT)
                {
                    int col = originCol + i;
                    if (col >= 0 && col < mapGrid.GetLength(1))
                    {
                        affectedTiles.Add(new int[] { originRow, col });
                        if (!mapGrid[originRow, col].Walkable && !mapGrid[originRow, col].Destroyable)
                        {
                            propagationRIGHT = false;
                        }
                    }
                    else
                    {
                        propagationRIGHT = false;
                    }
                }
            }
            return affectedTiles;
        }
    }

    /// <summary>
    /// Vertical pattern: explosion spreads only vertically (up and down)
    /// </summary>
    [Serializable]
    public class VerticalPattern : ExplosionPattern
    {
        public override List<int[]> GetAffectedTiles(Tile[,] mapGrid, int originRow, int originCol, int power)
        {
            List<int[]> affectedTiles = new List<int[]>();
            bool propagationUP = true, propagationDOWN = true;

            // Always include the origin tile
            affectedTiles.Add(new int[] { originRow, originCol });

            for (int i = 1; i <= power; i++)
            {
                // UP
                if (propagationUP)
                {
                    int row = originRow - i;
                    if (row >= 0 && row < mapGrid.GetLength(0))
                    {
                        affectedTiles.Add(new int[] { row, originCol });
                        if (!mapGrid[row, originCol].Walkable && !mapGrid[row, originCol].Destroyable)
                        {
                            propagationUP = false;
                        }
                    }
                    else
                    {
                        propagationUP = false;
                    }
                }

                // DOWN
                if (propagationDOWN)
                {
                    int row = originRow + i;
                    if (row >= 0 && row < mapGrid.GetLength(0))
                    {
                        affectedTiles.Add(new int[] { row, originCol });
                        if (!mapGrid[row, originCol].Walkable && !mapGrid[row, originCol].Destroyable)
                        {
                            propagationDOWN = false;
                        }
                    }
                    else
                    {
                        propagationDOWN = false;
                    }
                }
            }
            return affectedTiles;
        }
    }
}

