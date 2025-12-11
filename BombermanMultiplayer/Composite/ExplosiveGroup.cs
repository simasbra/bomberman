using System;
using System.Collections.Generic;
using System.Drawing;
using BombermanMultiplayer.Objects;

namespace BombermanMultiplayer.Composite
{
    /// <summary>
    /// Composite class that groups multiple explosives together.
    /// Implements Composite pattern - treats groups and individual explosives uniformly.
    /// </summary>
    [Serializable]
    public class ExplosiveGroup : IExplosive
    {
        private List<IExplosive> explosives;
        private string name;

        /// <summary>
        /// Gets the name of this explosive group
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Gets the number of explosives in this group
        /// </summary>
        public int Count => explosives.Count;

        /// <summary>
        /// Initializes a new instance of ExplosiveGroup
        /// </summary>
        /// <param name="name">Name of the group</param>
        public ExplosiveGroup(string name)
        {
            this.name = name;
            this.explosives = new List<IExplosive>();
        }

        /// <summary>
        /// Adds an explosive to the group
        /// </summary>
        /// <param name="explosive">The explosive to add</param>
        public void Add(IExplosive explosive)
        {
            if (explosive != null && !explosives.Contains(explosive))
            {
                explosives.Add(explosive);
            }
        }

        /// <summary>
        /// Removes an explosive from the group
        /// </summary>
        /// <param name="explosive">The explosive to remove</param>
        public void Remove(IExplosive explosive)
        {
            if (explosive != null)
            {
                explosives.Remove(explosive);
            }
        }

        /// <summary>
        /// Gets a child explosive by index
        /// </summary>
        /// <param name="index">Index of the explosive</param>
        /// <returns>The explosive at the specified index, or null if invalid</returns>
        public IExplosive GetChild(int index)
        {
            if (index >= 0 && index < explosives.Count)
            {
                return explosives[index];
            }
            return null;
        }

        /// <summary>
        /// Updates all explosives in the group
        /// </summary>
        /// <param name="elapsedTime">Time elapsed since last update in milliseconds</param>
        /// <param name="mapGrid">The game map grid</param>
        /// <param name="players">Array of players in the game</param>
        public void Update(int elapsedTime, Tile[,] mapGrid, Player[] players)
        {
            // Update all explosives in the group
            for (int i = explosives.Count - 1; i >= 0; i--)
            {
                if (explosives[i] != null)
                {
                    explosives[i].Update(elapsedTime, mapGrid, players);
                }
            }
        }

        /// <summary>
        /// Triggers explosion for all explosives in the group
        /// </summary>
        /// <param name="mapGrid">The game map grid</param>
        /// <param name="players">Array of players in the game</param>
        public void Explode(Tile[,] mapGrid, Player[] players)
        {
            foreach (var explosive in explosives)
            {
                if (explosive != null)
                {
                    explosive.Explode(mapGrid, players);
                }
            }
        }

        /// <summary>
        /// Checks if any explosive in the group is exploding
        /// </summary>
        /// <returns>True if any explosive is exploding, false otherwise</returns>
        public bool IsExploding()
        {
            foreach (var explosive in explosives)
            {
                if (explosive != null && explosive.IsExploding())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the position of the group (returns first explosive's position, or 0,0 if empty)
        /// </summary>
        /// <returns>Point representing the position</returns>
        public Point GetPosition()
        {
            if (explosives.Count > 0 && explosives[0] != null)
            {
                return explosives[0].GetPosition();
            }
            return new Point(0, 0);
        }

        /// <summary>
        /// Gets all explosives in the group
        /// </summary>
        /// <returns>List of explosives</returns>
        public List<IExplosive> GetExplosives()
        {
            return new List<IExplosive>(explosives);
        }
    }
}
