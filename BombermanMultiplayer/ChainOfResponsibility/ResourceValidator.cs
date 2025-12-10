using BombermanMultiplayer.Commands.Interface;
using BombermanMultiplayer.Commands;
using System;

namespace BombermanMultiplayer.ChainOfResponsibility
{
    /// <summary>
    /// Validates resources - checks if player has necessary resources for action
    /// Third validator: Player must have bombs available, sufficient mana, etc.
    /// </summary>
    public class ResourceValidator : ValidationHandler
    {
        public override bool Validate(ICommand command, Game game)
        {
            byte playerNum = command.PlayerNumber;
            var player = game.players[playerNum - 1];

            // Check bomb/explosive drop commands
            if (command is DropBombCommand || command is DropMineCommand || command is DropGrenadeCommand)
            {
                // Count current explosives from this player
                int currentExplosives = 0;
                foreach (var bomb in game.BombsOnTheMap)
                {
                    if (bomb.Proprietary == playerNum)
                        currentExplosives++;
                }

                foreach (var mine in game.MinesOnTheMap)
                {
                    if (mine.Proprietary == playerNum)
                        currentExplosives++;
                }

                foreach (var grenade in game.GrenadesOnTheMap)
                {
                    if (grenade.Proprietary == playerNum)
                        currentExplosives++;
                }

                // Check if player has reached explosive limit
                if (currentExplosives >= player.BombNumb)
                {
                    Console.WriteLine($"[3. Resources] X REJECTED - Explosive limit reached ({currentExplosives}/{player.BombNumb})");
                    return false;
                }
                
                Console.WriteLine($"[3. Resources] + PASSED - Resources available ({currentExplosives}/{player.BombNumb} explosives)");
            }
            else
            {
                // Move commands and others don't need resource check
                Console.WriteLine($"[3. Resources] + PASSED - No resource check needed");
            }

            return base.Validate(command, game);
        }
    }
}
