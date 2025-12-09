using System.Drawing;
using System.Windows.Forms;

namespace BombermanMultiplayer.State
{
    public class LobbyState : IGameState
    {
        public void Enter(Game game)
        {
            game.Paused = true;
            game.Over = false;
            game.LogicTimer.Stop();
        }

        public void HandleInput(Keys key, Game game)
        {
            // Waiting room inputs could be handled here (e.g., ready signal)
        }

        public void HandleKeyUp(Keys key, Game game)
        {
        }

        public void Update(Game game)
        {
            // No main loop ticking in lobby
        }
    }
}
