using BombermanMultiplayer.Decoratorrr;
using BombermanMultiplayer.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Media;
using System.Text;
using System.Windows.Forms;
using BombermanMultiplayer.Factory;

namespace BombermanMultiplayer
{
    [Serializable]
    public class Tile : GameObject
    {
        public bool Walkable = false;
        public bool Destroyable = false;
        public bool Occupied = false;
        public bool Fire = false;

        [NonSerialized]
        public Bonus BonusHere = null;
        [NonSerialized]
        public Bomb bomb = null;

        public int FireTime = 500; 

        public Image GetSprite()
        {
            return Sprite;
        }

        public Tile(int x_, int y_, int totalFrame_, int frameWidth_, int frameHeigt_,  bool walkable, bool destroyable)
            : base(x_, y_, totalFrame_, frameWidth_, frameHeigt_)
        {
            Walkable = walkable;
            Destroyable = destroyable;
        }

    }
}
