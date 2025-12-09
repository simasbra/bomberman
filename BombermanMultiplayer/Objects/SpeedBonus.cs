using System;

namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// Represents a speed bonus that temporarily increases player movement speed
    /// </summary>
    public class SpeedBonus : Bonus
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

        public override int GetDuration()
        {
            return this.Duration;
        }

        public override double GetSpeedMultiplier()
        {
            return this.SpeedMultiplier;
        }

        public override string GetDescription()
        {
            return $"SpeedBonus (x{SpeedMultiplier}, {Duration}ms)";
        }
    }
}