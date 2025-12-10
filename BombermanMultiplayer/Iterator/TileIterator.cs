namespace BombermanMultiplayer.Iterator
{
    public class TileIterator : IGameIterator<Tile>
    {
        private readonly Tile[,] grid;
        private int row = 0;
        private int col = -1;
        private readonly int maxRow, maxCol;
        private Tile lastReturned = null;

        public TileIterator(Tile[,] grid)
        {
            this.grid = grid;
            maxRow = grid.GetLength(0);
            maxCol = grid.GetLength(1);
        }

        public bool HasNext()
        {
            return row < maxRow && col < maxCol - 1 || row < maxRow - 1;
        }

        public Tile Next()
        {
            if (!HasNext()) throw new InvalidOperationException();

            col++;
            if (col >= maxCol)
            {
                col = 0;
                row++;
            }

            lastReturned = grid[row, col];
            return lastReturned;
        }

        public void Remove()
        {
            throw new NotSupportedException("Cannot remove tiles");
        }
    }
}