using System;
using System.Windows.Forms;
using BombermanMultiplayer.Objects;

namespace BombermanMultiplayer.State
{
	/// <summary>
	/// State Pattern - Concrete State: Playing
	/// </summary>
	public sealed class PlayingState : IGameState
	{
		private static readonly PlayingState _instance = new PlayingState();
		public static PlayingState Instance => _instance;

		private PlayingState() { }

		public string StateName => "Playing";

		public void Enter(Game game)
		{
			Console.WriteLine("[PlayingState] Entered - Game is active!");
			game.LogicTimer.Start();
			game.Paused = false;
			game.Over = false;
		}

		public void Exit(Game game)
		{
			Console.WriteLine("[PlayingState] Exiting...");
		}

		public void HandleInput(Keys key, Game game)
		{
			switch (key)
			{
				case Keys.Escape:
					game.ChangeState(PausedState.Instance);
					break;

				// Player 1 - WASD (naudoja Orientation)
				case Keys.W:
					SetPlayerOrientation(game, 0, Player.MovementDirection.UP);
					break;
				case Keys.S:
					SetPlayerOrientation(game, 0, Player.MovementDirection.DOWN);
					break;
				case Keys.A:
					SetPlayerOrientation(game, 0, Player.MovementDirection.LEFT);
					break;
				case Keys.D:
					SetPlayerOrientation(game, 0, Player.MovementDirection.RIGHT);
					break;

				// Player 2 - Arrow keys
				case Keys.Up:
					SetPlayerOrientation(game, 1, Player.MovementDirection.UP);
					break;
				case Keys.Down:
					SetPlayerOrientation(game, 1, Player.MovementDirection.DOWN);
					break;
				case Keys.Left:
					SetPlayerOrientation(game, 1, Player.MovementDirection.LEFT);
					break;
				case Keys.Right:
					SetPlayerOrientation(game, 1, Player.MovementDirection.RIGHT);
					break;

				// Player 3 - IJKL
				case Keys.I:
					SetPlayerOrientation(game, 2, Player.MovementDirection.UP);
					break;
				case Keys.K:
					SetPlayerOrientation(game, 2, Player.MovementDirection.DOWN);
					break;
				case Keys.J:
					SetPlayerOrientation(game, 2, Player.MovementDirection.LEFT);
					break;
				case Keys.L:
					SetPlayerOrientation(game, 2, Player.MovementDirection.RIGHT);
					break;

				// Player 4 - NumPad
				case Keys.NumPad8:
					SetPlayerOrientation(game, 3, Player.MovementDirection.UP);
					break;
				case Keys.NumPad5:
					SetPlayerOrientation(game, 3, Player.MovementDirection.DOWN);
					break;
				case Keys.NumPad4:
					SetPlayerOrientation(game, 3, Player.MovementDirection.LEFT);
					break;
				case Keys.NumPad6:
					SetPlayerOrientation(game, 3, Player.MovementDirection.RIGHT);
					break;
			}
		}

		public void HandleKeyUp(Keys key, Game game)
		{
			// Player 1: WASD
			if (key == Keys.W || key == Keys.S || key == Keys.A || key == Keys.D)
				game.players[0].Orientation = Player.MovementDirection.NONE;
			// Player 2: Arrow keys
			else if (key == Keys.Up || key == Keys.Down || key == Keys.Left || key == Keys.Right)
				game.players[1].Orientation = Player.MovementDirection.NONE;
			// Player 3: IJKL
			else if (key == Keys.I || key == Keys.K || key == Keys.J || key == Keys.L)
				game.players[2].Orientation = Player.MovementDirection.NONE;
			// Player 4: NumPad
			else if (key == Keys.NumPad8 || key == Keys.NumPad5 || key == Keys.NumPad4 || key == Keys.NumPad6)
				game.players[3].Orientation = Player.MovementDirection.NONE;
		}

		public void Update(Game game)
		{
			// Check for game over
			int alivePlayers = 0;
			int lastAliveIndex = -1;

			for (int i = 0; i < game.players.Length; i++)
			{
				if (!game.players[i].Dead)
				{
					alivePlayers++;
					lastAliveIndex = i;
				}
			}

			if (alivePlayers <= 1)
			{
				game.Winner = (byte)(lastAliveIndex + 1);
				game.ChangeState(GameOverState.Instance);
			}
		}

		public void TogglePause(Game game)
		{
			game.ChangeState(PausedState.Instance);
		}

		private void SetPlayerOrientation(Game game, int playerIndex, Player.MovementDirection direction)
		{
			if (playerIndex < game.players.Length && !game.players[playerIndex].Dead)
			{
				game.players[playerIndex].Orientation = direction;
			}
		}
	}
}