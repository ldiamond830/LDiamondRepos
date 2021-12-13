using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace LockAndStock
{
    public enum GameState
    {
        title,
        game,
        gameover,

    }

    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch _spriteBatch;
        private GameState currentState = GameState.title;
        private SpriteFont font;
        private KeyboardState kbState;
        private KeyboardState prevKbState;
        private MouseState mouse;
        private GameTime gameTime = new GameTime();
        private double fireRate;
        private Player player;
        private Texture2D playerTexture;
        private Texture2D bulletTexture;
        private Texture2D AnCapTexture;
        private Texture2D TraderTexture;
        private Texture2D CryptoNerdTexture;
        private Texture2D crossHair;
        private Texture2D titleScreen;
        private double spawnTime;
        private double spawnTimer;
        private List<enemy> enemyList = new List<enemy>();
        private List<PowerUp> powerUpList = new List<PowerUp>();
        private Random rng = new Random();
        private Texture2D healthUpTexture;
        private Texture2D invicibleTexture;
        private Texture2D speedUpTexture;
        private Texture2D clearScreenTexture;
        private Texture2D gameOverScreen;
        private double inviciblityTimer;
        private SoundEffect shot;
        private SoundEffect healthPickUpSfx;
        private SoundEffect speedPickUpSfx;
        private SoundEffect invincblePickUpSfx;
        private SoundEffect clearPickUpSfx;
        private SoundEffect AnCapVoiceLine;
        private SoundEffect TraderVoiceLine;
        private SoundEffect CryptoNerdVoiceLine;
        private SoundEffect shotSoundEffect;
        private SoundEffect gameOverSound;
        private int finalScore;
        private bool gameOverSfxActive = false;
        private SpriteFont bigFont;
        private Texture2D gamePlayBackground;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferWidth = 1920;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 1080;
            graphics.ApplyChanges();


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("File");
            playerTexture = Content.Load<Texture2D>("playerAvatar");
            bulletTexture = Content.Load<Texture2D>("bullet");
            AnCapTexture = Content.Load<Texture2D>("anCapTexture");
            AnCapVoiceLine = Content.Load<SoundEffect>("anCapVoiceLineWav");
            TraderTexture = Content.Load<Texture2D>("traderTexture");
            TraderVoiceLine = Content.Load<SoundEffect>("traderVoiceLineWav");
            CryptoNerdTexture = Content.Load<Texture2D>("cyrptoNerdTexture");
            CryptoNerdVoiceLine = Content.Load<SoundEffect>("cryptoNerdVoiceLine2Wav");
            crossHair = Content.Load<Texture2D>("crossHairCorrect");
            shot = Content.Load<SoundEffect>("shotGunBlastWav");
            healthUpTexture = Content.Load<Texture2D>("healthUp");
            invicibleTexture = Content.Load<Texture2D>("invincibleTexture");
            speedUpTexture = Content.Load<Texture2D>("speedUpTexture");
            clearScreenTexture = Content.Load<Texture2D>("clearScreenTexture");
            titleScreen = Content.Load<Texture2D>("graphicDesignIsMyPassion");
            healthPickUpSfx = Content.Load<SoundEffect>("healthUpSfx");
            speedPickUpSfx = Content.Load<SoundEffect>("speedUpSfx");
            invincblePickUpSfx = Content.Load<SoundEffect>("invincilbeSfx");
            clearPickUpSfx = Content.Load<SoundEffect>("clearScreenSfx");
            gameOverSound = Content.Load<SoundEffect>("gameOverSfx");
            gameOverScreen = Content.Load<Texture2D>("GameOverScreen");
            bigFont = Content.Load<SpriteFont>("bigFont");
            gamePlayBackground = Content.Load<Texture2D>("gamePlayBackGround2");
            player = new Player(5, 10, playerTexture, bulletTexture, new Rectangle(125, 125, 125, 125), shot);
            spawnTime = SetSpawnTimer();
            inviciblityTimer = 2;

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            kbState = Keyboard.GetState();
            mouse = Mouse.GetState();
            // TODO: Add your update logic here
            switch (currentState)
            {
                case GameState.title:
                    if (SingleKeyPress(Keys.Enter, kbState))
                    {
                        currentState = GameState.game;

                    }
                    break;

                case GameState.game:

                    player.Update(fireRate, mouse, kbState);



                    foreach (PowerUp powerUp in powerUpList)
                    {

                        powerUp.Update(player, inviciblityTimer, enemyList);

                        if (powerUp.EndInvincible)
                        {
                            inviciblityTimer = 2;
                        }
                    }

                    for (int i = 0; i < powerUpList.Count; i++)
                    {
                        if (!powerUpList[i].IsActive)
                        {
                            powerUpList.RemoveAt(i);
                        }
                    }

                    if (spawnTimer > spawnTime)
                    {
                        SpawnEnemies(rng);
                        spawnTimer = 0;
                        spawnTime = SetSpawnTimer();
                    }

                    foreach (Bullet bullet in player.BulletList)
                    {
                        if (bullet.IsActive)
                        {
                            bullet.Update();
                        }
                    }

                    if (player.HasShot == false)
                    {
                        fireRate += gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                    {
                        fireRate = 0;
                        player.HasShot = false;
                    }


                    foreach (enemy enemy in enemyList)
                    {
                        if (enemy.IsAlive && !(enemy is CryptoNerd))
                        {
                            enemy.Update(player, rng);
                        }
                        else if (enemy.IsAlive && enemy is CryptoNerd)
                        {
                            enemy.Update(player, rng);
                        }

                        foreach (Bullet bullet in player.BulletList)
                        {

                            if (enemy.IsAlive && !(enemy is CryptoNerd))
                            {
                                enemy.hitCheck(bullet, player);
                            }
                            else if (enemy.IsAlive && enemy is CryptoNerd)
                            {
                                enemy.hitCheck(bullet, player);
                            }

                        }
                        if (enemy.IsHit == true)
                        {
                            //1 in 10 chance of spawning a power up
                            int chance = rng.Next(1, 11);
                            if (chance == 1)
                            {
                                GeneratePowerUp(enemy.Position);
                            }

                        }
                    }

                    for (int i = 0; i < enemyList.Count; i++)
                    {
                        if (!enemyList[i].IsAlive)
                        {
                            enemyList.RemoveAt(i);
                        }
                    }

                    for (int i = 0; i < player.BulletList.Count; i++)
                    {
                        if (!player.BulletList[i].IsActive)
                        {
                            player.BulletList.RemoveAt(i);
                        }
                    }

                    spawnTimer += gameTime.ElapsedGameTime.TotalSeconds;

                    if (player.Health == 0)
                    {
                        currentState = GameState.gameover;
                    }

                    if (player.IsInvincible)
                    {
                        inviciblityTimer -= gameTime.ElapsedGameTime.TotalSeconds;



                    }


                    break;

                case GameState.gameover:
                    finalScore = player.Score;
                    if (gameOverSfxActive == false)
                    {
                        gameOverSound.Play(1, 0, 0);
                        gameOverSfxActive = true;
                    }


                    if (SingleKeyPress(Keys.Enter, kbState))
                    {
                        currentState = GameState.title;
                        finalScore = 0;
                        player.Reset();
                        enemyList.Clear();
                        powerUpList.Clear();


                    }

                    break;

            }


            prevKbState = kbState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();

            switch (currentState)
            {
                case GameState.title:
                    _spriteBatch.Draw(titleScreen, new Rectangle(0, 0, 1920, 1080), Color.White);
                    break;
                case GameState.game:

                    _spriteBatch.Draw(gamePlayBackground, new Rectangle(0, 0, 1920, 1080), Color.White);

                    player.Draw(_spriteBatch);

                    foreach (PowerUp powerUp in powerUpList)
                    {
                        if (powerUp.IsActive && powerUp.IsVisible)
                            powerUp.Draw(_spriteBatch);
                    }

                    foreach (Bullet bullet in player.BulletList)
                    {
                        if (bullet.IsActive)
                        {
                            bullet.Draw(_spriteBatch);
                        }
                    }

                    foreach (enemy enemy in enemyList)
                    {
                        if (enemy.IsAlive)
                        {
                            enemy.Draw(_spriteBatch);
                        }

                    }
                    _spriteBatch.Draw(crossHair, new Rectangle(mouse.X, mouse.Y, 50, 50), Color.White);

                    _spriteBatch.DrawString(font, string.Format("{0}", player.Health), new Vector2(0, 150), Color.Black);
                    _spriteBatch.DrawString(font, string.Format("{0}", player.Score), new Vector2(0, 200), Color.Black);
                    break;

                case GameState.gameover:
                    _spriteBatch.Draw(gameOverScreen, new Rectangle(0, 0, 1920, 1080), Color.White);
                    _spriteBatch.DrawString(bigFont, string.Format("final score {0}", finalScore), new Vector2(300, 1080 / 2 + 150), Color.Black);
                    break;
            }

            _spriteBatch.End();



            base.Draw(gameTime);
        }

        private bool SingleKeyPress(Keys key, KeyboardState kbState)
        {
            //if the specified key is currently down and was previously up, returns as true
            if (kbState.IsKeyDown(key) == true && prevKbState.IsKeyUp(key) == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public double SetSpawnTimer()
        {
            if (player.Score <= 1500)
            {
                return 1;
            }
            else if (player.Score > 1500 && player.Score <= 3000)
            {
                return 0.9;
            }
            else if (player.Score > 3000 && player.Score <= 4500)
            {
                return 0.8;
            }
            else if (player.Score > 4500 && player.Score <= 6000)
            {
                return 0.7;
            }
            else if (player.Score > 6000 && player.Score <= 7500)
            {
                return 0.6;
            }
            else
            {
                return 0.5;
            }
        }

        public void GeneratePowerUp(Rectangle position)
        {
            int selection = rng.Next(1, 5);

            if (selection == 1)
            {
                powerUpList.Add(new PowerUp(new Rectangle(position.X, position.Y, 50, 50), type.healthUP, healthUpTexture, healthPickUpSfx));
            }
            else if (selection == 2)
            {
                powerUpList.Add(new PowerUp(new Rectangle(position.X, position.Y, 50, 50), type.invincibility, invicibleTexture, invincblePickUpSfx));
            }
            else if (selection == 3)
            {
                powerUpList.Add(new PowerUp(new Rectangle(position.X, position.Y, 50, 50), type.speedUP, speedUpTexture, speedPickUpSfx));
            }
            else
            {
                powerUpList.Add(new PowerUp(new Rectangle(position.X, position.Y, 50, 50), type.clearScreen, clearScreenTexture, clearPickUpSfx));
            }
        }

        public void SpawnEnemies(Random rng)
        {
            if (enemyList.Count <= 20)
            {
                //spawns 1-2 enemies
                int selection = rng.Next(1, 3);



                //selects position
                selection = rng.Next(1, 5);
                Rectangle Position = default;
                if (selection == 1)
                {
                    Position = new Rectangle(rng.Next(-100, 0), rng.Next(0, 1080), 100, 100);
                }
                else if (selection == 2)
                {
                    Position = new Rectangle(rng.Next(0, 1920), rng.Next(-100, 0), 100, 100);
                }
                else if (selection == 3)
                {
                    Position = new Rectangle(rng.Next(0, 1920), rng.Next(1080, 1180), 100, 100);
                }
                else
                {
                    Position = new Rectangle(rng.Next(1920, 2000), rng.Next(0, 1080), 100, 100);
                }



                selection = rng.Next(1, 11);
                if (selection < 5)
                {
                    enemyList.Add(new Trader(TraderTexture, TraderVoiceLine, Position));
                }
                else if (selection >= 5 && selection < 8)
                {
                    enemyList.Add(new CryptoNerd(CryptoNerdTexture, CryptoNerdVoiceLine, Position));
                }
                else
                {
                    enemyList.Add(new AnCap(AnCapTexture, AnCapVoiceLine, Position));
                }


            }



        }
    }
}
