using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Interpreter
{
	public class ConsoleCommandHandler
	{
		[DllImport("kernel32.dll")]
		private static extern bool AllocConsole();

		[DllImport("kernel32.dll")]
		private static extern bool FreeConsole();

		private readonly Game _game;
		private readonly CommandParser _parser;
		private Thread _consoleThread;
		private bool _running;

		public ConsoleCommandHandler(Game game)
		{
			_game = game;
			_parser = new CommandParser();
			_running = false;
		}

		public void Start()
		{
			if (_running)
				return;

			_running = true;

			AllocConsole();

			_consoleThread = new Thread(ReadCommands);
			_consoleThread.IsBackground = true;
			_consoleThread.Start();
		}

		public void Stop()
		{
			_running = false;
			FreeConsole();
		}

		private void ReadCommands()
		{
			Console.WriteLine("╔════════════════════════════════════════════╗");
			Console.WriteLine("║     BOMBERMAN COMMAND INTERPRETER          ║");
			Console.WriteLine("╠════════════════════════════════════════════╣");
			Console.WriteLine("║ Komandos:                                  ║");
			Console.WriteLine("║   move player1 up/down/left/right          ║");
			Console.WriteLine("║   move player2 up/down/left/right          ║");
			Console.WriteLine("║   move player3 up/down/left/right          ║");
			Console.WriteLine("║   move player4 up/down/left/right          ║");
			Console.WriteLine("║   exit - uždaryti console                  ║");
			Console.WriteLine("╚════════════════════════════════════════════╝");
			Console.WriteLine();

			while (_running)
			{
				try
				{
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.Write("> ");
					Console.ResetColor();

					string input = Console.ReadLine();

					if (string.IsNullOrWhiteSpace(input))
						continue;

					if (input.Trim().ToLower() == "exit")
					{
						Console.WriteLine("Console uždaromas...");
						Stop();
						break;
					}

					ExecuteCommand(input);
				}
				catch (Exception ex)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine($"Klaida: {ex.Message}");
					Console.ResetColor();
				}
			}
		}

		private void ExecuteCommand(string input)
		{
			IExpression expression = _parser.Parse(input);

			if (expression == null)
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Nežinoma komanda. Naudok: move player1/2/3/4 up/down/left/right");
				Console.ResetColor();
				return;
			}

			var context = new GameCommandContext(_game);

			expression.Interpret(context);

			if (context.Success)
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine($"✓ {context.Message}");
			}
			else
			{
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"✗ {context.Message}");
			}
			Console.ResetColor();
		}
	}
}
