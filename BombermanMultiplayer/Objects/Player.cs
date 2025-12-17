using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Media;
using System.Diagnostics;
using System.Collections;
using BombermanMultiplayer.Objects;
using BombermanMultiplayer.Strategy.Interface.BombermanMultiplayer.Objects;
using BombermanMultiplayer.Strategy;
using BombermanMultiplayer.Flyweight;

namespace BombermanMultiplayer
{
    [Serializable]
    public class Player : GameObject
    {
        public byte PlayerNumero;
        public string Name = "Player";
        private byte _Vitesse = 5;
        private bool _Dead = false;
        private byte _BombNumb = 2;
        private byte _Lifes = 1;

        //Player can have 2 bonus at the same time
        public BonusType[] BonusSlot = new BonusType[2];
        public short[] BonusTimer = new short[2];

        public MovementDirection Orientation  = MovementDirection.NONE;
        public MovementDirection LastOrientation = MovementDirection.UP;
        public IBonusEffectStrategy[] ActiveStrategies = new IBonusEffectStrategy[2];

        [NonSerialized]
        public ExplosiveFactory ExplosiveFactory;



        public int Wait = 500;

        public enum MovementDirection
        {
            UP,
            DOWN,
            LEFT,
            RIGHT,
            NONE
        }



        #region Accessors



        public byte Lifes
        {
            get { return _Lifes; }
            set { 
                    _Lifes = value; }
        }


        public byte BombNumb
        {
            get { return _BombNumb; }
            set { _BombNumb = value; }
        }

        public byte Vitesse
        {
            get { return _Vitesse; }
            set
            {
                if (value > 0)
                    _Vitesse = value;
                else _Vitesse = 2;
            }

        }

        




        public bool Dead
        {
            get { return _Dead; }
            set
            {

                _Dead = value;

            }

        }

        #endregion




        public Player(byte lifes, int totalFrames, int frameWidth, int frameHeight, 
                     int caseligne, int casecolonne, int TileWidth, int TileHeight, 
                     int frameTime, byte playerNumero)
            : base(casecolonne * TileWidth, caseligne * TileHeight, totalFrames, frameWidth, frameHeight, frameTime)
        {
            CasePosition = new int[2] { caseligne, casecolonne };
            Lifes = lifes;
            Wait = 0;
            PlayerNumero = playerNumero;
            ExplosiveFactory = new ClassicExplosiveFactory();

        }

        #region Deplacements


        //Check the player's location
        public void LocationCheck(int tileWidth, int tileHeight)
        {
            //Player is considerate to be on a case when at least half of his sprite is on it
            //Hauteur
            this.CasePosition[0] = (this.Source.Y + this.Source.Height / 2) / tileHeight; //Ligne
            this.CasePosition[1] = (this.Source.X + this.Source.Width / 2) / tileWidth; //Colonne


        }

        public void Move()
        {
            switch (this.Orientation)
            {
                case MovementDirection.UP:
                    LastOrientation = MovementDirection.UP;
                    DeplHaut();
                    break;
                case MovementDirection.DOWN:
                    LastOrientation = MovementDirection.DOWN;
                    DeplBas();
                    break;
                case MovementDirection.LEFT:
                    LastOrientation = MovementDirection.LEFT;
                    DeplGauche();
                    break;
                case MovementDirection.RIGHT:
                    LastOrientation = MovementDirection.RIGHT;
                    DeplDroite();
                    break;
                default:
                    this.frameindex = 0;
                    break;
            }

        }


        public void DeplHaut()
        {
                base.Bouger(0, -Vitesse);
        }

        public void DeplBas()
        {
                base.Bouger(0, Vitesse);
        }

        public void DeplGauche()
        {
                base.Bouger(-Vitesse, 0);
        }

        public void DeplDroite()
        {
                base.Bouger(Vitesse, 0);
        }

        public void NO()
        {
            base.Bouger(-Vitesse / 2, 0);
            base.Bouger(0, Vitesse / 2);
        }
        public void NE()
        {
            
            base.Bouger(Vitesse / 2, 0);
            base.Bouger(0, Vitesse / 2);
        }
        public void SO()
        {

            base.Bouger(-Vitesse / 2, 0);
            base.Bouger(0, -Vitesse / 2);
        }
        public void SE()
        {

            base.Bouger(Vitesse / 2, 0);
            base.Bouger(0, -Vitesse / 2);
        }




        #endregion

        #region Actions

        /// <summary>
        /// Upgrade player's arsenal to advanced weapons
        /// </summary>
        public void UpgradeArsenal()
        {
            ExplosiveFactory = new AdvancedExplosiveFactory();
        }

        /// <summary>
        /// Downgrade player's arsenal to classic weapons
        /// </summary>
        public void DowngradeArsenal()
        {
            ExplosiveFactory = new ClassicExplosiveFactory();
        }
        public void DropBomb(TileContext[,] MapGrid, List<Bomb> BombsOnTheMap, Player otherplayer)
        {
            if (this.BombNumb > 0 && !MapGrid[this.CasePosition[0], this.CasePosition[1]].Occupied)
            {
                // Use factory to create appropriate bomb type
                Bomb newBomb = ExplosiveFactory.CreateBomb(
                    this.CasePosition[0], 
                    this.CasePosition[1],
                    48, 48, 
                    this.PlayerNumero
                );
                
                BombsOnTheMap.Add(newBomb);
                MapGrid[this.CasePosition[0], this.CasePosition[1]].bomb = newBomb;
                MapGrid[this.CasePosition[0], this.CasePosition[1]].Occupied = true;
                this.BombNumb--;
            }
        }

