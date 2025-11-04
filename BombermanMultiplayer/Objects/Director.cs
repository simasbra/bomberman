using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// Director class that manages the construction of Player objects using builders
    /// </summary>
    public class Director
    {
        /// <summary>
        /// Builds a Red player with default configuration
        /// </summary>
        /// <param name="builder">The builder to use for constructing the player</param>
        /// <returns>A fully constructed Red Player</returns>
        public Player BuildRedPlayer(IBuilder builder)
        {
            return builder
                .SetNumber(0)
                .SetName("Red Player")
                .SetNumBombs(2)
                .SetLived(3)
                .Build();
        }

        /// <summary>
        /// Builds a Blue player with default configuration
        /// </summary>
        /// <param name="builder">The builder to use for constructing the player</param>
        /// <returns>A fully constructed Blue Player</returns>
        public Player BuildBluePlayer(IBuilder builder)
        {
            return builder
                .SetNumber(1)
                .SetName("Blue Player")
                .SetNumBombs(2)
                .SetLived(3)
                .Build();
        }
    }
}
