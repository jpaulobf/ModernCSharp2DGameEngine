namespace Game;

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Game.Stages;

/**
 * Author: Joao P B Faria
 * Date: Oct/2022
 * Description: This class is responsible for the game control (update, render & keyboard tracking)
 */
public class GameController : IGame
{
    //-----------------------------------------------------//
    //--- Window & Buffering control                    ---//
    //-----------------------------------------------------//
    private volatile BufferedGraphics BufferedGraphics;
    private volatile Bitmap BufferedImage;
    private volatile Graphics InternalGraphics;
    public Size Resolution                  { get; set; }
    public Size WindowSize                  { get; }
    public InterpolationMode Interpolation  { get; }
    protected int InternalResolutionWidth   = 1000; //738
    protected int InternalResolutionHeight  = 700; //516;
    private float ScaleW                    = 1.0F;
    private float ScaleH                    = 1.0F;
    private bool WindowResizing             = false;
    private bool Terminate                  = false;

    //-----------------------------------------------------//
    //--- Game Elements control                         ---//
    //-----------------------------------------------------//
    private volatile bool IS_LEFT_KEY_DOWN  = false;
    private volatile bool IS_RIGHT_KEY_DOWN = false;
    private volatile bool IS_DOWN_KEY_DOWN  = false;
    private volatile bool IS_UP_KEY_DOWN    = false;
    private volatile bool IS_SHOT_KEY_DOWN  = false;
    private volatile bool Paused            = false;
    private volatile bool ResetAfterDead    = false;
    private volatile bool ShowPlayerSprite  = false;
    private long Framecounter               = 0;
    private Util.SoundPlayerEx GameMusic    = new Util.SoundPlayerEx(Util.Utility.getCurrentPath() + "\\sfx\\main.wav");
    private StateMachine GameStateMachine   = new StateMachine();
    private long ResetCounter               = 0;
    private Font PauseFont                  = new Font("Arial", 16);
    private volatile bool SkipDraw          = false;
    private Point PausePoint;
    private HUD Hud;
    private GameStages Stages;
    private Player Player;
    private Score Score;
    private Menu Menu;
    private Options Options;
    private GameOver GameOver;
    private Exit Exit;
    

    /**
     * Game constructor
     */
    public GameController(Size resolution, Size windowSize, InterpolationMode interpolationMode) {

        //store the window resolution
        this.Resolution         = resolution;
        this.WindowSize         = windowSize;

        //set the iterpolation mode
        this.Interpolation = interpolationMode;

        //create the imagebuffer
        this.BufferedImage      = new Bitmap(InternalResolutionWidth, InternalResolutionHeight);
        this.BufferedGraphics   = BufferedGraphicsManager.Current.Allocate(Graphics.FromImage(this.BufferedImage), new Rectangle(0, 0, windowSize.Width, windowSize.Height));
        this.InternalGraphics   = BufferedGraphics.Graphics;

        //define the interpolation mode
        this.InternalGraphics.InterpolationMode = this.Interpolation;

        //calc the scale
        this.ScaleW = (float)((float)windowSize.Width/(float)this.InternalResolutionWidth);
        this.ScaleH = (float)((float)windowSize.Height/(float)this.InternalResolutionHeight);

        //transform the image based on calc scale
        this.InternalGraphics.ScaleTransform(ScaleW, ScaleH);

        //screen center
        this.PausePoint = new Point((int)windowSize.Width/2 - 80, (int)windowSize.Height/2 - 50);

        // Load the game classes
        this.Menu       = new Menu(this);
        this.Options    = new Options(this);
    }

