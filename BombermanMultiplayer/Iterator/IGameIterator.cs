namespace BombermanMultiplayer.Iterator
{
    /// <summary>
    /// Provides a way to iterate through a collection of game elements.
    /// </summary>
    /// <typeparam name="T">The type of the game elements in the collection.</typeparam>
    public interface IGameIterator<T>
    {
        /// <summary>
        /// Determines if the iterator has more elements to iterate through.
        /// </summary>
        /// <returns>true if there are more elements, false otherwise.</returns>
        bool HasNext();

        /// <summary>
        /// Gets the next element in the sequence.
        /// </summary>
        /// <returns>The next game element.</returns>
        T Next();

        /// <summary>
        /// Removes the current element from the underlying collection.
        /// </summary>
        void Remove();
    }
}