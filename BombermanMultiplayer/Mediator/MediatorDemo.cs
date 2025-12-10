using BombermanMultiplayer.Mediator.BombermanMultiplayer.Mediator;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Mediator
{
	/// <summary>
	/// Demonstracija kaip naudoti Mediator pattern
	/// 
	/// ============================================================
	/// OBSERVER vs MEDIATOR - PAGRINDINIS SKIRTUMAS:
	/// ============================================================
	/// 
	/// OBSERVER Pattern (jau turi projekte - GameState):
	/// ┌─────────────┐
	/// │  GameState  │ ──────► Observer1.Update()
	/// │  (Subject)  │ ──────► Observer2.Update()  
	/// │             │ ──────► Observer3.Update()
	/// └─────────────┘
	/// - Vienkryptė komunikacija: Subject → Observers
	/// - Visi observers gauna TĄ PATĮ pranešimą (Update)
	/// - Observers nežino vienas apie kitą
	/// 
	/// MEDIATOR Pattern (šis):
	/// ┌─────────┐     ┌───────────────┐     ┌─────────┐
	/// │ Player  │◄───►│   Mediator    │◄───►│  World  │
	/// └─────────┘     │               │     └─────────┘
	///                 │  Koordinuoja  │
	/// ┌─────────┐     │  komunikaciją │
	/// │  Bomb   │◄───►│               │
	/// └─────────┘     └───────────────┘
	/// - Dvikryptė komunikacija: Colleague ↔ Mediator ↔ Colleague
	/// - Mediator nusprendžia KAS ir KAIP turi reaguoti
	/// - Skirtingi colleagues gauna SKIRTINGUS pranešimus
	/// - Colleagues nežino vienas apie kitą
	/// 
	/// ============================================================
	/// </summary>
	public static class MediatorDemo
	{
		/// <summary>
		/// Paleisti demonstraciją
		/// </summary>
		public static void Run()
		{
			Console.WriteLine("========== MEDIATOR PATTERN DEMO ==========\n");

			// 1. Sukurti tikrus game objektus (naudojant tavo esamas klases)
			// Čia sukuriame paprastus pavyzdžius demonstracijai
			var player = new Player(1, 2, 33, 33, 1, 1, 48, 48, 80, 1);
			var world = new World(624, 624, 48, 48, 1);
			var bomb = new ClassicBomb(1, 1, 1, 1, 1, 1, 1, 1, 1); // Naudojame ClassicBomb kaip Bomb pavyzdį

			// 2. Sukurti Mediatorių
			var mediator = new GameMediator();

			// 3. Sukurti Colleagues (wrapperius)
			var playerColleague = new PlayerColleague(player);
			var worldColleague = new WorldColleague(world);
			var bombColleague = new BombColleague(bomb);

			// 4. Užregistruoti visus colleagues pas Mediatorių
			mediator.RegisterPlayer(playerColleague);
			mediator.RegisterWorld(worldColleague);
			mediator.RegisterBomb(bombColleague);

			// 5. Demonstracija - žaidėjas padeda bombą
			Console.WriteLine("--- Scenarijus 1: Žaidėjas padeda bombą ---");
			playerColleague.PlaceBomb();
			// Mediator automatiškai informuoja World ir Bomb

			Console.WriteLine("\n--- Scenarijus 2: Bomba sprogsta ---");
			bombColleague.Explode();
			// Mediator automatiškai informuoja World (sunaikinti sienas) ir Player (damage check)

			Console.WriteLine("\n--- Scenarijus 3: Žaidėjas pajuda ---");
			playerColleague.Move();
			// Mediator automatiškai tikrina kolizijas su Bomb ir bonus su World

			Console.WriteLine("\n--- Scenarijus 4: Siena sunaikinama ---");
			worldColleague.DestroyWall(3, 3);
			// Mediator informuoja Player apie galimą bonus

			Console.WriteLine("\n========== DEMO PABAIGA ==========");
		}
	}
}






