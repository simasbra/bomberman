using System;

namespace BombermanMultiplayer.Iterator
{
    /// <summary>
    /// Iterates through a collection of players.
    /// Implements the <see cref="IIterator{T}"/> interface to provide iteration functionality for player objects.
    /// </summary>
    public class PlayerIterator : IIterator<Player>
    {
        private readonly Player[] Players;
        private int CurrentIndex = 0;
        private Player LastReturned = null;

        /// <summary>
        /// Iterates through a collection of players.
        /// Implements the <see cref="IIterator{T}"/> interface to provide iteration functionality for player objects.
        /// </summary>
        /// <param name="players">Players array</param>
        public PlayerIterator(Player[] players)
        {
            Players = players;
        }

        /// <summary>
        /// Checks if there are more elements to iterate over.
        /// </summary>
        /// <returns>True if there are more elements, false otherwise.</returns>
        public bool HasNext()
        {
            return CurrentIndex < Players.Length;
        }

        /// <summary>
        /// Advances the iterator to the next player and returns it.
        /// </summary>
        /// <returns>The next player in the collection.</returns>
        /// <exception cref="InvalidOperationException">Thrown when there are no more players to iterate through.</exception>
        public Player Next()
        {
            if (!HasNext()) throw new InvalidOperationException();
            LastReturned = Players[CurrentIndex];
            CurrentIndex++;
            return LastReturned;
        }

        /// <summary>
        /// Removes the current element from the collection.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when this method is not supported.</exception>
        public void Remove()
        {
            throw new NotSupportedException("Cannot remove players");
        }
    }
}