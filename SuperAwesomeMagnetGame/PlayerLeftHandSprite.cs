using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperAwesomeMagnetGame
{
    class PlayerLeftHandSprite : PlayerSprite
    {
        Vector2 boundsOffset = new Vector2(120, 55);

        public SpriteEffects HandFlip { get; set; }

        public PlayerLeftHandSprite(Texture2D image, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, int pointValue)
            : base(image, position, frameSize, collisionOffset, currentFrame, sheetSize,
            speed, pointValue) { }

        public PlayerLeftHandSprite(Texture2D image, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, int pointValue, int millisecondsPerFrame)
            : base(image, position, frameSize, collisionOffset, currentFrame, sheetSize,
            speed, pointValue, millisecondsPerFrame) { }

        //public Vector2 Position
        //{
        //    get { return position; }
        //    set { position = value; }
        //}

        public override Vector2 Direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;
                KeyboardState HandInput = Keyboard.GetState();
                if (HandInput.IsKeyDown(Keys.Left))
                {
                    inputDirection.X -= 1;
                }
                if (HandInput.IsKeyDown(Keys.Right))
                {
                    inputDirection.X += 1;
                }
                if (HandInput.IsKeyDown(Keys.Up))
                {
                    inputDirection.Y -= 1;
                }
                if (HandInput.IsKeyDown(Keys.Down))
                {
                    inputDirection.Y += 1;
                }

                GamePadState padState = GamePad.GetState(PlayerIndex.One);
                if (padState.ThumbSticks.Left.X != 0) inputDirection.X += padState.ThumbSticks.Left.X;
                if (padState.ThumbSticks.Left.Y != 0) inputDirection.Y += padState.ThumbSticks.Left.Y;

                return inputDirection * speed;
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            KeyboardState HandStates = Keyboard.GetState();

            position += Direction;

            if (position.X < -30) position.X = -30;
            if (position.Y < boundsOffset.Y) position.Y = boundsOffset.Y;
            if (position.X > 874) position.X = 874;
            if (position.Y > 768 - frameSize.Y) position.Y = 768 - frameSize.Y;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position, new Rectangle(currentFrame.X * frameSize.X,
                currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0,
                new Vector2(25, 25), 1f, HandFlip, 1f);
        }
    }
}
