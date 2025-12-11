using BombermanMultiplayer.Decoratorrr;
using BombermanMultiplayer.Objects;
using BombermanMultiplayer.Visitor;
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
    public class Tile : GameObject, IVisitable
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

        public Tile(int x_, int y_, int totalFrame_, int frameWidth_, int frameHeigt_,  bool walkable, bool destroyable)
            : base(x_, y_, totalFrame_, frameWidth_, frameHeigt_)
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

        //public void SpawnBonus()
        //{
        //    Random r = new Random((int)DateTime.Now.Ticks);
        //    int num = r.Next(0, 4);
        //    Bonus baseBonus = null;

        //    // Sukuriame bazinį bonusą
        //    if (num == 0)
        //    {
        //        baseBonus = new Bonus(this.Source.X, this.Source.Y, 1, this.Source.Width, this.Source.Height, BonusType.PowerBomb);
        //        baseBonus.LoadSprite(Properties.Resources.SuperBomb);
        //    }
        //    else if (num == 1)
        //    {
        //        baseBonus = new Bonus(this.Source.X, this.Source.Y, 1, this.Source.Width, this.Source.Height, BonusType.SpeedBoost);
        //        baseBonus.LoadSprite(Properties.Resources.SpeedUp);
        //    }
        //    else if (num == 2)
        //    {
        //        baseBonus = new Bonus(this.Source.X, this.Source.Y, 1, this.Source.Width, this.Source.Height, BonusType.Desamorce);
        //        baseBonus.LoadSprite(Properties.Resources.Deactivate);
        //    }
        //    else if (num == 3)
        //    {
        //        baseBonus = new Bonus(this.Source.X, this.Source.Y, 1, this.Source.Width, this.Source.Height, BonusType.Armor);
        //        baseBonus.LoadSprite(Properties.Resources.Armor);
        //    }

        //    if (baseBonus != null)
        //    {
        //        // TESTAVIMUI: Užkomentuok šią sekciją ir atkomentuok žemiau esančią "TESTING MODE" sekciją
        //        // kad visada gautum visus 3 dekoratorius

        //        // === PRODUCTION MODE: Atsitiktiniai dekoratoriai ===
        //        //int decoratorChance = r.Next(0, 100);

        //        //if (decoratorChance < 50)
        //        //{
        //        //    // 50% - paprastas bonus be dekoratorių
        //        //    this.BonusHere = baseBonus;
        //        //}
        //        //else if (decoratorChance < 80)
        //        //{
        //        //    // 30% - bonus su 1 dekoratoriumi
        //        //    this.BonusHere = new MultiplierDecorator(baseBonus, 1.5);
        //        //}
        //        //else if (decoratorChance < 95)
        //        //{
        //        //    // 15% - bonus su 2 dekoratoriais
        //        //    Bonus decorated = new MultiplierDecorator(baseBonus, 2.0);
        //        //    this.BonusHere = new DurationDecorator(decorated, 3000);
        //        //}
        //        //else
        //        //{
        //        //    // 5% - bonus su 3 dekoratoriais (super retas!)
        //        //    Bonus decorated = new MultiplierDecorator(baseBonus, 2.5);
        //        //    decorated = new DurationDecorator(decorated, 5000);
        //        //    this.BonusHere = new GlowEffectDecorator(decorated, "Golden", 90);
        //        //}

        //        // === TESTING MODE: Visada 3 dekoratoriai ===
        //        // Užkomentuok viršuje esančią "PRODUCTION MODE" sekciją
        //        // ir atkomentuok šią sekciją testavimui

        //        Bonus decorated = new MultiplierDecorator(baseBonus, 2.5);
        //        decorated = new DurationDecorator(decorated, 5000);
        //        this.BonusHere = new GlowEffectDecorator(decorated, "Golden", 90);

        //        System.Diagnostics.Debug.WriteLine($"TESTING: Created fully decorated bonus: {this.BonusHere.GetDescription()}");

        //        if (num == 0)
        //            this.BonusHere.LoadSprite(Properties.Resources.SuperBomb);
        //        else if (num == 1)
        //            this.BonusHere.LoadSprite(Properties.Resources.SpeedUp);
        //        else if (num == 2)
        //            this.BonusHere.LoadSprite(Properties.Resources.Deactivate);
        //        else if (num == 3)
        //            this.BonusHere.LoadSprite(Properties.Resources.Armor);

        //        this.BonusHere.CheckCasePosition(this.Source.Width, this.Source.Height);

        //        // Debug output (gali palikti arba ištrinti)
        //        System.Diagnostics.Debug.WriteLine($"Spawned bonus: {this.BonusHere.GetDescription()}");
        //    }
        //}

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

        #region IVisitable Implementation (Visitor Pattern)

        /// <summary>
        /// Accepts a visitor (Visitor pattern)
        /// </summary>
        /// <param name="visitor">The visitor to accept</param>
        public void Accept(IGameObjectVisitor visitor)
        {
            visitor?.VisitTile(this);
        }

        #endregion
    }
}
