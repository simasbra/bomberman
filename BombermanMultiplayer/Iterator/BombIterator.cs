namespace BombermanMultiplayer.Iterator
{
    public class BombIterator : IGameIterator<Bomb>
    {
        private readonly List<Bomb> bombs;
        private int currentIndex = 0;
        private Bomb lastReturned = null;

        public BombIterator(List<Bomb> bombs)
        {
            this.bombs = bombs;
        }

        public bool HasNext()
        {
            return currentIndex < bombs.Count;
        }

        public Bomb Next()
        {
            if (!HasNext()) throw new InvalidOperationException("No more bombs");
            lastReturned = bombs[currentIndex];
            currentIndex++;
            return lastReturned;
        }

        public void Remove()
        {
            if (lastReturned == null) throw new InvalidOperationException("Nothing to remove");
            bombs.Remove(lastReturned);
            currentIndex--;
            lastReturned = null;
        }
    }
}