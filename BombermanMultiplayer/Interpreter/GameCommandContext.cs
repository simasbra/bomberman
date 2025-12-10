using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Interpreter
{
	public class GameCommandContext
	{
		public Game Game { get; set; }

		public string Command { get; set; }
		public string PlayerName { get; set; }
		public string Direction { get; set; }

		public bool Success { get; set; }
		public string Message { get; set; }
		public int PlayerNumber { get; set; }

		public GameCommandContext(Game game)
		{
			Game = game;
			Success = false;
			Message = "";
		}
	}
}
