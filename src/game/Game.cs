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
    private bool WindowActionInProgress = false;
    private HUD Hud;
    private Stages Stages;
    private PlayerSprite PlayerSprite;
    public Size Resolution { get; set; }
    public Size WindowSize { get; }
    private bool KEY_LEFT   = false;
    private bool KEY_RIGHT  = false;
    private bool Paused     = false;
    protected int InternalResolutionWidth = 738;
    protected int InternalResolutionHeight = 516;
    private float ScaleW = 1.0F;
    private float ScaleH = 1.0F;
    private Font PauseFont = new Font("Arial", 16);
    private Point PausePoint;
    public InterpolationMode InterpolationMode { get; }

    /**
     * Game-class constructor
     */
    public Game(Size resolution, Size windowSize, InterpolationMode interpolationMode) {

        //store the window resolution
        this.Resolution         = resolution;
        this.WindowSize         = windowSize;

        //set the iterpolation mode
        this.InterpolationMode = interpolationMode;

        //create the imagebuffer
        this.BufferedImage      = new Bitmap(InternalResolutionWidth, InternalResolutionHeight);
        this.BufferedGraphics   = BufferedGraphicsManager.Current.Allocate(Graphics.FromImage(this.BufferedImage), new Rectangle(0, 0, windowSize.Width, windowSize.Height));
        this.InternalGraphics   = BufferedGraphics.Graphics;

        //define the interpolation mode
        this.InternalGraphics.InterpolationMode = this.InterpolationMode;

        //calc the scale
        this.ScaleW = (float)((float)windowSize.Width/(float)this.InternalResolutionWidth);
        this.ScaleH = (float)((float)windowSize.Height/(float)this.InternalResolutionHeight);

        //transform the image based on calc scale
        this.InternalGraphics.ScaleTransform(ScaleW, ScaleH);

        //screen center
        this.PausePoint = new Point((int)windowSize.Width/2 - 80, (int)windowSize.Height/2 - 50);

        //load the resources
        this.Load();
    }

    public void Load()
    {
        // Load new sprite class
        this.Hud            = new HUD(this);
        this.Stages         = new Stages(this);
        this.PlayerSprite   = new PlayerSprite(this, "img\\airplanetile.png", 32, 32, 350, 387, 100);
    }

    public void Unload()
    {
        // Unload graphics
        // Turn off game music
    }

    public void Update(long frametime)
    {
        if (!Paused) {
            // Calculate sprite movement based on Sprite Velocity and GameTimeElapsed
            float moveDistance = (float)(PlayerSprite.Velocity * ((double)frametime / 10_000_000));
            PlayerSprite.Righting = false;
            PlayerSprite.Lefting = false;

            if (KEY_LEFT) {
                PlayerSprite.X -= moveDistance;
                PlayerSprite.Lefting = true;
            }

            if (KEY_RIGHT) {
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
    }

    public void Draw(long frametime)
    {
        if (!this.WindowActionInProgress) {
            this.Stages.Draw(this.InternalGraphics, frametime);

            this.Hud.Draw(this.InternalGraphics);

            // Draw Player Sprite
            this.PlayerSprite.Draw(this.InternalGraphics);
        }

        if (Paused) {
            this.InternalGraphics.FillRectangle(new SolidBrush(Color.FromArgb(180, 255, 255, 255)), 0, PausePoint.Y - 20, this.WindowSize.Width, 60);
            this.InternalGraphics.FillRectangle(Brushes.LightGray, 0, PausePoint.Y - 20, this.WindowSize.Width, 2);
            this.InternalGraphics.FillRectangle(Brushes.LightGray, 0, PausePoint.Y + 40, this.WindowSize.Width, 2);
            this.InternalGraphics.DrawString("Game Paused!", PauseFont, Brushes.Black, PausePoint);
        }
    }

    public void KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyValue == 37) {
            KEY_LEFT = true;
        } else if (e.KeyValue == 39) {
            KEY_RIGHT = true;
        }
    }

    public void KeyPress(object sender, KeyPressEventArgs e) {}

    public void KeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyValue == 37) {
            KEY_LEFT = false;
        } else if (e.KeyValue == 39) {
            KEY_RIGHT = false;
        }

        if (e.KeyValue == 80 || e.KeyValue == 19) {
            this.PauseGame();
        }
    }

    private void PauseGame()
    {
        this.Paused = !this.Paused;
    }

    public void Render(Graphics targetGraphics) {
        this.BufferedGraphics.Render(targetGraphics);
    }

    public void Resize(object sender, System.EventArgs e)
    {
        //stop the render method
        this.WindowActionInProgress = true;
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
            this.InternalGraphics.InterpolationMode = this.InterpolationMode;

            this.Stages.Resize(sender, e);

        } catch (Exception ex) {
            Console.WriteLine(ex);
        } finally {
            this.WindowActionInProgress = false;
        }
    }

    //Accessors
    public Graphics GetGraphics()               {   return (this.InternalGraphics);         }
    public int GetInternalResolutionWidth()     {   return (this.InternalResolutionWidth);  }
    public int GetInternalResolutionHeight()    {   return (this.InternalResolutionHeight); }
    public float getScaleW()                    {  return (this.ScaleW);                    }
    public float getScaleH()                    {   return (this.ScaleH);                   }
}
