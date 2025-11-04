using BombermanMultiplayer.Commands.Interface;

namespace BombermanMultiplayer.Adapters
{
    /// <summary>
    /// Interface for converting network packets into executable commands
    /// </summary>
    public interface IPacketCommandAdapter
    {
        /// <summary>
        /// Converts a network packet into an executable command
        /// </summary>
        /// <param name="packet">The packet to convert</param>
        /// <returns>An ICommand that can be executed</returns>
        ICommand convertToCommand(Packet packet);
    }
}
