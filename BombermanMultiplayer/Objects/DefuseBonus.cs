using System;

namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// Represents a defuse bonus that allows players to defuse or disarm bombs
    /// </summary>
    public sealed class DefuseBonus : Bonus
    {
        /// <summary>
        /// Gets or sets the duration of the defuse bonus effect in milliseconds
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// Initializes a new instance of the DefuseBonus class
        /// </summary>
        /// <param name="x">The x-coordinate position of the bonus</param>
        /// <param name="y">The y-coordinate position of the bonus</param>
        /// <param name="frameNumber">The frame number for sprite animation</param>
        /// <param name="frameWidth">The width of the bonus sprite frame</param>
        /// <param name="frameHeight">The height of the bonus sprite frame</param>
        /// <param name="duration">The duration of the defuse bonus effect in milliseconds</param>
        public DefuseBonus(int x, int y, int frameNumber, int frameWidth, int frameHeight, int duration)
            : base(x, y, frameNumber, frameWidth, frameHeight, BonusType.Desamorce)
        {
            this.Duration = duration;
        }

        /// <summary>
        /// Returns the duration of the defuse bonus effect in milliseconds.
        /// </summary>
        /// <returns>The duration of the defuse bonus effect.</returns>
        public override int GetDuration()
        {
            return this.Duration;
        }

        /// <summary>
        /// Applies the defuse bonus effect to a player.
        /// </summary>
        /// <param name="player">The player to apply the effect to.</param>
        /// <returns>void</returns>
        protected override void ApplyEffect(Player player)
        {
            // Enable defuse ability — typically done via strategy pattern (already supported)
            // Here we just register the ability (actual deactivation logic is in Player.Deactivate)
            // The presence in BonusSlot[] is enough to enable Deactivate() check
        }
    }
}