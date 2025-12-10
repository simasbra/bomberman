using System;

namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// Represents a speed bonus that temporarily increases player movement speed
    /// </summary>
    public sealed class SpeedBonus : Bonus
    {
        /// <summary>
        /// Gets or sets the multiplier applied to the player's movement speed
        /// </summary>
        public double SpeedMultiplier { get; set; }

        /// <summary>
        /// Gets or sets the duration of the speed bonus effect in milliseconds
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Initializes a new instance of the SpeedBonus class
        /// </summary>
        /// <param name="x">The x-coordinate position of the bonus</param>
        /// <param name="y">The y-coordinate position of the bonus</param>
        /// <param name="frameNumber">The frame number for sprite animation</param>
        /// <param name="frameWidth">The width of the bonus sprite frame</param>
        /// <param name="frameHeight">The height of the bonus sprite frame</param>
        /// <param name="speedMultiplier">The multiplier to apply to player speed (e.g., 1.5 for 50% faster)</param>
        /// <param name="duration">The duration of the speed effect in milliseconds</param>
        public SpeedBonus(int x, int y, int frameNumber, int frameWidth, int frameHeight, double speedMultiplier, int duration)
            : base(x, y, frameNumber, frameWidth, frameHeight, BonusType.SpeedBoost)
        {
            this.SpeedMultiplier = speedMultiplier;
            this.Duration = duration;
        }

        /// <summary>
        /// Gets the duration of the speed bonus effect.
        /// </summary>
        /// <returns>The duration of the speed boost in milliseconds.</returns>
        public override int GetDuration()
        {
            return this.Duration;
        }

        /// <summary>
        /// Gets the speed multiplier for this bonus.
        /// </summary>
        /// <returns>The current speed multiplier value.</returns>
        public override double GetSpeedMultiplier()
        {
            return this.SpeedMultiplier;
        }

        /// <summary>
        /// Returns a string description of the SpeedBonus object.
        /// </summary>
        /// <returns>A string representing the speed bonus details in the format "SpeedBonus (x{speedMultiplier}, {duration}ms)".</returns>
        public override string GetDescription()
        {
            return $"SpeedBonus (x{SpeedMultiplier}, {Duration}ms)";
        }

        /// <summary>
        /// Applies a speed boost effect to the specified player.
        /// </summary>
        /// <param name="player">The player object to apply the speed boost to.</param>
        /// <returns>void</returns>
        protected override void ApplyEffect(Player player)
        {
            // Apply speed boost via strategy or direct field — here we assume strategy is set elsewhere
            // Or directly modify if no strategy system is active yet
            player.Vitesse = (byte)(player.Vitesse * SpeedMultiplier);
        }

        /// <summary>
        /// Determines whether to play a special effect when applying the bonus.
        /// For the SpeedBonus, this method always returns true as it has a cool whoosh effect!
        /// </summary>
        /// <returns>True if a special effect should be played; otherwise, false.</returns>
        protected override bool ShouldPlaySpecialEffect()
        {
            return true; // Speed bonus has a cool whoosh effect!
        }
    }
}