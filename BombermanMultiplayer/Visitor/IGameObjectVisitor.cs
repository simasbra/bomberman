using BombermanMultiplayer.Objects;

namespace BombermanMultiplayer.Visitor
{
    /// <summary>
    /// Visitor interface for game objects.
    /// Defines visit methods for each type of game object.
    /// </summary>
    public interface IGameObjectVisitor
    {
        /// <summary>
        /// Visits a Player object
        /// </summary>
        /// <param name="player">The player to visit</param>
        void VisitPlayer(Player player);

        /// <summary>
        /// Visits a Bomb object
        /// </summary>
        /// <param name="bomb">The bomb to visit</param>
        void VisitBomb(Bomb bomb);

        /// <summary>
        /// Visits a Mine object
        /// </summary>
        /// <param name="mine">The mine to visit</param>
        void VisitMine(Mine mine);

        /// <summary>
        /// Visits a Grenade object
        /// </summary>
        /// <param name="grenade">The grenade to visit</param>
        void VisitGrenade(Grenade grenade);

        /// <summary>
        /// Visits a Tile object
        /// </summary>
        /// <param name="tile">The tile to visit</param>
        void VisitTile(Tile tile);
    }
}
