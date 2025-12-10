using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Mediator
{
	namespace BombermanMultiplayer.Mediator
	{
		/// <summary>
		/// WorldColleague - World klasės wrapper, kuris dalyvauja Mediator pattern
		/// 
		/// World nežino apie Player ar Bomb tiesiogiai.
		/// Visa komunikacija vyksta per Mediatorių.
		/// </summary>
		public class WorldColleague : IColleague
		{
			private IGameMediator _mediator;
			private World _world;

			public WorldColleague(World world)
			{
				_world = world;
			}

			/// <summary>
			/// Nustato mediatorių
			/// </summary>
			public void SetMediator(IGameMediator mediator)
			{
				_mediator = mediator;
			}

			// ========== VEIKSMAI, KURIUOS WORLD INICIJUOJA ==========

			/// <summary>
			/// Kai siena sunaikinama - World praneša Mediatoriui
			/// </summary>
			public void DestroyWall(int x, int y)
			{
				Console.WriteLine($"[World] Siena ({x}, {y}) sunaikinta");
				// Atnaujinti tile
				if (_world.MapGrid != null &&
					x >= 0 && x < _world.MapGrid.GetLength(0) &&
					y >= 0 && y < _world.MapGrid.GetLength(1))
				{
					_world.MapGrid[x, y].Walkable = true;
					_world.MapGrid[x, y].Destroyable = false;
				}
				// Pranešti Mediatoriui
				_mediator?.Notify(this, "WallDestroyed");
			}

			// ========== REAKCIJOS Į KITŲ COLLEAGUES VEIKSMUS ==========

			/// <summary>
			/// Kviečiamas Mediatoriaus, kai žaidėjas padeda bombą
			/// </summary>
			public void OnBombPlaced()
			{
				Console.WriteLine("[World] Tile pažymėtas kaip užimtas bomba");
				// Čia būtų tile atnaujinimas - pažymėti, kad yra bomba
			}

			/// <summary>
			/// Kviečiamas Mediatoriaus, kai bomba sprogsta
			/// </summary>
			public void OnExplosion()
			{
				Console.WriteLine("[World] Tikrinu kurios sienos sunaikintos sprogimo metu");
				// Čia būtų logika sunaikinti sienas sprogimo zonoje
			}

			/// <summary>
			/// Kviečiamas Mediatoriaus, kai žaidėjas pajuda
			/// </summary>
			public void CheckBonusPickup()
			{
				Console.WriteLine("[World] Tikrinu ar žaidėjas paėmė bonus");
				// Čia būtų bonus pickup logika
			}

			/// <summary>
			/// Kviečiamas Mediatoriaus, kai siena sunaikinta
			/// </summary>
			public void OnWallDestroyed()
			{
				Console.WriteLine("[World] Atnaujinu žemėlapio būseną");
				// Papildomi atnaujinimai po sienos sunaikinimo
			}
		}
	}
}
