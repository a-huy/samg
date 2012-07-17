using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperAwesomeMagnetGame
{
    class Background : Sprite
    {
        public Background(Texture2D image, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, int pointValue)
            : base(image, position, frameSize, collisionOffset, currentFrame, sheetSize,
            speed, pointValue) { }

        public Background(Texture2D image, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, int pointValue, int millisecondsPerFrame)
            : base(image, position, frameSize, collisionOffset, currentFrame, sheetSize,
            speed, pointValue, millisecondsPerFrame) { }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public Point FrameSize
        {
            get
            {
                return frameSize;
            }
        }

        public override Vector2 Direction
        {
            get
            {
                return speed;
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += speed;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position, new Rectangle(currentFrame.X * frameSize.X,
                currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0,
                new Vector2(25, 25), 1f, SpriteEffects.None, 0.1f);
        }
    }
}
