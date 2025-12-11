using System.Drawing;

namespace BombermanMultiplayer.Composite
{
    /// <summary>
    /// Interface for explosive objects in the Composite pattern.
    /// Allows treating individual explosives and groups uniformly.
    /// </summary>
    public interface IExplosive
    {
        /// <summary>
        /// Updates the explosive's state
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since last update in milliseconds</param>
        void Update(int elapsedTime);

        /// <summary>
        /// Triggers the explosion
        /// </summary>
        void Explode();

        /// <summary>
        /// Checks if the explosive is currently exploding
        /// </summary>
        /// <returns>True if exploding, false otherwise</returns>
        bool IsExploding();

        /// <summary>
        /// Gets the position of the explosive
        /// </summary>
        /// <returns>Point representing the position</returns>
        Point GetPosition();
    }
}
