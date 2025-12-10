namespace BombermanMultiplayer.Iterator
{
    public class PlayerIterator : IGameIterator<Player>
    {
        private readonly Player[] players;
        private int currentIndex = 0;
        private Player lastReturned = null;

        public PlayerIterator(Player[] players)
        {
            this.players = players;
            {
                this.players = players;
            }

            public bool HasNext() => currentIndex < players.Length;

            public Player Next()
            {
                if (!HasNext()) throw new InvalidOperationException();
                lastReturned = players[currentIndex];
                currentIndex++;
                return lastReturned;
            }

            public void Remove()
            {
                throw new NotSupportedException("Cannot remove players");
            }
        }
    }