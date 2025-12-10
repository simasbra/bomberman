using BombermanMultiplayer.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Interpreter
{
	public class MoveCommandExpression : IExpression
	{
		private readonly PlayerExpression _playerExpression;
		private readonly DirectionExpression _directionExpression;

		public MoveCommandExpression(PlayerExpression playerExpression, DirectionExpression directionExpression)
		{
			_playerExpression = playerExpression;
			_directionExpression = directionExpression;
		}

		public void Interpret(GameCommandContext context)
		{
			_playerExpression.Interpret(context);
			if (!context.Success)
				return;

			_directionExpression.Interpret(context);
			if (!context.Success)
				return;

			ExecuteMove(context);
		}

		private void ExecuteMove(GameCommandContext context)
		{
			int playerIndex = context.PlayerNumber - 1;

			if (playerIndex < 0 || playerIndex >= context.Game.players.Length)
			{
				context.Success = false;
				context.Message = $"Žaidėjas {context.PlayerNumber} neegzistuoja";
				return;
			}

			Player player = context.Game.players[playerIndex];

			if (player == null || player.Dead)
			{
				context.Success = false;
				context.Message = $"Žaidėjas {context.PlayerNumber} yra miręs arba neegzistuoja";
				return;
			}

			Player.MovementDirection direction;
			switch (context.Direction)
			{
				case "up":
					direction = Player.MovementDirection.UP;
					break;
				case "down":
					direction = Player.MovementDirection.DOWN;
					break;
				case "left":
					direction = Player.MovementDirection.LEFT;
					break;
				case "right":
					direction = Player.MovementDirection.RIGHT;
					break;
				default:
					context.Success = false;
					context.Message = "Nežinoma kryptis";
					return;
			}

			var moveCommand = new MovePlayerCommand(player, direction);
			moveCommand.Execute();

			context.Game.CommandHistory.Add(moveCommand);

			context.Success = true;
			context.Message = $"Žaidėjas {context.PlayerNumber} pajudėjo {context.Direction}";
		}
	}
}