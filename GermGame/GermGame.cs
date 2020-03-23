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

namespace GermGame
{

    public class GermGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // declaring variables to do with background images and size
        Rectangle backgroundRectangle;
        Texture2D background;
        Texture2D menuBackground;
        Texture2D instructionsBackground;
        Texture2D highScoreBackground;
        int screenWidth;
        int screenHeight;

        // declaring variables to do with movement and animation of sprite
        Texture2D spriteSheet;
        Vector2 spriteMovement;

        // declaring variables of text at the top of gameplay
        SpriteFont textIndicator;
        Vector2[] textIndicatorVec;
        String[] textIndicatorString;

        // declaring variables of lives, score, final score and time running boolean
        int lives = 3;
        int score = 0;
        double finalScore = 0;
        bool timeRunning = true;

        int level = 1;
        TimeSpan screenStartTime = TimeSpan.Zero;

        // declaring scoreboard arrays
        Double[] scoreboard;
        Vector2[] scoreboardPosition;
        Int32[] scoreboardRank;

        // declaring time variable and time after death
        TimeSpan time;
        TimeSpan timeAfterDeath = TimeSpan.Zero;

        // declaring enemy textures
        Texture2D enemyPinkOne;
        Texture2D enemyPurpleOne;
        Texture2D enemyGreenOne;

        // declaring pink enemy arrays
        Vector2[] enemyPinkMovement;
        Rectangle[] enemyPinkRec;
        Boolean[] boolPink;
        SpriteEffects[] spriteFXPink;

        // declaring purple enemy arrays
        Vector2[] enemyPurpleMovement;
        Rectangle[] enemyPurpleRec;
        Boolean[] boolPurple;
        SpriteEffects[] spriteFXPurple;

        // declaring green enemy arrays
        Vector2[] enemyGreenMovement;
        Rectangle[] enemyGreenRec;
        Boolean[] boolGreen;
        SpriteEffects[] spriteFXGreen;

        // declaring enemy start position array
        Boolean[] enemyStartPos;

        // declaring died and finished variables
        bool hasDied = false;
        bool hasFinished = false;

        // declaring facing direction variables
        bool facingRight = true;
        bool facingLeft = false;

        // declaring restart and has just started variables
        bool restart = false;
        bool hasJustStarted = true;

        // declaring popup variables
        Texture2D popup;
        Rectangle popupRec;
        Vector2[] popupText;
        SpriteFont popupTitle;
        String[] popupString;
        SpriteFont smallFont;

        // declaring instructions screen fonts
        SpriteFont instructionsFont;
        SpriteFont instructionsFontTitle;
        SpriteFont keyFont;

        // declaring collision variables
        Rectangle spriteRectangle;
        Rectangle spriteCollision;

        // declaring platform array
        Rectangle[] platforms;

        // declaring finish door variable
        Rectangle finishDoor;

        // declaring screenstate and which it will start on
        ScreenState currentScreen = ScreenState.Start;

        // declaring menu fonts, text and vectors
        SpriteFont menuTitle;
        SpriteFont menuButton;
        String[] menuButtonText;
        Vector2[] menuButtonVectors;
        Rectangle[] menuButtons;

        // declaring mouse rectangle
        Rectangle mouseRectangle;

        // declaring animation state
        State currentState = State.Standing;

        // declaring sprite selection variable
        Rectangle spriteSelection;

        // declaring timespans for walking animation
        TimeSpan timeWalk = TimeSpan.Zero;
        TimeSpan timeWalkTwo = TimeSpan.Zero;

        // declaring variables for jumping
        Boolean allowJump = true;
        int jump = 12;

        // declaring variables for firing time
        TimeSpan timeFiring = TimeSpan.Zero;
        bool canFire = true;

        // graphical element - declaring boolean which when true asks user if they are sure they want to quit
        bool exitDialog = false;
        Rectangle yesRec;
        Rectangle noRec;

        public GermGame()
        {
            graphics = new GraphicsDeviceManager(this);

            // setting the height and width of the window
            this.graphics.PreferredBackBufferWidth = 1024;
            this.graphics.PreferredBackBufferHeight = 768;

            Content.RootDirectory = "Content";
        }

        // setting screen states
        enum ScreenState
        {
            Start,
            Instructions,
            Gameplay,
            HighScores
        }

        // setting walking states
        enum State
        {
            Standing,
            WalkingRight,
            WalkingLeft,
            WalkingRightAndFiring,
            WalkingLeftAndFiring,
            FiringRight,
            FiringLeft,
            Jumping,
            Falling
        }


