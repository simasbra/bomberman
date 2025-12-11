using System.Drawing;
using BombermanMultiplayer.Objects;

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
        /// <param name="mapGrid">The game map grid</param>
        /// <param name="players">Array of players in the game</param>
        void Update(int elapsedTime, Tile[,] mapGrid, Player[] players);

        /// <summary>
        /// Triggers the explosion
        /// </summary>
        /// <param name="mapGrid">The game map grid</param>
        /// <param name="players">Array of players in the game</param>
        void Explode(Tile[,] mapGrid, Player[] players);

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
