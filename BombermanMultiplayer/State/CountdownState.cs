using System.Drawing;
using System.Windows.Forms;

namespace BombermanMultiplayer.State
{
    public class CountdownState : IGameState
    {
        private int _countdownMs;
        private readonly int _initialMs;

        public int RemainingSeconds => (_countdownMs + 999) / 1000;

        public CountdownState(int durationMs = 3000)
        {
            _countdownMs = durationMs;
            _initialMs = durationMs;
        }

        public void Enter(Game game)
        {
            game.Paused = false;
            game.Over = false;
            game.LogicTimer.Start();
        }

        public void HandleInput(Keys key, Game game)
        {
            // Ignore gameplay input during countdown
        }

        public void HandleKeyUp(Keys key, Game game)
        {
        }

        public void Update(Game game)
        {
            _countdownMs -= (int)game.LogicTimer.Interval;
            if (_countdownMs <= 0)
            {
                game.SetState(new RunningState());
            }
        }
    }
}
