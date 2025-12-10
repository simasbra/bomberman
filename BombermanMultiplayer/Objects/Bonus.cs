using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// Represents a bonus item that can be collected by players in the game.
    /// Uses the Template Method pattern to define the steps for applying a bonus effect to a player.
    /// </summary>
    public abstract class Bonus : GameObject
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
        protected Bonus(int x, int y, int frameNumber, int frameWidth, int frameHeight, BonusType type)
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

        public virtual double GetSpeedMultiplier()
        {
            return 1.0;
        }

        public virtual int GetDuration()
        {
            return 0;
        }

        public virtual double GetPowerMultiplier()
        {
            return 1.0;
        }

        public virtual string GetDescription()
        {
            return $"Basic {Type} bonus";
        }

        #region Template Method

        /// <summary>
        /// Template method defining the algorithm for applying this bonus to a player.
        /// The steps are fixed, but concrete bonuses can customize behavior via hooks and overrides.
        /// </summary>
        /// <param name="player">The player receiving the bonus</param>
        public void ApplyToPlayer(Player player)
        {
            if (player == null)
                return;

            if (!CanApply(player))
                return;

            ApplyEffect(player);

            if (ShouldPlaySpecialEffect())
            {
                PlaySpecialEffect();
            }

            RegisterInPlayerSlot(player);
        }

        /// <summary>
        /// Optional hook: determines whether the bonus can be applied to the player at this moment.
        /// Default implementation always returns true.
        /// </summary>
        /// <param name="player">The player who would receive the bonus</param>
        /// <returns>true if the bonus can be applied; otherwise false</returns>
        protected virtual bool CanApply(Player player)
        {
            return true;
        }

        /// <summary>
        /// Optional hook: determines whether a special visual or sound effect should be played.
        /// Default implementation returns false.
        /// </summary>
        /// <returns>true if special effect should be played; otherwise false</returns>
        protected virtual bool ShouldPlaySpecialEffect()
        {
            return false;
        }

        /// <summary>
        /// Primitive operation — must be implemented by concrete bonus types.
        /// Applies the core gameplay effect (speed, power, armor, etc.).
        /// </summary>
        /// <param name="player">The player receiving the effect</param>
        protected abstract void ApplyEffect(Player player);

        /// <summary>
        /// Primitive operation — must be implemented by concrete bonus types.
        /// Plays a special visual or sound effect when the bonus is collected (e.g. sparkle, fanfare).
        /// Only called if ShouldPlaySpecialEffect() returns true.
        /// </summary>
        protected virtual void PlaySpecialEffect()
        {
            // Default: do nothing (most bonuses are silent)
        }

        /// <summary>
        /// Finds a free bonus slot in the player and registers this bonus type + timer.
        /// This logic is shared by all bonuses — no need to override.
        /// </summary>
        /// <param name="player">The player receiving the bonus</param>
        private void RegisterInPlayerSlot(Player player)
        {
            for (int i = 0; i < player.BonusSlot.Length; i++)
            {
                if (player.BonusSlot[i] == BonusType.None)
                {
                    player.BonusSlot[i] = this.Type;
                    player.BonusTimer[i] = (short)(GetDuration() / 16);
                    return;
                }
            }
            // If no free slot, overwrite the oldest one
            player.BonusSlot[0] = this.Type;
            player.BonusTimer[0] = (short)(GetDuration() / 16);
        }

        #endregion
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