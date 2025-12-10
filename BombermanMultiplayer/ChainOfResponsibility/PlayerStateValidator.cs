using BombermanMultiplayer.Commands.Interface;
using System;

namespace BombermanMultiplayer.ChainOfResponsibility
{
    /// <summary>
    /// Validates player state - checks if player can perform actions
    /// Second validator: Player must be alive and have valid index
    /// </summary>
    public class PlayerStateValidator : ValidationHandler
    {
        public override bool Validate(ICommand command, Game game)
        {
            byte playerNum = command.PlayerNumber;

            // Check if player number is valid
            if (playerNum < 1 || playerNum > game.players.Length)
            {
                Console.WriteLine($"\n=== Validation Chain: {command.GetType().Name} (Player {playerNum}) ===");
                Console.WriteLine($"[1. PlayerState] X REJECTED - Invalid player number: {playerNum}");
                Console.WriteLine($"=== Validation Result: FAILED (Invalid Player) X ===\n");
                return false;
            }

            var player = game.players[playerNum - 1];

            // Check if player is dead
            if (player.Dead)
            {
                Console.WriteLine($"\n=== Validation Chain: {command.GetType().Name} (Player {playerNum}) ===");
                Console.WriteLine($"[1. PlayerState] X REJECTED - Player {playerNum} is dead");
                Console.WriteLine($"=== Validation Result: FAILED (Player Dead) X ===\n");
                return false;
            }

            Console.WriteLine($"\n=== Validation Chain: {command.GetType().Name} (Player {playerNum}) ===");
            Console.WriteLine($"[1. PlayerState] + PASSED - Player {playerNum} is alive and active");
            return base.Validate(command, game);
        }
    }
}
