using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BombermanMultiplayer.Commands;
using BombermanMultiplayer.Commands.Interface;

namespace BombermanMultiplayer.Adapters
{
    /// <summary>
    /// Adapter that converts network Packet objects into executable ICommand objects
    /// Implements the Adapter design pattern to bridge network communication and command execution
    /// </summary>
    public class PacketCommandAdapter
    {
        private readonly Game _game;

        /// <summary>
        /// Initializes a new instance of the PacketCommandAdapter class
        /// </summary>
        /// <param name="game">The game instance that commands will operate on</param>
        public PacketCommandAdapter(Game game)
        {
            _game = game ?? throw new ArgumentNullException(nameof(game));
        }

        /// <summary>
        /// Converts a network packet into an executable command
        /// </summary>
        /// <param name="packet">The packet to convert</param>
        /// <returns>An ICommand that can be executed, or null if packet cannot be converted</returns>
        public ICommand ConvertToCommand(Packet packet)
        {
            if (packet == null || packet.Empty())
                return null;

            PacketType type = packet.GetPacketType();
            Sender sender = packet.GetSender();

            if (type != PacketType.KeyDown)
                return null;

            Keys key = packet.GetPayload<Keys>();
            int playerIndex = GetPlayerIndex(sender);

            if (playerIndex == -1)
                return null;

            Player player = _game.players[playerIndex];

            if (player.Dead)
                return null;

            int otherPlayerIndex = (playerIndex + 1) % _game.players.Length;
            Player otherPlayer = _game.players[otherPlayerIndex];

            switch (key)
            {
                case Keys.Up:
                    return new MovePlayerCommand(player, Player.MovementDirection.UP);

                case Keys.Down:
                    return new MovePlayerCommand(player, Player.MovementDirection.DOWN);

                case Keys.Left:
                    return new MovePlayerCommand(player, Player.MovementDirection.LEFT);

                case Keys.Right:
                    return new MovePlayerCommand(player, Player.MovementDirection.RIGHT);

                case Keys.Space:
                    return new DropBombCommand(player, _game.world.MapGrid, _game.BombsOnTheMap, otherPlayer);

                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the player array index from the Sender enum
        /// </summary>
        /// <param name="sender">The sender identifier</param>
        /// <returns>Player index (0-3) or -1 if sender is not a player</returns>
        private int GetPlayerIndex(Sender sender)
        {
            switch (sender)
            {
                case Sender.Player1: return 0;
                case Sender.Player2: return 1;
                case Sender.Player3: return 2;
                case Sender.Player4: return 3;
                default: return -1;
            }
        }
    }
}
