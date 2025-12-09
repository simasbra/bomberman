using System.Drawing;
using System.Windows.Forms;

namespace BombermanMultiplayer.State
{
    public class RunningState : IGameState
    {
        public void Enter(Game game)
        {
            game.Paused = false;
            game.Over = false;
            game.LogicTimer.Start();
        }

        public void HandleInput(Keys key, Game game)
        {
            game.HandleGameplayKeyDown(key);
        }

        public void HandleKeyUp(Keys key, Game game)
        {
            game.HandleGameplayKeyUp(key);
        }

        public void Update(Game game)
        {
            game.RunGameLoopTick();
        }

    }
}
