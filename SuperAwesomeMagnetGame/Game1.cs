using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace SuperAwesomeMagnetGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteManager spriteManager;
        bool tButtonPressed = false;
        bool cButtonPressed = false;
        public enum GameState { StartGame, Tutorial, Credits, InGame, EndGame, WinGame };
        GameState currentGameState = GameState.StartGame;
        public SpriteManager.EndState status;
        Background endBack;
        Background startBack;
        Background tutorialBack;
        Background winBack;
        Background creditsBack;
        SpriteFont endStats;
        Song backgroundMusic;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();

            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);
            spriteManager.Enabled = false;
            spriteManager.Visible = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            endBack = new Background(Content.Load<Texture2D>(@"Images/EndScreen"), new Vector2(25, 25),
                new Point(1024, 768), 0, new Point(0, 0), new Point(1, 1), Vector2.Zero, 0);
            startBack = new Background(Content.Load<Texture2D>(@"Images/StartScreen"), new Vector2(25, 25),
                new Point(1024, 768), 0, new Point(0, 0), new Point(1, 1), Vector2.Zero, 0);
            tutorialBack = new Background(Content.Load<Texture2D>(@"Images/tutorialscreen"), new Vector2(25, 25),
                new Point(1024, 768), 0, new Point(0, 0), new Point(1, 1), Vector2.Zero, 0);
            winBack = new Background(Content.Load<Texture2D>(@"Images/WinScreen"), new Vector2(25, 25),
                new Point(1024, 768), 0, new Point(0, 0), new Point(1, 1), Vector2.Zero, 0);
            creditsBack = new Background(Content.Load<Texture2D>(@"Images/CreditsScreen"), new Vector2(25, 25),
                new Point(1024, 768), 0, new Point(0, 0), new Point(1, 1), Vector2.Zero, 0);

            backgroundMusic = Content.Load<Song>(@"Audio/Background  Music");
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;

            endStats = Content.Load<SpriteFont>(@"fonts/arial");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) this.Exit();

            switch (currentGameState)
            {
                case GameState.StartGame:
                    startBack.Update(gameTime, Window.ClientBounds);
                    if (Keyboard.GetState().IsKeyDown(Keys.N))
                    {
                        currentGameState = GameState.InGame;
                        spriteManager.Enabled = true;
                        spriteManager.Visible = true;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.T)) tButtonPressed = true;
                    if (Keyboard.GetState().IsKeyUp(Keys.T) && tButtonPressed)
                    {
                        tButtonPressed = false;
                        currentGameState = GameState.Tutorial;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.C)) cButtonPressed = true;
                    if (Keyboard.GetState().IsKeyUp(Keys.C) && cButtonPressed)
                    {
                        cButtonPressed = false;
                        currentGameState = GameState.Credits;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Escape)) this.Exit();
                    break;
                case GameState.Tutorial:
                    tutorialBack.Update(gameTime, Window.ClientBounds);
                    if (Keyboard.GetState().GetPressedKeys().Length > 0) tButtonPressed = true;
                    if (Keyboard.GetState().GetPressedKeys().Length == 0 && tButtonPressed)
                    {
                        tButtonPressed = false;
                        currentGameState = GameState.StartGame;
                    }
                    break;
                case GameState.Credits:
                    creditsBack.Update(gameTime, Window.ClientBounds);
                    if (Keyboard.GetState().GetPressedKeys().Length > 0) cButtonPressed = true;
                    if (Keyboard.GetState().GetPressedKeys().Length == 0 && cButtonPressed)
                    {
                        cButtonPressed = false;
                        currentGameState = GameState.StartGame;
                    }
                    break;
                case GameState.InGame:
                    if (spriteManager.currentNumberOfLives == 0)
                    {
                        currentGameState = GameState.EndGame;
                        spriteManager.Enabled = false;
                        spriteManager.Visible = false;
                    }
                    if (spriteManager.currentLevel == 11)
                    {
                        currentGameState = GameState.WinGame;
                        spriteManager.Enabled = false;
                        spriteManager.Visible = false;
                    }
                    break;
                case GameState.EndGame:
                    endBack.Update(gameTime, Window.ClientBounds);
                    if (Keyboard.GetState().IsKeyDown(Keys.M)) tButtonPressed = true;
                    if (Keyboard.GetState().IsKeyUp(Keys.M) && tButtonPressed)
                    {
                        tButtonPressed = false;
                        currentGameState = GameState.StartGame;
                        spriteManager.currentNumberOfLives = 3;
                        spriteManager.currentLevel = 1;
                        spriteManager.restart = true;
                    }
                    break;
                case GameState.WinGame:
                    winBack.Update(gameTime, Window.ClientBounds);
                    if (Keyboard.GetState().IsKeyDown(Keys.M)) tButtonPressed = true;
                    if (Keyboard.GetState().IsKeyUp(Keys.M) && tButtonPressed)
                    {
                        tButtonPressed = false;
                        currentGameState = GameState.StartGame;
                        spriteManager.currentNumberOfLives = 3;
                        spriteManager.currentLevel = 1;
                        spriteManager.restart = true;
                    }
                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            switch (currentGameState)
            {
                case GameState.StartGame:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                    startBack.Draw(gameTime, spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.Tutorial:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                    tutorialBack.Draw(gameTime, spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.Credits:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                    creditsBack.Draw(gameTime, spriteBatch);
                    spriteBatch.End();
                    break;
                case GameState.InGame:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    break;
                case GameState.EndGame:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                    endBack.Draw(gameTime, spriteBatch);
                        spriteBatch.DrawString(endStats,
                        "You DIED with a score of " + spriteManager.currentScore + " at level " + spriteManager.currentLevel,
                        new Vector2(345, 450), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    spriteBatch.DrawString(endStats,
                        "Press M to access the menu or ESC to quit",
                        new Vector2(330, 485), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    spriteBatch.End();
                    break;
                case GameState.WinGame:
                    spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
                    winBack.Draw(gameTime, spriteBatch);
                    spriteBatch.DrawString(endStats,
                        "You managed to beat the game with a score of " + spriteManager.currentScore + " !",
                        new Vector2(285, 450), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    spriteBatch.DrawString(endStats,
                        "Press M to access the menu or ESC to quit",
                        new Vector2(330, 485), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    spriteBatch.End();
                    break;
            }
            base.Draw(gameTime);
        }
    }
}
