using BombermanMultiplayer.Commands.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Commands
{
    /// <summary>
    /// Command for dropping a grenade on the map
    /// Grenades have fastest detonation and widest blast radius
    /// Uses factory pattern to create appropriate grenade type
    /// </summary>
    public class DropGrenadeCommand : ICommand
    {
        private readonly Player _player;
        private readonly Tile[,] _mapGrid;
        private readonly List<Grenade> _grenadesOnTheMap;
        private readonly Player _otherPlayer;
        private readonly DateTime _timestamp;

        private Grenade _droppedGrenade;
        private readonly int _caseRow;
        private readonly int _caseCol;

        public DateTime Timestamp => _timestamp;
        public byte PlayerNumber => _player.PlayerNumero;

        /// <summary>
        /// Initialize drop grenade command
        /// </summary>
        public DropGrenadeCommand(Player player, Tile[,] mapGrid, List<Grenade> grenadesOnTheMap, Player otherPlayer)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _mapGrid = mapGrid ?? throw new ArgumentNullException(nameof(mapGrid));
            _grenadesOnTheMap = grenadesOnTheMap ?? new List<Grenade>();
            _otherPlayer = otherPlayer;
            _timestamp = DateTime.Now;

            _caseRow = player.CasePosition[0];
            _caseCol = player.CasePosition[1];
        }

        /// <summary>
        /// Execute grenade drop - creates grenade using player's factory and places on map
        /// </summary>
        public void Execute()
        {
            if (_player.Dead)
                return;

            if (!_mapGrid[_caseRow, _caseCol].Occupied)
            {
                // Grenades are NOT blocking tiles - they are projectiles
                Grenade grenadeToAdd;
                if (_player.ExplosiveFactory != null)
                {
                    grenadeToAdd = _player.ExplosiveFactory.CreateGrenade(_caseRow, _caseCol, 48, 48, (short)_player.PlayerNumero);
                }
                else
                {
                    grenadeToAdd = new ClassicGrenade(_caseRow, _caseCol, 8, 48, 48, 1000, 48, 48, (short)_player.PlayerNumero);
                }

                _droppedGrenade = grenadeToAdd;
                _grenadesOnTheMap.Add(_droppedGrenade);

                _mapGrid[_caseRow, _caseCol].Occupied = true;
                
                // Determine throw distance based on grenade type
                int throwDistance = 3; // Classic grenade default
                if (_droppedGrenade is AdvancedGrenade)
                {
                    throwDistance = 5; // Advanced grenades throw further
                }

                // Throw grenade in the direction player is facing
                // Use LastOrientation if player is not currently moving (Orientation == NONE)
                Player.MovementDirection throwDirection = _player.Orientation;
                if (throwDirection == Player.MovementDirection.NONE)
                {
                    throwDirection = _player.LastOrientation; // Use last known direction
                }

                int targetRow = _caseRow;
                int targetCol = _caseCol;

                switch (throwDirection)
                {
                    case Player.MovementDirection.UP:
                        targetRow = _caseRow - throwDistance;
                        break;
                    case Player.MovementDirection.DOWN:
                        targetRow = _caseRow + throwDistance;
                        break;
                    case Player.MovementDirection.LEFT:
                        targetCol = _caseCol - throwDistance;
                        break;
                    case Player.MovementDirection.RIGHT:
                        targetCol = _caseCol + throwDistance;
                        break;
                    default:
                        targetRow = _caseRow - throwDistance; // Fallback to UP
                        break;
                }

                // Throw grenade toward target position
                _droppedGrenade.Throw(targetRow, targetCol);
            }
        }

        /// <summary>
        /// Undo grenade drop - removes grenade from map
        /// </summary>
        public void Undo()
        {
            if (_droppedGrenade == null)
                return;

            _grenadesOnTheMap.Remove(_droppedGrenade);

            _mapGrid[_caseRow, _caseCol].Occupied = false;

            _droppedGrenade.Dispose();
            _droppedGrenade = null;
        }

        public override string ToString()
        {
            return $"DropGrenadeCommand: Þaidëjas {PlayerNumber} numetë granatà pozicijoje [{_caseRow},{_caseCol}] - {Timestamp:HH:mm:ss.fff}";
        }
    }
}