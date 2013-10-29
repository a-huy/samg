using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperAwesomeMagnetGame
{
    class PlayerSprite : Sprite
    {
        Point Facing = new Point();
        int chargeChange;
        bool isFacingRight = true, isMoving = false;
        bool directionChangeIndex = true;
        bool chargeChangeIndex = false;
        bool isPositive = false;
        bool charge_button_pressed = false;

        public PlayerSprite(Texture2D image, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, int pointValue)
            : base(image, position, frameSize, collisionOffset, currentFrame, sheetSize,
            speed, pointValue) { }

        public PlayerSprite(Texture2D image, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame,
            Point sheetSize, Vector2 speed, int pointValue, int millisecondsPerFrame)
            : base(image, position, frameSize, collisionOffset, currentFrame, sheetSize,
            speed, pointValue, millisecondsPerFrame) { }

        public bool Shoot { get; set; }

        public bool Release { get; set; }

        public bool IsPositive
        {
            get { return isPositive; }
            set { isPositive = value; }
        }

        public bool IsFacingRight
        {
            get { return isFacingRight; }
            set { isFacingRight = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
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
                Vector2 inputDirection = Vector2.Zero;
                KeyboardState States = Keyboard.GetState();
                if (States.IsKeyDown(Keys.Left))
                {
                    inputDirection.X -= 1;
                    isMoving = true;
                    IsFacingRight = false;
                }
                if (States.IsKeyDown(Keys.Right))
                {
                    inputDirection.X += 1;
                    isMoving = true;
                    IsFacingRight = true;
                }
                if (States.IsKeyDown(Keys.Up))
                {
                    inputDirection.Y -= 1;
                    isMoving = true;
                }
                if (States.IsKeyDown(Keys.Down))
                {
                    inputDirection.Y += 1;
                    isMoving = true;
                }

                GamePadState padState = GamePad.GetState(PlayerIndex.One);
                if (padState.ThumbSticks.Left.X != 0) inputDirection.X += padState.ThumbSticks.Left.X;
                if (padState.ThumbSticks.Left.Y != 0) inputDirection.Y += padState.ThumbSticks.Left.Y;

                return inputDirection * speed;
            }
        }

        public Point UpdateFacing()
        {
            if (IsFacingRight)
            {
                Facing.X = 0 + chargeChange;
                Facing.Y = 2 + chargeChange;
            }
            if (!IsFacingRight)
            {
                Facing.X = 2 + chargeChange;
                Facing.Y = 4 + chargeChange;
            }
            return Facing;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            KeyboardState States = Keyboard.GetState();

            if (States.IsKeyDown(Keys.Space)) Shoot = true;
            if (States.IsKeyUp(Keys.Space) && Shoot) Release = true;

            if (States.IsKeyDown(Keys.LeftShift) || States.IsKeyDown(Keys.RightShift)) charge_button_pressed = true;
            if ((States.IsKeyUp(Keys.LeftShift) && States.IsKeyUp(Keys.RightShift)) && charge_button_pressed)
            {
                charge_button_pressed = false;
                if (IsPositive)
                {
                    IsPositive = false;
                    chargeChange = 0;
                }
                else if (!IsPositive)
                {
                    IsPositive = true;
                    chargeChange = 4;
                }
            }

            position += Direction;
            UpdateFacing();

            if ((directionChangeIndex && !IsFacingRight) || (!directionChangeIndex && IsFacingRight))
            {
                directionChangeIndex = IsFacingRight;
                currentFrame.X = 0;
                currentFrame.Y = Facing.X;
            }

            if ((chargeChangeIndex && !IsPositive) || (!chargeChangeIndex && IsPositive))
            {
                chargeChangeIndex = IsPositive;
                currentFrame.X = 0;
                currentFrame.Y = Facing.X;
            }

            if (position.X < 0) position.X = 0;
            if (position.Y < 0) position.Y = 0;
            if (position.X > 1024 - frameSize.X) position.X = 1024 - frameSize.X;
            if (position.Y > 768 - frameSize.Y) position.Y = 768 - frameSize.Y;

            if (isMoving)
            {
                isMoving = false;
                ++timeSinceLastFrame;
                if (timeSinceLastFrame == 2)
                {
                    timeSinceLastFrame = 0;
                    
                    ++currentFrame.X;
                    if (currentFrame.X >= sheetSize.X)
                    {
                        currentFrame.X = 0;
                        ++currentFrame.Y;
                        if (currentFrame.Y >= Facing.Y)
                            currentFrame.Y = Facing.X;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, position, new Rectangle(currentFrame.X * frameSize.X,
                currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y), Color.White, 0f,
                Vector2.Zero, 1f, SpriteEffects.None, 0);

            base.Draw(gameTime, spriteBatch);
        }
    }
}
