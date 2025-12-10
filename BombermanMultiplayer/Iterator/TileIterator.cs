using System;

namespace BombermanMultiplayer.Iterator
{
    /// <summary>
    /// Iterator for traversing the 2D Tile[,] grid in World.MapGrid
    /// </summary>
    public class TileIterator : IGameIterator<Tile>
    {
        private readonly Tile[,] Grid;
        private int Row = 0;
        private int Column = -1;
        private readonly int MaxRows;
        private readonly int MaxColumns;

        public TileIterator(Tile[,] grid)
        {
            Grid = grid;
            MaxRows = grid.GetLength(0);
            MaxColumns = grid.GetLength(1);
        }

        public bool HasNext()
        {
            return Row < MaxRows && (Column < MaxColumns - 1 || Row < MaxRows - 1);
        }

        public Tile Next()
        {
            if (!HasNext())
            {
                throw new InvalidOperationException("No more tiles in the grid");
            }

            Column++;
            if (Column >= MaxColumns)
            {
                Column = 0;
                Row++;
            }

            return Grid[Row, Column];
        }

        public void Remove()
        {
            throw new NotSupportedException("Tiles cannot be removed from the map grid");
        }
    }
}