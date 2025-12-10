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
		/// BombColleague - Bomb klasės wrapper, kuris dalyvauja Mediator pattern
		/// 
		/// Bomb nežino apie Player ar World tiesiogiai.
		/// Kai bomba sprogsta, ji praneša Mediatoriui, kuris informuoja kitus.
		/// </summary>
		public class BombColleague : IColleague
		{
			private IGameMediator _mediator;
			private Bomb _bomb;

			public BombColleague(Bomb bomb)
			{
				_bomb = bomb;
			}

			/// <summary>
			/// Nustato mediatorių
			/// </summary>
			public void SetMediator(IGameMediator mediator)
			{
				_mediator = mediator;
			}

			// ========== VEIKSMAI, KURIUOS BOMB INICIJUOJA ==========

			/// <summary>
			/// Kai bomba sprogsta - praneša Mediatoriui
			/// </summary>
			public void Explode()
			{
				Console.WriteLine($"[Bomb] Bomba sprogsta! Galia: {_bomb.Power}");
				_bomb.Exploding = true;
				// Pranešti Mediatoriui - jis informuos Player ir World
				_mediator?.Notify(this, "BombExploded");
			}

			// ========== REAKCIJOS Į KITŲ COLLEAGUES VEIKSMUS ==========

			/// <summary>
			/// Kviečiamas Mediatoriaus, kai žaidėjas padeda bombą
			/// </summary>
			public void StartTimer()
			{
				Console.WriteLine($"[Bomb] Pradedamas {_bomb.DetonationTime}ms laikmatis");
				// Čia būtų pradedamas detonacijos laikmatis
				// Po laiko pasibaigimo iškviestų Explode()
			}

			/// <summary>
			/// Kviečiamas Mediatoriaus, kai žaidėjas pajuda
			/// </summary>
			public void CheckPlayerCollision()
			{
				Console.WriteLine("[Bomb] Tikrinu ar žaidėjas užlipo ant minos");
				// Tikrinti ar žaidėjo pozicija sutampa su bomba/mina
			}
		}
	}
}
