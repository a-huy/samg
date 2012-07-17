using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperAwesomeMagnetGame
{
    class Magnet : Sprite
    {
        Vector2 origin = new Vector2(20, 20);

        public Magnet(Texture2D image, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, int pointValue)
            : base(image, position, frameSize, collisionOffset, currentFrame, sheetSize,
            speed, pointValue) { }

        public Magnet(Texture2D image, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, int pointValue, int millisecondsPerFrame)
            : base(image, position, frameSize, collisionOffset, currentFrame, sheetSize,
            speed, pointValue, millisecondsPerFrame) { }

        public Magnet(Texture2D image, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, int pointValue, Charge charge)
            : base(image, position, frameSize, collisionOffset, currentFrame, sheetSize,
            speed, pointValue, charge) { }

        public Charge Polarity
        {
            get { return charge; }
        }

        public Vector2 Position
        {
            get { return position; }
        }

        public Vector2 Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public override Vector2 Direction
        {
            get
            {
                KeyboardState MagnetInput = Keyboard.GetState();
                if (MagnetInput.IsKeyDown(Keys.W)) speed.Y--;
                if (MagnetInput.IsKeyDown(Keys.S)) speed.Y++;
                return speed; 
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += Direction;

            base.Update(gameTime, clientBounds);
        }
    }
}
