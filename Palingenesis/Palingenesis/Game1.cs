using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

//Name: G-Force
//Date: 3/16/21
//Updated: 5/3/21
//Professor Mesh
//Purpose: To intialize the base states of our game.
//check
namespace Palingenesis
{
    /// <summary>
    /// enum used in the FSM
    /// </summary>
    enum GameState
    {
        Menu,
        ScoreBoard,
        Game,
        GameOver,
        Dialouge,
        Pause,
        Instructions,
        EnterName
    }
   
    
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch _spriteBatch;

        #region config game attributes

        Rectangle fullScreen = new Rectangle(0, 0, 1920, 1080);

        private Random rng = new Random();

        //game starts in the menu state
        private GameState currentState;
       
        private GameTime gametime = new GameTime();

        private KeyboardState kbState;
        private KeyboardState prevKbState;
        #endregion

        #region timer attributes
        private double ringTimer; 
        private double specialTimer; // Represents the time elapsed in the game to be used for boss attacks
        private double bossUpdateTimer;
        private double scoreTimer = 0;
        private double attackTimer;
        private double elapsed = 2;
        private const double Elapsed = 2;
        private SortedList timesSorted = new SortedList();
        private double time; // Represents total time elapsed in the game
        #endregion

        #region Texture2D attributes
        private Texture2D playerAsset; // Represents player image
        private Texture2D attackTextureRG;
        private Texture2D attackTextureNA;
        private Texture2D attackTexturePlayer;
        private Texture2D riceGoddessTexture;
        private Texture2D RiceGoddessBackground;
        private Texture2D NagaBackground;
        private Texture2D titleScreen;
        private Texture2D namescreen;
        private Texture2D instructions;
        private Texture2D scoreBoard;
        private Texture2D gameOver;
        private Texture2D bar;
        private Texture2D RGteleportTexture;
        private Texture2D NAteleportTexture;
        private Texture2D nagaBossTexture;
        private Texture2D instructionScreen;
        #endregion

        #region VN variables are for visual novel.
        private SpriteFont font;
        private SpriteFont fontVN;
        private Texture2D RGVNDefault;
        private Texture2D RGVNAlternate;
        private Texture2D RGVNWise;
        private Texture2D playerVNDefault;
        private Texture2D playerVNAlternate;
        private Texture2D RGbackgroundVN;
        private Texture2D textboxVN;
        private Texture2D textboxNameVN;
        private Texture2D NAVNDefault;
        private Texture2D NAVNAlternate;
        private Texture2D NAVNExasperated;
        private Texture2D NAVNMad;
        private Texture2D NAbackgroundVN;
        private Texture2D pauseScreen;
        #endregion

        #region Load sound effects and songs
        private SoundEffect takeDamadge;
        private SoundEffect deathSound;
        private SoundEffect forwardVN;
        private SoundEffect hit;
        private SoundEffect error;
        private Song RiceGoddessOST;
        private Song VNRiceGoddessOST;
        private Song NagaOST;
        private Song VNNagaOST;
        #endregion

        #region Game bools
        private bool isRiceGoddessLoaded; // Bool that represents whether Rice Goddess boss has been loaded in this game
        private bool isNagaBossLoaded; // Bool that represents whether Naga boss has been loaded in this game
        private bool Boss1Beaten;
        private bool Boss2Beaten;
        private bool IsMusicPlaying;
        private bool hasAdvanced;
        #endregion

        #region Game objects
        private Player player; // Represents the actual player
        private Boss boss; //boss object
        private Boss riceGoddess; // Rice Goddess boss
        private Boss nagaBoss; // Naga Boss

        //health bar objects
        HealthBar BossHealth;
        HealthBar PlayerHealth;
        #endregion

        private Dictionary<string, double> scoreBoardInfo = new Dictionary<string, double>();
        private int PlayerTime = 0;
        private string name = "";

        int randomChoice;
        private int dialogueNum = 0;
        private int numberOfDialougeFrames;
        private int bossesDefeated;
        private const int NumberOfBosses = 2;
        private const int WindowWidth = 1920;
        private const int WindowHeight = 1080;
        private bool happened = false;

        private List<Dialogue> dialougeList;
        private String currentLine;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            isRiceGoddessLoaded = false; // Sets it so Rice Goddess is not loaded yet
            isNagaBossLoaded = false; // Sets it so Naga boss is not loaded yet
            bossesDefeated = 0;
            Boss1Beaten = false;
            Boss2Beaten = false;
            hasAdvanced = false;
            dialougeList = new List<Dialogue>();

            Random rng = new Random();
            randomChoice = rng.Next(1,3);

