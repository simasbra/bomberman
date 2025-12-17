using System.Drawing;

namespace BombermanMultiplayer.Facade
{
    public class WorldRenderer
    {
        /// <summary>
        /// Renders the current state of the world onto the specified graphics surface.
        /// </summary>
        /// <remarks></remarks>
        /// <param name="gr">The <see cref="Graphics"/> object used to draw the world. Cannot be <see langword="null"/>.</param>
        /// <param name="world">The <see cref="World"/> object representing the state to be drawn. Cannot be <see langword="null"/>.</param>
        public void Draw(Graphics gr, World world)
        {
            if (gr == null || world == null) return;

            world.RefreshTileSprites();
            world.Draw(gr);
        }
    }
}