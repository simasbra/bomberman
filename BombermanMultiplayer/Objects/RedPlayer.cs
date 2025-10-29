using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// Represents a Red team player in the Bomberman game
    /// </summary>
    [Serializable]
    public class RedPlayer : Player
    {
        /// <summary>
        /// Initializes a new instance of the RedPlayer class
        /// </summary>
        /// <param name="lifes">The initial number of lives</param>
        /// <param name="totalFrames">The total number of animation frames</param>
        /// <param name="frameWidth">The width of each frame</param>
        /// <param name="frameHeight">The height of each frame</param>
        /// <param name="caseligne">The starting row position on the grid</param>
        /// <param name="casecolonne">The starting column position on the grid</param>
        /// <param name="TileWidth">The width of a tile in pixels</param>
        /// <param name="TileHeight">The height of a tile in pixels</param>
        /// <param name="frameTime">The time duration for each frame</param>
        /// <param name="playerNumero">The player number identifier</param>
        public RedPlayer(byte lifes, int totalFrames, int frameWidth, int frameHeight, int caseligne, int casecolonne, int TileWidth, int TileHeight, int frameTime, byte playerNumero)
            : base(lifes, totalFrames, frameWidth, frameHeight, caseligne, casecolonne, TileWidth, TileHeight, frameTime, playerNumero)
        {
            this.Name = "Red Player";
        }
    }
}
