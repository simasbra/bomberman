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
    }
}
