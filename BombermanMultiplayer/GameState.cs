using BombermanMultiplayer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer
{
    [Serializable]
    public class GameState : ISubject
    {
        // Observer pattern: List of observers
        [NonSerialized]
        private List<IObserver> observers = new List<IObserver>();

        // State data
        private SaveGameData state;

        //If the game is paused
        public bool Paused = false;

        // Support for up to 4 players using arrays
        public short[][] XY_Position_Players = new short[4][];
        public short[] framePlayers = new short[4];
        public Player.MovementDirection[] orientationPlayers = new Player.MovementDirection[4];
        public short[] NbBomb_Players = new short[4];
        public bool[] deadPlayers = new bool[4];
        public BonusType[][] BonusSlotPlayers = new BonusType[4][];
        public short[][] BonusTimerPlayers = new short[4][];
        public string[] NamePlayers = new string[4];

        public byte Winner = 0;
        public bool Over;

        //Bomb list
        public List<Bomb> bombsList;

        public Byte[,] map;

        public GameState()
        {
            for (int i = 0; i < 4; i++)
            {
                XY_Position_Players[i] = new short[2];
                BonusSlotPlayers[i] = new BonusType[2];
                BonusTimerPlayers[i] = new short[2];
                NamePlayers[i] = "";
            }
            observers = new List<IObserver>();
        }

        /// <summary>
        /// Get the current game state data
        /// </summary>
        /// <returns>Current SaveGameData state</returns>
        public SaveGameData GetState()
        {
            return state;
        }

        /// <summary>
        /// Set the game state and notify all observers
        /// </summary>
        /// <param name="newState">New SaveGameData state</param>
        public void SetState(SaveGameData newState)
        {
            this.state = newState;
            Notify();
        }

        /// <summary>
        /// Attach an observer to receive notifications
        /// </summary>
        /// <param name="observer">The observer to attach</param>
        public void Attach(IObserver observer)
        {
            if (observer != null && !observers.Contains(observer))
            {
                observers.Add(observer);
            }
        }

        /// <summary>
        /// Detach an observer from receiving notifications
        /// </summary>
        /// <param name="observer">The observer to detach</param>
        public void Detach(IObserver observer)
        {
            if (observer != null)
            {
                observers.Remove(observer);
            }
        }

        /// <summary>
        /// Notify all attached observers of a state change
        /// </summary>
        public void Notify()
        {
            foreach (IObserver observer in observers)
            {
                observer.Update();
            }
        }
    }
}
