using BombermanMultiplayer.Commands.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Commands
{
    public class MovePlayerCommand : ICommand
    {
        private readonly Player _player;
        private readonly Player.MovementDirection _direction;
        private readonly Bitmap _sprite;
        private readonly DateTime _timestamp;

        private readonly Player.MovementDirection _previousDirection;
        private readonly Point _previousPosition;

        public DateTime Timestamp => _timestamp;
        public byte PlayerNumber => _player.PlayerNumero;
        public Player.MovementDirection Direction => _direction;

        public MovePlayerCommand(Player player, Player.MovementDirection direction, Bitmap sprite = null)
        {
            _player = player ?? throw new ArgumentNullException(nameof(player));
            _direction = direction;
            _sprite = sprite;
            _timestamp = DateTime.Now;

            _previousDirection = player.Orientation;
            _previousPosition = new Point(player.Source.X, player.Source.Y);
        }

        public void Execute()
        {
            if (_player.Dead)
                return;

            _player.Orientation = _direction;

            if (_sprite != null)
            {
                _player.LoadSprite(_sprite);
            }
        }

        public void Undo()
        {
            if (_player.Dead)
                return;

            _player.Orientation = _previousDirection;
            _player.Source = new Rectangle(_previousPosition.X, _previousPosition.Y,
                                          _player.Source.Width, _player.Source.Height);
        }

        public override string ToString()
        {
            return $"MovePlayerCommand: Žaidėjas {PlayerNumber} juda {_direction} - {Timestamp:HH:mm:ss.fff}";
        }
    }
}
