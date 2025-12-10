using System;
using System.Drawing;
using BombermanMultiplayer.Objects;

namespace BombermanMultiplayer.Flyweight
{
    [Serializable]
    public class TileContext
    {
        public int X { get; set; }
        public int Y { get; set; }

        [NonSerialized]
        public Bonus BonusHere;

        [NonSerialized]
        public Bomb Bomb;

        public bool Fire;
        public bool Occupied;
        public int FireTime = 500;

        private readonly Tile IntrinsicTile;

        public bool Walkable
        {
            get { return IntrinsicTile.Walkable; }
        }

        public bool Destroyable
        {
            get { return IntrinsicTile.Destroyable; }
        }

        public TileContext(Tile intrinsicTile, int x, int y)
        {
            IntrinsicTile = intrinsicTile;
            X = x;
            Y = y;
        }

        public void Draw(Graphics gr)
        {
            var tempRect = IntrinsicTile.Source;
            IntrinsicTile.Source = new Rectangle(X, Y, tempRect.Width, tempRect.Height);
            IntrinsicTile.Draw(gr);
            IntrinsicTile.Source = tempRect;

            if (BonusHere != null) BonusHere.Draw(gr);
        }
    }
}