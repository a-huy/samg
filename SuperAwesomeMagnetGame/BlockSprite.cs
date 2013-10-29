using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperAwesomeMagnetGame
{
    class BlockSprite : Sprite
    {
        public BlockSprite(Texture2D image, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, int pointValue)
            : base(image, position, frameSize, collisionOffset, currentFrame, sheetSize,
            speed, pointValue) { }

        public BlockSprite(Texture2D image, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, int pointValue, int millisecondsPerFrame)
            : base(image, position, frameSize, collisionOffset, currentFrame, sheetSize,
            speed, pointValue, millisecondsPerFrame) { }

        public override Vector2 Direction
        {
            get { return speed; }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            position += Direction;

            if (position.X < 0) speed.X = -speed.X;
            if (position.Y < 0) speed.Y = -speed.Y;
            if (position.X > 1024 - frameSize.X) speed.X = -speed.X;
            if (position.Y > 768 - frameSize.Y) speed.Y = -speed.Y;
        }
    }
}
