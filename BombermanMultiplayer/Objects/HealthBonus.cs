using System;

namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// Represents a health bonus that provides armor or additional lives to the player
    /// </summary>
    public sealed class HealthBonus : Bonus
    {
        /// <summary>
        /// Gets or sets the duration of the health bonus effect in milliseconds
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets the amount of health or lives added to the player
        /// </summary>
        public int HealthIncrease { get; set; }

        /// <summary>
        /// Initializes a new instance of the HealthBonus class
        /// </summary>
        /// <param name="x">The x-coordinate position of the bonus</param>
        /// <param name="y">The y-coordinate position of the bonus</param>
        /// <param name="frameNumber">The frame number for sprite animation</param>
        /// <param name="frameWidth">The width of the bonus sprite frame</param>
        /// <param name="frameHeight">The height of the bonus sprite frame</param>
        /// <param name="duration">The duration of the health bonus effect in milliseconds</param>
        /// <param name="healthIncrease">The amount of health or lives to add</param>
        public HealthBonus(int x, int y, int frameNumber, int frameWidth, int frameHeight, int duration, int healthIncrease)
            : base(x, y, frameNumber, frameWidth, frameHeight, BonusType.Armor)
        {
            this.Duration = duration;
            this.HealthIncrease = healthIncrease;
        }

        /// <summary>
        /// Returns the duration of the health bonus effect in milliseconds.
        /// </summary>
        /// <returns>The duration in milliseconds</returns>
        public override int GetDuration()
        {
            return this.Duration;
        }

        /// <summary>
        /// Applies the effect of a health bonus to a player.
        /// </summary>
        /// <param name="player">The player who will receive the bonus effect.</param>
        protected override void ApplyEffect(Player player)
        {
            player.Lifes += (byte)HealthIncrease;
        }

        /// <summary>
        /// Determines whether the health bonus can be applied to a player.
        /// </summary>
        /// <param name="player">The player attempting to apply the bonus.</param>
        /// <returns>True if the bonus can be applied; otherwise, false.</returns>
        protected override bool CanApply(Player player)
        {
            return player.Lifes < 5; // Example limit — prevents infinite stacking
        }

        /// <summary>
        /// Determines whether a special effect should be played when applying the bonus to a player.
        /// </summary>
        /// <returns>True if the special effect should be played; otherwise, false.</returns>
        protected override bool ShouldPlaySpecialEffect()
        {
            return true; // Shield clang sound + glow
        }
    }
}