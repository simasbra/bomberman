using BombermanMultiplayer.Commands.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Commands
{
    /// <summary>
    /// Command for dropping a mine on the map
    /// Mines activate when an enemy player steps on them
    /// Uses factory pattern to create appropriate mine type
    /// </summary>
    public class DropMineCommand : ICommand
    {
        private readonly Player _player;
        private readonly Tile[,] _mapGrid;
        private readonly List<Mine> _minesOnTheMap;
        private readonly Player _otherPlayer;
        private readonly DateTime _timestamp;

        private Mine _droppedMine;
        private readonly int _caseRow;
        private readonly int _caseCol;

        public DateTime Timestamp => _timestamp;
        public byte PlayerNumber => _player.PlayerNumero;

        /// <summary>
        /// Initialize drop mine command
        /// </summary>
        public DropMineCommand(Player player, Tile[,] mapGrid, List<Mine> minesOnTheMap, Player otherPlayer)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _mapGrid = mapGrid ?? throw new ArgumentNullException(nameof(mapGrid));
            _minesOnTheMap = minesOnTheMap ?? new List<Mine>();
            _otherPlayer = otherPlayer;
            _timestamp = DateTime.Now;

            _caseRow = player.CasePosition[0];
            _caseCol = player.CasePosition[1];
        }

        /// <summary>
        /// Execute mine drop - creates mine using player's factory and places on map
        /// </summary>
        public void Execute()
        {
            if (_player.Dead)
                return;
                
            if (_minesOnTheMap != null && _minesOnTheMap.Count >= 2) return;

            if (!_mapGrid[_caseRow, _caseCol].Occupied)
            {
                Mine mineToAdd;
                if (_player.ExplosiveFactory != null)
                {
                    mineToAdd = _player.ExplosiveFactory.CreateMine(_caseRow, _caseCol, 48, 48, (short)_player.PlayerNumero);
                }
                else
                {
                    mineToAdd = new ClassicMine(_caseRow, _caseCol, 8, 48, 48, 48, 48, (short)_player.PlayerNumero);
                }

                _droppedMine = mineToAdd;
                _minesOnTheMap.Add(_droppedMine);

                // Mines block the tile, but CheckProximity will activate them when player steps on
                //_mapGrid[_caseRow, _caseCol].Occupied = true;
                
                // IMPORTANT: Don't activate immediately - wait for player to step on it
                // CheckProximity() in MinesLogic will activate when player reaches this tile
            }
        }

        /// <summary>
        /// Undo mine drop - removes mine from map
        /// </summary>
        public void Undo()
        {
            if (_droppedMine == null)
                return;

            _minesOnTheMap.Remove(_droppedMine);

            _mapGrid[_caseRow, _caseCol].Occupied = false;

            _droppedMine.Dispose();
            _droppedMine = null;
        }

        public override string ToString()
        {
            return $"DropMineCommand: Žaidėjas {PlayerNumber} numetė miną pozicijoje [{_caseRow},{_caseCol}] - {Timestamp:HH:mm:ss.fff}";
        }
    }
}