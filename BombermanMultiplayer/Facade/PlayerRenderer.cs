using System.Drawing;

namespace BombermanMultiplayer.Facade
{
    /// <summary>
    /// Provides functionality to render players on a graphical surface.
    /// </summary>
    public class PlayerRenderer
    {
        /// <summary>
        /// Renders the players on the specified graphics surface, with an option to display their positions.
        /// </summary>
        /// <param name="gr">The <see cref="Graphics"/> object used to draw the players. Cannot be <see langword="null"/>.</param>
        /// <param name="players">An array of <see cref="Player"/> objects to be drawn. Cannot be <see langword="null"/>. Null elements within
        /// the array are ignored.</param>
        /// <param name="showPlayerPositions">A value indicating whether to draw the positions of the players in addition to their visuals. If <see
        /// langword="true"/>, player positions are rendered; otherwise, they are not.</param>
        public void Draw(Graphics gr, Player[] players, bool showPlayerPositions)
        {
            if (gr == null || players == null) return;

            // Load explosion pattern sprite sheet if available
            Image explosionPatternSprite = null;
            try
            {
                explosionPatternSprite = Properties.Resources.explosion_pattern;
            }
            catch
            {
                // Sprite not loaded, skip pattern indicator
            }

            for (int i = 0; i < players.Length; i++)
            {
                var player = players[i];
                if (player == null) continue;

                player.Draw(gr);

                if (showPlayerPositions)
                    player.DrawPosition(gr);

                // Draw explosion pattern indicator above player
                if (explosionPatternSprite != null && !player.Dead)
                {
                    int patternIndex = player.GetExplosionPatternIndex();
                    int frameWidth = 32; // Assuming each frame is 32 pixels wide
                    int frameHeight = 32; // Assuming each frame is 32 pixels high

                    // Position above player (centered)
                    int indicatorX = player.Source.X + player.Source.Width / 2 - frameWidth / 2;
                    int indicatorY = player.Source.Y - frameHeight - 5; // 5 pixels above player

                    // Draw the current pattern frame from sprite sheet
                    Rectangle destRect = new Rectangle(indicatorX, indicatorY, frameWidth, frameHeight);
                    Rectangle srcRect = new Rectangle(patternIndex * frameWidth, 0, frameWidth, frameHeight);

                    gr.DrawImage(explosionPatternSprite, destRect, srcRect, GraphicsUnit.Pixel);
                }
            }
        }
    }
}