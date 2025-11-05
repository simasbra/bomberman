using System.Drawing;

namespace BombermanMultiplayer.Facade
{
    public class PlayerRenderer
    {
        public void Draw(Graphics gr, Player[] players, bool showPlayerPositions)
        {
            if (gr == null || players == null) return;

            for (int i = 0; i < players.Length; i++)
            {
                var player = players[i];
                if (player == null) continue;

                player.Draw(gr);

                if (showPlayerPositions)
                    player.DrawPosition(gr);
            }
        }
    }
}