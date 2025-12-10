using System;
using System.Windows.Forms;

namespace BombermanMultiplayer.State
{
	/// <summary>
	/// State Pattern - Concrete State: Paused
	/// </summary>
	public sealed class PausedState : IGameState
	{
		private static readonly PausedState _instance = new PausedState();
		public static PausedState Instance => _instance;

		private PausedState() { }

		public string StateName => "Paused";

		public void Enter(Game game)
		{
			Console.WriteLine("[PausedState] Entered - Game paused");
			game.LogicTimer.Stop();
			game.Paused = true;
		}

		public void Exit(Game game)
		{
			Console.WriteLine("[PausedState] Exiting...");
			game.Paused = false;
		}

		public void HandleInput(Keys key, Game game)
		{
			switch (key)
			{
				case Keys.Escape:
				case Keys.P:
					game.ChangeState(PlayingState.Instance);
					break;
				case Keys.F5:
					game.SaveGame("quicksave.bmb");
					break;
				case Keys.F9:
					game.LoadGame("quicksave.bmb");
					break;
			}
		}

		public void HandleKeyUp(Keys key, Game game)
		{
			// No key up handling while paused
		}

		public void Update(Game game)
		{
			// No updates while paused
		}

		public void TogglePause(Game game)
		{
			// Resume game
			game.ChangeState(PlayingState.Instance);
		}
	}
}