    /**
     * Update the game
     */
    public void Update(long frametime)
    {
        if (GameStateMachine.GetCurrentGameState() == StateMachine.MENU)
        {
            this.Menu.Update(frametime);
        }
        else if (GameStateMachine.GetCurrentGameState() == StateMachine.OPTIONS)
        {
            this.Options.Update(frametime);
        }
        else if (GameStateMachine.GetCurrentGameState() == StateMachine.EXITING)
        {
            this.Exit.Update(frametime);
        }
        else if (GameStateMachine.GetCurrentGameState() == StateMachine.IN_GAME)
        {
            this.Framecounter += frametime;

            //start the music in the first frametime
            if (this.Framecounter == frametime)
            {
                this.PlayMusic();
            }

            if (!Paused && !this.Player.Colliding) 
            {
                this.Player.GoStraight();

                if (this.ShowPlayerSprite) 
                {
                    if (IS_LEFT_KEY_DOWN) 
                    {
                        this.Player.GoLeft(frametime);
                    }

                    if (IS_RIGHT_KEY_DOWN) 
                    {
                        this.Player.GoRight(frametime);
                    }

                    if (IS_DOWN_KEY_DOWN) 
                    {
                        this.Player.HalfSpeed();
                    }

                    if (IS_UP_KEY_DOWN)
                    {
                        this.Player.DoubleSpeed();
                    }

                    if (IS_SHOT_KEY_DOWN) 
                    {
                        this.Player.Shooting();
                    }
                }
                
                this.Hud.Update(frametime);
                this.Stages.Update(frametime);
                this.Player.Update(frametime);
                this.Score.Update(frametime);
            } 
            else if (this.Player.Colliding)
            {
                this.Stages.Update(frametime, this.Player.Colliding);
            }

            //if the player hit something, resetcounter will start
            if (this.ResetAfterDead) 
            {
                this.ResetCounter += frametime;
            }

            //after 3 seconds, the game restart
            if (this.ResetCounter >= 30_000_000) 
            {
                this.ResetCounter = 0;
                this.ResetAfterCollision();
            }
        }
        else if (this.GameStateMachine.GetCurrentGameState() == StateMachine.GAME_OVER)
        {
            this.Framecounter += frametime;

            this.GameOver.Update(frametime);

            if (this.Framecounter == 10_000_000)
            {
                this.Framecounter = 0;
                this.GameStateMachine.SetStateToMenu();
            }
        }
    }

    /**
     * Draw the game
     */
    public void Draw(long frametime)
    {
        if (!this.WindowResizing) 
        {
            if (!this.SkipDraw)
            {
                if (GameStateMachine.GetCurrentGameState() == StateMachine.MENU)
                {
                    this.Menu.Draw(this.InternalGraphics);
                }
                else if (GameStateMachine.GetCurrentGameState() == StateMachine.OPTIONS)
                {
                    this.Options.Draw(this.InternalGraphics);
                }
                else if (GameStateMachine.GetCurrentGameState() == StateMachine.EXITING)
                {
                    //draw the stage bg & enemies
                    this.Stages.Draw(this.InternalGraphics, frametime);

                    //draw the HUD
                    this.Hud.Draw(this.InternalGraphics);

                    //draw the Score
                    this.Score.Draw(this.InternalGraphics);

                    // Draw Player Sprite
                    this.Player.Draw(this.InternalGraphics);

                    //Draw Exiting Board
                    this.Exit.Draw(this.InternalGraphics);
                }
                else if (GameStateMachine.GetCurrentGameState() == StateMachine.IN_GAME)
                {
                    //draw the stage bg & enemies
                    this.Stages.Draw(this.InternalGraphics, frametime);

                    //draw the HUD
                    this.Hud.Draw(this.InternalGraphics);

                    //draw the Score
                    this.Score.Draw(this.InternalGraphics);

                    if (this.ShowPlayerSprite) 
                    {
                        // Draw Player Sprite
                        this.Player.Draw(this.InternalGraphics);
                    }

                    //draw pause message
                    if (this.Paused) 
                    {
                        this.InternalGraphics.FillRectangle(new SolidBrush(Color.FromArgb(180, 255, 255, 255)), 0, PausePoint.Y - 20, this.WindowSize.Width, 60);
                        this.InternalGraphics.FillRectangle(Brushes.LightGray, 0, PausePoint.Y - 20, this.WindowSize.Width, 2);
                        this.InternalGraphics.FillRectangle(Brushes.LightGray, 0, PausePoint.Y + 40, this.WindowSize.Width, 2);
                        this.InternalGraphics.DrawString("Game Paused!", PauseFont, Brushes.Black, PausePoint);
                    }
                }
                else if (GameStateMachine.GetCurrentGameState() == StateMachine.GAME_OVER)
                {
                    this.GameOver.Draw(this.InternalGraphics);
                }
            }
            this.SkipDraw = false;
        }
    }
    
    /**
     * Render the BackBuffer
     */
    public void Render(Graphics targetGraphics) 
    {
        this.BufferedGraphics.Render(targetGraphics);
    }

    /**
     * Game key down
     */
    public void KeyDown(object? sender, KeyEventArgs e)
    {
        if (this.GameStateMachine.GetCurrentGameState() == StateMachine.IN_GAME)
        {
            if (e.KeyValue == 37) 
            {
                IS_LEFT_KEY_DOWN = true;
            } 
            else if (e.KeyValue == 39) 
            {
                IS_RIGHT_KEY_DOWN = true;
            } 
            
            //you can right/left + down/up
            if (e.KeyValue == 38)
            {
                IS_UP_KEY_DOWN = true;
            }
            else if (e.KeyValue == 40)
            {
                IS_DOWN_KEY_DOWN = true;
            }

            if (e.KeyValue == 32) 
            {
                if (this.ShowPlayerSprite) 
                {
                    IS_SHOT_KEY_DOWN = true;
                }
            }

            if (e.KeyValue == 32 || e.KeyValue == 37 || e.KeyValue == 38 || e.KeyValue == 39 || e.KeyValue == 40) 
            {
                if (this.ShowPlayerSprite) 
                {
                    this.Stages.Start();
                    this.Player.Flying = true;
                }
            }
        }
    }

