using System;
using System.Windows.Forms;
using BombermanMultiplayer.Commands;
using BombermanMultiplayer.Commands.Interface;

namespace BombermanMultiplayer.Adapters
{
    public class PacketCommandAdapter : IPacketCommandAdapter
    {
        private readonly Game Game;
        private readonly Packet Packet;
        private readonly Game _game;//pridedu, nes ne kitaip neveikia

        /// <summary>
        /// Initializes a new instance of the PacketCommandAdapter class with a reference to the game
        /// </summary>
        /// <param name="game">The game instance to which commands should be applied</param>
        /// <param name="packet">The packet to be converted</param>
        public PacketCommandAdapter(Packet packet, Game game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
            Packet = packet ?? throw new ArgumentNullException(nameof(packet));
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

            Player player = Game.players[playerIndex];

            if (player.Dead)
                return null;

            int otherPlayerIndex = (playerIndex + 1) % Game.players.Length;
            Player otherPlayer = Game.players[otherPlayerIndex];

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
                    return new DropBombCommand(player, Game.world.MapGrid, Game.BombsOnTheMap, otherPlayer);

                case Keys.M:
                    return new DropMineCommand(player, Game.world.MapGrid, Game.MinesOnTheMap, otherPlayer);

                case Keys.G:
                    return new DropGrenadeCommand(player, Game.world.MapGrid, Game.GrenadesOnTheMap, otherPlayer);

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