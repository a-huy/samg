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
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        SpriteBatch spriteBatch;
        PlayerSprite player;
        PlayerRightHandSprite playerRightHand;
        PlayerLeftHandSprite playerLeftHand;
        Background back1;
        Background back2;
        Background back3;
        List<EnemySprite> spriteList = new List<EnemySprite>();
        List<EnemySprite> livesList = new List<EnemySprite>();
        List<Magnet> magnetList = new List<Magnet>();
        List<Background> backgroundList = new List<Background>();
        List<BlockSprite> blockList = new List<BlockSprite>();
        List<PowerupSprite> powerupList = new List<PowerupSprite>();
        SpriteFont score;
        SpriteFont level;
        Random random = new Random();
        SoundEffect Hit;
        SoundEffectInstance HitInstance;
        SoundEffect Kill;
        SoundEffectInstance KillInstance;
        SoundEffect Shoot;
        SoundEffectInstance ShootInstance;
        SoundEffect Reflect;
        SoundEffectInstance ReflectInstance;
        SoundEffect Clear;
        SoundEffectInstance ClearInstance;
        SoundEffect Cross;
        SoundEffectInstance CrossInstance;
        SoundEffect PowerupSpawn;
        SoundEffectInstance PowerupSpawnInstance;
        SoundEffect NoBlock;
        SoundEffectInstance NoBlockSoundInstance;
        SoundEffect LifeUp;
        SoundEffectInstance LifeUpInstance;
        SoundEffect SlowDown;
        SoundEffectInstance SlowDownInstance;
        SoundEffect AddBlock;
        SoundEffectInstance AddBlockInstance;

        public int currentNumberOfLives = 3;
        public int currentScore;
        public int currentLevel = 1;
        public int elapsedTime = 0;
        public int occurTime = 240;
        public bool isEffected = false;
        public bool restart;
        public bool[] shortcut = new bool[10];
        public enum EndState { win, lose };

        public void AddScore(int score)
        {
            currentScore += score;
        }

        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            player = new PlayerSprite(Game.Content.Load<Texture2D>(@"Images/MagnaBoyWalkingSheet"),
                new Vector2((Game.Window.ClientBounds.Width / 2) - 60,(Game.Window.ClientBounds.Height / 2) - 52),
                new Point(120, 105), 10, new Point(0, 0), new Point(8, 2), new Vector2(10, 10), 0);

            playerRightHand = new PlayerRightHandSprite(Game.Content.Load<Texture2D>(@"Images/MagnaBoyHand"),
                (player.Position + new Vector2(150, 50)), new Point(50, 50), 0, new Point(0, 0),
                new Point(1, 1), new Vector2(10, 10), 0);

            playerLeftHand = new PlayerLeftHandSprite(Game.Content.Load<Texture2D>(@"Images/MagnaBoyHand"),
                (player.Position + new Vector2(-30, 50)), new Point(50, 50), 0, new Point(0, 0),
                new Point(1, 1), new Vector2(10, 10), 0);

            back1 = new Background(Game.Content.Load<Texture2D>(@"Images/Background"), new Vector2(25, 25),
                new Point(1024, 768), 0, new Point(0, 0), new Point(1, 1), new Vector2(-5, 0), 0);

            back2 = new Background(Game.Content.Load<Texture2D>(@"Images/Background"),
                new Vector2(back1.Position.X + back1.FrameSize.X, 25), new Point(1024, 768), 0,
                new Point(0, 0), new Point(1, 1), new Vector2(-5, 0), 0);

            back3 = new Background(Game.Content.Load<Texture2D>(@"Images/Background"),
                new Vector2(back2.Position.X + back2.FrameSize.X, 25), new Point(1024, 768), 0,
                new Point(0, 0), new Point(1, 1), new Vector2(-5, 0), 0);

            score = Game.Content.Load<SpriteFont>(@"fonts/arial");
            level = Game.Content.Load<SpriteFont>(@"fonts/arial");

            for (int i = 0; i < currentNumberOfLives; i++)
            {
                int space = 10 + i * 40;
                livesList.Add(new EnemySprite(Game.Content.Load<Texture2D>(@"Images/MagnaBoyWalkingSheet"),
                    new Vector2(space, 35), new Point(120, 105), 10, new Point(0, 0), new Point(1, 1),
                    Vector2.Zero, 0, .25f));
            }

            backgroundList.Add(back1);
            backgroundList.Add(back2);
            backgroundList.Add(back3);

            Hit = Game.Content.Load<SoundEffect>(@"Audio/HitSound");
            Shoot = Game.Content.Load<SoundEffect>(@"Audio/ShootSound");
            Kill = Game.Content.Load<SoundEffect>(@"Audio/KillSound");
            Reflect = Game.Content.Load<SoundEffect>(@"Audio/ReflectSound");
            Clear = Game.Content.Load<SoundEffect>(@"Audio/ClearSound");
            Cross = Game.Content.Load<SoundEffect>(@"Audio/CrossSound");
            PowerupSpawn = Game.Content.Load<SoundEffect>(@"Audio/PowerupSpawn");
            NoBlock = Game.Content.Load<SoundEffect>(@"Audio/NoBlockSound");
            LifeUp = Game.Content.Load<SoundEffect>(@"Audio/LifeUpSound");
            SlowDown = Game.Content.Load<SoundEffect>(@"Audio/SlowSound");
            AddBlock = Game.Content.Load<SoundEffect>(@"Audio/AddBlockSound");

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (restart)
            {
                restart = false;
                spriteList.Clear();
                magnetList.Clear();
                blockList.Clear();
                powerupList.Clear();
                player.Position = new Vector2((Game.Window.ClientBounds.Width / 2) - 60, (Game.Window.ClientBounds.Height / 2) - 52);
                playerRightHand.Position = (player.Position + new Vector2(150, 50));
                playerLeftHand.Position = (player.Position + new Vector2(-30, 50));
                elapsedTime = 0;
            }

            player.Update(gameTime, Game.Window.ClientBounds);
            playerRightHand.Update(gameTime, Game.Window.ClientBounds);
            playerLeftHand.Update(gameTime, Game.Window.ClientBounds);

            KeyboardState powerupStates = Keyboard.GetState();

            if (powerupStates.IsKeyDown(Keys.D1)) shortcut[0] = true;
            if (powerupStates.IsKeyDown(Keys.D2)) shortcut[1] = true;
            if (powerupStates.IsKeyDown(Keys.D3)) shortcut[2] = true;
            if (powerupStates.IsKeyDown(Keys.D4)) shortcut[3] = true;
            if (powerupStates.IsKeyDown(Keys.D5)) shortcut[4] = true;
            if (powerupStates.IsKeyDown(Keys.D6)) shortcut[5] = true;
            if (powerupStates.IsKeyDown(Keys.D7)) shortcut[6] = true;
            if (powerupStates.IsKeyDown(Keys.D8)) shortcut[7] = true;
            if (powerupStates.IsKeyDown(Keys.D9)) shortcut[8] = true;
            if (powerupStates.IsKeyDown(Keys.D0)) shortcut[9] = true;
            if (powerupStates.IsKeyUp(Keys.D1) && shortcut[0] == true)
            {
                shortcut[0] = false;
                powerupList.Add(new PowerupSprite(Game.Content.Load<Texture2D>(@"Images/LifePowerup"),
                            new Vector2(random.Next(Game.Window.ClientBounds.Width - 50), random.Next(Game.Window.ClientBounds.Height - 50)), 
                            new Point(50, 50), 0, new Point(0, 0), new Point(1, 1), 
                            new Vector2(random.Next(11) - 5,random.Next(11) - 5), 0, Sprite.PowerType.life, 360));
            }
            if (powerupStates.IsKeyUp(Keys.D2) && shortcut[1] == true)
            {
                shortcut[1] = false;
                powerupList.Add(new PowerupSprite(Game.Content.Load<Texture2D>(@"Images/BombPowerup"),
                            new Vector2(random.Next(Game.Window.ClientBounds.Width - 50), random.Next(Game.Window.ClientBounds.Height - 50)),
                            new Point(50, 50), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(random.Next(11) - 5, random.Next(11) - 5), 0, Sprite.PowerType.bomb, 360));
            }
            if (powerupStates.IsKeyUp(Keys.D3) && shortcut[2] == true)
            {
                shortcut[2] = false;
                powerupList.Add(new PowerupSprite(Game.Content.Load<Texture2D>(@"Images/SlowPowerup"),
                            new Vector2(random.Next(Game.Window.ClientBounds.Width - 50), random.Next(Game.Window.ClientBounds.Height - 50)),
                            new Point(50, 50), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(random.Next(11) - 5, random.Next(11) - 5), 0, Sprite.PowerType.slow, 360));
            }
            if (powerupStates.IsKeyUp(Keys.D4) && shortcut[3] == true)
            {
                shortcut[3] = false;
                powerupList.Add(new PowerupSprite(Game.Content.Load<Texture2D>(@"Images/CrossPowerup"),
                            new Vector2(random.Next(Game.Window.ClientBounds.Width - 50), random.Next(Game.Window.ClientBounds.Height - 50)),
                            new Point(50, 50), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(random.Next(11) - 5, random.Next(11) - 5), 0, Sprite.PowerType.cross, 360));
            }
            if (powerupStates.IsKeyUp(Keys.D5) && shortcut[4] == true)
            {
                shortcut[4] = false;
                powerupList.Add(new PowerupSprite(Game.Content.Load<Texture2D>(@"Images/NoBlockPowerup"),
                            new Vector2(random.Next(Game.Window.ClientBounds.Width - 50), random.Next(Game.Window.ClientBounds.Height - 50)),
                            new Point(50, 50), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(random.Next(11) - 5, random.Next(11) - 5), 0, Sprite.PowerType.noblock, 360));
            }
            if (powerupStates.IsKeyUp(Keys.D6) && shortcut[5] == true)
            {
                shortcut[5] = false;
                powerupList.Add(new PowerupSprite(Game.Content.Load<Texture2D>(@"Images/AddBlockPowerup"),
                            new Vector2(random.Next(Game.Window.ClientBounds.Width - 50), random.Next(Game.Window.ClientBounds.Height - 50)),
                            new Point(50, 50), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(random.Next(11) - 5, random.Next(11) - 5), 0, Sprite.PowerType.addblock, 360));
            }
            if (powerupStates.IsKeyUp(Keys.D7) && shortcut[6] == true)
            {
                shortcut[6] = false;
                currentLevel++;
            }
            if (powerupStates.IsKeyUp(Keys.D8) && shortcut[7] == true && currentLevel > 1)
            {
                shortcut[7] = false;
                currentLevel--;
            }
            if (powerupStates.IsKeyUp(Keys.D9) && shortcut[8] == true)
            {
                shortcut[8] = false;
                spriteList.Add(new EnemySprite(Game.Content.Load<Texture2D>(@"Images/BlueEnemy"),
                        new Vector2(1024-50, random.Next(718)), new Point(50, 50), 5,
                        new Point(0, 0), new Point(1, 1), new Vector2(-(random.Next(5) + 1), random.Next(5) + 1),
                        100, Sprite.Charge.positive));
            }
            if (powerupStates.IsKeyUp(Keys.D0) && shortcut[9] == true)
            {
                shortcut[9] = false;
                spriteList.Add(new EnemySprite(Game.Content.Load<Texture2D>(@"Images/RedEnemy"),
                        new Vector2(1024-50, random.Next(718)), new Point(50, 50), 5,
                        new Point(0, 0), new Point(1, 1), new Vector2(-(random.Next(5) + 1), (random.Next(13) - 6)),
                        100, Sprite.Charge.negative));
            }

            foreach (Sprite s in spriteList) s.Update(gameTime, Game.Window.ClientBounds);
            foreach (Background b in backgroundList) b.Update(gameTime, Game.Window.ClientBounds);
            foreach (Magnet m in magnetList) m.Update(gameTime, Game.Window.ClientBounds);
            foreach (EnemySprite l in livesList) l.Update(gameTime, Game.Window.ClientBounds);
            foreach (BlockSprite bs in blockList) bs.Update(gameTime, Game.Window.ClientBounds);
            foreach (PowerupSprite p in powerupList) p.Update(gameTime, Game.Window.ClientBounds);

            if (player.Release)
            {
                player.Release = false;
                player.Shoot = false;
                if (!player.IsPositive)
                {
                    if (player.IsFacingRight)
                        magnetList.Add(new Magnet(Game.Content.Load<Texture2D>(@"Images/NegativeMagnet"),
                            playerRightHand.Position + new Vector2(0, -25), new Point(40, 40), 0, new Point(0, 0),
                            new Point(1, 1), new Vector2(20, 0), 0, Sprite.Charge.negative));
                    else
                        magnetList.Add(new Magnet(Game.Content.Load<Texture2D>(@"Images/NegativeMagnet"),
                            playerLeftHand.Position + new Vector2(-50, -25), new Point(40, 40), 0, new Point(0, 0),
                            new Point(1, 1), new Vector2(-20, 0), 0, Sprite.Charge.negative));
                }
                else if (player.IsPositive)
                {
                    if (player.IsFacingRight)
                        magnetList.Add(new Magnet(Game.Content.Load<Texture2D>(@"Images/PositiveMagnet"),
                            playerRightHand.Position + new Vector2(0, -25), new Point(40, 40), 0, new Point(0, 0),
                            new Point(1, 1), new Vector2(20, 0), 0, Sprite.Charge.positive));
                    else
                        magnetList.Add(new Magnet(Game.Content.Load<Texture2D>(@"Images/PositiveMagnet"),
                            playerLeftHand.Position + new Vector2(-50, -25), new Point(40, 40), 0, new Point(0, 0),
                            new Point(1, 1), new Vector2(-20, 0), 0, Sprite.Charge.positive));
                }
                ShootInstance = Shoot.CreateInstance();
                ShootInstance.Play();
            }

            if (!player.IsFacingRight) playerLeftHand.HandFlip = SpriteEffects.FlipHorizontally;
            if (back1.Position.X < -back1.FrameSize.X) back1.Position = new Vector2(back3.Position.X + back3.FrameSize.X, 25);
            if (back2.Position.X < -back2.FrameSize.X) back2.Position = new Vector2(back1.Position.X + back1.FrameSize.X, 25);
            if (back3.Position.X < -back3.FrameSize.X) back3.Position = new Vector2(back2.Position.X + back2.FrameSize.X, 25);

            elapsedTime++;
            if (elapsedTime == 3600)
            {
                elapsedTime = 0;
                currentLevel++;
                blockList.Add(new BlockSprite(Game.Content.Load<Texture2D>(@"Images/Block"),
                    new Vector2(random.Next(Game.Window.ClientBounds.Width - 50), random.Next(Game.Window.ClientBounds.Height - 50)),
                    new Point(50, 50), 0, new Point(0, 0), new Point(1, 1), new Vector2(random.Next(5) - 2, random.Next(5) - 2), 0));
            }
            if (elapsedTime % ((currentLevel + 1) * 60) == 0)
            {
                for (int i = 0; i < currentLevel; i++)
                {
                    int index = random.Next(2) + 1;
                    int side = random.Next(2) + 1;
                    if (side == 1) side = 0; else side = Game.Window.ClientBounds.Width - 50;
                    if (index == 1) spriteList.Add(new EnemySprite(Game.Content.Load<Texture2D>(@"Images/BlueEnemy"),
                        new Vector2(side, random.Next(718)), new Point(50, 50), 5,
                        new Point(0, 0), new Point(1, 1), new Vector2(-(random.Next(5) + 1), random.Next(5) + 1),
                        100, Sprite.Charge.positive));
                    else spriteList.Add(new EnemySprite(Game.Content.Load<Texture2D>(@"Images/RedEnemy"),
                        new Vector2(side, random.Next(718)), new Point(50, 50), 5,
                        new Point(0, 0), new Point(1, 1), new Vector2(-(random.Next(5) + 1), (random.Next(13) - 6)),
                        100, Sprite.Charge.negative));
                }
            }
            if (elapsedTime % 600 == 0)
            {
                int releaseChoice = random.Next(2);
                if (releaseChoice == 0)
                {
                    int releaseType = random.Next(6);
                    if (releaseType == 0)
                    {
                        powerupList.Add(new PowerupSprite(Game.Content.Load<Texture2D>(@"Images/LifePowerup"),
                            new Vector2(random.Next(Game.Window.ClientBounds.Width - 50), random.Next(Game.Window.ClientBounds.Height - 50)), 
                            new Point(50, 50), 0, new Point(0, 0), new Point(1, 1), 
                            new Vector2(random.Next(11) - 5,random.Next(11) - 5), 0, Sprite.PowerType.life, 360));
                    }
                    else if (releaseType == 1)
                    {
                        powerupList.Add(new PowerupSprite(Game.Content.Load<Texture2D>(@"Images/BombPowerup"),
                            new Vector2(random.Next(Game.Window.ClientBounds.Width - 50), random.Next(Game.Window.ClientBounds.Height - 50)),
                            new Point(50, 50), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(random.Next(11) - 5, random.Next(11) - 5), 0, Sprite.PowerType.bomb, 360));
                    }
                    else if (releaseType == 2)
                    {
                        powerupList.Add(new PowerupSprite(Game.Content.Load<Texture2D>(@"Images/SlowPowerup"),
                            new Vector2(random.Next(Game.Window.ClientBounds.Width - 50), random.Next(Game.Window.ClientBounds.Height - 50)),
                            new Point(50, 50), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(random.Next(11) - 5, random.Next(11) - 5), 0, Sprite.PowerType.slow, 360));
                    }
                    else if (releaseType == 3)
                    {
                        powerupList.Add(new PowerupSprite(Game.Content.Load<Texture2D>(@"Images/CrossPowerup"),
                            new Vector2(random.Next(Game.Window.ClientBounds.Width - 50), random.Next(Game.Window.ClientBounds.Height - 50)),
                            new Point(50, 50), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(random.Next(11) - 5, random.Next(11) - 5), 0, Sprite.PowerType.cross, 360));
                    }
                    else if (releaseType == 4)
                    {
                        powerupList.Add(new PowerupSprite(Game.Content.Load<Texture2D>(@"Images/NoBlockPowerup"),
                            new Vector2(random.Next(Game.Window.ClientBounds.Width - 50), random.Next(Game.Window.ClientBounds.Height - 50)),
                            new Point(50, 50), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(random.Next(11) - 5, random.Next(11) - 5), 0, Sprite.PowerType.noblock, 360));
                    }
                    else if (releaseType == 5)
                    {
                        powerupList.Add(new PowerupSprite(Game.Content.Load<Texture2D>(@"Images/AddBlockPowerup"),
                            new Vector2(random.Next(Game.Window.ClientBounds.Width - 50), random.Next(Game.Window.ClientBounds.Height - 50)),
                            new Point(50, 50), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(random.Next(11) - 5, random.Next(11) - 5), 0, Sprite.PowerType.addblock, 360));
                    }
                    PowerupSpawnInstance = PowerupSpawn.CreateInstance();
                    PowerupSpawnInstance.Play();
                }
            }

            for (int i = 0; i < blockList.Count; i++)
            {
                if (blockList[i].CollisionRect.Intersects(player.CollisionRect))
                {
                    player.Position -= new Vector2(player.Speed.X, player.Speed.Y);
                    playerRightHand.Position -= new Vector2(playerRightHand.Speed.X, playerRightHand.Speed.Y);
                    playerLeftHand.Position -= new Vector2(playerLeftHand.Speed.X, playerLeftHand.Speed.Y);
                }
                for (int n = 0; n < spriteList.Count; n++)
                {
                    if (blockList[i].CollisionRect.Intersects(spriteList[n].CollisionRect))
                    {
                        spriteList[n].Speed = new Vector2(-spriteList[n].Speed.X, -spriteList[n].Speed.Y);
                    }
                }
            }

            for (int i = 0; i < powerupList.Count; i++)
            {
                if (powerupList[i].CollisionRect.Intersects(player.CollisionRect))
                {
                    if (powerupList[i].Type == Sprite.PowerType.life)
                    {
                        LifeUpInstance = LifeUp.CreateInstance();
                        LifeUpInstance.Play();
                        currentNumberOfLives++;
                        livesList.Add(new EnemySprite(Game.Content.Load<Texture2D>(@"Images/MagnaBoyWalkingSheet"),
                            new Vector2(10 + (currentNumberOfLives - 1) * 40, 35), new Point(120, 105), 10,
                            new Point(0, 0), new Point(1, 1), Vector2.Zero, 0, .25f));
                    }
                    else if (powerupList[i].Type == Sprite.PowerType.bomb)
                    {
                        ClearInstance = Clear.CreateInstance();
                        ClearInstance.Play();
                        for (int n = 0; n < spriteList.Count; n++) currentScore += 100;
                        spriteList.Clear();
                    }
                    else if (powerupList[i].Type == Sprite.PowerType.slow)
                    {
                        SlowDownInstance = SlowDown.CreateInstance();
                        SlowDownInstance.Play();
                        isEffected = true;
                        player.Speed -= new Vector2(7, 7);
                        playerRightHand.Speed -= new Vector2(7, 7);
                        playerLeftHand.Speed -= new Vector2(7, 7);
                    }
                    else if (powerupList[i].Type == Sprite.PowerType.cross)
                    {
                        CrossInstance = Cross.CreateInstance();
                        CrossInstance.Play();
                        int choice = random.Next(2);
                        if (choice == 1) magnetList.Add(new Magnet(Game.Content.Load<Texture2D>(@"Images/NegativeMagnet"),
                            player.Position, new Point(40, 40), 0, new Point(0, 0), new Point(1, 1), new Vector2(-20, -20), 
                            0, Sprite.Charge.negative));
                        else magnetList.Add(new Magnet(Game.Content.Load<Texture2D>(@"Images/PositiveMagnet"),
                            player.Position, new Point(40, 40), 0, new Point(0, 0), new Point(1, 1), new Vector2(-20, -20),
                            0, Sprite.Charge.positive));
                        choice = random.Next(2);
                        if (choice == 1) magnetList.Add(new Magnet(Game.Content.Load<Texture2D>(@"Images/NegativeMagnet"),
                            player.Position + new Vector2(60, 0), new Point(40, 40), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(0, -20), 0, Sprite.Charge.negative));
                        else magnetList.Add(new Magnet(Game.Content.Load<Texture2D>(@"Images/PositiveMagnet"),
                            player.Position + new Vector2(60, 0), new Point(40, 40), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(0, -20), 0, Sprite.Charge.positive));
                        choice = random.Next(2);
                        if (choice == 1) magnetList.Add(new Magnet(Game.Content.Load<Texture2D>(@"Images/NegativeMagnet"),
                            player.Position + new Vector2(120, 0), new Point(40, 40), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(20, -20), 0, Sprite.Charge.negative));
                        else magnetList.Add(new Magnet(Game.Content.Load<Texture2D>(@"Images/PositiveMagnet"),
                            player.Position + new Vector2(120, 0), new Point(40, 40), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(20, -20), 0, Sprite.Charge.positive));
                        choice = random.Next(2);
                        if (choice == 1) magnetList.Add(new Magnet(Game.Content.Load<Texture2D>(@"Images/NegativeMagnet"),
                            player.Position + new Vector2(120, 53), new Point(40, 40), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(20, 0), 0, Sprite.Charge.negative));
                        else magnetList.Add(new Magnet(Game.Content.Load<Texture2D>(@"Images/PositiveMagnet"),
                            player.Position + new Vector2(120, 53), new Point(40, 40), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(20, 0), 0, Sprite.Charge.positive));
                        choice = random.Next(2);
                        if (choice == 1) magnetList.Add(new Magnet(Game.Content.Load<Texture2D>(@"Images/NegativeMagnet"),
                            player.Position + new Vector2(120, 105), new Point(40, 40), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(20, 20), 0, Sprite.Charge.negative));
                        else magnetList.Add(new Magnet(Game.Content.Load<Texture2D>(@"Images/PositiveMagnet"),
                            player.Position + new Vector2(120, 105), new Point(40, 40), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(20, 20), 0, Sprite.Charge.positive));
                        choice = random.Next(2);
                        if (choice == 1) magnetList.Add(new Magnet(Game.Content.Load<Texture2D>(@"Images/NegativeMagnet"),
                            player.Position + new Vector2(60, 105), new Point(40, 40), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(0, 20), 0, Sprite.Charge.negative));
                        else magnetList.Add(new Magnet(Game.Content.Load<Texture2D>(@"Images/PositiveMagnet"),
                            player.Position + new Vector2(60, 105), new Point(40, 40), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(0, 20), 0, Sprite.Charge.positive));
                        choice = random.Next(2);
                        if (choice == 1) magnetList.Add(new Magnet(Game.Content.Load<Texture2D>(@"Images/NegativeMagnet"),
                            player.Position + new Vector2(0, 105), new Point(40, 40), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(-20, 20), 0, Sprite.Charge.negative));
                        else magnetList.Add(new Magnet(Game.Content.Load<Texture2D>(@"Images/PositiveMagnet"),
                            player.Position + new Vector2(0, 105), new Point(40, 40), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(-20, 20), 0, Sprite.Charge.positive));
                        choice = random.Next(2);
                        if (choice == 1) magnetList.Add(new Magnet(Game.Content.Load<Texture2D>(@"Images/NegativeMagnet"),
                            player.Position + new Vector2(0, 53), new Point(40, 40), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(-20, 0), 0, Sprite.Charge.negative));
                        else magnetList.Add(new Magnet(Game.Content.Load<Texture2D>(@"Images/PositiveMagnet"),
                            player.Position + new Vector2(0, 53), new Point(40, 40), 0, new Point(0, 0), new Point(1, 1),
                            new Vector2(-20, 0), 0, Sprite.Charge.positive));
                    }
                    else if (powerupList[i].Type == Sprite.PowerType.noblock)
                    {
                        NoBlockSoundInstance = NoBlock.CreateInstance();
                        NoBlockSoundInstance.Play();
                        if (blockList.Count != 0)blockList.RemoveAt(0);
                    }
                    else if (powerupList[i].Type == Sprite.PowerType.addblock)
                    {
                        AddBlockInstance = AddBlock.CreateInstance();
                        AddBlockInstance.Play();
                        blockList.Add(new BlockSprite(Game.Content.Load<Texture2D>(@"Images/Block"),
                            new Vector2(random.Next(Game.Window.ClientBounds.Width - 50), random.Next(Game.Window.ClientBounds.Height - 50)),
                            new Point(50, 50), 0, new Point(0, 0), new Point(1, 1), new Vector2(random.Next(5) - 2, random.Next(5) - 2), 0));
                    }
                    powerupList.Remove(powerupList[i]);
                    i--;
                }
            }

            for (int i = 0; i < spriteList.Count; i++)
            {
                if (spriteList[i].CollisionRect.Intersects(player.CollisionRect))
                {
                    currentNumberOfLives--;
                    HitInstance = Hit.CreateInstance();
                    HitInstance.Play();
                    spriteList.Remove(spriteList[i]);
                    livesList.RemoveAt(livesList.Count - 1);
                }
            }

            for (int i = 0; i < spriteList.Count; ++i)
            {
                for (int n = 0; n < magnetList.Count; ++n)
                {
                    if (i >= 0 && spriteList.Count > 0 && spriteList[i].CollisionRect.Intersects(magnetList[n].CollisionRect))
                    {
                        if (spriteList[i].Polarity != magnetList[n].Polarity)
                        {
                            AddScore(spriteList[i].PointValue);
                            KillInstance = Kill.CreateInstance();
                            KillInstance.Play();
                            spriteList.Remove(spriteList[i]);
                            magnetList.Remove(magnetList[n]);
                            n--;
                            i--;
                        }
                        else
                        {
                            ReflectInstance = Reflect.CreateInstance();
                            ReflectInstance.Play();
                            magnetList[n].Speed = new Vector2(-magnetList[n].Speed.X, -magnetList[n].Speed.Y);
                        }
                    }
                }
            }

            for (int i = 0; i < magnetList.Count; i++)
            {
                if (magnetList[i].Position.X > Game.Window.ClientBounds.Width ||
                    magnetList[i].Position.X < 0 ||
                    magnetList[i].Position.Y > Game.Window.ClientBounds.Height ||
                    magnetList[i].Position.Y < 0) magnetList.Remove(magnetList[i]);
            }

            if (isEffected)
            {
                occurTime--;
                if (occurTime == 0)
                {
                    occurTime = 240;
                    player.Speed += new Vector2(7, 7);
                    playerRightHand.Speed += new Vector2(7, 7);
                    playerLeftHand.Speed += new Vector2(7, 7);
                    isEffected = false;
                }
            }

            for (int i = 0; i < powerupList.Count; i++)
            {
                powerupList[i].Countdown--;
                if (powerupList[i].Countdown == 0)
                {
                    powerupList.Remove(powerupList[i]);
                    i--;
                }
            }

            livesList.Clear();
            for (int i = 0; i < currentNumberOfLives; i++)
            {
                int space = 10 + i * 40;
                livesList.Add(new EnemySprite(Game.Content.Load<Texture2D>(@"Images/MagnaBoyWalkingSheet"),
                    new Vector2(space, 35), new Point(120, 105), 10, new Point(0, 0), new Point(1, 1),
                    Vector2.Zero, 0, .25f));
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            player.Draw(gameTime, spriteBatch);
            if (player.IsFacingRight) playerRightHand.Draw(gameTime, spriteBatch);
            else playerLeftHand.Draw(gameTime, spriteBatch);

            for (int i = 0; i < spriteList.Count; i++) spriteList[i].Draw(gameTime, spriteBatch);
            foreach (Background b in backgroundList) b.Draw(gameTime, spriteBatch);
            foreach (Magnet m in magnetList) m.Draw(gameTime, spriteBatch);
            foreach (BlockSprite bs in blockList) bs.Draw(gameTime, spriteBatch);
            foreach (PowerupSprite p in powerupList) p.Draw(gameTime, spriteBatch);

            spriteBatch.DrawString(score, "Score : " + currentScore,
                new Vector2(10, 10), Color.White, 0, Vector2.Zero,
                1, SpriteEffects.None, 1);

            spriteBatch.DrawString(level, "Level : " + currentLevel,
                new Vector2(Game.Window.ClientBounds.Width - 90, 10),
                Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

            foreach (EnemySprite l in livesList) l.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
    }
}