            currentState = GameState.Menu;
           
            //Media player properties (change volume if sounds are too loud)
            MediaPlayer.Volume = 1;
            MediaPlayer.IsRepeating = true;
            IsMusicPlaying = false;

            graphics.PreferredBackBufferWidth = WindowWidth;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = WindowHeight;   // set this value to the desired height of your window

            graphics.ApplyChanges();
            base.Initialize();

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            
            #region Load title screen textures
            titleScreen = Content.Load<Texture2D>("titlescreen");
            instructions = Content.Load<Texture2D>("Instructions Screen (new)");
            namescreen = Content.Load<Texture2D>("enter_name_screen");
            pauseScreen = Content.Load<Texture2D>("PauseScreen");
            scoreBoard= Content.Load<Texture2D>("ScoreBoard");
            name = "";
            #endregion

            #region Load gameplay textures
            riceGoddessTexture = Content.Load<Texture2D>("RiceGoddessSprite");
            nagaBossTexture = Content.Load<Texture2D>("naga fire worm");
            playerAsset = Content.Load<Texture2D>("theophaniaInGame");
            attackTextureRG = Content.Load<Texture2D>("rice");
            attackTextureNA = Content.Load<Texture2D>("Fireball");
            attackTexturePlayer = Content.Load<Texture2D>("playerProjectileCorrected");
            RiceGoddessBackground = Content.Load<Texture2D>("RiceGodessBackground");
            NagaBackground = Content.Load<Texture2D>("NagaBackgroundFight");
            bar = Content.Load<Texture2D>("bar");
            RGteleportTexture = Content.Load<Texture2D>("RGTeleport");
            NAteleportTexture = Content.Load<Texture2D>("NagaTeleport");
            takeDamadge = Content.Load<SoundEffect>("takeDamadge"); 
            deathSound= Content.Load<SoundEffect>("deathSound");
            hit = Content.Load<SoundEffect>("Bonk");
            RiceGoddessOST = Content.Load<Song>("Sad but True - The HU");
            NagaOST = Content.Load<Song>("Landia - Kirby's Return to Dream Land");
            #endregion

            #region Load VN textures
            textboxVN = Content.Load<Texture2D>("textbox");
            textboxNameVN = Content.Load<Texture2D>("rustyName");
            RGbackgroundVN = Content.Load<Texture2D>("RGbackgroundVN");
            playerVNDefault = Content.Load<Texture2D>("theophaniaExpression1");
            playerVNAlternate = Content.Load<Texture2D>("TheoVisualNovelExpression2");
            RGVNDefault = Content.Load<Texture2D>("riceGoddessExpression1");
            RGVNAlternate = Content.Load<Texture2D>("RiceGoddessVisualNovelExpression2");
            RGVNWise = Content.Load<Texture2D>("ricegoddessWise");
            NAbackgroundVN = Content.Load<Texture2D>("NagaBG");
            NAVNDefault= Content.Load<Texture2D>("nagaBossVNMain");
            NAVNAlternate = Content.Load<Texture2D>("nagaBossVnAlternate");
            NAVNExasperated = Content.Load<Texture2D>("nagaclosedNervous");
            NAVNMad = Content.Load<Texture2D>("nagaopenMad");
            forwardVN = Content.Load<SoundEffect>("forward_sound");
            VNRiceGoddessOST = Content.Load<Song>("10 Min.Meditation Music for Positive Energy - GUARANTEED Find Inner Peace within 10 Min.");
            VNNagaOST = Content.Load<Song>("Fire Sanctuary - The Legend of Zelda Skyward Sword");
            #endregion

            //Loading fonts
            font = Content.Load<SpriteFont>("font");
            fontVN = Content.Load<SpriteFont>("bigfont");

            //Load GameOver Textures
            gameOver = Content.Load<Texture2D>("GameOverScreen");

            player = new Player(150, 10, 10, 20, playerAsset, new Rectangle(200, 200, 50, 50),hit, WindowHeight, WindowWidth, attackTexturePlayer);
            boss = null;
            time = 0;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            kbState = Keyboard.GetState();

