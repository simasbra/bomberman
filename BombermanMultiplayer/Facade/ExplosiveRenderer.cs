using System.Collections.Generic;
using System.Drawing;

namespace BombermanMultiplayer.Facade
{
    /// <summary>
    /// Provides functionality to render a collection of explosive objects, such as bombs, mines, and grenades, onto a
    /// graphical surface.
    /// </summary>
    public class ExplosiveRenderer
    {
        /// <summary>
        /// Draws the specified collection of bombs, mines, and grenades onto the provided graphics surface.
        /// </summary>
        /// <remarks>Each item in the provided lists is drawn only if it is not <see langword="null"/>. If
        /// <paramref name="gr"/> is <see langword="null"/>, the method does nothing.</remarks>
        /// <param name="gr">The <see cref="Graphics"/> object used to render the items. Cannot be <see langword="null"/>.</param>
        /// <param name="bombs">A list of <see cref="Bomb"/> objects to draw. Can be <see langword="null"/> or empty.</param>
        /// <param name="mines">A list of <see cref="Mine"/> objects to draw. Can be <see langword="null"/> or empty.</param>
        /// <param name="grenades">A list of <see cref="Grenade"/> objects to draw. Can be <see langword="null"/> or empty.</param>
        public void Draw(Graphics gr, List<Bomb> bombs, List<Mine> mines, List<Grenade> grenades)
        {
            if (gr == null) return;

            if (bombs != null)
            {
                foreach (var b in bombs) b?.Draw(gr);
            }
            if (mines != null)
            {
                foreach (var m in mines) m?.Draw(gr);
            }
            if (grenades != null)
            {
                foreach (var g in grenades)
                {
                    if (g == null) continue;
                    g.Draw(gr);
                }
            }
        }
    }
}