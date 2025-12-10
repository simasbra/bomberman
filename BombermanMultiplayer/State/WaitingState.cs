using System;
using System.Windows.Forms;

namespace BombermanMultiplayer.State
{
	/// <summary>
	/// State Pattern - Concrete State: Waiting/Lobby
	/// </summary>
	public sealed class WaitingState : IGameState
	{
		private static readonly WaitingState _instance = new WaitingState();
		public static WaitingState Instance => _instance;

		private WaitingState() { }

		public string StateName => "Waiting";

		public void Enter(Game game)
		{
			Console.WriteLine("[WaitingState] Entered - Waiting for players...");
			game.LogicTimer.Stop();
			game.Paused = true;
		}

		public void Exit(Game game)
		{
			Console.WriteLine("[WaitingState] Exiting...");
		}

		public void HandleInput(Keys key, Game game)
		{
			switch (key)
			{
				case Keys.Enter:
				case Keys.Space:
					game.ChangeState(PlayingState.Instance);
					break;
				case Keys.Escape:
					// Return to main menu
					break;
			}
		}

		public void HandleKeyUp(Keys key, Game game)
		{
			// No key up handling in waiting state
		}

		public void Update(Game game)
		{
			// No updates while waiting
		}

		public void TogglePause(Game game)
		{
			// Start game instead of pause
			game.ChangeState(PlayingState.Instance);
		}
	}
}