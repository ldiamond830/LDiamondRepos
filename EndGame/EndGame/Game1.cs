using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace EndGame
{
    public enum GameState
    {
        Menu,
        Question,
        Battle,
        GameOver,
        Victory,
        Pause

    }
    public class Game1 : Game
    {
        //declaring variables
        private GameTime gameTime;
        private double BossTimer;
        private double playerShootTimer = 3;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private int progress = 0;
        private GameState currentState = GameState.Menu;
        private KeyboardState kbState;
        private KeyboardState prevKbState;
        private MouseState mouseState;
        private Player player;
        private string name;
        private SpriteFont stockFont;
        //initializing each boss
        private Alive alive; //boss 1
        private  Castle_sMadeOfSand castlesMadeOfSand; //boss 2
        private What_sInMyHead whatsInMyHead; //boss 3
        private BonesOfBirds bonesOfBirds; // boss 4
        private Black_Hole_Sun blackHoleSun; // boss 5
        private Last_Remaining_Light lastRemainingLight; //boss 6
        private Boss currentBoss;

        //initialzing textures
        private Texture2D bulletTexture;
        private Texture2D playerTexture;
        private Texture2D aliveTexture1;
        
        private Texture2D aliveTexture2;
        private Texture2D aliveTexture3;
        private Texture2D aliveTexture4;
        
        private Texture2D castlesMadeOfSandTexture;
        private Texture2D killBoxTextures;

        private Texture2D WhatsInMyHeadTexture1;
        private Texture2D WhatsInMyHeadTexture2;
        private Texture2D axeTexture;
        private Texture2D pitTexture;

        private Texture2D bonesOfBirdsTexture;
        private Texture2D tornadoTexture;
        private SoundEffect screechSFX;
       
        private List<Button> buttonList = new List<Button>();



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            stockFont = Content.Load<SpriteFont>("stockFont");

            _graphics.PreferredBackBufferWidth = 1920;  // sets window size
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //assigning assets
            bulletTexture = Content.Load<Texture2D>("BulletPlace");
            playerTexture = Content.Load<Texture2D>("playerPlaceHolder");
            aliveTexture1 = Content.Load<Texture2D>("alivePlaceHolder1");
            
            aliveTexture2 = Content.Load<Texture2D>("alivePlaceHolder2");
            aliveTexture3 = Content.Load<Texture2D>("alivePlaceHolder3");
            aliveTexture4 = Content.Load<Texture2D>("alivePlaceHolder4");

            WhatsInMyHeadTexture1 = Content.Load<Texture2D>("what'sInMyHead1");
            WhatsInMyHeadTexture2 = Content.Load<Texture2D>("what'sInMyHead2");
            axeTexture = Content.Load<Texture2D>("axeTemp");
            pitTexture = Content.Load<Texture2D>("pitTempCorrected");

            castlesMadeOfSandTexture = Content.Load<Texture2D>("castleMadeOfSandBoss");
            killBoxTextures = Content.Load<Texture2D>("killbox");

            bonesOfBirdsTexture = Content.Load<Texture2D>("bonesOfBirds");
            tornadoTexture = Content.Load<Texture2D>("tornadoArt");
            screechSFX = Content.Load<SoundEffect>("screechSound");

            //initializing player
            player = new Player(100, 100, 8, 10, bulletTexture, playerTexture, new Rectangle(100, 600, 100, 100), 1);
            
            
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //gets the state of the keyboard at the start of each frame
            kbState = Keyboard.GetState();
            
            //finite state machine
            switch (currentState)
            {
                //transition to the first question state when the player presses enter
                case GameState.Menu:
                    if(SingleKeyPress(Keys.Enter, kbState))
                    {
                        currentState = GameState.Question;
                    }
                    break;

                case GameState.Question:
                    //calls a method to intiialize each boss
                    LoadBoss();
                    //if the player is just starting the game asks them to input their name
                    if (progress == 0)
                    {

                        if(ScoreboardInput() != null && ScoreboardInput() != "back")
                        {
                            name += ScoreboardInput();
                        }
                        //allows the player to delete characters from their name
                        else if(ScoreboardInput() == "back")
                        {
                            name = name.Remove(name.Length - 1, 1);
                        }
                       //sets the current boss to boss 1
                        currentBoss = alive;
                    }
                    //otherwise will set the current boss to the next boss in order based on the progess variable
                    if (progress == 1)
                    {
                        currentBoss = castlesMadeOfSand;
                    }
                    if (progress == 2)
                    {
                        currentBoss = whatsInMyHead;
                    }
                    if (progress == 3)
                    {
                        currentBoss = bonesOfBirds;
                    }
                    if (progress == 4)
                    {
                        currentBoss = blackHoleSun;
                    }
                    if (progress == 5)
                    {
                        currentBoss = lastRemainingLight;
                    }

                    //if the player has completed boss 1 this method is used to check if the player has clicked on any of the buttons
                    if(progress != 0)
                    {
                        for (int i = 0; i < buttonList.Count; i++)
                        {
                            buttonList[i].Update();
                        }
                           
                        
                    }

                    //separate statement so that enter all ways progresses the player 
                    if (SingleKeyPress(Keys.Enter, kbState))
                    {
                        currentState = GameState.Battle;
                    }
                    break;

                case GameState.Battle:
                    //allows the player to pause at any time during a fight by pressing P
                    if(SingleKeyPress(Keys.P, kbState))
                    {
                        currentState = GameState.Pause;
                    }

                    //takes the inputs for the player character
                    player.Update(kbState, currentBoss, playerShootTimer);

                    //removes any projectiles that have already hit or gone off screen so that the cleaner can delete them
                    for (int i = 0; i < player.ProjectileList.Count; i++)
                    {
                        if (player.ProjectileList[i].HasHit)
                        {
                            player.ProjectileList.Remove(player.ProjectileList[i]);
                        }
                        

                    }

                    //updates each currently active projectile the player has fired
                    for (int i = 0; i < player.ProjectileList.Count; i++)
                    {
                        player.ProjectileList[i].Update();
                    }

                    //removes any projectiles that have already hit or gone off screen so that the cleaner can delete them
                    for (int i = 0; i < currentBoss.BulletList.Count; i++)
                    {

                        if(currentBoss.BulletList[i].HasHit == true)
                        {
                            currentBoss.BulletList.RemoveAt(i);
                        }

                    }

                    //updates each currently active projectile the boss has fired
                    if(currentBoss.BulletList.Count >= 1)
                    {
                        for (int i = 0; i < currentBoss.BulletList.Count; i++)
                        {
                            currentBoss.BulletList[i].Update();
                        }
                    }

                    //timer controlling the player's firerate
                    if (!player.HasShot)
                    {
                        playerShootTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    
                    //resets the time when the player shoots
                    if(player.HasShot)
                    {
                        player.HasShot = false;
                        playerShootTimer = 0;
                    }

                    //these two statement are pretty much only used for the alive boss as I figured it would neaten up my code to move all this to the a method in the boss method 
                    BossTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    if (currentBoss.AttackSwitch)
                    {
                        currentBoss.AttackSwitch = false;
                        BossTimer = 0;
                    }

                    //updates the current boss
                    if (progress == 0)
                    {
                        
                        alive.Update(BossTimer);
                    }
                    if (progress == 1)
                    {
                        castlesMadeOfSand.Update(gameTime);
                    }
                    if (progress == 2)
                    {
                        whatsInMyHead.Update(gameTime);
                    }
                    if (progress == 3)
                    {
                        bonesOfBirds.Update(gameTime);
                    }
                    if (progress == 4)
                    {
                        //blackHoleSun.Update(gameTime);
                    }
                    if (progress == 5)
                    {
                        //lastRemainingLight.Update(gameTime);
                    }

                    //when the bosses health reaches zero
                    if(currentBoss.Health <= 0)
                    {
                        //changes the game state to question
                        currentState = GameState.Question;
                        //increase's the progress meter
                        progress++;
                        //resets the player's health
                        player.Reset();
                        //creates each button for the current question
                        CreateOptions(progress);
                        
                        playerShootTimer = 0;
                    }



                    break;

                case GameState.Pause:
                    //allows the player to go back into the current fight
                    if(SingleKeyPress(Keys.P, kbState))
                    {
                        currentState = GameState.Battle;
                    }
                    //allows the player to exit to menu and restart
                    if(SingleKeyPress(Keys.Escape, kbState))
                    {
                        progress = 0;
                        player.Reset();
                        currentState = GameState.Menu;
                    }
                    break;

                case GameState.GameOver:
                    //placeholder
                    break;

                case GameState.Victory:
                    //placeholder
                    break;
            }
            //gets the keyboard state at the end of this frame so that when it rolls over to the next frame it can sever as the previous state
            prevKbState = kbState;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            //draws all assets based on the current game state
            switch (currentState)
            {
                case GameState.Menu:
                    _spriteBatch.DrawString(stockFont, "titlePlaceHolder", new Vector2(960, 540), Color.White);
                    break;

                case GameState.Question:
                    if(progress == 0 )
                    {
                        _spriteBatch.DrawString(stockFont, "Who are you?", new Vector2(940, 300), Color.White);
                        if (name != null)
                        {
                            _spriteBatch.DrawString(stockFont, name, new Vector2(960, 540), Color.Black);
                        }
                        
                    }
                    else if( progress > 0)
                    {
                        if(progress == 1)
                        {
                            _spriteBatch.DrawString(stockFont, "How did you get here?", new Vector2(880, 300), Color.White);
                        }
                        else if (progress == 2)
                        {
                            _spriteBatch.DrawString(stockFont, "How do you feel", new Vector2(880, 300), Color.White);
                        }
                        else if (progress == 3)
                        {
                            _spriteBatch.DrawString(stockFont, "What are you going to miss", new Vector2(880, 300), Color.White);
                        }
                        else if (progress == 4)
                        {
                            _spriteBatch.DrawString(stockFont, "What od you think will happen next", new Vector2(880, 300), Color.White);
                        }
                        else
                        {
                            _spriteBatch.DrawString(stockFont, "This is it", new Vector2(880, 300), Color.White);
                        }

                        foreach (Button button in buttonList)
                        {
                            button.Draw(_spriteBatch);
                        }
                    }
                    break;

                case GameState.Battle:
                    _spriteBatch.DrawString(stockFont, "Battle", new Vector2(10, 10), Color.White);
                    _spriteBatch.DrawString(stockFont, string.Format("{0}", player.Health), new Vector2(10, 30), Color.White);
                    _spriteBatch.DrawString(stockFont, string.Format("{0}", currentBoss.Health), new Vector2(10, 50), Color.White);
                    _spriteBatch.DrawString(stockFont, "Battle", new Vector2(10, 10), Color.White);
                    player.Draw(_spriteBatch, Color.White);
                    currentBoss.Draw(_spriteBatch, Color.White);

                    foreach (Bullet bullet in player.ProjectileList)
                    {
                        bullet.Draw(_spriteBatch, Color.White);

                    }

                    foreach(BossBullet bullet in currentBoss.BulletList)
                    {
                        bullet.Draw(_spriteBatch, Color.White);
                    }


                        break;

                case GameState.Pause:
                    _spriteBatch.DrawString(stockFont, "PAUSE", new Vector2(960, 100), Color.Black);
                    break;

                case GameState.GameOver:
                    _spriteBatch.DrawString(stockFont, "GAME OVER", new Vector2(960, 100), Color.Black);
                    break;

                case GameState.Victory:

                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }



        //helper methods
        private void LoadBoss()
        {
            //initializes each boss
            alive = new Alive(bulletTexture, aliveTexture1, aliveTexture2, player, aliveTexture3, aliveTexture4, aliveTexture1);
            
            castlesMadeOfSand = new Castle_sMadeOfSand(bulletTexture, castlesMadeOfSandTexture, player, killBoxTextures);
            
            whatsInMyHead = new What_sInMyHead(bulletTexture, WhatsInMyHeadTexture1, player, WhatsInMyHeadTexture2, axeTexture, pitTexture);

            bonesOfBirds = new BonesOfBirds(bulletTexture, bonesOfBirdsTexture, player, screechSFX, tornadoTexture);
           
            /*
            blackHoleSun = new Black_Hole_Sun();
            lastRemainingLight = new Last_Remaining_Light();
            */

        }

        private void CreateOptions(int stage)
        {
            //clears the previous set
            buttonList.Clear();

            if(stage == 0)
            {
                //has player input their name rather than ask a question
            }
            //otherwise declares 3 choices for each question
            else if(stage == 1)
            {
               
                buttonList.Add(new Button(GraphicsDevice, new Rectangle(850, 400, 400, 100), "Just drifted I guess", stockFont, Color.Black, Effect.damageOverTime));
                buttonList.Add(new Button(GraphicsDevice, new Rectangle(850, 550, 400, 100), "I scratched and clawed my way", stockFont, Color.Black, Effect.increaseSpeed));
                buttonList.Add(new Button(GraphicsDevice, new Rectangle(850, 700, 400, 100), "no idea", stockFont, Color.Black, Effect.increaseFireRate));
                

                
            }
            else if (stage == 2)
            {

                buttonList.Add(new Button(GraphicsDevice, new Rectangle(850, 400, 400, 100), "Fine I guess", stockFont, Color.Black, Effect.increaseHealth));
                buttonList.Add(new Button(GraphicsDevice, new Rectangle(850, 550, 400, 100), "angry, the world is ending", stockFont, Color.Black, Effect.increaseDamage));
                buttonList.Add(new Button(GraphicsDevice, new Rectangle(850, 700, 400, 100), "sad, the world is ending", stockFont, Color.Black, Effect.increaseShotSize));
                
                

            }
            else if (stage == 3)
            {

                buttonList.Add(new Button(GraphicsDevice, new Rectangle(850, 400, 400, 100), "Someone", stockFont, Color.Black, Effect.increaseFireRate));
                buttonList.Add(new Button(GraphicsDevice, new Rectangle(850, 550, 400, 100), "Something", stockFont, Color.Black, Effect.increaseHealth));
                buttonList.Add(new Button(GraphicsDevice, new Rectangle(850, 700, 400, 100), "Not much", stockFont, Color.Black, Effect.increaseSpeed));
                
                

            }
            else if(stage == 4)
            {
                buttonList.Add(new Button(GraphicsDevice, new Rectangle(850, 400, 400, 100), "Everything with start again from zero", stockFont, Color.Black, Effect.increaseHealth));
                buttonList.Add(new Button(GraphicsDevice, new Rectangle(850, 550, 400, 100), "nothing I guess", stockFont, Color.Black, Effect.increaseDamage));
                buttonList.Add(new Button(GraphicsDevice, new Rectangle(850, 700, 400, 100), "does it matter?", stockFont, Color.Black, Effect.damageOverTime));

            }

            else
            {
                buttonList.Add(new Button(GraphicsDevice, new Rectangle(850, 400, 400, 100), "Okay", stockFont, Color.Black, Effect.increaseHealth));
                buttonList.Add(new Button(GraphicsDevice, new Rectangle(850, 550, 400, 100), "Fuck", stockFont, Color.Black, Effect.increaseDamage));
                buttonList.Add(new Button(GraphicsDevice, new Rectangle(850, 700, 400, 100), "that isn't a question", stockFont, Color.Black, Effect.reset));

                


            }

            foreach(Button button in buttonList)
            {
                PickButtonEffect(button, button.Effect);
            }
            
        }

        //assigns an effect based on the effect enum to each button
        private void PickButtonEffect(Button button, Effect effect)
        {

            if (effect == Effect.damageOverTime)
            {
                button.OnLeftButtonClick += AddDotEffect;

            }
            else if (effect == Effect.increaseDamage)
            {
                button.OnLeftButtonClick += IncreaseDamageEffect;
            }
            else if (effect == Effect.increaseSpeed)
            {
                button.OnLeftButtonClick += IncreaseSpeedEffect;
            }
            else if (effect == Effect.increaseHealth)
            {
                button.OnLeftButtonClick += IncreaseHealthEffect;
            }
            else if (effect == Effect.increaseShotSize)
            {
                button.OnLeftButtonClick += IncreaseShotSize;
            }
            else if (effect == Effect.increaseFireRate)
            {
                button.OnLeftButtonClick += IncreaseFireRateEffect;
            }

            //reset case
            else
            {
                button.OnLeftButtonClick += ResetEffect;

            }


            

            
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

        //methods to run for each button effect
        private void AddDotEffect()
        {
            if (player.HasDOT == false)
            {
                player.HasDOT = true;
            }
            else
            {
                player.DotDamage = 10;
            }
            currentState = GameState.Battle;
        }

        //stat boost methods to be called by pressing the button promopts that appear between each fight
        private void IncreaseDamageEffect()
        {
            player.Damage += 5;
            currentState = GameState.Battle;
        }

        private void IncreaseHealthEffect()
        {
            player.FullHealth += 10;
            currentState = GameState.Battle;
        }

        private void IncreaseSpeedEffect()
        {
            player.Speed += 5;
            currentState = GameState.Battle;
        }
        
        private void IncreaseShotSize()
        {
            player.BulletSize = 30;
            currentState = GameState.Battle;
        }

        private void IncreaseFireRateEffect()
        {
            player.FireRate -= 0.3;
            currentState = GameState.Battle;
        }

        private void ResetEffect()
        {
            player.HasDOT = false;
            player.Damage = 10;
            player.FireRate = 3;
            player.FullHealth = 100;
            currentState = GameState.Battle;
        }

        //allows the player to input their name by checking for each key that is pressed and released 
        private string ScoreboardInput()
        {
            if (SingleKeyPress(Keys.A, kbState))
            {
                return "A";
            }
            if (SingleKeyPress(Keys.B, kbState))
            {
                return "B";
            }
            if (SingleKeyPress(Keys.C, kbState))
            {
                return "C";
            }
            if (SingleKeyPress(Keys.D, kbState))
            {
                return "D";
            }
            if (SingleKeyPress(Keys.E, kbState))
            {
                return "E";
            }
            if (SingleKeyPress(Keys.F, kbState))
            {
                return "F";
            }
            if (SingleKeyPress(Keys.G, kbState))
            {
                return "G";
            }
            if (SingleKeyPress(Keys.H, kbState))
            {
                return "H";
            }
            if (SingleKeyPress(Keys.I, kbState))
            {
                return "I";
            }
            if (SingleKeyPress(Keys.J, kbState))
            {
                return "J";
            }
            if (SingleKeyPress(Keys.K, kbState))
            {
                return "K";
            }
            if (SingleKeyPress(Keys.L, kbState))
            {
                return "L";
            }
            if (SingleKeyPress(Keys.M, kbState))
            {
                return "M";
            }
            if (SingleKeyPress(Keys.N, kbState))
            {
                return "N";
            }
            if (SingleKeyPress(Keys.O, kbState))
            {
                return "O";
            }
            if (SingleKeyPress(Keys.P, kbState))
            {
                return "P";
            }
            if (SingleKeyPress(Keys.Q, kbState))
            {
                return "Q";
            }
            if (SingleKeyPress(Keys.R, kbState))
            {
                return "R";
            }
            if (SingleKeyPress(Keys.S, kbState))
            {
                return "S";
            }
            if (SingleKeyPress(Keys.T, kbState))
            {
                return "T";
            }
            if (SingleKeyPress(Keys.U, kbState))
            {
                return "U";
            }
            if (SingleKeyPress(Keys.V, kbState))
            {
                return "V";
            }
            if (SingleKeyPress(Keys.W, kbState))
            {
                return "W";
            }
            if (SingleKeyPress(Keys.X, kbState))
            {
                return "X";
            }
            if (SingleKeyPress(Keys.Y, kbState))
            {
                return "Y";
            }
            if (SingleKeyPress(Keys.Z, kbState))
            {
                return "Z";
            }
            if (SingleKeyPress(Keys.Back, kbState))
            {
                return "back";
            }

            return null;

        }
    }
}
