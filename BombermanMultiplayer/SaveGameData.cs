using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer
{
    [Serializable]
    public struct SaveGameData
    {
        public List<Bomb> bombsOnTheMap;
        public Tile[,] MapGrid;
        public Player[] players;


        public SaveGameData(List<Bomb> bombsOnTheMap_, Tile[,] MapGrid_, Player[] players)
        {
            this.bombsOnTheMap = bombsOnTheMap_;
            this.MapGrid = MapGrid_;
            this.players = players;
        }


    }
}
