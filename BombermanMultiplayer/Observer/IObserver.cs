using System;
using System.Collections.Generic;

namespace BombermanMultiplayer
{
    /// <summary>
    /// Observer interface for Observer pattern
    /// Defines the update method that observers must implement
    /// </summary>
    public interface IObserver
    {
        /// <summary>
        /// Called when the subject notifies observers of a state change
        /// </summary>
        void Update();
    }
}
