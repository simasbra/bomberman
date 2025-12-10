using BombermanMultiplayer.Mediator;
using BombermanMultiplayer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BombermanMultiplayer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
			MediatorDemo.Run();

			//PrototypeDemo.RunDemo();
			//System.Console.WriteLine("\nDemonstracija baigta. Uždarykite console langą");
			Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainMenu());
			//Application.Run(new Lobby());
			//Application.Run(new GameWindow());
		}
    }
}
