using System.Collections.Generic;
using System.Drawing;

namespace BombermanMultiplayer.Facade
{
    public class ExplosiveRenderer
    {
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