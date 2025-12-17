using System;
using System.Drawing;
using BombermanMultiplayer.Objects;
using BombermanMultiplayer.Decoratorrr;
using BombermanMultiplayer.Factory;

namespace BombermanMultiplayer.Flyweight
{
    [Serializable]
    public class TileContext
    {
        public int X { get; set; }
        public int Y { get; set; }

        [NonSerialized]
        public Bonus BonusHere;

        public bool BonusSerialized
        {
            get { return BonusHere != null; }
            set { /* handled by gamestate logic if needed */ }
        }

        [NonSerialized]
        public Bomb bomb;

        public bool Fire;
        public bool Occupied;
        public int FireTime = 500;

        public bool Walkable { get; set; }
        public bool Destroyable { get; set; }

        public Tile IntrinsicTile { get; private set; }

        public int IntrinsicTile_Source_Width => IntrinsicTile.Source.Width;
        public int IntrinsicTile_Source_Height => IntrinsicTile.Source.Height;

        public TileContext(Tile intrinsicTile, int x, int y)
        {
            IntrinsicTile = intrinsicTile;
            X = x;
            Y = y;
            Walkable = intrinsicTile.Walkable;
            Destroyable = intrinsicTile.Destroyable;
        }

        public void RebindIntrinsic(Tile tile)
        {
            IntrinsicTile = tile;
        }

        /// <summary>
        /// Spawns a random bonus on this tile.
        /// </summary>
        public void SpawnBonus()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            int num = r.Next(0, 4);
            Bonus baseBonus = null;

            int tileWidth = IntrinsicTile.Source.Width;
            int tileHeight = IntrinsicTile.Source.Height;

            // Use factories instead of direct instantiation
            if (num == 0)
            {
                baseBonus = new PowerBonusFactory().CreateBonus(X, Y, 1, tileWidth, tileHeight);
            }
            else if (num == 1)
            {
                baseBonus = new SpeedBonusFactory().CreateBonus(X, Y, 1, tileWidth, tileHeight);
            }
            else if (num == 2)
            {
                baseBonus = new DefuseBonusFactory().CreateBonus(X, Y, 1, tileWidth, tileHeight);
            }
            else if (num == 3)
            {
                baseBonus = new HealthBonusFactory().CreateBonus(X, Y, 1, tileWidth, tileHeight);
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
                        BonusHere.LoadSprite(Properties.Resources.SuperBomb);
                        break;
                    case 1:
                        BonusHere.LoadSprite(Properties.Resources.SpeedUp);
                        break;
                    case 2:
                        BonusHere.LoadSprite(Properties.Resources.Deactivate);
                        break;
                    case 3:
                        BonusHere.LoadSprite(Properties.Resources.Armor);
                        break;
                }

                BonusHere.CheckCasePosition(tileWidth, tileHeight);
                System.Diagnostics.Debug.WriteLine($"Spawned bonus: {BonusHere.GetDescription()}");
            }
        }

        /// <summary>
        /// Draws the tile and any objects on it.
        /// </summary>
        /// <param name="gr">Graphics object to draw on.</param>
        public void Draw(Graphics gr)
        {
            // Draw the base tile
            IntrinsicTile.DrawAt(gr, X, Y);

            // Draw fire if active
            if (Fire)
            {
                gr.DrawImage(Properties.Resources.Fire, X, Y, IntrinsicTile.Source.Width, IntrinsicTile.Source.Height);
            }

            // Draw bonus if present
            if (BonusHere != null)
            {
                BonusHere.Draw(gr);
            }
        }

        /// <summary>
        /// Loads a sprite for the intrinsic tile.
        /// </summary>
        /// <param name="sprite">Image to load.</param>
        public void LoadSprite(Image sprite)
        {
            IntrinsicTile.LoadSprite(sprite);
        }

        /// <summary>
        /// Unloads the sprite from the intrinsic tile.
        /// </summary>
        public void UnloadSprite()
        {
            IntrinsicTile.UnloadSprite();
        }
    }
}