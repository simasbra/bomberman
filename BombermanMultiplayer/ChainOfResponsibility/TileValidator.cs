using BombermanMultiplayer.Commands.Interface;
using BombermanMultiplayer.Commands;
using System;

namespace BombermanMultiplayer.ChainOfResponsibility
{
    /// <summary>
    /// Validates tile/position - checks if action can be performed at current location
    /// Fourth validator: Tile must be walkable, not occupied, etc.
    /// </summary>
    public class TileValidator : ValidationHandler
    {
        public override bool Validate(ICommand command, Game game)
        {
            byte playerNum = command.PlayerNumber;
            var player = game.players[playerNum - 1];

            // Check bomb drop commands - tile must not already have a bomb
            if (command is DropBombCommand || command is DropMineCommand || command is DropGrenadeCommand)
            {
                int row = player.CasePosition[0];
                int col = player.CasePosition[1];

                // Check if tile is within bounds
                if (row < 0 || row >= game.world.MapGrid.GetLength(0) ||
                    col < 0 || col >= game.world.MapGrid.GetLength(1))
                {
                    Console.WriteLine($"[4. Tile] X REJECTED - Position out of bounds ({row}, {col})");
                    return false;
                }

                var tile = game.world.MapGrid[row, col];

                // Check if tile already has a bomb
                if (tile.bomb != null)
                {
                    Console.WriteLine($"[4. Tile] X REJECTED - Tile already has explosive at ({row}, {col})");
                    return false;
                }

                Console.WriteLine($"[4. Tile] + PASSED - Tile ({row}, {col}) is available");
            }
            else
            {
                // Not a drop command - Tile validator only checks bomb placement, not movement
                // (Movement validator handles walkability check separately)
                Console.WriteLine($"[4. Tile] + PASSED - Not a bomb drop command");
            }

            return base.Validate(command, game);
        }
    }
}
