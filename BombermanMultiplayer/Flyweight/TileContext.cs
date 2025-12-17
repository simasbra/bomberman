using System;
using System.Drawing;
using BombermanMultiplayer.Decoratorrr;
using BombermanMultiplayer.Factory;
using BombermanMultiplayer.Objects;
using BombermanMultiplayer.Properties;

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

        public int[] CasePosition;

        public TileType TileType { get; private set; }

        public bool Walkable
        {
            get { return GetIntrinsicTile().Walkable; }
        }

        public bool Destroyable
        {
            get { return GetIntrinsicTile().Destroyable; }
        }

        public TileContext(TileType tileType, int x, int y, int row, int col)
        {
            TileType = tileType;
            X = x;
            Y = y;
            CasePosition = new int[] { row, col };
        }

        private Tile GetIntrinsicTile()
        {
            return TileFlyweightFactory.GetTile(TileType);
        }

        public void ConvertToFloor()
        {
            TileType = TileType.Floor;
        }

        public void Draw(Graphics gr, int frameWidth, int frameHeight)
        {
            var tile = GetIntrinsicTile();
            if (tile.GetSprite() != null)
            {
                gr.DrawImage(tile.GetSprite(), new Rectangle(X, Y, frameWidth, frameHeight), 0, 0, frameWidth, frameHeight, GraphicsUnit.Pixel);
                gr.DrawRectangle(Pens.Red, new Rectangle(X, Y, frameWidth, frameHeight));
            }

            if (BonusHere != null) BonusHere.Draw(gr);
        }

        // public new void Draw(Graphics gr)
        // {
        //     if (Sprite != null)
        //     {
        //         gr.DrawImage(Sprite, Source, frameindex * Source.Width, 0, Source.Width, Source.Height, GraphicsUnit.Pixel);
        //         gr.DrawRectangle(Pens.Red, Source);
        //     }
        //
        //     if (BonusHere != null)
        //     {
        //         BonusHere.Draw(gr);
        //     }
        // }

        public void SpawnBonus(int frameWidth, int frameHeight)
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            int num = r.Next(0, 4);
            Bonus baseBonus = null;

            if (num == 0)
            {
                baseBonus = new PowerBonusFactory().CreateBonus(X, Y, 1, frameWidth, frameHeight);
            }
            else if (num == 1)
            {
                baseBonus = new SpeedBonusFactory().CreateBonus(X, Y, 1, frameWidth, frameHeight);
            }
            else if (num == 2)
            {
                baseBonus = new DefuseBonusFactory().CreateBonus(X, Y, 1, frameWidth, frameHeight);
            }
            else if (num == 3)
            {
                baseBonus = new HealthBonusFactory().CreateBonus(X, Y, 1, frameWidth, frameHeight);
            }

            if (baseBonus != null)
            {
                // PROD: Random decorator application (unchanged logic)
                int decoratorChance = r.Next(0, 100);
                if (decoratorChance < 50)
                {
                    // 50% - plain bonus
                    BonusHere = baseBonus;
                }
                else if (decoratorChance < 80)
                {
                    // 30% - one decorator
                    BonusHere = new MultiplierDecorator(baseBonus, 1.5);
                }
                else if (decoratorChance < 95)
                {
                    // 15% - two decorators
                    Bonus decorated = new MultiplierDecorator(baseBonus, 2.0);
                    BonusHere = new DurationDecorator(decorated, 3000);
                }
                else
                {
                    // 5% - three decorators (golden!)
                    Bonus decorated = new MultiplierDecorator(baseBonus, 2.5);
                    decorated = new DurationDecorator(decorated, 5000);
                    BonusHere = new GlowEffectDecorator(decorated, "Golden", 90);
                }

                // Load correct sprite based on original type
                switch (num)
                {
                    case 0:
                        BonusHere.LoadSprite(Resources.SuperBomb);
                        break;
                    case 1:
                        BonusHere.LoadSprite(Resources.SpeedUp);
                        break;
                    case 2:
                        BonusHere.LoadSprite(Resources.Deactivate);
                        break;
                    case 3:
                        BonusHere.LoadSprite(Resources.Armor);
                        break;
                }

                BonusHere.CheckCasePosition(frameWidth, frameHeight);
            }
        }
    }
}