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
                .SetLives(3)
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
                .SetLives(3)
                .Build();
        }

        /// <summary>
        /// Builds a custom player with specified configuration
        /// </summary>
        /// <param name="builder">The builder to use for constructing the player</param>
        /// <param name="playerNumber">The player number identifier</param>
        /// <param name="name">The player's display name</param>
        /// <param name="bombCount">The initial number of bombs</param>
        /// <param name="lives">The initial number of lives</param>
        /// <returns>A fully constructed Player with custom configuration</returns>
        public Player BuildCustomPlayer(IBuilder builder, byte playerNumber, string name, byte bombCount, byte lives)
        {
            return builder
                .SetNumber(playerNumber)
                .SetName(name)
                .SetNumBombs(bombCount)
                .SetLives(lives)
                .Build();
        }

        /// <summary>
        /// Builds a player with bonus configuration
        /// </summary>
        /// <param name="builder">The builder to use for constructing the player</param>
        /// <param name="playerNumber">The player number identifier</param>
        /// <param name="name">The player's display name</param>
        /// <param name="bombCount">The initial number of bombs</param>
        /// <param name="lives">The initial number of lives</param>
        /// <param name="bonuses">Array of bonuses to add to the player</param>
        /// <returns>A fully constructed Player with bonuses</returns>
        public Player BuildPlayerWithBonuses(IBuilder builder, byte playerNumber, string name, byte bombCount, byte lives, params BonusType[] bonuses)
        {
            builder
                .SetNumber(playerNumber)
                .SetName(name)
                .SetNumBombs(bombCount)
                .SetLives(lives);

            foreach (var bonus in bonuses)
            {
                builder.AddBonus(bonus);
            }

            return builder.Build();
        }
    }
}
