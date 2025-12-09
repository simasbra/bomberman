using System.Drawing;
using System.Windows.Forms;

namespace BombermanMultiplayer.State
{
    public class GameOverState : IGameState
    {
        public void Enter(Game game)
        {
            game.Over = true;
            game.Paused = true;
            game.LogicTimer.Stop();
        }

        public void HandleInput(Keys key, Game game)
        {
            // Press R to restart the game
            if (key == Keys.R)
            {
                game.RestartGame();
            }
        }

        public void HandleKeyUp(Keys key, Game game)
        {
        }

        public void Update(Game game)
        {
            // No ticking when game is over
        }
    }
}
