using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Interpreter
{
	public class CommandParser
	{
		public IExpression Parse(string input)
		{
			if (string.IsNullOrWhiteSpace(input))
				return null;

			// Padalijame komandą į dalis: "move player1 up" -> ["move", "player1", "up"]
			string[] parts = input.Trim().ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

			if (parts.Length < 1)
				return null;

			string command = parts[0];

			switch (command)
			{
				case "move":
					return ParseMoveCommand(parts);
				default:
					return null;
			}
		}

		private IExpression ParseMoveCommand(string[] parts)
		{
			// move komandai reikia 3 dalių: move player1 up
			if (parts.Length != 3)
				return null;

			string playerName = parts[1];   // "player1"
			string direction = parts[2];     // "up"

			var playerExpression = new PlayerExpression(playerName);
			var directionExpression = new DirectionExpression(direction);

			return new MoveCommandExpression(playerExpression, directionExpression);
		}
	}
}