        protected override void Initialize()
        {
            // setting platform array
            platforms = new Rectangle[15];

            // setting text indicator array
            textIndicatorString = new String[3];
            textIndicatorVec = new Vector2[3];

            // setting text indicator vector points
            textIndicatorVec[0] = new Vector2(8, 8);
            textIndicatorVec[1] = new Vector2(668, 8);
            textIndicatorVec[2] = new Vector2(850, 8);

            // setting enemy boolean array
            boolPink = new Boolean[4];
            boolPurple = new Boolean[3];
            boolGreen = new Boolean[3];

            // setting enemy movement array
            enemyPinkMovement = new Vector2[4];
            enemyPurpleMovement = new Vector2[3];
            enemyGreenMovement = new Vector2[3];

            // setting enemy rectangle array
            enemyPinkRec = new Rectangle[4];
            enemyPurpleRec = new Rectangle[3];
            enemyGreenRec = new Rectangle[3];

            // setting enemy spriteeffects array
            spriteFXPink = new SpriteEffects[4];
            spriteFXPurple = new SpriteEffects[3];
            spriteFXGreen = new SpriteEffects[3];

            // assigning pink enemy movement array vectors
            enemyPinkMovement[0] = new Vector2(40, 476);
            enemyPinkMovement[1] = new Vector2(636, 476);
            enemyPinkMovement[2] = new Vector2(205, 188);
            enemyPinkMovement[3] = new Vector2(352, 134);

            // assigning purple enemy movement array vectors
            enemyPurpleMovement[0] = new Vector2(224, 391);
            enemyPurpleMovement[1] = new Vector2(118, 94);
            enemyPurpleMovement[2] = new Vector2(780, 178);

            // assigning green enemy movement array vectors
            enemyGreenMovement[0] = new Vector2(446, 333);
            enemyGreenMovement[1] = new Vector2(24, 211);
            enemyGreenMovement[2] = new Vector2(684, 138);

            // setting enemy start position array
            enemyStartPos = new Boolean[10];

            // loop which assigns enemy starting position to true
            for (int loop = 0; loop < 10; loop++)
            {
                enemyStartPos[loop] = true;
            }

            // assigning rectangle points to all platforms and finish door
            platforms[0] = new Rectangle(434, 399, 95, 141); //leftFridge
            platforms[1] = new Rectangle(529, 399, 73, 141); // rightFridge
            platforms[2] = new Rectangle((int)467, (int)337, 112, 7); //fridgePlatform
            platforms[3] = new Rectangle((int)0, (int)480, 474, 13); //leftBottomPlatform
            platforms[4] = new Rectangle((int)602, (int)480, 432, 13); //rightBottomPlatform
            platforms[5] = new Rectangle((int)185, (int)426, 25, 1); //leftFirstPlatform
            platforms[6] = new Rectangle((int)256, (int)392, 145, 7); //leftSecondPlatform
            platforms[7] = new Rectangle((int)640, (int)275, 32, 7); //clock
            platforms[8] = new Rectangle((int)717, (int)237, 143, 13); //cupboard
            platforms[9] = new Rectangle((int)810, (int)177, 204, 12); //topRightPlatform
            platforms[10] = new Rectangle((int)355, (int)138, 396, 12); //topMiddlePlatform
            platforms[11] = new Rectangle((int)142, (int)93, 204, 12); //topMiddlePlatform
            platforms[12] = new Rectangle((int)211, (int)192, 122, 12); //window
            platforms[13] = new Rectangle((int)141, (int)237, 45, 6); //pictureRight
            platforms[14] = new Rectangle((int)58, (int)211, 45, 6); //pictureLeft

            finishDoor = new Rectangle((int)155, (int)94, 38, 12); // finishDoor

            // assigning rectangle points
            popupRec = new Rectangle((int)311, 216, 420, 226);

            // assigning text array and assigning vector points
            popupText = new Vector2[6];
            popupText[0] = new Vector2(326, 226);
            popupText[1] = new Vector2(345, 285);
            popupText[2] = new Vector2(345, 321);
            popupText[3] = new Vector2(345, 357);
            popupText[4] = new Vector2(345, 393);
            popupText[5] = new Vector2(382, 420);

            // assigning string arrays
            popupString = new String[6];
            menuButtonText = new String[4];

            // assigning menu button vector and rectangle arrays
            menuButtonVectors = new Vector2[4];
            menuButtons = new Rectangle[4];

            // setting menu button vector points for text
            menuButtonVectors[0] = new Vector2(430, 200);
            menuButtonVectors[1] = new Vector2(342, 325);
            menuButtonVectors[2] = new Vector2(356, 453);
            menuButtonVectors[3] = new Vector2(455, 573);

            // setting menu button rectangle sizes
            menuButtons[0] = new Rectangle(332, 177, 343, 93);
            menuButtons[1] = new Rectangle(332, 303, 343, 93);
            menuButtons[2] = new Rectangle(332, 430, 343, 93);
            menuButtons[3] = new Rectangle(332, 550, 343, 93);

            // declaring scoreboard arrays
            scoreboard = new double[10];
            scoreboardPosition = new Vector2[10];
            scoreboardRank = new int[10];

            // assigning all scoreboard scores to 0 at the stat
            for (int loop = 0; loop < 9; loop++)
            {
                scoreboard[loop] = 0;
            }

            // showing all pink enemies
            for (int loop = 0; loop <= 3; loop++)
            {
                boolPink[loop] = true;
            }

            // showing all purple and green enemies
            for (int loop = 0; loop <= 2; loop++)
            {
                boolPurple[loop] = true;
                boolGreen[loop] = true;
            }

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // loading all textures to do with the background
            menuBackground = Content.Load<Texture2D>("images/menubg");
            background = Content.Load<Texture2D>("images/bg");
            highScoreBackground = Content.Load<Texture2D>("images/highscorebg");
            instructionsBackground = Content.Load<Texture2D>("images/instructionsbg");
            screenWidth = graphics.GraphicsDevice.Viewport.Width;
            screenHeight = graphics.GraphicsDevice.Viewport.Height;

            // loading spritesheet and starting sprite selection
            spriteSheet = Content.Load<Texture2D>("images/spritesheet");
            spriteSelection = new Rectangle(0, 0, 50, 60);

            // loading enemy textures
            enemyPinkOne = Content.Load<Texture2D>("images/enemy1");
            enemyPurpleOne = Content.Load<Texture2D>("images/enemy2");
            enemyGreenOne = Content.Load<Texture2D>("images/enemy3");

            // loading popup texture
            popup = Content.Load<Texture2D>("images/popup");

            // loading gameplay fonts
            textIndicator = Content.Load<SpriteFont>("font/font");
            popupTitle = Content.Load<SpriteFont>("font/popUpFont");
            smallFont = Content.Load<SpriteFont>("font/smallFont");

            // loading menu fonts
            menuTitle = Content.Load<SpriteFont>("font/menuTitle");
            menuButton = Content.Load<SpriteFont>("font/menuButton");

            // loading instruction fonts
            instructionsFont = Content.Load<SpriteFont>("font/instructionsFont");
            instructionsFontTitle = Content.Load<SpriteFont>("font/instructionsFontTitle");
            keyFont = Content.Load<SpriteFont>("font/keyFont");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // importing methods into update, as movement uses time gameTime is required
            Movement(gameTime);
            Level();
            Collision();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            // importing methods into draw, as player and 'timescoreandlives' uses time gameTime is required
            Background(gameTime);
            Menu();
            Player(gameTime);
            Enemies();
            TimeScoreAndLives(gameTime);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected void Background(GameTime gameTime)
        {
            bool screenStart = true;


            // making rectangle the size of the screen
            backgroundRectangle = new Rectangle(0, 0, screenWidth, screenHeight);

            // if the menu shows draw the menu background
            if (currentScreen == ScreenState.Start)
            {
                spriteBatch.Draw(menuBackground, backgroundRectangle, Color.White);
            }

            // if the gameplay screen is showing draw the gameplay background
            if (currentScreen == ScreenState.Gameplay)
            {
                spriteBatch.Draw(background, backgroundRectangle, Color.White);
            }
            screenStartTime += gameTime.ElapsedGameTime;
            if (currentScreen == ScreenState.Gameplay && screenStart == true && screenStartTime < TimeSpan.FromMilliseconds(2000))
            {
                spriteBatch.DrawString(textIndicator, "LEVEL " + level, new Vector2(screenWidth / 2 - 45, screenHeight / 2 - 100), Color.Black);
                if (screenStartTime > TimeSpan.FromMilliseconds(2000))
                {
                    screenStart = false;
                    screenStartTime = TimeSpan.Zero;
                }
            }

        }

        protected void Player(GameTime gameTime)
        {
            // if the gameplay screen is showing
            if (currentScreen == ScreenState.Gameplay)
            {
                // if user has just started move sprite into correct location
                if (hasJustStarted == true)
                {
                    spriteMovement.X = 10.0f;
                    spriteMovement.Y = 480.0f;
                    hasJustStarted = false;
                }

                // declaring local variables
                int middleOfRec = 0;
                int middleOfCol = 0;
                int recWidth = 0;
                int colWidth = 0;

                // sort collision rectangle area so it collides correctly
                // also the firing rectangle is moved 15 pixels left so they fire in the correct direction
                if (facingLeft == true)
                {
                    middleOfRec = (int)spriteMovement.X - 15;
                    recWidth = 36;
                    middleOfCol = (int)spriteMovement.X + 21;
                    colWidth = 29;
                }
                // sort collision rectangle area so it collides correctly
                // also the firing rectangle is moved 15 pixels right so they fire in the correct direction
                if (facingRight == true)
                {
                    middleOfRec = (int)spriteMovement.X + 29;
                    recWidth = 36;
                    middleOfCol = (int)spriteMovement.X;
                    colWidth = 29;
                }

                // if user has died reset their position. Also because when the player is being moved back it can
                // collide in the process so they can lose another life so they die until 300 milliseconds after being
                // put back in the correct position
                if (hasDied == true)
                {
                    restart = true;
                    spriteMovement.X = 10;
                    spriteMovement.Y = 480;

                    timeAfterDeath += gameTime.ElapsedGameTime;

                    if (timeAfterDeath > TimeSpan.FromMilliseconds(300))
                    {
                        hasDied = false;
                        timeAfterDeath = TimeSpan.Zero;
                    }
                }

                // draw sprite sheet and rectangles
                spriteBatch.Draw(spriteSheet, spriteMovement, spriteSelection, Color.White);
                spriteCollision = new Rectangle((int)middleOfCol, (int)spriteMovement.Y, colWidth, 60);
                spriteRectangle = new Rectangle((int)middleOfRec, (int)spriteMovement.Y, recWidth, 60);
            }
        }

        protected void Movement(GameTime gameTime)
        {
            // shorten keyboard get state code
            KeyboardState keys = Keyboard.GetState();

            #region Time
            // if the current screen is gameplay run the time
            if (currentScreen == ScreenState.Gameplay)
            {
                if (timeRunning == true)
                {
                    time += gameTime.ElapsedGameTime;
                }
                else
                {
                }
            }
            #endregion

            #region WalkingRight
            // if the user is walking right
            if ((keys.IsKeyDown(Keys.Right)) && (keys.IsKeyUp(Keys.Space)))
            {
                // move user right
                spriteMovement.X += 3;
                facingRight = true;
                facingLeft = false;

                currentState = State.WalkingRight;

                if (currentState == State.WalkingRight)
                {
                    // change sprite selection
                    spriteSelection = new Rectangle(50, 0, 50, 60);

                    // start timer one
                    timeWalk += gameTime.ElapsedGameTime;

                    // if timer one is more than 200 milliseconds
                    if (timeWalk > TimeSpan.FromMilliseconds(200))
                    {
                        // change back sprite selection
                        spriteSelection = new Rectangle(0, 0, 50, 60);
                        // start timer two
                        timeWalkTwo += gameTime.ElapsedGameTime;

                        // if timer two is more than 200 milliseconds
                        if (timeWalkTwo > TimeSpan.FromMilliseconds(200))
                        {
                            // change sprite selection again
                            spriteSelection = new Rectangle(50, 0, 50, 60);
                            // reset both timers so it can repeat
                            timeWalkTwo = TimeSpan.Zero;
                            timeWalk = TimeSpan.Zero;
                        }
                    }

                }
            }
            #endregion

            #region WalkingLeft
            // if the user is walking left
            if ((keys.IsKeyDown(Keys.Left)) && (keys.IsKeyUp(Keys.Space)))
            {
                // move user left
                spriteMovement.X += -3;
                facingLeft = true;
                facingRight = false;

                currentState = State.WalkingLeft;

                if (currentState == State.WalkingLeft)
                {
                    // change sprite selection
                    spriteSelection = new Rectangle(0, 62, 50, 60);
                    // start timer one
                    timeWalk += gameTime.ElapsedGameTime;

                    // if timer one is more than 200 milliseconds
                    if (timeWalk > TimeSpan.FromMilliseconds(200))
                    {
                        // change back sprite selection
                        spriteSelection = new Rectangle(50, 62, 50, 60);
                        // start timer two
                        timeWalkTwo += gameTime.ElapsedGameTime;

                        // if timer two is more than 200 milliseconds
                        if (timeWalkTwo > TimeSpan.FromMilliseconds(200))
                        {
                            // change sprite selection again
                            spriteSelection = new Rectangle(0, 62, 50, 60);
                            // reset both timers so it can repeat
                            timeWalkTwo = TimeSpan.Zero;
                            timeWalk = TimeSpan.Zero;
                        }
                    }

                }
            }
            #endregion

            #region WalkingRightAndFiring
            // if the user is walking right and firing
            if ((keys.IsKeyDown(Keys.Right)) && (keys.IsKeyDown(Keys.Space)))
            {
                // move user right
                spriteMovement.X += 3;

                if (canFire == true) // if you havnt pressed fire for too long you can fire
                {
                    currentState = State.WalkingRightAndFiring;

                    if (currentState == State.WalkingRightAndFiring)
                    {
                        // change sprite selection
                        spriteSelection = new Rectangle(100, 0, 50, 60);
                        // start timer one
                        timeWalk += gameTime.ElapsedGameTime;

                        // if timer one is more than 200 milliseconds
                        if (timeWalk > TimeSpan.FromMilliseconds(200))
                        {
                            // change back sprite selection
                            spriteSelection = new Rectangle(150, 0, 50, 60);
                            // start timer two
                            timeWalkTwo += gameTime.ElapsedGameTime;

                            // if timer two is more than 200 milliseconds
                            if (timeWalkTwo > TimeSpan.FromMilliseconds(200))
                            {
                                // change sprite selection again
                                spriteSelection = new Rectangle(100, 0, 50, 60);
                                // reset both timers so it can repeat
                                timeWalkTwo = TimeSpan.Zero;
                                timeWalk = TimeSpan.Zero;
                            }
                        }

                    }
                }
                else // else you cant fire and will only walk
                {
                    facingRight = true;
                    facingLeft = false;

                    currentState = State.WalkingRight;

                    if (currentState == State.WalkingRight)
                    {
                        // change sprite selection
                        spriteSelection = new Rectangle(50, 0, 50, 60);

                        // start timer one
                        timeWalk += gameTime.ElapsedGameTime;

                        // if timer one is more than 200 milliseconds
                        if (timeWalk > TimeSpan.FromMilliseconds(200))
                        {
                            // change back sprite selection
                            spriteSelection = new Rectangle(0, 0, 50, 60);
                            // start timer two
                            timeWalkTwo += gameTime.ElapsedGameTime;

                            // if timer two is more than 200 milliseconds
                            if (timeWalkTwo > TimeSpan.FromMilliseconds(200))
                            {
                                // change sprite selection again
                                spriteSelection = new Rectangle(50, 0, 50, 60);
                                // reset both timers so it can repeat
                                timeWalkTwo = TimeSpan.Zero;
                                timeWalk = TimeSpan.Zero;
                            }
                        }

                    }
                }
            }
            #endregion

            #region WalkingLeftAndFiring
            // if the user is walking left and firing
            if ((keys.IsKeyDown(Keys.Left)) && (keys.IsKeyDown(Keys.Space)))
            {
                // move user left
                spriteMovement.X += -3;

                if (canFire == true) // if you havnt pressed fire for too long you can fire
                {
                    currentState = State.WalkingLeftAndFiring;

                    if (currentState == State.WalkingLeftAndFiring)
                    {
                        // change sprite selection
                        spriteSelection = new Rectangle(100, 62, 50, 60);
                        // start timer one
                        timeWalk += gameTime.ElapsedGameTime;
                        // if timer one is more than 200 milliseconds
                        if (timeWalk > TimeSpan.FromMilliseconds(200))
                        {
                            // change back sprite selection
                            spriteSelection = new Rectangle(150, 62, 50, 60);
                            // start timer two
                            timeWalkTwo += gameTime.ElapsedGameTime;

                            // if timer two is more than 200 milliseconds
                            if (timeWalkTwo > TimeSpan.FromMilliseconds(200))
                            {
                                // change sprite selection again
                                spriteSelection = new Rectangle(100, 62, 50, 60);
                                // reset both timers so it can repeat
                                timeWalkTwo = TimeSpan.Zero;
                                timeWalk = TimeSpan.Zero;
                            }
                        }

                    }
                }
                else // else you cant and will only walk
                {
                    facingLeft = true;
                    facingRight = false;

                    currentState = State.WalkingLeft;

                    if (currentState == State.WalkingLeft)
                    {
                        // change sprite selection
                        spriteSelection = new Rectangle(0, 62, 50, 60);
                        // start timer one
                        timeWalk += gameTime.ElapsedGameTime;

                        // if timer one is more than 200 milliseconds
                        if (timeWalk > TimeSpan.FromMilliseconds(200))
                        {
                            // change back sprite selection
                            spriteSelection = new Rectangle(50, 62, 50, 60);
                            // start timer two
                            timeWalkTwo += gameTime.ElapsedGameTime;

                            // if timer two is more than 200 milliseconds
                            if (timeWalkTwo > TimeSpan.FromMilliseconds(200))
                            {
                                // change sprite selection again
                                spriteSelection = new Rectangle(0, 62, 50, 60);
                                // reset both timers so it can repeat
                                timeWalkTwo = TimeSpan.Zero;
                                timeWalk = TimeSpan.Zero;
                            }
                        }

                    }
                }
            }
            #endregion

            #region Other stuff

            // if right is not being pressed but was the last key that was pressed switch to the correct sprite selection
            if ((keys.IsKeyUp(Keys.Right)) && (facingRight == true))
            {
                spriteSelection = new Rectangle(0, 0, 50, 60);
            }
            // if left is not being pressed but was the last key that was pressed switch to the correct sprite selection
            if ((keys.IsKeyUp(Keys.Left)) && (facingLeft == true))
            {
                spriteSelection = new Rectangle(0, 62, 50, 60);
            }
            // if left and right is being pressed at the same time face right to avoid glitches
            if ((keys.IsKeyDown(Keys.Left)) && (keys.IsKeyDown(Keys.Right)))
            {
                spriteSelection = new Rectangle(0, 0, 50, 60);
            }
            // if left, right and space is being pressed depending on which way you are facing will depend on what sprite selection shows
            if ((keys.IsKeyDown(Keys.Left)) && (keys.IsKeyDown(Keys.Right) && (keys.IsKeyDown(Keys.Space))))
            {
                if (facingLeft == true)
                {
                    spriteSelection = new Rectangle(0, 62, 50, 60);
                }
                if (facingRight == true)
                {
                    spriteSelection = new Rectangle(0, 0, 50, 60);
                }
            }
            #endregion

            #region StandingAndFiringRight

            // if the user is standing and firing right
            if ((keys.IsKeyDown(Keys.Space)) && (keys.IsKeyUp(Keys.Right)) && (facingRight == true) && canFire == true)
            {
                currentState = State.Standing;

                if (currentState == State.Standing)
                {
                    // change sprite selection
                    spriteSelection = new Rectangle(100, 0, 50, 60);
                    // start timer one
                    timeWalk += gameTime.ElapsedGameTime;

                    // if timer one is more than 200 milliseconds
                    if (timeWalk > TimeSpan.FromMilliseconds(200))
                    {
                        // change back sprite selection
                        spriteSelection = new Rectangle(0, 0, 50, 60);
                        // start timer two
                        timeWalkTwo += gameTime.ElapsedGameTime;

                        // if timer two is more than 200 milliseconds
                        if (timeWalkTwo > TimeSpan.FromMilliseconds(200))
                        {
                            // change sprite selection again
                            spriteSelection = new Rectangle(100, 0, 50, 60);
                            // reset both timers so it can repeat
                            timeWalkTwo = TimeSpan.Zero;
                            timeWalk = TimeSpan.Zero;
                        }
                    }

                }
            }
            #endregion

            #region StandingAndFiringLeft
            // if the user is standing and firing left
            if ((keys.IsKeyDown(Keys.Space)) && (keys.IsKeyUp(Keys.Left)) && (facingLeft == true) && canFire == true)
            {
                currentState = State.Standing;

                if (currentState == State.Standing)
                {
                    // change sprite selection
                    spriteSelection = new Rectangle(100, 62, 50, 60);
                    // start timer one
                    timeWalk += gameTime.ElapsedGameTime;

                    // if timer one is more than 200 milliseconds
                    if (timeWalk > TimeSpan.FromMilliseconds(200))
                    {
                        // change back sprite selection
                        spriteSelection = new Rectangle(0, 62, 50, 60);
                        // start timer two
                        timeWalkTwo += gameTime.ElapsedGameTime;

                        // if timer two is more than 200 milliseconds
                        if (timeWalkTwo > TimeSpan.FromMilliseconds(200))
                        {
                            // change sprite selection again
                            spriteSelection = new Rectangle(100, 62, 50, 60);
                            // reset both timers so it can repeat
                            timeWalkTwo = TimeSpan.Zero;
                            timeWalk = TimeSpan.Zero;
                        }
                    }

                }
            }
            #endregion

            #region PinkEnemyMovement
            // if pink enemy is at the starting position
            if (enemyStartPos[0] == true)
            {
                // move right and flip horizontally
                enemyPinkMovement[0].X += 2;
                spriteFXPink[0] = SpriteEffects.FlipHorizontally;

                // if they have reached the end
                if (enemyPinkMovement[0].X == 200)
                {
                    enemyStartPos[0] = false;
                }

            }
            else
            {
                // move left and don't flip
                enemyPinkMovement[0].X -= 2;
                spriteFXPink[0] = SpriteEffects.None;

                // if they have reached the start
                if (enemyPinkMovement[0].X == 40)
                {
                    enemyStartPos[0] = true;
                }
            }
            // if pink enemy is at the starting position
            if (enemyStartPos[1] == true)
            {
                // move right and flip horizontally
                enemyPinkMovement[1].X += 2;
                spriteFXPink[1] = SpriteEffects.FlipHorizontally;

                // if they have reached the end
                if (enemyPinkMovement[1].X == 942)
                {
                    enemyStartPos[1] = false;
                }

            }
            else
            {
                // move left and don't flip
                enemyPinkMovement[1].X -= 2;
                spriteFXPink[1] = SpriteEffects.None;

                // if they have reached the start
                if (enemyPinkMovement[1].X == 636)
                {
                    enemyStartPos[1] = true;
                }
            }

            // if pink enemy is at the starting position
            if (enemyStartPos[2] == true)
            {
                // move right and flip horizontally
                enemyPinkMovement[2].X += 1;
                spriteFXPink[2] = SpriteEffects.FlipHorizontally;

                // if they have reached the end
                if (enemyPinkMovement[2].X == 280)
                {
                    enemyStartPos[2] = false;
                }

            }
            else
            {
                // move left and don't flip
                enemyPinkMovement[2].X -= 1;
                spriteFXPink[2] = SpriteEffects.None;

                // if they have reached the start
                if (enemyPinkMovement[2].X == 205)
                {
                    enemyStartPos[2] = true;
                }
            }

            // if pink enemy is at the starting position
            if (enemyStartPos[3] == true)
            {
                // move right and flip horizontally
                enemyPinkMovement[3].X += 2;
                spriteFXPink[3] = SpriteEffects.FlipHorizontally;

                // if they have reached the end
                if (enemyPinkMovement[3].X == 684)
                {
                    enemyStartPos[3] = false;
                }

            }
            else
            {
                // move left and don't flip
                enemyPinkMovement[3].X -= 2;
                spriteFXPink[3] = SpriteEffects.None;

                // if they have reached the start
                if (enemyPinkMovement[3].X == 352)
                {
                    enemyStartPos[3] = true;
                }
            }
            #endregion

            #region PurpleEnemyMovement

            // if purple enemy is at the starting position
            if (enemyStartPos[4] == true)
            {
                // move right and flip horizontally
                enemyPurpleMovement[0].X += 2;
                spriteFXPurple[0] = SpriteEffects.FlipHorizontally;

                // if they have reached the end
                if (enemyPurpleMovement[0].X == 350)
                {
                    enemyStartPos[4] = false;
                }

            }
            else
            {
                // move left and don't flip
                enemyPurpleMovement[0].X -= 2;
                spriteFXPurple[0] = SpriteEffects.None;


                // if they have reached the start
                if (enemyPurpleMovement[0].X == 224)
                {
                    enemyStartPos[4] = true;
                }
            }

            // if purple enemy is at the starting position
            if (enemyStartPos[5] == true)
            {
                // move right and flip horizontally
                enemyPurpleMovement[1].X += 2;
                spriteFXPurple[1] = SpriteEffects.FlipHorizontally;

                // if they have reached the end
                if (enemyPurpleMovement[1].X == 280)
                {
                    enemyStartPos[5] = false;
                }

            }
            else
            {
                // move left and don't flip
                enemyPurpleMovement[1].X -= 2;
                spriteFXPurple[1] = SpriteEffects.None;

                // if they have reached the start
                if (enemyPurpleMovement[1].X == 118)
                {
                    enemyStartPos[5] = true;
                }
            }

            // if purple enemy is at the starting position
            if (enemyStartPos[6] == true)
            {
                // move right and flip horizontally
                enemyPurpleMovement[2].X += 2;
                spriteFXPurple[2] = SpriteEffects.FlipHorizontally;

                // if they have reached the end
                if (enemyPurpleMovement[2].X == 950)
                {
                    enemyStartPos[6] = false;
                }

            }
            else
            {
                // move left and don't flip
                enemyPurpleMovement[2].X -= 2;
                spriteFXPurple[2] = SpriteEffects.None;

                // if they have reached the start
                if (enemyPurpleMovement[2].X == 780)
                {
                    enemyStartPos[6] = true;
                }
            }

            #endregion

            #region GreenEnemyMovement

            // if green enemy is at the starting position
            if (enemyStartPos[7] == true)
            {
                // move right and flip horizontally
                enemyGreenMovement[0].X += 2;
                spriteFXGreen[0] = SpriteEffects.FlipHorizontally;

                // if they have reached the end
                if (enemyGreenMovement[0].X == 542)
                {
                    enemyStartPos[7] = false;
                }

            }
            else
            {
                // move left and don't flip
                enemyGreenMovement[0].X -= 2;
                spriteFXGreen[0] = SpriteEffects.None;

                // if they have reached the start
                if (enemyGreenMovement[0].X == 446)
                {
                    enemyStartPos[7] = true;
                }
            }

            // if green enemy is at the starting position
            if (enemyStartPos[8] == true)
            {
                // move right and flip horizontally
                enemyGreenMovement[1].X += 1;
                spriteFXGreen[1] = SpriteEffects.FlipHorizontally;

                // if they have reached the end
                if (enemyGreenMovement[1].X == 58)
                {
                    enemyStartPos[8] = false;
                }

            }
            else
            {
                // move left and don't flip
                enemyGreenMovement[1].X -= 1;
                spriteFXGreen[1] = SpriteEffects.None;

                // if they have reached the start
                if (enemyGreenMovement[1].X == 24)
                {
                    enemyStartPos[8] = true;
                }
            }

            // if green enemy is at the starting position
            if (enemyStartPos[9] == true)
            {
                // move right and flip horizontally
                enemyGreenMovement[2].X -= 2;
                spriteFXGreen[2] = SpriteEffects.None;

                // if they have reached the end
                if (enemyGreenMovement[2].X == 352)
                {
                    enemyStartPos[9] = false;
                }

            }
            else
            {
                // move left and don't flip
                enemyGreenMovement[2].X += 2;
                spriteFXGreen[2] = SpriteEffects.FlipHorizontally;

                // if they have reached the start
                if (enemyGreenMovement[2].X == 684)
                {
                    enemyStartPos[9] = true;
                }
            }

            #endregion

            #region hasFinished

            // if the user presses R when the game has finished values will be reset
            if (keys.IsKeyDown(Keys.R) && hasFinished == true)
            {
                spriteSelection = new Rectangle(0, 0, 50, 60);
                time = TimeSpan.Zero;
                restart = true;
                timeRunning = true;
                spriteMovement.X = 10;
                spriteMovement.Y = 460;
                hasDied = false;
                hasFinished = false;
                lives = 3;
                score = 0;

                for (int loop = 0; loop <= 3; loop++)
                {
                    boolPink[loop] = true;
                }
                for (int loop = 0; loop <= 2; loop++)
                {
                    boolPurple[loop] = true;
                    boolGreen[loop] = true;
                }
            }

            #endregion

            #region Jump

            if (keys.IsKeyDown(Keys.Up) && allowJump == true)
            {
                spriteMovement.Y -= jump;

                if (jump > -12)
                {
                    jump = jump - 1;
                }
            }
            else { spriteMovement.Y += 5; }
            if (keys.IsKeyUp(Keys.Up))
            {
                spriteMovement.Y += 5;
                if (jump < 12)
                {
                    allowJump = false;
                    jump += 1;
                }
                else { allowJump = true; }
            }
            #endregion


            #region If user holds fire for too long

            // if the user presses space and can fire a timer will start, when that timer reaches 1 second, they cannot fire
            if (keys.IsKeyDown(Keys.Space) && canFire == true)
            {
                timeFiring += gameTime.ElapsedGameTime;

                if (timeFiring > TimeSpan.FromMilliseconds(1000))
                {
                    canFire = false;
                }
                else
                {
                    canFire = true;
                }
            }

            // when space is up, the user can re-fire and the timer is reset back to zero
            if (keys.IsKeyUp(Keys.Space) && canFire == false)
            {
                canFire = true;
                timeFiring = TimeSpan.Zero;
            }
            #endregion

            #region Restart user facing right

            // if the user is facing left when they restart they set to face right
            if (restart == true && facingLeft == true)
            {
                spriteSelection = new Rectangle(0, 0, 50, 60);
                facingRight = true;
                facingLeft = false;
                restart = false;

            }
            #endregion

        }

        protected void Level()
        {
            // declaring local variables
            int maxX = graphics.GraphicsDevice.Viewport.Width;
            int minX = -20;

            // if the sprite goes to the right of the screen they are put to the left
            if (spriteMovement.X >= maxX)
            {
                spriteMovement.X = 10.0f;
            }

            // if the sprite goes to the left of the screen they are put to the right
            if (spriteMovement.X <= minX)
            {
                spriteMovement.X = 1000.0f;
            }


        }

        protected void Enemies()
        {
            // if the current screen is the gameplay
            if (currentScreen == ScreenState.Gameplay)
            {
                // for loop
                for (int loop = 0; loop <= 3; loop++)
                {
                    // this draws the pink enemies if the boolean in the array is true, if it is false it is not drawn
                    if (boolPink[loop] == true)
                    {
                        spriteBatch.Draw(enemyPinkOne, enemyPinkMovement[loop], null, Color.White, 0, new Vector2(0, 0), 1, spriteFXPink[loop], 0);
                        enemyPinkRec[loop] = new Rectangle((int)enemyPinkMovement[loop].X, (int)enemyPinkMovement[loop].Y, (int)enemyPinkOne.Width, (int)enemyPinkOne.Height);
                    }

                    // if the sprite collides with a pink enemy and the pink enemy is visible they die and lose a life
                    if ((spriteCollision.Intersects(enemyPinkRec[loop]) && boolPink[loop]) == true)
                    {
                        if (hasDied == false && lives > 0)
                        {
                            lives -= 1;
                            hasDied = true;
                        }
                    }
                    // if the sprite fires at a pink enemy the enemy dissappears and 100 is added to the score
                    if ((spriteRectangle.Intersects(enemyPinkRec[loop]) && boolPink[loop] == true && (Keyboard.GetState().IsKeyDown(Keys.Space)) && hasDied == false) && canFire == true)
                    {
                        boolPink[loop] = false;
                        score += 100;
                    }
                }

                // for loop
                for (int loop = 0; loop <= 2; loop++)
                {
                    // this draws the purple enemies if the boolean in the array is true, if it is false it is not drawn
                    if (boolPurple[loop] == true)
                    {
                        spriteBatch.Draw(enemyPurpleOne, enemyPurpleMovement[loop], null, Color.White, 0, new Vector2(0, 0), 1, spriteFXPurple[loop], 0);
                        enemyPurpleRec[loop] = new Rectangle((int)enemyPurpleMovement[loop].X, (int)enemyPurpleMovement[loop].Y, (int)enemyPurpleOne.Width, (int)enemyPurpleOne.Height);
                    }
                    else { }

                    // this draws the green enemies if the boolean in the array is true, if it is false it is not drawn
                    if (boolGreen[loop] == true)
                    {
                        spriteBatch.Draw(enemyGreenOne, enemyGreenMovement[loop], null, Color.White, 0, new Vector2(0, 0), 1, spriteFXGreen[loop], 0);
                        enemyGreenRec[loop] = new Rectangle((int)enemyGreenMovement[loop].X, (int)enemyGreenMovement[loop].Y, (int)enemyGreenOne.Width, (int)enemyGreenOne.Height);
                    }
                    else { }

                    // if the sprite collides with a purple or green enemy and the enemy is visible they die and lose a life
                    if (((spriteCollision.Intersects(enemyPurpleRec[loop]) && boolPurple[loop]) == true) || ((spriteCollision.Intersects(enemyGreenRec[loop]) && boolGreen[loop]) == true))
                    {
                        if (hasDied == false && lives > 0)
                        {
                            lives -= 1;
                            hasDied = true;
                        }
                    }

                    // if the sprite fires at a purple enemy the enemy dissappears and 100 is added to the score
                    if ((spriteRectangle.Intersects(enemyPurpleRec[loop]) && boolPurple[loop] == true && (Keyboard.GetState().IsKeyDown(Keys.Space)) && hasDied == false) && canFire == true)
                    {
                        boolPurple[loop] = false;
                        score += 100;
                    }

                    // if the sprite fires at a green enemy the enemy dissappears and 100 is added to the score
                    if ((spriteRectangle.Intersects(enemyGreenRec[loop]) && boolGreen[loop] == true && (Keyboard.GetState().IsKeyDown(Keys.Space)) && hasDied == false) && canFire == true)
                    {
                        boolGreen[loop] = false;
                        score += 100;
                    }
                }
            }
        }

        protected void Collision()
        {
            // if the current screen is gameplay
            if (currentScreen == ScreenState.Gameplay)
            {
                // if the sprite collides with a the left side of the fridge a left force applied
                if (spriteCollision.Intersects(platforms[0]))
                {
                    if (spriteMovement.X >= platforms[0].X)
                    {
                        spriteMovement.X -= 3;
                    }
                }
                // if the sprite collides with a the right side of the fridge a right force applied
                if (spriteCollision.Intersects(platforms[1]))
                {
                    if (spriteMovement.X >= platforms[1].X)
                    {
                        spriteMovement.X += 3;
                    }
                }
                // if a sprite collides with a platform the sprite's X position equals the platform (minus 4 to get the sprite
                // to stand exactly on the platform)
                if (spriteCollision.Intersects(platforms[2]))
                {
                    if (spriteMovement.Y > platforms[2].Y)
                    {
                        spriteMovement.Y = platforms[2].Y - 4;
                        jump = 13;
                    }
                }
                // if a sprite collides with platforms the sprites X position equals the platform
                for (int loop = 3; loop <= 4; loop++)
                {
                    if (spriteCollision.Intersects(platforms[loop]))
                    {
                        if (spriteMovement.Y > platforms[loop].Y)
                        {
                            spriteMovement.Y = platforms[loop].Y;
                            jump = 13;
                        }
                    }
                }
                // if a sprite collides with platforms the sprites X position equals the platform minus 4, if they press down they will jump down
                if (spriteCollision.Intersects(platforms[5]) && (Keyboard.GetState().IsKeyUp(Keys.Down)))
                {
                    if (spriteMovement.Y >= platforms[5].Y)
                    {
                        spriteMovement.Y = platforms[5].Y - 4;
                        jump = 13;
                    }
                }
                // if a sprite collides with platforms the sprites X position equals the platform minus 2, if they press down they will jump down
                if (spriteCollision.Intersects(platforms[6]) && (Keyboard.GetState().IsKeyUp(Keys.Down)))
                {
                    if (spriteMovement.Y >= platforms[6].Y)
                    {
                        spriteMovement.Y = platforms[6].Y - 2;
                        jump = 13;
                    }
                }
                // if a sprite collides with platforms the sprites X position equals the platform, if they press down they will jump down
                for (int loop = 7; loop <= 14; loop++)
                {
                    if (spriteCollision.Intersects(platforms[loop]))
                    {
                        if (spriteMovement.Y >= platforms[loop].Y && (Keyboard.GetState().IsKeyUp(Keys.Down)))
                        {
                            spriteMovement.Y = platforms[loop].Y;
                            jump = 13;
                        }
                    }
                }
                // if the user presses enter on the door they have finished
                if (spriteCollision.Intersects(finishDoor) && (Keyboard.GetState().IsKeyDown(Keys.Enter)))
                {
                    hasFinished = true;
                }
            }
        }

        protected void TimeScoreAndLives(GameTime gameTime)
        {
            // declaring local string variables
            String timeText;
            String timeTextScore;

            // if current screen is gameplay
            if (currentScreen == ScreenState.Gameplay)
            {
                // adding text to the strings
                textIndicatorString[0] = "LIVES: " + lives.ToString();
                textIndicatorString[1] = "SCORE: " + score.ToString();

                if (time.Seconds < 10) // if seconds is less than 10 add a 0 in front to appear "01, 02" etc
                {
                    timeText = string.Format("{0}:0{1}", time.Minutes, time.Seconds); // formatting time for display on screen
                    timeTextScore = string.Format("{0}.0{1}", time.Minutes, time.Seconds); //formatting time for calculation
                }
                else
                {
                    timeText = string.Format("{0}:{1}", time.Minutes, time.Seconds); // formatting time for display on screen
                    timeTextScore = string.Format("{0}.{1}", time.Minutes, time.Seconds); //formatting time for calculation
                }

                textIndicatorString[2] = "TIME: " + timeText;

                // drawing the lives, score and time text at the top of the screen
                for (int loop = 0; loop <= 2; loop++)
                {
                    spriteBatch.DrawString(textIndicator, textIndicatorString[loop], textIndicatorVec[loop], Color.Black);
                }

                // if lives is zero, show a popup with GAME OVER inside, also show the 3 germs which have beat the user
                // the user also goes off the screen to -80 to show that the game is over and they cannot control the player
                if (lives == 0)
                {
                    spriteBatch.Draw(popup, popupRec, Color.White);
                    spriteBatch.DrawString(popupTitle, "GAME OVER", new Vector2(382, 226), Color.Black);
                    spriteBatch.DrawString(textIndicator, "THE GERMS WIN", new Vector2(415, 277), Color.Black);
                    spriteBatch.DrawString(textIndicator, "Press Space to return to menu", new Vector2(340, 400), Color.Black);

                    spriteBatch.Draw(enemyPinkOne, new Vector2(410, 310), Color.White);
                    spriteBatch.Draw(enemyPurpleOne, new Vector2(485, 310), Color.White);
                    spriteBatch.Draw(enemyGreenOne, new Vector2(575, 310), Color.White);

                    spriteMovement.Y = -80;
                    timeRunning = false;

                    // if user presses space return to menu screen
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        currentScreen = ScreenState.Start;
                    }
                }

                // if the user hs finished stop the timer and calculate the final score. Also draw a popup
                // and show LEVEL complete and the score, time and lives. If the user presses R they will restart
                // and if they press space they will finish
                if (hasFinished == true)
                {
                    timeRunning = false;

                    finalScore = score * lives / Convert.ToDouble(timeTextScore);


                    spriteBatch.Draw(popup, popupRec, Color.White);

                    popupString[0] = "LEVEL COMPLETE";
                    popupString[1] = "SCORE: " + score.ToString();
                    popupString[2] = "LIVES: " + lives.ToString();
                    popupString[3] = "TIME: " + timeText;
                    popupString[4] = "FINAL SCORE: " + finalScore.ToString("0");
                    popupString[5] = "PRESS R TO RESTART OR SPACE TO FINISH";

                    // draw LEVEL COMPLETE
                    for (int loop = 0; loop < 1; loop++)
                    {
                        spriteBatch.DrawString(popupTitle, popupString[loop], popupText[loop], Color.Black);
                    }

                    // draw score, lives, time and final score
                    for (int loop = 1; loop < 5; loop++)
                    {
                        spriteBatch.DrawString(textIndicator, popupString[loop], popupText[loop], Color.Black);
                    }

                    // draw "press R to restart or space to finish"
                    for (int loop = 5; loop < 6; loop++)
                    {
                        spriteBatch.DrawString(smallFont, popupString[loop], popupText[loop], Color.Black);
                    }

                    // move sprite off screen
                    spriteMovement.Y = -80;

                    // if user presses space, go to high scores screen
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        currentScreen = ScreenState.HighScores;
                    }
                }
            }
        }

        protected void Menu()
        {
            // rectangle which follows the mouse
            mouseRectangle = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1);

            // if the menu screen is showing
            if (currentScreen == ScreenState.Start)
            {
                // add text to strings
                menuButtonText[0] = "START";
                menuButtonText[1] = "INSTRUCTIONS";
                menuButtonText[2] = "HIGH SCORES";
                menuButtonText[3] = "EXIT";

                // show the mouse cursor
                this.IsMouseVisible = true;

                // font in bottom right with my name and unit
                spriteBatch.DrawString(keyFont, "Made by Laurence Hammond - Unit 12", new Vector2(752, 745), Color.Black);

                // the name of the game
                spriteBatch.DrawString(menuTitle, "Germania", new Vector2(372, 38), Color.Black);

                // for loop which draws the buttons and the text on the menu
                for (int loop = 0; loop < 4; loop++)
                {
                    spriteBatch.DrawString(menuButton, menuButtonText[loop], menuButtonVectors[loop], Color.Black);
                }

                // if the user goes over the first button and presses the left mouse button
                if (mouseRectangle.Intersects(menuButtons[0]) && exitDialog == false)
                {
                    // improved validation as the user must now only press the left mouse button to proceed
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed && Mouse.GetState().RightButton == ButtonState.Released && Keyboard.GetState().IsKeyUp(Keys.Space))
                    {
                        // reset score and time and set hasFinished to false
                        hasFinished = false;
                        time = TimeSpan.Zero;
                        timeRunning = true;
                        hasDied = true; // reset the position back
                        lives = 3; // give 3 lives back after death above
                        score = 0;

                        // show the pink enemies
                        for (int loop = 0; loop <= 3; loop++)
                        {
                            boolPink[loop] = true;
                        }

                        // show the purple and green enemies
                        for (int loop = 0; loop <= 2; loop++)
                        {
                            boolPurple[loop] = true;
                            boolGreen[loop] = true;
                        }
                        // show the gameplay screen
                        currentScreen = ScreenState.Gameplay;
                    }
                }

                // if the user goes over the second button and presses the left mouse button
                if (mouseRectangle.Intersects(menuButtons[1]) && exitDialog == false)
                {
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed && Mouse.GetState().RightButton == ButtonState.Released && Keyboard.GetState().IsKeyUp(Keys.Space))
                    {
                        // show instructions screen
                        currentScreen = ScreenState.Instructions;
                    }
                }

                // if the user goes over the third button and presses the left mouse button
                if (mouseRectangle.Intersects(menuButtons[2]) && exitDialog == false)
                {
                    // improved validation as the user must now only press the left mouse button to proceed
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed && Mouse.GetState().RightButton == ButtonState.Released && Keyboard.GetState().IsKeyUp(Keys.Space))
                    {
                        // show high scores screen
                        currentScreen = ScreenState.HighScores;
                    }
                }

                // if the user goes over the fourth button and presses the left mouse button
                if (mouseRectangle.Intersects(menuButtons[3]) && exitDialog == false)
                {
                    // improved validation as the user must now only press the left mouse button to proceed
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed && Mouse.GetState().RightButton == ButtonState.Released && Keyboard.GetState().IsKeyUp(Keys.Space))
                    {
                        // exit dialog is true
                        exitDialog = true;
                    }
                }

                // if exit dialog is true then a popup will appear asking if the user wants to exit. This improves the
                // graphical element of my game as now if they click exit by mistake, they can press NO and not leave the game
                if (exitDialog == true)
                {
                    spriteBatch.Draw(popup, popupRec, Color.White);
                    spriteBatch.DrawString(instructionsFontTitle, "Are you sure you want to exit?", new Vector2(345, 246), Color.Black);
                    spriteBatch.DrawString(instructionsFontTitle, "YES                            NO", new Vector2(380, 395), Color.Black);
                    yesRec = new Rectangle(373, 395, 70, 45);
                    noRec = new Rectangle(610, 395, 70, 45);

                    // if the user presses YES the game will exit
                    if (mouseRectangle.Intersects(yesRec))
                    {
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            this.Exit();
                        }
                    }
                    // if the user presses NO the game will return
                    if (mouseRectangle.Intersects(noRec))
                    {
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            exitDialog = false;
                        }
                    }
                }

            }

            // if the current screen is the high scores screen
            if (currentScreen == ScreenState.HighScores)
            {
                // declare back button rectangle and give it coordinates
                Rectangle backRectangle;
                backRectangle = new Rectangle(12, 36, 154, 61);

                // draw the high score background
                spriteBatch.Draw(highScoreBackground, backgroundRectangle, Color.White);

                // my name and unit in bottom right
                spriteBatch.DrawString(keyFont, "Made by Laurence Hammond - Unit 12", new Vector2(752, 745), Color.Black);

                // draw high scores on screen
                spriteBatch.DrawString(menuTitle, "High Scores", new Vector2(372, 60), Color.Black);

                // draw RANK, NAME and SCORE in the top
                spriteBatch.DrawString(menuButton, "RANK", new Vector2(237, 152), Color.Black);
                spriteBatch.DrawString(menuButton, "NAME", new Vector2(376, 152), Color.Black);
                spriteBatch.DrawString(menuButton, "SCORE", new Vector2(625, 152), Color.Black);

                // give vectors to where the scoreboard text will go
                scoreboardPosition[0] = new Vector2(341, 215);
                scoreboardPosition[1] = new Vector2(341, 266);
                scoreboardPosition[2] = new Vector2(341, 315);
                scoreboardPosition[3] = new Vector2(341, 366);
                scoreboardPosition[4] = new Vector2(341, 415);
                scoreboardPosition[5] = new Vector2(341, 466);
                scoreboardPosition[6] = new Vector2(341, 515);
                scoreboardPosition[7] = new Vector2(341, 566);
                scoreboardPosition[8] = new Vector2(341, 615);
                scoreboardPosition[9] = new Vector2(318, 666);

                // give scoreboard rank numbers
                scoreboardRank[0] = 1;
                scoreboardRank[1] = 2;
                scoreboardRank[2] = 3;
                scoreboardRank[3] = 4;
                scoreboardRank[4] = 5;
                scoreboardRank[5] = 6;
                scoreboardRank[6] = 7;
                scoreboardRank[7] = 8;
                scoreboardRank[8] = 9;
                scoreboardRank[9] = 10;

                // if the final score is higher than first place, put new score in and shuffle other scores down
                if (finalScore > scoreboard[0])
                {
                    scoreboard[9] = scoreboard[8];
                    scoreboard[8] = scoreboard[7];
                    scoreboard[7] = scoreboard[6];
                    scoreboard[6] = scoreboard[5];
                    scoreboard[5] = scoreboard[4];
                    scoreboard[4] = scoreboard[3];
                    scoreboard[3] = scoreboard[2];
                    scoreboard[2] = scoreboard[1];
                    scoreboard[1] = scoreboard[0];
                    scoreboard[0] = finalScore;
                }
                // if the final score is higher than second place but not first, put new score in and shuffle other scores down
                if (finalScore < scoreboard[0] && finalScore > scoreboard[1])
                {
                    scoreboard[9] = scoreboard[8];
                    scoreboard[8] = scoreboard[7];
                    scoreboard[7] = scoreboard[6];
                    scoreboard[6] = scoreboard[5];
                    scoreboard[5] = scoreboard[4];
                    scoreboard[4] = scoreboard[3];
                    scoreboard[3] = scoreboard[2];
                    scoreboard[2] = scoreboard[1];
                    scoreboard[1] = finalScore;
                }
                // if the final score is higher than third place but not second, put new score in and shuffle other scores down
                if (finalScore < scoreboard[1] && finalScore > scoreboard[2])
                {
                    scoreboard[9] = scoreboard[8];
                    scoreboard[8] = scoreboard[7];
                    scoreboard[7] = scoreboard[6];
                    scoreboard[6] = scoreboard[5];
                    scoreboard[5] = scoreboard[4];
                    scoreboard[4] = scoreboard[3];
                    scoreboard[3] = scoreboard[2];
                    scoreboard[2] = finalScore;
                }
                // if the final score is higher than fourth place but not third, put new score in and shuffle other scores down
                if (finalScore < scoreboard[2] && finalScore > scoreboard[3])
                {
                    scoreboard[9] = scoreboard[8];
                    scoreboard[8] = scoreboard[7];
                    scoreboard[7] = scoreboard[6];
                    scoreboard[6] = scoreboard[5];
                    scoreboard[5] = scoreboard[4];
                    scoreboard[4] = scoreboard[3];
                    scoreboard[3] = finalScore;
                }
                // if the final score is higher than fifth place but not fourth, put new score in and shuffle other scores down
                if (finalScore < scoreboard[3] && finalScore > scoreboard[4])
                {
                    scoreboard[9] = scoreboard[8];
                    scoreboard[8] = scoreboard[7];
                    scoreboard[7] = scoreboard[6];
                    scoreboard[6] = scoreboard[5];
                    scoreboard[5] = scoreboard[4];
                    scoreboard[4] = finalScore;
                }
                // if the final score is higher than sixth place but not fifth, put new score in and shuffle other scores down
                if (finalScore < scoreboard[4] && finalScore > scoreboard[5])
                {
                    scoreboard[9] = scoreboard[8];
                    scoreboard[8] = scoreboard[7];
                    scoreboard[7] = scoreboard[6];
                    scoreboard[6] = scoreboard[5];
                    scoreboard[5] = finalScore;
                }
                // if the final score is higher than seventh place but not sixth, put new score in and shuffle other scores down
                if (finalScore < scoreboard[5] && finalScore > scoreboard[6])
                {
                    scoreboard[9] = scoreboard[8];
                    scoreboard[8] = scoreboard[7];
                    scoreboard[7] = scoreboard[6];
                    scoreboard[6] = finalScore;
                }
                // if the final score is higher than eighth place but not seventh, put new score in and shuffle other scores down
                if (finalScore < scoreboard[6] && finalScore > scoreboard[7])
                {
                    scoreboard[9] = scoreboard[8];
                    scoreboard[8] = scoreboard[7];
                    scoreboard[7] = finalScore;
                }
                // if the final score is higher than ninth place but not tenth, put new score in and shuffle other scores down
                if (finalScore < scoreboard[7] && finalScore > scoreboard[8])
                {
                    scoreboard[9] = scoreboard[8];
                    scoreboard[8] = finalScore;
                }
                // if the final score is higher than tenth place put new score in
                if (finalScore < scoreboard[8] && finalScore > scoreboard[9])
                {
                    scoreboard[9] = finalScore;
                }

                // draw scoreboard with scores and rank
                for (int loop = 0; loop < 10; loop++)
                {

                    spriteBatch.DrawString(menuButton, scoreboardRank[loop] + "   PLAYER 1" + "    " + scoreboard[loop].ToString("0"), scoreboardPosition[loop], Color.Black);
                }

                // draw back button
                spriteBatch.DrawString(menuButton, "<Back", new Vector2(25, 43), Color.Black);

                // if the mouse goes over the back button and presses the left mouse button go back to menu
                if (mouseRectangle.Intersects(backRectangle))
                {
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        currentScreen = ScreenState.Start;
                    }
                }

            }

            // if the screen is the instructions screen
            if (currentScreen == ScreenState.Instructions)
            {
                // declaring back button and giving the rectangle coordinates
                Rectangle backRectangle;
                backRectangle = new Rectangle(12, 36, 154, 61);

                // draw the isntructions background
                spriteBatch.Draw(instructionsBackground, backgroundRectangle, Color.White);

                // my name and unit on the bottom right of the screen
                spriteBatch.DrawString(keyFont, "Made by Laurence Hammond - Unit 12", new Vector2(752, 745), Color.Black);

                // draw back button
                spriteBatch.DrawString(menuButton, "<Back", new Vector2(25, 43), Color.Black);

                // draw Instructions as a title
                spriteBatch.DrawString(menuTitle, "Instructions", new Vector2(372, 60), Color.Black);

                // draw "How to Play", "Scoring" and "Controls" as headers in the text
                spriteBatch.DrawString(instructionsFontTitle, "How to Play", new Vector2(136, 160), Color.Black);
                spriteBatch.DrawString(instructionsFontTitle, "Scoring", new Vector2(136, 291), Color.Black);
                spriteBatch.DrawString(instructionsFontTitle, "Controls", new Vector2(136, 447), Color.Black);

                // draw the text for the instructions
                spriteBatch.DrawString(instructionsFont, "The objective of the game is to destroy the germs " +
                "using your players\nbleach and clean the area you are in. If a germ touches you, you will\nlose a life." +
                    " Then to finish the level you find the door and press enter.", new Vector2(136, 190), Color.Black);
                spriteBatch.DrawString(instructionsFont, "You are rewarded at the end of the level with a score. Your score " +
                "is\ncalculated by the amount of enemies you killed multiplied by the lives\nyou have left divided by the time.\n" +
                    "Final Score= Enemies killed x Lives / Time", new Vector2(136, 320), Color.Black);
                spriteBatch.DrawString(instructionsFont, "To control the player left and right you use the left and right arrow\nkeys." +
                "To make the player jump you press the up key. To fire the\nbleach you press the spacebar. To finish the level you " +
                    "press enter on\na door. ", new Vector2(136, 472), Color.Black);

                // draw text which goes over the keys on the screen
                spriteBatch.DrawString(keyFont, "FIRE", new Vector2(257, 687), Color.Black);
                spriteBatch.DrawString(keyFont, "FINISH\nWHEN\nOVER\nDOOR", new Vector2(540, 642), Color.Black);
                spriteBatch.DrawString(keyFont, "JUMP", new Vector2(781, 612), Color.Black);
                spriteBatch.DrawString(keyFont, "LEFT", new Vector2(709, 687), Color.Black);
                spriteBatch.DrawString(keyFont, "RIGHT", new Vector2(862, 687), Color.Black);

                // if the mouse goes over the back rectangle and the left mouse is clicked go back to the menu
                if (mouseRectangle.Intersects(backRectangle))
                {
                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        currentScreen = ScreenState.Start;
                    }
                }
            }
        }
    }
}
