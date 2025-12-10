namespace BombermanMultiplayer.Objects
{
    /// <summary>
    /// Represents a power bonus that enhances bomb damage capabilities
    /// </summary>
    public sealed class PowerBonus : Bonus
    {
        /// <summary>
        /// Gets or sets the amount of damage increase for the player's bombs
        /// </summary>
        public int DamageIncrease { get; set; }

        /// <summary>
        /// Gets or sets the amount of damage decrease for opponent's bombs
        /// </summary>
        public int OpponentDamageDecrease { get; set; }

        /// <summary>
        /// Initializes a new instance of the PowerBonus class
        /// </summary>
        /// <param name="x">The x-coordinate position of the bonus</param>
        /// <param name="y">The y-coordinate position of the bonus</param>
        /// <param name="frameNumber">The frame number for sprite animation</param>
        /// <param name="frameWidth">The width of the bonus sprite frame</param>
        /// <param name="frameHeight">The height of the bonus sprite frame</param>
        /// <param name="damageIncrease">The amount to increase player's bomb damage</param>
        /// <param name="opponentDamageDecrease">The amount to decrease opponent's bomb damage</param>
        public PowerBonus(
            int x,
            int y,
            int frameNumber,
            int frameWidth,
            int frameHeight,
            int damageIncrease,
            int opponentDamageDecrease)
            : base(x, y, frameNumber, frameWidth, frameHeight, BonusType.PowerBomb)
        {
            this.DamageIncrease = damageIncrease;
            this.OpponentDamageDecrease = opponentDamageDecrease;
        }

        public override string GetDescription()
        {
            return $"PowerBonus (Damage Increase: {DamageIncrease})";
        }

        protected override void ApplyEffect(Player player)
        {
            // Example: increase bomb power — actual implementation may use strategy or modify bomb factory
            // For now, we just upgrade arsenal if not already
            player.UpgradeArsenal();
        }
    }
}