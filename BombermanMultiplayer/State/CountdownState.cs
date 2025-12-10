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
            // Reset player positions and states
            game.players[0].Reset(1, 1);
            game.players[1].Reset(game.world.MapGrid.GetLength(0) - 2, game.world.MapGrid.GetLength(0) - 2);
            game.players[2].Reset(1, game.world.MapGrid.GetLength(1) - 2);
            game.players[3].Reset(game.world.MapGrid.GetLength(0) - 2, 1);

            // Clear all explosives
            game.BombsOnTheMap.Clear();
            game.MinesOnTheMap.Clear();
            game.GrenadesOnTheMap.Clear();

            // Reset world (regenerate destructible blocks)
            game.world.RegenerateMap();

            // Notify UI to reload sprites
            game.RaiseRestartRequested();

            // Reset winner
            game.Winner = 0;

            // Increment games played counter
            game.GamesPlayed++;

            // Reset death state tracking
            for (int i = 0; i < 4; i++)
            {
                game.previousDeathStates[i] = false;
            }

            // Start countdown timer and game logic
            game.Paused = false;
            game.Over = false;
            game.LogicTimer.Start();
        }

        public void Exit(Game game)
        {
            // No special cleanup needed when countdown ends
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

        public void TogglePause(Game game)
        {
            // Countdown cannot be paused - do nothing
        }
    }
}
