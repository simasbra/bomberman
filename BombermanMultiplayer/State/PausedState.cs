using System.Drawing;
using System.Windows.Forms;

namespace BombermanMultiplayer.State
{
    public class PausedState : IGameState
    {
        public void Enter(Game game)
        {
            game.Paused = true;
            game.LogicTimer.Stop();
        }

        public void Exit(Game game)
        {
            // Resume when leaving paused state
            game.Paused = false;
        }

        public void HandleInput(Keys key, Game game)
        {
            if (key == Keys.Escape)
            {
                game.SetState(new RunningState());
            }
        }

        public void HandleKeyUp(Keys key, Game game)
        {
            // No movement while paused
        }

        public void Update(Game game)
        {
            // No game logic while paused
        }

        public void TogglePause(Game game)
        {
            // Paused state → Running state
            game.SetState(new RunningState());
        }
    }
}
