using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Mediator
{
	using System;
	using System.Collections.Generic;

	namespace BombermanMultiplayer.Mediator
	{
		/// <summary>
		/// Concrete Mediator - koordinuoja komunikaciją tarp Player, World ir Bomb
		/// 
		/// SKIRTUMAS NUO OBSERVER:
		/// - Observer: Vienkryptė komunikacija (Subject -> visi Observers gauna tą patį Update())
		/// - Mediator: Dvikryptė komunikacija (Colleague -> Mediator -> skirtingi Colleagues gauna skirtingus pranešimus)
		/// 
		/// Mediator žino apie visus colleagues ir nusprendžia, ką daryti su kiekvienu įvykiu.
		/// Colleagues nežino vienas apie kitą - jie tik žino apie Mediatorių.
		/// </summary>
		public class GameMediator : IGameMediator
		{
			// Colleagues - Mediator laiko nuorodas į visus dalyvius
			private PlayerColleague _player;
			private WorldColleague _world;
			private BombColleague _bomb;

			/// <summary>
			/// Registruoja Player kaip colleague
			/// </summary>
			public void RegisterPlayer(PlayerColleague player)
			{
				_player = player;
				_player.SetMediator(this);
			}

			/// <summary>
			/// Registruoja World kaip colleague
			/// </summary>
			public void RegisterWorld(WorldColleague world)
			{
				_world = world;
				_world.SetMediator(this);
			}

			/// <summary>
			/// Registruoja Bomb kaip colleague
			/// </summary>
			public void RegisterBomb(BombColleague bomb)
			{
				_bomb = bomb;
				_bomb.SetMediator(this);
			}

			/// <summary>
			/// Pagrindinis metodas - priima pranešimus ir koordinuoja atsakymus
			/// Čia yra ESMINIS skirtumas nuo Observer - Mediator nusprendžia,
			/// kokie veiksmai turi įvykti ir kas turi būti informuotas
			/// </summary>
			public void Notify(object sender, string eventType)
			{
				switch (eventType)
				{
					case "BombPlaced":
						// Kai žaidėjas padeda bombą:
						// 1. World turi pažymėti tile kaip užimtą
						// 2. Bomb turi pradėti skaičiuoti laiką
						Console.WriteLine("[Mediator] Žaidėjas padėjo bombą");
						_world?.OnBombPlaced();
						_bomb?.StartTimer();
						break;

					case "BombExploded":
						// Kai bomba sprogsta:
						// 1. World turi sunaikinti sienas sprogimo zonoje
						// 2. Player turi patikrinti ar gavo damage
						Console.WriteLine("[Mediator] Bomba sprogo!");
						_world?.OnExplosion();
						_player?.OnExplosionNearby();
						break;

					case "PlayerMoved":
						// Kai žaidėjas pajuda:
						// 1. Bomb tikrina ar žaidėjas užlipo ant minos
						// 2. World tikrina ar yra bonus toje vietoje
						Console.WriteLine("[Mediator] Žaidėjas pajudėjo");
						_bomb?.CheckPlayerCollision();
						_world?.CheckBonusPickup();
						break;

					case "WallDestroyed":
						// Kai siena sunaikinama:
						// 1. World atnaujina tile
						// 2. Gali atsirasti bonus - Player informuojamas
						Console.WriteLine("[Mediator] Siena sunaikinta");
						_world?.OnWallDestroyed();
						_player?.OnBonusMayAppear();
						break;

					default:
						Console.WriteLine($"[Mediator] Nežinomas įvykis: {eventType}");
						break;
				}
			}
		}
	}
}
