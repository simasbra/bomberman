using System;
using System.Collections.Generic;

namespace BombermanMultiplayer.Iterator
{
    /// <summary>
    /// Iterator for bomb objects, providing a way to traverse through a list of bombs.
    /// </summary>
    public class BombIterator : IIterator<Bomb>
    {
        private readonly List<Bomb> Bombs;
        private int CurrentIndex = 0;
        private Bomb LastReturned = null;

        /// <summary>
        /// Iterates over a collection of bomb objects.
        /// <param name="bombs">Bombs list</param>
        /// </summary>
        public BombIterator(List<Bomb> bombs)
        {
            Bombs = bombs;
        }

        /// <summary>
        /// Determines if there are more bomb objects to iterate over.
        /// </summary>
        /// <returns>True if there are more bombs, otherwise false.</returns>
        public bool HasNext()
        {
            return CurrentIndex < Bombs.Count;
        }

        /// <summary>
        /// Returns the next bomb in the iteration.
        /// </summary>
        /// <returns>The next bomb object.</returns>
        /// <exception cref="InvalidOperationException">Thrown when there are no more bombs to iterate over.</exception>
        public Bomb Next()
        {
            if (!HasNext()) throw new InvalidOperationException("No more bombs");
            LastReturned = Bombs[CurrentIndex];
            CurrentIndex++;
            return LastReturned;
        }

        /// <summary>
        /// Removes the last element returned by the iterator from the underlying collection.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when Remove is called before calling Next.</exception>
        public void Remove()
        {
            if (LastReturned == null) throw new InvalidOperationException("Nothing to remove");
            Bombs.Remove(LastReturned);
            CurrentIndex--;
            LastReturned = null;
        }
    }
}