using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperAwesomeMagnetGame
{
    abstract class Sprite
    {
        const int defaultMillisecondsPerFrame = 16;
        public enum Charge { positive, negative, neutral };
        public enum PowerType { life, bomb, slow, cross, noblock, addblock };

        protected Texture2D image;
        protected Vector2 position;
        protected Vector2 speed;
        protected Point sheetSize;
        protected Point frameSize;
        protected Point currentFrame;
        protected int timeSinceLastFrame = 0;
        protected int millisecondsPerFrame;
        protected int pointValue;
        protected int countdown;
        protected PowerType powerType;
        protected float scale = 1;
        protected Charge charge;
        int collisionOffset;

        public Sprite(Texture2D image, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed, int pointValue)
            : this(image, position, frameSize, collisionOffset,
            currentFrame, sheetSize, speed, pointValue, defaultMillisecondsPerFrame) { }

        public Sprite(Texture2D image, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int pointValue, int millisecondsPerFrame)
        {
            this.image = image;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.pointValue = pointValue;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        public Sprite(Texture2D image, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 speed, int pointValue, float scale)
            : this(image, position, frameSize, collisionOffset, currentFrame, sheetSize, speed,
            pointValue, defaultMillisecondsPerFrame) { this.scale = scale; }

        public Sprite(Texture2D image, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 speed, int pointValue, Charge charge)
            : this(image, position, frameSize, collisionOffset, currentFrame, sheetSize, speed,
            pointValue, defaultMillisecondsPerFrame) { this.charge = charge; }

        public Sprite(Texture2D image, Vector2 position, Point frameSize, int collisionOffset,
            Point currentFrame, Point sheetSize, Vector2 speed, int pointValue, PowerType powerType, int countdown)
            : this(image, position, frameSize, collisionOffset, currentFrame, sheetSize, speed,
            pointValue, defaultMillisecondsPerFrame) { this.powerType = powerType; this.countdown = countdown; }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;
                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;
                    if (currentFrame.Y >= sheetSize.Y)
                        currentFrame.Y = 0;
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position, new Rectangle(currentFrame.X * frameSize.X,
                currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0,
                Vector2.Zero, scale, SpriteEffects.None, 1f);
        }

        public abstract Vector2 Direction { get; }

        public Rectangle CollisionRect
        {
            get
            {
                return new Rectangle((int)position.X + collisionOffset,
                    (int)position.Y + collisionOffset, frameSize.X - (collisionOffset * 2),
                    frameSize.Y - (collisionOffset * 2));
            }
        }
    }
}
