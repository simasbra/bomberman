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
		/// PlayerColleague - Player klasės wrapper, kuris dalyvauja Mediator pattern
		/// 
		/// Colleague nežino apie kitus colleagues (World, Bomb).
		/// Jis tik žino apie Mediatorių ir per jį komunikuoja.
		/// </summary>
		public class PlayerColleague : IColleague
		{
			private IGameMediator _mediator;
			private Player _player;

			public PlayerColleague(Player player)
			{
				_player = player;
			}

			/// <summary>
			/// Nustato mediatorių - dabar šis colleague gali siųsti pranešimus
			/// </summary>
			public void SetMediator(IGameMediator mediator)
			{
				_mediator = mediator;
			}

			// ========== VEIKSMAI, KURIUOS PLAYER INICIJUOJA ==========

			/// <summary>
			/// Žaidėjas padeda bombą - praneša per Mediatorių
			/// </summary>
			public void PlaceBomb()
			{
				Console.WriteLine($"[Player] Žaidėjas {_player.PlayerNumero} padeda bombą");
				// Vietoj tiesioginio kreipimosi į World ar Bomb,
				// pranešame Mediatoriui, kuris koordinuos
				_mediator?.Notify(this, "BombPlaced");
			}

			/// <summary>
			/// Žaidėjas pajuda - praneša per Mediatorių
			/// </summary>
			public void Move()
			{
				Console.WriteLine($"[Player] Žaidėjas {_player.PlayerNumero} pajudėjo");
				_mediator?.Notify(this, "PlayerMoved");
			}

			// ========== REAKCIJOS Į KITŲ COLLEAGUES VEIKSMUS ==========

			/// <summary>
			/// Kviečiamas Mediatoriaus, kai netoliese sprogo bomba
			/// </summary>
			public void OnExplosionNearby()
			{
				Console.WriteLine($"[Player] Žaidėjas {_player.PlayerNumero} tikrina ar pataikė sprogimas");
				// Čia būtų tikrinama ar žaidėjas yra sprogimo zonoje
				// Jei taip - mažinamas health
			}

			/// <summary>
			/// Kviečiamas Mediatoriaus, kai gali atsirasti bonus
			/// </summary>
			public void OnBonusMayAppear()
			{
				Console.WriteLine($"[Player] Žaidėjas {_player.PlayerNumero} informuotas apie galimą bonus");
				// Žaidėjas gali reaguoti - pvz., UI atnaujinimas
			}
		}
	}
}
