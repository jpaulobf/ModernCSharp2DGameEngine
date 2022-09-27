namespace game;

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using game.stages;

public class Game : GameInterface
{
    private BufferedGraphics BufferedGraphics;
    private Bitmap BufferedImage;
    private Graphics InternalGraphics;
    public Size Resolution                  { get; set; }
    public Size WindowSize                  { get; }
    public InterpolationMode Interpolation  { get; }
    protected int InternalResolutionWidth   = 738;
    protected int InternalResolutionHeight  = 516;
    private float ScaleW                    = 1.0F;
    private float ScaleH                    = 1.0F;
    private bool WindowResizing             = false;
    private bool IS_LEFT_KEY_DOWN           = false;
    private bool IS_RIGHT_KEY_DOWN          = false;
    private bool Paused                     = false;
    private bool ResetAfterDead             = false;
    private long ResetCounter               = 0;
    private Font PauseFont                  = new Font("Arial", 16);
    private Point PausePoint;
    private HUD Hud;
    private Stages Stages;
    private PlayerSprite PlayerSprite;

    /**
     * Game-class constructor
     */
    public Game(Size resolution, Size windowSize, InterpolationMode interpolationMode) {

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

        // Load new sprite class
        this.Hud            = new HUD(this);
        this.Stages         = new Stages(this);
        this.PlayerSprite   = new PlayerSprite(this, "img\\airplanetile.png", 32, 32, 350, 387, 100);
    }

    public void Update(long frametime)
    {
        if (!Paused && !this.PlayerSprite.Colliding) 
        {
            // Calculate sprite movement based on Sprite Velocity and GameTimeElapsed
            float moveDistance = (float)(PlayerSprite.Velocity * ((double)frametime / 10_000_000));
            PlayerSprite.Righting = false;
            PlayerSprite.Lefting = false;

            if (IS_LEFT_KEY_DOWN) {
                PlayerSprite.X -= moveDistance;
                PlayerSprite.Lefting = true;
            }

            if (IS_RIGHT_KEY_DOWN) {
                PlayerSprite.X += moveDistance;
                PlayerSprite.Righting = true;
            }

            if (PlayerSprite.X > this.InternalResolutionWidth) {
                PlayerSprite.X = 0;
            } else if (PlayerSprite.X < 0) {
                PlayerSprite.X = this.InternalResolutionWidth;
            }

            this.Hud.Update(frametime);
            this.Stages.Update(frametime);
            this.PlayerSprite.Update(frametime);
        }

        if (this.ResetAfterDead) 
        {
            this.ResetCounter += frametime;
        }

        if (this.ResetCounter >= 30_000_000) 
        {
            this.ResetCounter = 0;
            this.ResetAfterCollision();
        }
    }

    public void Draw(long frametime)
    {
        if (!this.WindowResizing) {
            this.Stages.Draw(this.InternalGraphics, frametime);

            this.Hud.Draw(this.InternalGraphics);

            // Draw Player Sprite
            this.PlayerSprite.Draw(this.InternalGraphics);
        }

        if (this.Paused) {
            this.InternalGraphics.FillRectangle(new SolidBrush(Color.FromArgb(180, 255, 255, 255)), 0, PausePoint.Y - 20, this.WindowSize.Width, 60);
            this.InternalGraphics.FillRectangle(Brushes.LightGray, 0, PausePoint.Y - 20, this.WindowSize.Width, 2);
            this.InternalGraphics.FillRectangle(Brushes.LightGray, 0, PausePoint.Y + 40, this.WindowSize.Width, 2);
            this.InternalGraphics.DrawString("Game Paused!", PauseFont, Brushes.Black, PausePoint);
        }
    }

    public void KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyValue == 37) {
            IS_LEFT_KEY_DOWN = true;
        } else if (e.KeyValue == 39) {
            IS_RIGHT_KEY_DOWN = true;
        }
    }

    public void KeyPress(object sender, KeyPressEventArgs e) {}

    public void KeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyValue == 37) {
            IS_LEFT_KEY_DOWN = false;
        } else if (e.KeyValue == 39) {
            IS_RIGHT_KEY_DOWN = false;
        }

        if (e.KeyValue == 80 || e.KeyValue == 19) {
            this.PauseGame();
        }

        if (e.KeyValue == 82) {
            this.Reset();
        }
    }

    private void PauseGame()
    {
        this.Paused = !this.Paused;
    }

    private void Reset() {
        this.IS_LEFT_KEY_DOWN   = false;
        this.IS_RIGHT_KEY_DOWN  = false;
        this.Paused             = false;
        this.Hud.Reset();
        this.Stages.Reset();
        this.PlayerSprite.Reset();
    }

    public void Render(Graphics targetGraphics) {
        this.BufferedGraphics.Render(targetGraphics);
    }

    public async void Resize(object sender, System.EventArgs e)
    {
        //stop the render method
        this.WindowResizing = true;
        System.Threading.Thread.Sleep(1);
        
        try {
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

        } catch (Exception ex) {
            Console.WriteLine(ex);
        } finally {
            await Task.Run(() => this.WindowResizing = false);
        }
    }

    //Accessors
    public Graphics GetGraphics()               {   return (this.InternalGraphics);         }
    public int GetInternalResolutionWidth()     {   return (this.InternalResolutionWidth);  }
    public int GetInternalResolutionHeight()    {   return (this.InternalResolutionHeight); }
    public float getScaleW()                    {  return (this.ScaleW);                    }
    public float getScaleH()                    {   return (this.ScaleH);                   }
    public PlayerSprite GetPlayerSprite()       {   return (this.PlayerSprite);             }

    /**
     * Control the game to show the colision and reset the level
     */
    public void SetEnemyCollision()
    {
        if (this.Hud.PlayerIsAlive()) {
            this.Hud.PlayerDecreaseLive();
            this.ResetAfterDead = true;
        } else {
            //GAMEOVER
        }
            
        //TODO:
        //ok - Check for GameOver
        //ok - Pause the current scene
        //ok - Wait for the explosion animation
        //ok - Dec 1 live
        //ok - Wait 3 seconds      
        //Restart the current stage
        //Unpause the current scene
    }

    private void ResetAfterCollision()
    {
        this.Stages.Reset();
        this.PlayerSprite.Reset();
        this.PlayerSprite.Colliding = false;
        this.ResetAfterDead         = false;
    }
}