        public void DropMine(TileContext[,] MapGrid, List<Mine> MinesOnTheMap, Player otherPlayer)
        {
            if (this.Dead) return;
            
            if (MinesOnTheMap != null && MinesOnTheMap.Count >= 2) return;
            if (!MapGrid[this.CasePosition[0], this.CasePosition[1]].Occupied)
            {
                Mine newMine = ExplosiveFactory.CreateMine(
                    this.CasePosition[0], this.CasePosition[1], 48, 48, this.PlayerNumero);
                MinesOnTheMap.Add(newMine);
                MapGrid[this.CasePosition[0], this.CasePosition[1]].Occupied = true;
            }
        }

        public void DropGrenade(TileContext[,] MapGrid, List<Grenade> GrenadesOnTheMap, Player otherPlayer)
        {
            if (this.Dead) return;
            if (GrenadesOnTheMap != null && GrenadesOnTheMap.Count >= 2) return;
            if (!MapGrid[this.CasePosition[0], this.CasePosition[1]].Occupied)
            {
                Grenade newGrenade = ExplosiveFactory.CreateGrenade(
                    this.CasePosition[0], this.CasePosition[1], 48, 48, this.PlayerNumero);
                GrenadesOnTheMap.Add(newGrenade);
                MapGrid[this.CasePosition[0], this.CasePosition[1]].Occupied = true;
            }
        }

        public void DrawPosition(Graphics g)
        {

            g.DrawString(CasePosition[0].ToString() + ":" + CasePosition[1].ToString(), new Font("Arial", 16), new SolidBrush(Color.Pink), this.Source.X, this.Source.Y);

        }
        public new void Draw(Graphics gr)
        {
            if (this.Sprite != null)
            {
                if (this.Dead)
                {
                    gr.DrawImage(this.Sprite, Source,0 , 0, Source.Width, Source.Height, GraphicsUnit.Pixel);
                    gr.DrawString("DEAD", new Font("Arial", 16), new SolidBrush(Color.Red), this.Source.X + Source.Width / 2, this.Source.Y - Source.Height / 2);
                    return;
                }

                for (int i = 0; i < 2; i++)
                {
                    switch (this.BonusSlot[i])
                    {
                        case BonusType.PowerBomb:
                            gr.DrawImage(Properties.Resources.SuperBomb, this.Source);
                            break;
                        case BonusType.SpeedBoost:
                            gr.DrawLine(new Pen(Color.Yellow, 6), this.Source.X, this.Source.Y + this.Source.Height, this.Source.X + this.Source.Width, this.Source.Y + this.Source.Height);
                            break;
                        case BonusType.Desamorce:
                            break;
                        case BonusType.Armor:
                            gr.DrawEllipse(new Pen(Color.Blue, 5), this.Source);
                            break;
                        case BonusType.None:
                            break;
                        default:
                            break;
                    }

                    gr.DrawImage(this.Sprite, Source, frameindex * Source.Width, 0, Source.Width, Source.Height, GraphicsUnit.Pixel);
                    gr.DrawRectangle(Pens.Red, this.Source);
                    gr.DrawString(this.Name, new Font(new Font("Arial", 10), FontStyle.Bold), Brushes.MediumVioletRed, this.Source.X, this.Source.Y - this.Source.Height/2);


                }

            }
        }


        public void Respawn(Player p, TileContext[,] MapGrid, int TileWidth, int TileHeight)
        {
            if (this.Wait > 2)
            {
                if (this.Lifes > 0)
                {
                    if ((1 - this.CasePosition[0]) < p.CasePosition[0] - this.CasePosition[0] &&
                       ((1 - this.CasePosition[1]) < p.CasePosition[1] - this.CasePosition[0]))
                    {
                        this.CasePosition[0] = 1;
                        this.CasePosition[1] = 1;


                    }
                    else
                    {
                        this.CasePosition[0] = ((MapGrid.GetLength(0) - 1) - 1);
                        this.CasePosition[1] = ((MapGrid.GetLength(0) - 1) - 1);

                    }
                    this._Source.X = this.CasePosition[0] * TileWidth;
                    this._Source.Y = this.CasePosition[1] * TileHeight;

                    this.Dead = false;
                }
            }
        }

