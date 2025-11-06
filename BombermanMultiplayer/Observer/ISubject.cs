using System;
using System.Collections.Generic;

namespace BombermanMultiplayer
{
    /// <summary>
    /// Subject interface for Observer pattern
    /// Defines methods for attaching, detaching, and notifying observers
    /// </summary>
    public interface ISubject
    {
        /// <summary>
        /// Attach an observer to receive notifications
        /// </summary>
        /// <param name="observer">The observer to attach</param>
        void Attach(IObserver observer);

        /// <summary>
        /// Detach an observer from receiving notifications
        /// </summary>
        /// <param name="observer">The observer to detach</param>
        void Detach(IObserver observer);

        /// <summary>
        /// Notify all attached observers of a state change
        /// </summary>
        void Notify();
    }
}
