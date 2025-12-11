namespace BombermanMultiplayer.Visitor
{
    /// <summary>
    /// Interface for objects that can accept visitors.
    /// Part of Visitor pattern - allows operations to be performed on objects without modifying their classes.
    /// </summary>
    public interface IVisitable
    {
        /// <summary>
        /// Accepts a visitor and allows it to perform operations on this object
        /// </summary>
        /// <param name="visitor">The visitor to accept</param>
        void Accept(IGameObjectVisitor visitor);
    }
}