            //game state switch statement
            switch (currentState)
            {
                case GameState.Menu:
                    //pressing enter on the main menu starts the game
                    if (SingleKeyPress(Keys.Enter, kbState))
                    {

                        LoadBoss(randomChoice);
                        player.Reset();
                        //boss.Reset();
                        scoreTimer = 0;
                        DialogueListAdd();
                        currentState = GameState.Instructions;
                        forwardVN.Play();
                        LoadHealthBars();
                        hasAdvanced = false;
                        //LoadScoreboard();

                        currentState = GameState.EnterName;
                        //MediaPlayer.Play(forwardVN);
                    }
                    break;

                case GameState.EnterName:
                    //allows user to enter 3 charachters as a name
                    if(name.Length < 3)
                    {
                        if(ScoreboardInput() != null && ScoreboardInput() != "back")
                        {
                            name += ScoreboardInput();
                        }
                        
                    }
                    if (ScoreboardInput() == "back" && name.Length >= 1)
                    {
                        name = name.Remove(name.Length - 1, 1);
                    }
                    if (SingleKeyPress(Keys.Enter, kbState) && name.Length > 0)
                    {
                        currentState = GameState.Instructions;
                        //adding the player name and a default time to the scoreboard
                        scoreBoardInfo.Add(name, 0);
                    }

                    break;

                case GameState.Instructions:
                    //when the player is finished reading instructions presses 
                    if(SingleKeyPress(Keys.Enter, kbState))
                    {
                        currentState = GameState.Dialouge;
                    }

                    break;

                case GameState.Game:
                    //updates player and bosses
                    player.Update();
                    boss.Update(player, bossUpdateTimer);
                    player.Attack(boss, prevKbState);
                    BossHealth.Update(boss.Health);
                    PlayerHealth.Update(player.Health);

                    #region If-Statements and loops for the game
                    //Play Rice Goddess music
                    if (randomChoice==1 && !IsMusicPlaying)
                    {
                        //MediaPlayer.Play(RiceGoddessOST);
                        IsMusicPlaying = true;
                    }

                    //Play Naga music
                    if (randomChoice==2 && !IsMusicPlaying)
                    {
                        //MediaPlayer.Play(NagaOST);
                        IsMusicPlaying = true;
                    }

                    //this is the timer component needed for the special attack method
                    float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
                    elapsed -= time;

                    //logic for drawing each order based on elapsed seconds
                    if (elapsed < 1 && boss.IsCharging == false)
                    {
                        //I moved the random variable generation oustide the AI method to save on memory
                        int tmp = rng.Next(0,7);

                        boss.AI(tmp, player, elapsed, gameTime);

                        //updates timer
                        elapsed = Elapsed;
                    }

                    //AI method runs every 2 seconds
                    /*if (timer > 2)e
                    */
                    //runs update on each bullet after the pattern is spawned by AI
                    for (int i = 0; i < boss.ProjectileList.Count; i++)
                    {
                        boss.ProjectileList[i].Update(specialTimer, ringTimer, boss);
                    }

                    for (int i = 0; i < player.ShotList.Count; i++)
                    {
                        player.ShotList[i].Update();
                    }

                    for (int i = 0; i < boss.ProjectileList.Count; i++)
                    {
                        if (boss.ProjectileList[i].HasHit == true)
                        {
                            boss.ProjectileList.RemoveAt(i);
                        }
                    }

                    for (int i = 0; i < player.ShotList.Count; i++)
                    {
                        if (player.ShotList[i].HasHit == true)
                        {
                            player.ShotList.RemoveAt(i);
                        }
                    }

                    scoreTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    attackTimer += gameTime.TotalGameTime.TotalSeconds;

                    if(boss.SpecialActive == true)
                    {
                        specialTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    }

                    if(boss.RingActive == true)
                    {
                        ringTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    }

                    if(specialTimer > 4)
                    {
                        specialTimer = 0;

                    }

                    if(boss.ChargeEnded == true)
                    {
                        bossUpdateTimer += gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                    {
                        bossUpdateTimer = 0;   
                    }
                   
                    //Console.WriteLine(gameTime.ElapsedGameTime.TotalSeconds);
                    //pressing escape during the game pauses
                    if (SingleKeyPress(Keys.P, kbState))
                    {
                        currentState = GameState.Pause;
                    }

                    //if the players health reaches zero game over
                    if(player.Health <= 0)
                    {
                        deathSound.Play();
                        currentState = GameState.GameOver;
                        IsMusicPlaying = false;
                        MediaPlayer.Stop();
                    }

                    //when the bosses health hit's zero, starts the next dialouge section
                    if ((boss.Health <= 0) && (Boss1Beaten == true))
                    {
                        Boss2Beaten = true;
                        player.Reset();
                        currentState = GameState.Dialouge;
                        IsMusicPlaying = false;
                        MediaPlayer.Stop();

                    }

                    else if (boss.Health <= 0)
                    {
                        //resets player health and changes to dialogue section
                        Boss1Beaten = true;
                        currentState = GameState.Dialouge;
                        IsMusicPlaying = false;
                        player.Health = player.MaxHealth;
                        PlayerHealth.ResetHealth(player.MaxHealth);
                        player.Reset();
                        MediaPlayer.Stop();

                        //Loads other boss that was not loaded already
                        if(randomChoice==1)
                        {
                            randomChoice = 2;
                            DialogueListAdd();
                            randomChoice = 1;


                        }
                        else if(randomChoice==2)
                        {
                            randomChoice = 1;
                            DialogueListAdd();
                            randomChoice = 2;
                        }

                    }
                    #endregion
                    break;

                case GameState.GameOver:
                    //pressing enter on the game over screen sends you to the menu
                    if(SingleKeyPress(Keys.Enter, kbState))
                    {
                        currentState = GameState.Menu;

                        //resetting all the game data
                        dialogueNum = 0;
                        dialougeList.Clear();
                        IsMusicPlaying = false;
                        Boss1Beaten = false;
                        Boss2Beaten = false;
                        hasAdvanced = false;
                    }
                    break;

                case GameState.Dialouge:
                    #region VN if statements
                    //plays rice godess VN music
                    if (randomChoice==1 && !IsMusicPlaying)
                    {
                        //MediaPlayer.Play(VNRiceGoddessOST);
                        IsMusicPlaying = true;
                    }

                    //plays rice godess VN music
                    else if (randomChoice == 2 && !IsMusicPlaying)
                    {
                        //MediaPlayer.Play(VNNagaOST);
                        IsMusicPlaying = true;
                    }
                    
                    //each time the user presses enter the dialouge advances by one line
                    if (SingleKeyPress((Keys.Enter), kbState))
                    { 
                        dialogueNum++;
                        forwardVN.Play();
                    }

                    //if you finish all the dialouge after beating both bosses it will take you to the scoreboard
                    if ((dialougeList[dialogueNum] == null) && (Boss1Beaten == true) && (Boss2Beaten == true))
                    {
                        currentState = GameState.ScoreBoard;

                        //resetting all the game data
                        dialogueNum = 0;
                        dialougeList.Clear();
                        IsMusicPlaying = false;
                        Boss1Beaten = false;
                        Boss2Beaten = false;
                        hasAdvanced = false;
                        MediaPlayer.Stop();
                        break;
                    }

                    //checks for dialogue after game
                    else if ((dialougeList[dialogueNum] == null) && (Boss1Beaten == true) && (Boss2Beaten == false)&& (hasAdvanced==false))
                    {
                        currentState = GameState.Dialouge;
                        dialogueNum = dialogueNum +2;

                        //Loads other boss that was not loaded already
                        if (randomChoice == 1)
                        {
                            randomChoice = 2;
                            LoadBoss(randomChoice);
                            LoadHealthBars();
                        }
                        else if (randomChoice == 2)
                        {
                            randomChoice = 1;
                            LoadBoss(randomChoice);
                            LoadHealthBars();
                        }
                        hasAdvanced = true;
                        IsMusicPlaying = false;
                    }

                    //when there is no remaining dialouge starts the next section of the game
                    else if (dialougeList[dialogueNum] == null)
                    {
                        boss.Reset();
                        currentState = GameState.Game;
                        IsMusicPlaying = false;
                        MediaPlayer.Stop();
                        dialogueNum++;
                    }
                    #endregion
                    break;

                case GameState.Pause:
                    //pressing escape on pause restarts the game
                    if(SingleKeyPress(Keys.R, kbState))
                    {
                        currentState = GameState.Game;
                    }
                    if(SingleKeyPress(Keys.M, kbState))
                    {
                        currentState = GameState.Menu;
                        MediaPlayer.Stop();
                    }
                    break;

                case GameState.ScoreBoard:
                    //write and save scoreboard
                    if (SingleKeyPress(Keys.Enter, kbState))
                    {
                        currentState = GameState.Menu;
                        scoreBoardInfo[name] = scoreTimer;
                        LoadScoreboard();
                        SaveScoreboard();
                    }
                    break;
            }

            prevKbState = kbState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CadetBlue);
            _spriteBatch.Begin();
            // TODO: Add your drawing code here
            switch (currentState)
            {
                case GameState.Menu:
                    //drawing title screen
                    _spriteBatch.Draw(titleScreen, fullScreen, Color.White);
                    break;

                case GameState.Instructions:
                    _spriteBatch.Draw(titleScreen, fullScreen, Color.White);
                    _spriteBatch.Draw(instructions, new Vector2(), Color.White);
                    break;

                case GameState.EnterName:
                    if(name != null)
                    {
                        _spriteBatch.Draw(namescreen, fullScreen, Color.White);
                        Vector2 pos = new Vector2(750, 670);
                        _spriteBatch.DrawString(fontVN, name, pos, Color.Black);
                    }                    
                    break;

                case GameState.Game:
                    //draws backgrounds for bosses
                    switch (randomChoice)
                    {
                        case 1:
                        _spriteBatch.Draw(RiceGoddessBackground, fullScreen, Color.White);
                            break;

                        case 2:
                            _spriteBatch.Draw(NagaBackground, fullScreen, Color.White);
                            break;

                    }

                    #region commented out for now/might delete later
                    //drawing game text
                    //_spriteBatch.DrawString(font, "PlaceHolder for game", new Vector2(0, 0), Color.White);
                    //_spriteBatch.DrawString(font, "Use WASD to move player", new Vector2(0, 20), Color.White);
                    //_spriteBatch.DrawString(font, "Use arrow keys to attack", new Vector2(0, 40), Color.White);

                    //_spriteBatch.DrawString(font, "Press P to pause", new Vector2(0, 60), Color.White);
                    //_spriteBatch.DrawString(font, string.Format("Player Health: {0}", player.Health), new Vector2(0, 80), Color.White);
                    //_spriteBatch.DrawString(font, string.Format("Boss Health: {0}", boss.Health), new Vector2(0, 100), Color.White);


                    //debug only
                    //_spriteBatch.DrawString(font, string.Format("{0}", specialTimer), new Vector2(0, 150), Color.White);
                    #endregion

                    //calling object draw methods
                    player.Draw(_spriteBatch, Color.White);

                    foreach (Bullet obj in boss.ProjectileList)
                    {
                        if (obj.HasHit)
                        {
                            player.Draw(_spriteBatch, Color.Red);
                        }
                    }
                    BossHealth.Draw(_spriteBatch);
                    PlayerHealth.Draw(_spriteBatch);

                    //drawing projectiles
                    boss.Draw(_spriteBatch, Color.White);
                    for(int i =0; i < boss.ProjectileList.Count; i++)
                    {
                        boss.ProjectileList[i].Draw(_spriteBatch, Color.White);
                        
                    }

                    for (int i = 0; i < player.ShotList.Count; i++)
                    {
                        player.ShotList[i].Draw(_spriteBatch, Color.White);

                    }                   
                    break;

                case GameState.GameOver:
                    _spriteBatch.Draw(gameOver, fullScreen, Color.White);
                    _spriteBatch.DrawString(font, "Press enter to return to Main menu", new Vector2(0, 0), Color.White);
                    
                    break;

                case GameState.Dialouge:
                    //draws object from dialgoe list using that object's dialogue method.
                    dialougeList[dialogueNum].Draw(_spriteBatch); // possible bug
                    
                    //_spriteBatch.DrawString(font, string.Format("{0}", currentLine), new Vector2(100, 500), Color.White);
                    break;
                  
                case GameState.Pause:
                    _spriteBatch.Draw(pauseScreen, fullScreen, Color.White);
                    break;

                case GameState.ScoreBoard:
                    //drawing final time on scoreboard
                    _spriteBatch.Draw(scoreBoard, fullScreen, Color.White);
                    SortedScores();
                    //_spriteBatch.DrawString(fontVN, name, new Vector2(650, 200), Color.White);
                    //_spriteBatch.DrawString(fontVN, String.Format("final time: {0:F} seconds", scoreTimer), new Vector2(1150, 200), Color.White);
                    dialogueNum = 0;
                    break;
            }

