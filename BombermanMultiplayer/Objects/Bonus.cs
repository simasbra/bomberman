using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// Represents a bonus item that can be collected by players in the game
    /// </summary>
    public class Bonus : GameObject
    {
        /// <summary>
        /// Gets or sets the type of bonus this object represents
        /// </summary>
        public BonusType Type = BonusType.None;

        /// <summary>
        /// Initializes a new instance of the Bonus class
        /// </summary>
        /// <param name="x">The x-coordinate position of the bonus</param>
        /// <param name="y">The y-coordinate position of the bonus</param>
        /// <param name="frameNumber">The frame number for sprite animation</param>
        /// <param name="frameWidth">The width of the bonus sprite frame</param>
        /// <param name="frameHeight">The height of the bonus sprite frame</param>
        /// <param name="type">The type of bonus being created</param>
        public Bonus(int x, int y, int frameNumber, int frameWidth, int frameHeight, BonusType type ) 
            : base(x, y, frameNumber, frameWidth, frameHeight)
        {
            this.Type = type;

        }

        /// <summary>
        /// Calculates and updates the grid position of the bonus based on its pixel coordinates
        /// </summary>
        /// <param name="TileWidth">The width of a grid tile in pixels</param>
        /// <param name="TileHeight">The height of a grid tile in pixels</param>
        public void CheckCasePosition(int TileWidth, int TileHeight)
        {
            this.CasePosition[0] = this.Source.Y / TileWidth; //Ligne
            this.CasePosition[1] = this.Source.X / TileWidth; //Colonne
        }

    }

    /// <summary>
    /// Defines the available types of bonuses that can appear in the game
    /// </summary>
    public enum BonusType
    {
        /// <summary>
        /// No bonus type
        /// </summary>
        None,
        /// <summary>
        /// Increases bomb power or damage
        /// </summary>
        PowerBomb,
        /// <summary>
        /// Increases player movement speed
        /// </summary>
        SpeedBoost,
        /// <summary>
        /// Allows defusing or disarming bombs
        /// </summary>
        Desamorce,
        /// <summary>
        /// Provides armor or health protection
        /// </summary>
        Armor,
        /// <summary>
        /// Provides advanced weapons and explosives
        /// </summary>
        AdvancedArsenal

    }
}
