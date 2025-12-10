using BombermanMultiplayer.Commands;
using BombermanMultiplayer.Commands.Interface;
using System;

namespace BombermanMultiplayer.ChainOfResponsibility
{
    /// <summary>
    /// Validates movement commands - checks if the next tile is walkable and not occupied.
    /// Third validator when handling MovePlayerCommand.
    /// </summary>
    public class MovementValidator : ValidationHandler
    {
        public override bool Validate(ICommand command, Game game)
        {
            if (command is MovePlayerCommand moveCommand)
            {
                byte playerNum = command.PlayerNumber;
                var player = game.players[playerNum - 1];

                int row = player.CasePosition[0];
                int col = player.CasePosition[1];

                // Determine target tile based on intended direction
                switch (moveCommand.Direction)
                {
                    case Player.MovementDirection.UP:
                        row -= 1; break;
                    case Player.MovementDirection.DOWN:
                        row += 1; break;
                    case Player.MovementDirection.LEFT:
                        col -= 1; break;
                    case Player.MovementDirection.RIGHT:
                        col += 1; break;
                    default:
                        break;
                }

                // Bounds check
                if (row < 0 || row >= game.world.MapGrid.GetLength(0) ||
                    col < 0 || col >= game.world.MapGrid.GetLength(1))
                {
                    Console.WriteLine($"[2. Movement] X REJECTED - Target out of bounds ({row}, {col})");
                    return false;
                }

                var tile = game.world.MapGrid[row, col];

                // Walkability / occupancy check
                if (!tile.Walkable || tile.Occupied)
                {
                    Console.WriteLine($"[2. Movement] X REJECTED - Tile blocked at ({row}, {col})");
                    return false;
                }

                // Optional: avoid walking onto active fire or bomb if needed
                Console.WriteLine($"[2. Movement] + PASSED - Tile ({row}, {col}) is walkable");
            }
            else
            {
                Console.WriteLine("[2. Movement] + PASSED - Not a movement command");
            }

            return base.Validate(command, game);
        }
    }
}