        public void Deactivate(TileContext[,] MapGrid, List<Bomb> bombsOnTheMap,  Player otherPlayer)
        {
            Bomb toDesamorce = null;

            //Check if player has the bonus
            if (!(this.ActiveStrategies[0] is DefuseBombEffectStrategy) &&
                !(this.ActiveStrategies[1] is DefuseBombEffectStrategy))
            {
                return;
            }

            if (MapGrid[this.CasePosition[0], this.CasePosition[1]].bomb != null)
            {
                toDesamorce = MapGrid[this.CasePosition[0], this.CasePosition[1]].bomb;

                if (toDesamorce.Proprietary == this.PlayerNumero)
                {
                    this.BombNumb++;
                }
                else
                {
                    otherPlayer.BombNumb++;
                }

                bombsOnTheMap.Remove(toDesamorce);
                toDesamorce.Dispose();
                toDesamorce = null;

                MapGrid[this.CasePosition[0], this.CasePosition[1]].bomb = null;
                MapGrid[this.CasePosition[0], this.CasePosition[1]].Occupied = false;
            }
            else
            {
                for (int i = -1; i < 2; i += 2)
                {
                    if (MapGrid[this.CasePosition[0] + i, this.CasePosition[1]].bomb != null)
                    {
                        toDesamorce = MapGrid[this.CasePosition[0] + i, this.CasePosition[1]].bomb;

                        if (toDesamorce.Proprietary == this.PlayerNumero)
                        {
                            this.BombNumb++;
                        }
                        else
                        {
                            otherPlayer.BombNumb++;
                        }

                        bombsOnTheMap.Remove(toDesamorce);
                        toDesamorce.Dispose();
                        toDesamorce = null;

                        MapGrid[this.CasePosition[0] + i, this.CasePosition[1]].bomb = null;
                        MapGrid[this.CasePosition[0] + i, this.CasePosition[1]].Occupied = false;


                    }
                    if (MapGrid[this.CasePosition[0], this.CasePosition[1] + i].bomb != null)
                    {
                        toDesamorce = MapGrid[this.CasePosition[0], this.CasePosition[1] + i].bomb;

                        if (toDesamorce.Proprietary == this.PlayerNumero)
                        {
                            this.BombNumb++;
                        }
                        else
                        {
                            otherPlayer.BombNumb++;
                        }

                        bombsOnTheMap.Remove(toDesamorce);
                        toDesamorce.Dispose();
                       
                        toDesamorce = null;

                        MapGrid[this.CasePosition[0], this.CasePosition[1] + i].bomb = null;

                        MapGrid[this.CasePosition[0], this.CasePosition[1] + i].Occupied = false;
                    }
                }


            }




        }

        #endregion

        #region Prototype Pattern Implementation

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>A new <see cref="Player"/> object that is a copy of the current instance.</returns>
        public new object Clone()
        {
            Player cloned = (Player)base.Clone();
            return cloned;
        }

        /// <summary>
        /// Creates a deep copy of the current <see cref="Player"/> instance, including all nested objects and arrays.
        /// </summary>
        /// <returns>A new <see cref="Player"/> instance that is a deep copy of the current instance.</returns>
        public new object DeepClone()
        {
            Player cloned = (Player)base.DeepClone();

            if (this.BonusSlot != null)
            {
                cloned.BonusSlot = (BonusType[])this.BonusSlot.Clone();
            }

            if (this.BonusTimer != null)
            {
                cloned.BonusTimer = (short[])this.BonusTimer.Clone();
            }

            if (this.ActiveStrategies != null)
            {
                cloned.ActiveStrategies = new IBonusEffectStrategy[this.ActiveStrategies.Length];
                for (int i = 0; i < this.ActiveStrategies.Length; i++)
                {
                    cloned.ActiveStrategies[i] = CreateStrategyClone(this.ActiveStrategies[i]);
                }
            }

            if (this.ExplosiveFactory != null)
            {
                if (this.ExplosiveFactory is AdvancedExplosiveFactory)
                {
                    cloned.ExplosiveFactory = new AdvancedExplosiveFactory();
                }
                else
                {
                    cloned.ExplosiveFactory = new ClassicExplosiveFactory();
                }
            }

            return cloned;
        }

        /// <summary>
        /// Creates a new instance of the same type as the specified bonus effect strategy.
        /// </summary>
        /// <remarks>Supported strategy types include: <list type="bullet"> <item><see
        /// cref="PowerBombEffectStrategy"/></item> <item><see cref="SpeedBoostEffectStrategy"/></item> <item><see
        /// cref="DefuseBombEffectStrategy"/></item> <item><see cref="ArmorEffectStrategy"/></item> </list> If the
        /// specified strategy is not one of the supported types, the method returns <see langword="null"/>.</remarks>
        /// <param name="strategy">The bonus effect strategy to clone. Must be one of the supported strategy types.</param>
        /// <returns>A new instance of the same type as <paramref name="strategy"/>, or <see langword="null"/> if the strategy
        /// type is not supported.</returns>
        private IBonusEffectStrategy CreateStrategyClone(IBonusEffectStrategy strategy)
        {
            if (strategy is PowerBombEffectStrategy)
                return new PowerBombEffectStrategy();
            else if (strategy is SpeedBoostEffectStrategy)
                return new SpeedBoostEffectStrategy();
            else if (strategy is DefuseBombEffectStrategy)
                return new DefuseBombEffectStrategy();
            else if (strategy is ArmorEffectStrategy)
                return new ArmorEffectStrategy();

            return null;
        }

        #endregion
    }
}
