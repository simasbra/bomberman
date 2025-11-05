using System.Drawing;

namespace BombermanMultiplayer.Facade
{
    public class WorldRenderer
    {
        public void Draw(Graphics gr, World world)
        {
            if (gr == null || world == null) return;

            world.refreshTileSprites();
            world.Draw(gr);
        }
    }
}