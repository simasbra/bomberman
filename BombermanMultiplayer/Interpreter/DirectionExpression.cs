using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Interpreter
{
	public class DirectionExpression : IExpression
	{
		private readonly string _direction;

		public DirectionExpression(string direction)
		{
			_direction = direction.ToLower();
		}

		public void Interpret(GameCommandContext context)
		{
			switch (_direction)
			{
				case "up":
				case "down":
				case "left":
				case "right":
					context.Direction = _direction;
					context.Success = true;
					break;
				default:
					context.Success = false;
					context.Message = $"Nežinoma kryptis: {_direction}. Naudok: up, down, left, right";
					break;
			}
		}
	}
}