    /**
     * Game Key press
     */
    public void KeyPress(object? sender, KeyPressEventArgs e) {}

    /**
     * Game Key up
     */
    public void KeyUp(object? sender, KeyEventArgs e)
    {
        if (this.GameStateMachine.GetCurrentGameState() == StateMachine.MENU)
        {
            this.Menu.KeyUp(sender, e);
        }
        else if (this.GameStateMachine.GetCurrentGameState() == StateMachine.OPTIONS)
        {
            this.Options.KeyUp(sender, e);
        }
        else if (GameStateMachine.GetCurrentGameState() == StateMachine.EXITING)
        {
            this.Exit.KeyUp(sender, e);
        }
        else if (this.GameStateMachine.GetCurrentGameState() == StateMachine.IN_GAME)
        {
            if (e.KeyValue == 37) 
            {
                IS_LEFT_KEY_DOWN = false;
            } 
            else if (e.KeyValue == 39) 
            {
                IS_RIGHT_KEY_DOWN = false;
            }
            
            if (e.KeyValue == 38)
            {
                IS_UP_KEY_DOWN = false;
            }
            else if (e.KeyValue == 40)
            {
                IS_DOWN_KEY_DOWN = false;
            }

            if (e.KeyValue == 32)
            {
                IS_SHOT_KEY_DOWN = false;
            }

            if (e.KeyValue == 80 || e.KeyValue == 19) 
            {
                this.PauseGame();
            }

            if (e.KeyValue == 27)
            {
                if (this.ShowPlayerSprite)
                {
                    this.GameStateMachine.SetStateToExiting();
                }
            }

            if (e.KeyValue == 82) 
            {
                this.Reset();
            }

            if (e.KeyValue == 78)
            {
                this.NextStage();
            }
        }
    }

    /**
     * Control the game to show the colision and reset the level
     */
    public void PlayerCollided()
    {
        this.GetPlayer().SetCollision();

        if (this.Player.PlayerIsAlive()) 
        {
            this.Player.PlayerDecreaseLive();
            this.ResetAfterDead = true;
        }
        else 
        {
            this.GameStateMachine.SetGameStateToGameOver();
        }
    }

    /**
     * Reset the current stage after collision
     */
    private void ResetAfterCollision()
    {
        this.Stages.Reset();
        this.Player.Reset(false);
        this.Hud.Reset();
        this.Player.Colliding   = false;
        this.ResetAfterDead     = false;
    }

    /**
     * Toggle PlayerSprite visible or not
     */
    public void TogglePlayerSprite() {
        this.ShowPlayerSprite = !this.ShowPlayerSprite;
    }

    public void DisablePlayerSprite() {
        this.ShowPlayerSprite = false;
    }

    /**
     * Pause the game
     */
    private void PauseGame()
    {
        this.Paused = !this.Paused;
    }

    /**
     * Reset the game
     */
    public void Reset() {
        this.IS_LEFT_KEY_DOWN           = false;
        this.IS_RIGHT_KEY_DOWN          = false;
        this.IS_SHOT_KEY_DOWN           = false;
        this.Paused                     = false;
        this.ResetAfterDead             = false;
        this.ResetCounter               = 0;
        this.Framecounter               = 0;
        this.Hud.Reset();
        this.Score.Reset();
        this.Stages.Reset();
        this.Player.Reset();
    }

    /**
     * Skip the Draw method (once)
     */
    public void SkipDrawOnce()
    {
        this.SkipDraw = true;
    }

    /**
     * Define the in game initial configs
     */    
    public void InitGameConfigurations()
    {
        this.InternalResolutionWidth   = 738;
        this.InternalResolutionHeight  = 516;

        //create the imagebuffer
        this.BufferedImage      = new Bitmap(InternalResolutionWidth, InternalResolutionHeight);
        this.BufferedGraphics   = BufferedGraphicsManager.Current.Allocate(Graphics.FromImage(this.BufferedImage), new Rectangle(0, 0, this.WindowSize.Width, this.WindowSize.Height));
        this.InternalGraphics   = BufferedGraphics.Graphics;

        //calc the scale
        this.ScaleW             = (float)((float)this.WindowSize.Width / (float)this.GetInternalResolutionWidth());
        this.ScaleH             = (float)((float)this.WindowSize.Height / (float)this.GetInternalResolutionHeight());

        //transform the image based on calc scale
        this.InternalGraphics.ScaleTransform(ScaleW, ScaleH);
        
        //create the game objects
        this.Player             = new Player(this);
        this.Hud                = new HUD(this);
        this.Score              = new Score(this);
        this.GameOver           = new GameOver(this);
        this.Stages             = new GameStages(this);
        this.Exit               = new Exit(this);
    }

