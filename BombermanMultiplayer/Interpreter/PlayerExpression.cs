using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Interpreter
{
	public class PlayerExpression : IExpression
	{
		private readonly string _playerName;

		public PlayerExpression(string playerName)
		{
			_playerName = playerName.ToLower();
		}

		public void Interpret(GameCommandContext context)
		{
			switch (_playerName)
			{
				case "player1":
					context.PlayerNumber = 1;
					context.PlayerName = _playerName;
					context.Success = true;
					break;
				case "player2":
					context.PlayerNumber = 2;
					context.PlayerName = _playerName;
					context.Success = true;
					break;
				case "player3":
					context.PlayerNumber = 3;
					context.PlayerName = _playerName;
					context.Success = true;
					break;
				case "player4":
					context.PlayerNumber = 4;
					context.PlayerName = _playerName;
					context.Success = true;
					break;
				default:
					context.Success = false;
					context.Message = $"Nežinomas žaidėjas: {_playerName}. Naudok: player1, player2, player3, player4";
					break;
			}
		}
	}
}
