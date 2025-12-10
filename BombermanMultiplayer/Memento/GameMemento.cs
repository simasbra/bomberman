using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BombermanMultiplayer.Objects;
using static BombermanMultiplayer.World;

namespace BombermanMultiplayer.Memento
{
    /// <summary>
    /// Memento - Immutable snapshot of game state.
    /// Only Game (Originator) can create and restore from this.
    /// Other classes cannot access internal state.
    /// </summary>
    public sealed class GameMemento
    {
        // Private fields - only Game class can access via RestoreFromMemento
        private readonly PlayerSnapshot[] _playerSnapshots;
        private readonly TileSnapshot[,] _worldSnapshot;
        private readonly List<BombSnapshot> _bombSnapshots;
        private readonly List<MineSnapshot> _mineSnapshots;
        private readonly List<GrenadeSnapshot> _grenadeSnapshots;
        private readonly byte _winner;
        private readonly int _gamesPlayed;
        private readonly DateTime _timestamp;
        private readonly string _stateName;

        /// <summary>
        /// Public readonly property - Caretaker can display but not modify
        /// </summary>
        public DateTime Timestamp => _timestamp;
        public string StateName => _stateName;
        public string Description => $"{_stateName} - {_timestamp:HH:mm:ss}";

        /// <summary>
        /// Internal constructor - only Game class (same assembly) can create
        /// </summary>
        internal GameMemento(
            Player[] players,
            Tile[,] worldMap,
            List<Bomb> bombs,
            List<Mine> mines,
            List<Grenade> grenades,
            byte winner,
            int gamesPlayed,
            string stateName)
        {
            _timestamp = DateTime.Now;
            _stateName = stateName;
            _winner = winner;
            _gamesPlayed = gamesPlayed;

            // Deep copy players
            _playerSnapshots = players.Select(p => new PlayerSnapshot(p)).ToArray();

            // Deep copy world
            int rows = worldMap.GetLength(0);
            int cols = worldMap.GetLength(1);
            _worldSnapshot = new TileSnapshot[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    _worldSnapshot[i, j] = new TileSnapshot(worldMap[i, j]);
                }
            }

            // Deep copy explosives
            _bombSnapshots = bombs.Select(b => new BombSnapshot(b)).ToList();
            _mineSnapshots = mines.Select(m => new MineSnapshot(m)).ToList();
            _grenadeSnapshots = grenades.Select(g => new GrenadeSnapshot(g)).ToList();
        }

        /// <summary>
        /// Internal restore method - only Game can call this
        /// </summary>
        internal void RestoreTo(Game game)
        {
            // Restore players
            for (int i = 0; i < game.players.Length && i < _playerSnapshots.Length; i++)
            {
                _playerSnapshots[i].RestoreTo(game.players[i]);
            }

            // Restore world tiles
            int rows = Math.Min(game.world.MapGrid.GetLength(0), _worldSnapshot.GetLength(0));
            int cols = Math.Min(game.world.MapGrid.GetLength(1), _worldSnapshot.GetLength(1));
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    _worldSnapshot[i, j].RestoreTo(game.world.MapGrid[i, j]);
                }
            }

            // Restore explosives
            game.BombsOnTheMap.Clear();
            game.MinesOnTheMap.Clear();
            game.GrenadesOnTheMap.Clear();

            // Note: Explosives are not fully restored since they are abstract classes
            // In a real scenario, you would need concrete implementations or different approach

            // Restore game state
            game.Winner = _winner;
            game.GamesPlayed = _gamesPlayed;
        }

        #region Snapshot Classes (Private nested - encapsulation)

        private class PlayerSnapshot
        {
            public int[] CasePosition;
            public int SourceX;
            public int SourceY;
            public int Health;
            public byte BombNumb;
            public byte Power;
            public byte Speed;
            public bool Dead;
            public byte PlayerNum;

            public PlayerSnapshot(Player p)
            {
                CasePosition = p.CasePosition == null ? new int[2] { 0, 0 } : (int[])p.CasePosition.Clone();
                SourceX = p.Source.X;
                SourceY = p.Source.Y;
                Dead = p.Dead;
                PlayerNum = p.PlayerNumero;
                
                // Try to get other properties if they exist
                try
                {
                    var prop = typeof(Player).GetProperty("Health");
                    Health = prop != null ? (int)prop.GetValue(p) : 3;
                }
                catch { Health = 3; }

                try
                {
                    var prop = typeof(Player).GetProperty("BombNumb");
                    BombNumb = prop != null ? (byte)prop.GetValue(p) : (byte)2;
                }
                catch { BombNumb = 2; }

                Power = 3;
                Speed = 5;
            }

            public void RestoreTo(Player p)
            {
                if (CasePosition != null)
                {
                    // Restore pixel position (Source) - this is what matters for rendering!
                    p.Source = new Rectangle(SourceX, SourceY, p.Source.Width, p.Source.Height);
                    
                    // Also restore CasePosition
                    p.CasePosition = new int[] { CasePosition[0], CasePosition[1] };
                    
                    Console.WriteLine($"[Memento] Restored Player {PlayerNum}: Pixel=({SourceX},{SourceY}) Tile=[{CasePosition[0]},{CasePosition[1]}] Dead={Dead}");
                }
                p.Dead = Dead;
            }
        }

        private class TileSnapshot
        {
            public bool Walkable;
            public bool Destroyable;
            public bool Occupied;
            public bool HasBonus;

            public TileSnapshot(Tile t)
            {
                Walkable = t.Walkable;
                Destroyable = t.Destroyable;
                Occupied = t.Occupied;
                HasBonus = t.BonusHere != null;
            }

            public void RestoreTo(Tile t)
            {
                t.Walkable = Walkable;
                t.Destroyable = Destroyable;
                t.Occupied = Occupied;
            }
        }

        private class BombSnapshot
        {
            public int[] CasePosition;
            public int Power;
            public short Owner;

            public BombSnapshot(Bomb b)
            {
                CasePosition = b.CasePosition == null ? new int[2] { 0, 0 } : (int[])b.CasePosition.Clone();
                Power = b.Power;
                Owner = b.Proprietary;
            }

            public Bomb CreateBomb()
            {
                // Cannot instantiate abstract Bomb - store as data only
                return null; // Will need to handle differently
            }
        }

        private class MineSnapshot
        {
            public int[] CasePosition;
            public int Power;
            public short Owner;

            public MineSnapshot(Mine m)
            {
                CasePosition = m.CasePosition == null ? new int[2] { 0, 0 } : (int[])m.CasePosition.Clone();
                Power = m.Power;
                Owner = m.Proprietary;
            }

            public Mine CreateMine()
            {
                // Cannot instantiate abstract Mine
                return null;
            }
        }

        private class GrenadeSnapshot
        {
            public int[] CasePosition;
            public int Power;
            public short Owner;

            public GrenadeSnapshot(Grenade g)
            {
                CasePosition = g.CasePosition == null ? new int[2] { 0, 0 } : (int[])g.CasePosition.Clone();
                Power = g.Power;
                Owner = g.Proprietary;
            }

            public Grenade CreateGrenade()
            {
                // Cannot instantiate abstract Grenade
                return null;
            }
        }

        #endregion
    }
}
