using BombermanMultiplayer.Commands.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Commands
{
    public class DropBombCommand : ICommand
    {
        private readonly Player _player;
        private readonly Tile[,] _mapGrid;
        private readonly List<Bomb> _bombsOnTheMap;
        private readonly Player _otherPlayer;
        private readonly DateTime _timestamp;

        private Bomb _droppedBomb;
        private readonly int _caseRow;
        private readonly int _caseCol;
        private readonly byte _previousBombCount;

        public DateTime Timestamp => _timestamp;
        public byte PlayerNumber => _player.PlayerNumero;

        public DropBombCommand(Player player, Tile[,] mapGrid, List<Bomb> bombsOnTheMap, Player otherPlayer)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _mapGrid = mapGrid ?? throw new ArgumentNullException(nameof(mapGrid));
            _bombsOnTheMap = bombsOnTheMap ?? throw new ArgumentNullException(nameof(bombsOnTheMap));
            _otherPlayer = otherPlayer;
            _timestamp = DateTime.Now;

            _caseRow = player.CasePosition[0];
            _caseCol = player.CasePosition[1];
            _previousBombCount = player.BombNumb;
        }

        public void Execute()
        {
            if (_player.Dead)
                return;

            if (_player.BombNumb > 0)
            {
                if (!_mapGrid[_caseRow, _caseCol].Occupied)
                {
                    _droppedBomb = new Bomb(_caseRow, _caseCol, 8, 48, 48, 2000, 48, 48, _player.PlayerNumero);
                    _bombsOnTheMap.Add(_droppedBomb);

                    _mapGrid[_caseRow, _caseCol].bomb = _droppedBomb;
                    _mapGrid[_caseRow, _caseCol].Occupied = true;

                    _player.BombNumb--;
                }
            }
        }

        public void Undo()
        {
            if (_droppedBomb == null)
                return;

            _bombsOnTheMap.Remove(_droppedBomb);

            _mapGrid[_caseRow, _caseCol].bomb = null;
            _mapGrid[_caseRow, _caseCol].Occupied = false;

            _player.BombNumb = _previousBombCount;

            _droppedBomb.Dispose();
            _droppedBomb = null;
        }

        public override string ToString()
        {
            return $"DropBombCommand: Žaidėjas {PlayerNumber} numetė bombą pozicijoje [{_caseRow},{_caseCol}] - {Timestamp:HH:mm:ss.fff}";
        }
    }
}
