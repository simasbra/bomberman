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

namespace BombermanMultiplayer
{
    [Serializable]
    public abstract class GameObject
    {
        //Rectangle allowing to 'materialize' the sprite
        protected Rectangle _Source;

        [NonSerialized]
        protected Image Sprite;

        //Current frame of the object
        protected int _frameindex;
        //Duration since displaying a frame

        //Duration of a frame
        protected float _frameTime = 125;


        protected int _totalElapsedTime = 0;

        //Number of frames in the animation
        protected int _totalFrames;

        //Position of the object at tile level
        protected int[] _CasePosition;

        #region Accessors


        public int[] CasePosition
         {
             get { return _CasePosition; }
             set
             {
                if (!(value[0] < 0 || value[1] < 0))
                {
                    _CasePosition = value;

                }
            }

         }
        public Rectangle Source
        {
            get { return _Source; }
            set { _Source = value; }
        }
        public float frameSpeed
        {
            get { return this._frameTime; }
            set
            {
                if (value <= 0) this._frameTime = 1; //No negative time
                else this._frameTime = value;
            }
        }

        public int totalFrames
        {
            get { return _totalFrames; }
        }
        public int frameindex
        {
            get { return _frameindex; }
            set { _frameindex = value; }
        }


        #endregion

        //Constructor
        public GameObject()
        { }

        public GameObject(int x, int y, int totalFrames, int frameWidth, int frameHeight)
            
        {
            _totalFrames = totalFrames;
            CasePosition = new int[2] { 0, 0 };
            _Source = new Rectangle(x, y, frameWidth, frameHeight);
            
        }

        public GameObject(int x, int y, int totalFrames, int frameWidth, int frameHeight, int frameTime)

        {
            _totalFrames = totalFrames;
            CasePosition = new int[2] { 0, 0 };
            _Source = new Rectangle(x, y, frameWidth, frameHeight);
            _frameTime = frameTime;
            

        }

        public void ChangeLocation(int x, int y)
        {
            this._Source.X = x;
            this._Source.Y = y;
        }

        
        public void LoadSprite(Image sprite)
        {

            this.Sprite = sprite;

        }
        public void UnloadSprite()
        {

            this.Sprite = null;

        }


        
        public void Draw(Graphics gr)
        {
            if (this.Sprite != null)
            {
                gr.DrawImage(this.Sprite, Source, frameindex * Source.Width, 0, Source.Width, Source.Height, GraphicsUnit.Pixel);
                gr.DrawRectangle(Pens.Red, this.Source);
            }
        }




        public void UpdateFrame(int elsapedTime)
        {

            _totalElapsedTime += elsapedTime;

            if (_totalElapsedTime > this.frameSpeed)
            {
                frameindex += 1;

                _totalElapsedTime = 0;

                if (frameindex > _totalFrames)
                {
                    frameindex = 0; // return to first sprite once series is finished

                }
            }


        }



        public void Move(int moveX, int moveY) // Just adds the movement amount to the object's position.
        {
            _Source.X += moveX;
            _Source.Y += moveY;
            
        }

        


    }
}
