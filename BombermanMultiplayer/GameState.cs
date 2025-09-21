using BombermanMultiplayer.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer
{
    [Serializable]
    public class GameState
    {

        //If the game is paused
        public bool Paused = false;

        public List<Player> players;

        public byte Winner = 0;
        public bool Over;

        //Bomb list
        public List<Bomb> bombsList;

        public Byte[,] map;

    }
}