            _spriteBatch.End();
            base.Draw(gameTime);
        }


        #region Game content methods (scroeboard, VN, bosses, healthbars, helper methods)
        /// <summary>
        /// Helper method to check if a key has been pressed
        /// </summary>
        /// <param name="key"></param>
        /// <param name="kbState"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Loads in boss data (not from a file)
        /// </summary>
        /// <param name="randomChoice"></param>
        private void LoadBoss(int randomChoice)
        {
            //randomly choosing boss
            if (randomChoice == 1 && isRiceGoddessLoaded == false) 
            {

                int health = 1200; // Makes health based on first element of data
                int moveSpeed = 0; // Makes moveSpeed based on second element of data
                int attackSpeed = 10; // Makes attackSpeed based on third element of data
                int damage = 10; // Makes damage based on fourth element of data
                riceGoddess = new Boss(health, moveSpeed, attackSpeed, damage, riceGoddessTexture, new Rectangle(500, 500, 100, 100), takeDamadge, WindowHeight, WindowWidth, bossName.RiceGoddess, attackTextureRG, RGteleportTexture); // Makes Rice Goddess using data gathered from the file
                boss = riceGoddess;
                isRiceGoddessLoaded = true;
            }
            else if (randomChoice==1)
            {
                boss = riceGoddess;
            }

            if (randomChoice == 2 && isNagaBossLoaded == false) // Initializes each boss based on the current line in the file
            {

                int health = 800; // Makes health based on first element of data
                int moveSpeed = 0; // Makes moveSpeed based on second element of data
                int attackSpeed = 20; // Makes attackSpeed based on third element of data
                int damage = 10; // Makes damage based on fourth element of data
                nagaBoss = new Boss(health, moveSpeed, attackSpeed, damage, nagaBossTexture, new Rectangle(500, 500, 200, 200), takeDamadge, WindowHeight, WindowWidth, bossName.NagaBoss, attackTextureNA, NAteleportTexture); // Makes Naga Boss using data gathered from the file
                boss = nagaBoss;
                isNagaBossLoaded = true;
            }
            else if (randomChoice==2)
            {
                boss = nagaBoss;
            }

            if (boss != null)
            {
                boss.Center();
            }
            else
            {
                boss = riceGoddess;
            }

        }

        #region scoreboard methods
        /// <summary>
        /// Saves the current scoreboard information to a file so that it may be saved and loaded in later on
        /// </summary>
        private void SaveScoreboard()
        {
            // Create the variable here since we need it after the try
            StreamWriter output = null;
            try
            {
                LoadScoreboard();

                // When we open for writing, create the file if it doesn't exist yet
                output = new StreamWriter("../../../Scoreboard.txt");

                foreach (KeyValuePair<string, double> kvp in scoreBoardInfo) // For every entry in the scoreboard info dictionary
                {
                    output.Write($"{kvp.Key.ToString()},{kvp.Value.ToString()}"); // Save the name and time
                    output.WriteLine();
                }
                // Confirmation message
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine("scoreboard failed to load");
            }
            // Ensure that we can close the file, as long as it was actually opened in the first place
            if (output != null)
            {
                output.Close();
            }
        }

        /// <summary>
        /// Loads the scoreboard data in from a file so it may be used later on to display
        /// </summary>
        private void LoadScoreboard()
        {
            // Create the variable here since we need it after the try
            StreamReader input = null;
            try
            {
                // Creating the streamreader opens the file
                input = new StreamReader("../../../Scoreboard.txt");
                string line = "DEFAULT";
                // Loops through the file one line at a time
                while ((line = input.ReadLine()) != null)
                {
                    string[] data = line.Split(',');
                    scoreBoardInfo.Add(data[0], double.Parse(data[1]));
                }
                // Confirmation message
                System.Diagnostics.Debug.WriteLine($"Score board loaded successfully!");

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Score board load failed: {e.Message}");
            }
            // Ensure that we can close the file, as long as it was actually opened in the first place
            if (input != null)
            {
                input.Close();
            }
        }

        /// <summary>
        /// Ensures the dictionary holds the top 5 times and sorts them from lowest to highest
        /// </summary>
        private void SortedScores()
        {
            /*
             * if count > 5
	                loop through da dictionary
	                find highest time
	                remove that
	                continue until count = 5
               else
	                sort the dictionary!
	                return sorted dictionary
            */

            double maxValue = 0;
            double minValue = int.MaxValue;
            int x = 1150;
            int y = 200;
            int namex = 650;
            int namey = 200;
            
            
            if(!happened)
            {
                LoadScoreboard();
                scoreBoardInfo[name] = scoreTimer;
                happened = true;
            }
            

            if (scoreBoardInfo.Count > 5)
            {
                while(scoreBoardInfo.Count > 5)
                {
                    foreach (KeyValuePair<string, double> item in scoreBoardInfo)
                    {
                        //seeing which value is the highest
                        if (item.Value > maxValue)
                        {
                            maxValue = item.Value;
                        }
                    }

                    //removing the max value
                    foreach (KeyValuePair<string, double> item in scoreBoardInfo)
                    {
                        if (item.Value == maxValue)
                        {
                            scoreBoardInfo.Remove(item.Key);
                        }
                    }
                }
               

            }
            else
            {
                //sort the dictionary!!!!!!!!!!!!!
                Dictionary<string, double> sortedScoreBoardInfo = new Dictionary<string, double>();
                foreach (KeyValuePair<string, double> item in scoreBoardInfo)
                {
                    sortedScoreBoardInfo.Add(item.Key, item.Value);
                }

                //var ordered = scoreBoardInfo.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

                while(sortedScoreBoardInfo.Count > 0)
                {
                    //finding min value
                    foreach (KeyValuePair<string, double> currentItem in sortedScoreBoardInfo)
                    {
                        if (currentItem.Value <= minValue)
                        {
                            minValue = currentItem.Value;
                        }
                    }
                    //prints lowest value to the screen
                    foreach (KeyValuePair<string, double> currentItem in sortedScoreBoardInfo)
                    {
                        if (currentItem.Value == minValue)
                        {
                            _spriteBatch.DrawString(fontVN, currentItem.Key, new Vector2(namex, namey), Color.White);
                            _spriteBatch.DrawString(fontVN, String.Format("final time: {0:F} seconds", currentItem.Value), new Vector2(x, y), Color.White);

                            //removes lowest value so we can find the next lowest value
                            sortedScoreBoardInfo.Remove(currentItem.Key);
                            minValue = int.MaxValue;
                        }
                       
                    }
                    //formatting the name and timer
                    namey += 175;
                    y += 175;

                }
                
                
            }
        }
        #endregion

        /// <summary>
        /// Adds dialougue to list, after reading in textures type in text and true or false based on if the player is speaking, insert null to break to fight
        /// </summary>
        private void DialogueListAdd()
        {            
            switch (randomChoice)
            {
                
                case 1:
                    //Adds RiceGoddess dialougue
                    string BossName = "Rice Goddess";
                    Color BossColor = Color.Green;
                    #region rice goddess dialougue
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "All this life in a place of death, kinda ironic.", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "This is not a place of death, it is a place of rebirth.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName,"Who are you supposed to be?", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "I am the rice goddess and I'm acting as a substitute for now.", false));
                    dialougeList.Add(new Dialogue(playerVNAlternate, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "So you got stuck as a temp, because some other god decided to bail?", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNWise, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "Yes, even though it is a duty I would rather not wish to perform I must still take \n responsibility!", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNAlternate, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "Responsibility, which entails me dealing with souls like you!", false));
                    dialougeList.Add(null);
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "I think that's enough.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "Had enough?", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNAlternate, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "We can continue if you'd like.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNWise, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "I only stopped for your sake", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "No, I'm good.", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNDefault, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "I'll give you my blessing, but remember that this is your path.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, RGVNWise, RGbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "ALL the responsiblity for what happens next lies on your shoulders.", false));
                    dialougeList.Add(null);
                    #endregion
                    break;

                case 2:
                    //Adds naga dialogue
                    BossName = "Naga";
                    BossColor = Color.Orange;
                    #region naga dialogue
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNAlternate, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "It's hot as hell down here.", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "Some heat will do some good to warm up that cold heart of yours.", false));
                    dialougeList.Add(new Dialogue(playerVNAlternate, NAVNAlternate, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "Who are you calling cold, you snake...thing...whatever you are.", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "I am Naga, and you should really hold your tongue when asking for favors.", false));
                    dialougeList.Add(new Dialogue(playerVNAlternate, NAVNAlternate, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "I didn't ask you for anything yet, you slimy snake.", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "Yet, but you wouldn't be here if you weren't seeking my blessing.", false));
                    dialougeList.Add(new Dialogue(playerVNAlternate, NAVNAlternate, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "Just shut up and get on with it already, eel.", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNExasperated, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "I hate the stubborn ones.", false));
                    dialougeList.Add(null);
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNExasperated, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "...", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNAlternate, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "Well I won, where's the blessing?", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNMad, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "I'd never bless such an arrogant soul!", false));
                    dialougeList.Add(new Dialogue(playerVNAlternate, NAVNAlternate, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "No fair!", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNMad, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "What's not fair is for everyone to put up with your attitiude.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNMad, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "You're nothing but a pain in the tail!", false));
                    dialougeList.Add(new Dialogue(playerVNAlternate, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "...", true));
                    dialougeList.Add(new Dialogue(playerVNAlternate, NAVNAlternate, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "Sorry Naga.", true));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "See that wasn't so hard, recieve my blessing.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNDefault, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "As you progress, don't forget that true strength manifests itself from within.", false));
                    dialougeList.Add(new Dialogue(playerVNDefault, NAVNAlternate, NAbackgroundVN, textboxVN, textboxNameVN, fontVN, BossColor, BossName, "...", true));
                    dialougeList.Add(null);
                    #endregion
                    break;
            }
        }

        /// <summary>
        /// Loads all the data for the health bars
        /// </summary>
        public void LoadHealthBars()
        {
            //Makes the rectangles for player and boss HP
            Rectangle BossHPBar = new Rectangle(WindowWidth / 2 - WindowWidth / 4, WindowHeight / 10, 800, 50);
            Rectangle PlayerHPBar = new Rectangle(WindowWidth / 25, WindowHeight - WindowHeight / 20, 350, 50);

            //Loads Health bar for Theophania
            PlayerHealth = new HealthBar(textboxVN, bar, PlayerHPBar, new Vector2((WindowWidth / 10 - PlayerHPBar.Width / 4), WindowHeight - WindowHeight / 10), fontVN, "Theophania", player.MaxHealth);
            switch (randomChoice)
            {
                case 1:
                    //Loads bigger rice goddess health bar
                    BossHealth = new HealthBar(textboxVN, bar, BossHPBar, new Vector2(WindowWidth / 2 - BossHPBar.Width / 4, (WindowHeight / 10) - WindowHeight / 20), fontVN, "Rice Goddess", boss.MaxHealth);
                    boss.TeleportTexture = RGteleportTexture;
                    break;

                case 2:
                    //Loads smaller Naga health bar
                    BossHealth = new HealthBar(textboxVN, bar, BossHPBar, new Vector2(WindowWidth / 2 - BossHPBar.Width / 4, (WindowHeight / 10) - WindowHeight / 20), fontVN, "Naga", boss.MaxHealth);
                    boss.TeleportTexture = NAteleportTexture;
                    break;
            }
        }

        /// <summary>
        /// Gets user input for the input name section
        /// </summary>
        /// <returns><string>
        private string ScoreboardInput()
        {
            if(SingleKeyPress(Keys.A, kbState))
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
        #endregion

        //old load boss method (needs refinement)
        /*
/// <summary>
/// Loads in boss data from file
/// </summary>
private void LoadBoss(int randomChoice)
{
    StreamReader input = null;
    try
    {
        input = new StreamReader("../../../BossFile.txt"); // Opens a StreamReader specifically to the BossFile file
        string line = "DEFAULT";
        int lineNumber = 1; // The current line in the file, initialized at 1
        string[] data;
        while (line != null) // If there are still lines in the BossFile document
        {
            if(lineNumber == 1) // Initializes each boss based on the current line in the file
            {
                line = input.ReadLine();
                data = line.Split(',');
                int health = int.Parse(data[0]); // Makes health based on first element of data
                int moveSpeed = int.Parse(data[1]); // Makes moveSpeed based on second element of data
                int attackSpeed = int.Parse(data[2]); // Makes attackSpeed based on third element of data
                int damage = int.Parse(data[3]); // Makes damage based on fourth element of data
                riceGoddess = new Boss(health, moveSpeed, attackSpeed, damage, riceGoddessTexture, new Rectangle(500, 500, 100, 100), takeDamadge, WindowHeight, WindowWidth, bossName.RiceGoddess, attackTextureRG); // Makes Rice Goddess using data gathered from the file
            }
            if(lineNumber == 2) // Initializes each boss based on the current line in the file
            {
                line = input.ReadLine();
                data = line.Split(',');
                int health = int.Parse(data[0]); // Makes health based on first element of data
                int moveSpeed = int.Parse(data[1]); // Makes moveSpeed based on second element of data
                int attackSpeed = int.Parse(data[2]); // Makes attackSpeed based on third element of data
                int damage = int.Parse(data[3]); // Makes damage based on fourth element of data
                nagaBoss = new Boss(health, moveSpeed, attackSpeed, damage, nagaBossTexture, new Rectangle(500, 500, 200, 200), takeDamadge, WindowHeight, WindowWidth, bossName.NagaBoss, attackTextureNA); // Makes Naga Boss using data gathered from the file
                line = input.ReadLine();
            }
            lineNumber++;
        }
    }
    catch (Exception e)
    {

    }
    //temporarily set so only the naga will appear
    if (randomChoice == 1) // If it randomly chooses to load the Rice Goddess
    {
        if (!isRiceGoddessLoaded) // If the Rice Goddess hasn't been loaded in this game yet
        {
            isRiceGoddessLoaded = true; // Changes bool so that Rice Goddess has been loaded in this game
            boss = riceGoddess; // Makes the current boss the Rice Goddess
        }
    }
    if (randomChoice == 2) // If it randomly chooses to load the Naga boss
    {
        if (!isNagaBossLoaded) // If the Naga boss hasn't been loaded in this game yet
        {
            isNagaBossLoaded = true; // Changes bool so that Naga boss has been loaded in this game
            boss = nagaBoss; // Makes the current boss the Naga Boss
        }
    }
    if (boss != null)
    {
        boss.Center();
    }

}
*/

    }
}