    /**
     * Set configurations to back to the menu
     */
    public void ToMenu()
    {
        this.InternalResolutionWidth    = 1000;
        this.InternalResolutionHeight   = 700;

        //create the imagebuffer
        this.BufferedImage      = new Bitmap(InternalResolutionWidth, InternalResolutionHeight);
        this.BufferedGraphics   = BufferedGraphicsManager.Current.Allocate(Graphics.FromImage(this.BufferedImage), new Rectangle(0, 0, this.WindowSize.Width, this.WindowSize.Height));
        this.InternalGraphics   = BufferedGraphics.Graphics;

        //calc the scale
        this.ScaleW             = (float)((float)this.WindowSize.Width / (float)this.GetInternalResolutionWidth());
        this.ScaleH             = (float)((float)this.WindowSize.Height / (float)this.GetInternalResolutionHeight());

        //transform the image based on calc scale
        this.InternalGraphics.ScaleTransform(ScaleW, ScaleH);

        //stop music
        this.StopMusic();

        //set the game state to menu
        this.SetGameStateToMenu();
    }

    /**
     * Play the main music
     */
    private void PlayMusic()
    {
        Task.Run(() =>
            {
                GameMusic.Stop();
                GameMusic.PlayLooping();
            }
        );
    }

    /**
     * Stop main music
     */
    private void StopMusic() 
    {
        Task.Run(() =>
            {
                GameMusic.Stop();
            }
        );
    }

    /**
     * Update the Fuel Marker in the HUD
     */
    public void UpdateFuelMarker()
    {
        this.Hud.UpdateFuelMarker(this.Player.FuelCounter);
    }

    /**
     * Update the Score based on destroyed item
     */
    public void UpdateScore(int type)
    {
        this.Score.ItemDestructed(type);
    }

    /**
     * Set game status to "in game"
     */
    public void SetGameStateToInGame()
    {
        this.GameStateMachine.SetStateToInGame();
    }

    /**
     * Set game status to "menu"
     */
    public void SetGameStateToMenu()
    {
        this.GameStateMachine.SetStateToMenu();
    }

    /**
     * Set game status to "options"
     */
    public void SetGameStateToOptions()
    {
        this.GameStateMachine.SetGameStateToOptions();
    }

    /**
     * Set game status to "exit"
     */
    public void ExitGame()
    {
        this.Terminate = true;
    }

    private void NextStage()
    {
        this.Reset();
        this.Stages.ControlStageLinesCount();
    }
    
    //Accessors
    public IEnumerable<GameSprite> GetCurrentScreenSprites()    {  return(this.Stages.GetCurrentScreenSprites());   }
    public Graphics GetGraphics()                               {   return (this.InternalGraphics);                 }
    public int GetInternalResolutionWidth()                     {   return (this.InternalResolutionWidth);          }
    public int GetInternalResolutionHeight()                    {   return (this.InternalResolutionHeight);         }
    public float getScaleW()                                    {   return (this.ScaleW);                           }
    public float getScaleH()                                    {   return (this.ScaleH);                           }
    public Player GetPlayer()                                   {   return (this.Player);                           }
    public bool GetTerminateStatus()                            {   return (this.Terminate);                        }
    public GameStages GetStages()                               {   return (this.Stages);                           }

    /**
     * Resize screen (buggy...)
     */
    public async void Resize(object? sender, System.EventArgs e)
    {
        //stop the render method
        this.WindowResizing = true;
        System.Threading.Thread.Sleep(1);
        
        try 
        {
            if (sender != null)
            {
                //calc new scale
                int width = ((Form)sender).Width;
                int height = ((Form)sender).Height;
                this.ScaleW = (float)((float)width/(float)this.InternalResolutionWidth);
                this.ScaleH = (float)((float)height/(float)this.InternalResolutionHeight);

                //Invalidate the current buffer
                BufferedGraphicsManager.Current.Invalidate();
                BufferedGraphicsManager.Current.Dispose();

                //apply new scale
                this.BufferedGraphics   = BufferedGraphicsManager.Current.Allocate(Graphics.FromImage(this.BufferedImage), new Rectangle(0, 0, width, height));
                this.InternalGraphics   = BufferedGraphics.Graphics;
                
                this.InternalGraphics.ScaleTransform(ScaleW, ScaleH);
                this.InternalGraphics.InterpolationMode = this.Interpolation;

                this.Stages.Resize(sender, e);
            }
        } 
        catch (Exception ex) 
        {
            Console.WriteLine(ex);
        } 
        finally 
        {
            await Task.Run(() => this.WindowResizing = false);
        }
    }
}