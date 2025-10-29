using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Commands.Interface
{
    public interface ICommand
    {
        void Execute();
        void Undo();
        DateTime Timestamp { get; }
        byte PlayerNumber { get; }
    }
}
