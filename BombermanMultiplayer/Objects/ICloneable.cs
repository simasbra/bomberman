using System;

namespace BombermanMultiplayer
{
    /// <summary>
    /// Defines methods to create shallow and deep copies of an object.
    /// </summary>
    public interface ICloneable
    {
        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new object that is a copy of this instance.</returns>
        object Clone();

        /// <summary>
        /// Creates a deep copy of the current object.
        /// </summary>
        /// <returns>A new object that is a deep copy of the current instance.</returns>
        object DeepClone();
    }
}
