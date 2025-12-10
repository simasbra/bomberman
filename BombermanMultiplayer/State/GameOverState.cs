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

        public void Exit(Game game)
        {
            game.Over = false;
        }

        public void HandleInput(Keys key, Game game)
        {
            // Press R to restart the game - transition to countdown state
            if (key == Keys.R)
            {
                game.SetState(new CountdownState(3000));
            }
        }

        public void HandleKeyUp(Keys key, Game game)
        {
        }

        public void Update(Game game)
        {
            // No ticking when game is over
        }

        public void TogglePause(Game game)
        {
            // GameOver cannot be paused - do nothing
        }
    }
}
