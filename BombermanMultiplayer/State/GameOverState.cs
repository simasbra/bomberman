using System;
using System.Windows.Forms;

namespace BombermanMultiplayer.State
{
	/// <summary>
	/// State Pattern - Concrete State: Game Over
	/// </summary>
	public sealed class GameOverState : IGameState
	{
		private static readonly GameOverState _instance = new GameOverState();
		public static GameOverState Instance => _instance;

		private GameOverState() { }

		public string StateName => "GameOver";

		public void Enter(Game game)
		{
			Console.WriteLine($"[GameOverState] Entered - Winner: Player {game.Winner}");
			game.LogicTimer.Stop();
			game.Over = true;
			game.Paused = true;
		}

		public void Exit(Game game)
		{
			Console.WriteLine("[GameOverState] Exiting...");
			game.Over = false;
		}

		public void HandleInput(Keys key, Game game)
		{
			switch (key)
			{
				case Keys.Enter:
				case Keys.Space:
				case Keys.R:
					RestartGame(game);
					game.ChangeState(PlayingState.Instance);
					break;
				case Keys.Escape:
					game.ChangeState(WaitingState.Instance);
					break;
			}
		}

		public void HandleKeyUp(Keys key, Game game)
		{
			// No key up handling in game over state
		}

		public void Update(Game game)
		{
			// No updates when game is over
		}

		public void TogglePause(Game game)
		{
			// Restart instead of pause
			RestartGame(game);
			game.ChangeState(PlayingState.Instance);
		}

		private void RestartGame(Game game)
		{
			foreach (var player in game.players)
			{
				player.Dead = false;
				player.Lifes = 1;
			}

			game.BombsOnTheMap.Clear();
			game.MinesOnTheMap.Clear();
			game.GrenadesOnTheMap.Clear();
			game.Winner = 0;
		}
	}
}