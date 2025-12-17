using BombermanMultiplayer.Decoratorrr;
using BombermanMultiplayer.Objects;
using System;
using System.Drawing;
using BombermanMultiplayer.Factory;

namespace BombermanMultiplayer
{
    [Serializable]
    public class Tile : GameObject
    {
        public bool Walkable = false;
        public bool Destroyable = false;

        [NonSerialized]
        public Bonus BonusHere = null;

        public Tile(int x, int y, int totalFrame, int frameWidth, int frameHeigt,  bool walkable, bool destroyable)
            : base(x, y, totalFrame, frameWidth, frameHeigt)
        {
            Walkable = walkable;
            Destroyable = destroyable;
        }

        public void SpawnBonus()
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            int num = r.Next(0, 4);
            Bonus baseBonus = null;

            // Use factories instead of direct instantiation
            if (num == 0)
            {
                baseBonus = new PowerBonusFactory().CreateBonus(Source.X, Source.Y, 1, Source.Width, Source.Height);
            }
            else if (num == 1)
            {
                baseBonus = new SpeedBonusFactory().CreateBonus(Source.X, Source.Y, 1, Source.Width, Source.Height);
            }
            else if (num == 2)
            {
                baseBonus = new DefuseBonusFactory().CreateBonus(Source.X, Source.Y, 1, Source.Width, Source.Height);
            }
            else if (num == 3)
            {
                baseBonus = new HealthBonusFactory().CreateBonus(Source.X, Source.Y, 1, Source.Width, Source.Height);
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

                BonusHere.CheckCasePosition(Source.Width, Source.Height);
                System.Diagnostics.Debug.WriteLine($"Spawned bonus: {BonusHere.GetDescription()}");
            }
        }

        public new void Draw(Graphics gr)
        {
            if (Sprite != null)
            {
                gr.DrawImage(Sprite, Source, frameindex * Source.Width, 0, Source.Width, Source.Height, GraphicsUnit.Pixel);
                gr.DrawRectangle(Pens.Red, Source);
            }
            if (BonusHere != null)
            {
                BonusHere.Draw(gr);
            }
        }

        /// <summary>
        /// Draws the tile at a specific location without modifying the internal Source rectangle.
        /// Useful for Flyweight pattern where multiple contexts share the same Tile object.
        /// </summary>
        /// <param name="gr">Graphics object to draw on.</param>
        /// <param name="x">X coordinate to draw at.</param>
        /// <param name="y">Y coordinate to draw at.</param>
        public void DrawAt(Graphics gr, int x, int y)
        {
            if (Sprite != null)
            {
                Rectangle destRect = new Rectangle(x, y, Source.Width, Source.Height);
                gr.DrawImage(Sprite, destRect, frameindex * Source.Width, 0, Source.Width, Source.Height, GraphicsUnit.Pixel);
                // Optionally draw debug rectangle if needed, but usually not for all tiles
                // gr.DrawRectangle(Pens.Red, destRect);
            }
        }
    }
}
