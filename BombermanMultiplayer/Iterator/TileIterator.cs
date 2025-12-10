using System;

namespace BombermanMultiplayer.Iterator
{
    /// <summary>
    /// Iterator for traversing the 2D Tile[,] grid in World.MapGrid
    /// </summary>
    public class TileIterator : IIterator<Tile>
    {
        private readonly Tile[,] Grid;
        private int Row = 0;
        private int Column = -1;
        private readonly int MaxRows;
        private readonly int MaxColumns;

        /// <summary>
        /// Iterator for traversing the 2D Tile[,] grid in World.MapGrid.
        /// </summary>
        /// <param name="grid">2D tile grid</param>
        public TileIterator(Tile[,] grid)
        {
            Grid = grid;
            MaxRows = grid.GetLength(0);
            MaxColumns = grid.GetLength(1);
        }

        /// <summary>
        /// Determines if there are more elements to iterate over.
        /// </summary>
        /// <returns>True if there are more elements, otherwise false.</returns>
        public bool HasNext()
        {
            return Row < MaxRows && (Column < MaxColumns - 1 || Row < MaxRows - 1);
        }

        /// <summary>
        /// Gets the next item in the iteration sequence.
        /// </summary>
        /// <returns>The next tile in the grid.</returns>
        /// <exception cref="InvalidOperationException">Thrown when there are no more tiles to iterate through.</exception>
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

        /// <summary>
        /// Removes the current item from the iterator.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when called on an empty iterator or after the last element has been removed.</exception>
        public void Remove()
        {
            throw new NotSupportedException("Tiles cannot be removed from the map grid");
        }
    }
}