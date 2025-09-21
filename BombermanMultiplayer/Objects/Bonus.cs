using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BombermanMultiplayer.Objects
{
    public class Bonus : GameObject
    {

        public BonusType Type = BonusType.None;


        public Bonus(int x, int y, int frameNumber, int frameWidth, int frameHeight, BonusType type ) 
            : base(x, y, frameNumber, frameWidth, frameHeight)
        {
            this.Type = type;

        }

        public void CheckCasePosition(int TileWidth, int TileHeight)
        {
            this.CasePosition[0] = this.Source.Y / TileWidth; //Row
            this.CasePosition[1] = this.Source.X / TileWidth; //Column
        }

    }

    public enum BonusType
    {
        None,
        PowerBomb,
        SpeedBoost,
        Deactivate,
        Armor

    }
}
