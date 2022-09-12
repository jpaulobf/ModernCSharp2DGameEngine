namespace game;

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using game.stages;

public class Game : GameInterface
{
    private BufferedGraphics bufferedGraphics;
    private Bitmap bufferedImage;
    private Graphics internalGraphics;
    private HUD hud;
    private Stages stages;
    private PlayerSprite playerSprite;
    private EnemySprite heliSprite;
    private EnemySprite shipSprite;
    private EnemySprite airplaneSprite;
    private StaticSprite fuelSprite;
    
    public Size Resolution { get; set; }
    public Size WindowSize { get; }
    private bool KEY_LEFT = false;
    private bool KEY_RIGHT = false;
    protected int InternalResolutionWidth = 738;
    protected int InternalResolutionHeight = 516;
    private float scaleW = 1.0F;
    private float scaleH = 1.0F;
    public InterpolationMode interpolationMode { get; }

    /**
        Game-class constructor
    */
    public Game(Size resolution, Size windowSize, InterpolationMode interpolationMode) {

        //store the window resolution
        this.Resolution         = resolution;
        this.WindowSize         = windowSize;

        //set the iterpolation mode
        this.interpolationMode = interpolationMode;

        //create the imagebuffer
        this.bufferedImage      = new Bitmap(InternalResolutionWidth, InternalResolutionHeight);
        this.bufferedGraphics   = BufferedGraphicsManager.Current.Allocate(Graphics.FromImage(this.bufferedImage), new Rectangle(0, 0, windowSize.Width, windowSize.Height));
        this.internalGraphics   = bufferedGraphics.Graphics;

        //define the interpolation mode
        this.internalGraphics.InterpolationMode = this.interpolationMode;

        //calc the scale
        this.scaleW = (float)((float)windowSize.Width/(float)this.InternalResolutionWidth);
        this.scaleH = (float)((float)windowSize.Height/(float)this.InternalResolutionHeight);

        //transform the image based on calc scale
        this.internalGraphics.ScaleTransform(scaleW, scaleH);

        //load the resources
        this.Load();
    }

    public void Load()
    {
        // Load new sprite class
        this.hud = new HUD(this);
        this.stages = new Stages(this);
        this.playerSprite = new PlayerSprite(this, "img\\airplanetile.png", 32, 32, 350, 387, 100);
        this.heliSprite = new EnemySprite(this, "img\\helitile.png", 36, 23, 302, 96, 100, 2, 50);
        this.shipSprite = new EnemySprite(this, "img\\ship.png", 73, 18, 225, 241, 100);
        this.fuelSprite = new StaticSprite(this, "img\\fuel.png", 32, 55, 417, 145);
        this.airplaneSprite = new EnemySprite(this, "img\\enemyairplane.png", 37, 14, 200, 50, 400);
    }

    public void Unload()
    {
        // Unload graphics
        // Turn off game music
    }

    public void Update(long frametime)
    {
        // Calculate sprite movement based on Sprite Velocity and GameTimeElapsed
        float moveDistance = (float)(playerSprite.Velocity * ((double)frametime / 10_000_000));
        playerSprite.Righting = false;
        playerSprite.Lefting = false;

        if (KEY_LEFT) {
            playerSprite.X -= moveDistance;
            playerSprite.Lefting = true;
        }

        if (KEY_RIGHT) {
            playerSprite.X += moveDistance;
            playerSprite.Righting = true;
        }

        if (playerSprite.X > this.InternalResolutionWidth) {
            playerSprite.X = 0;
        } else if (playerSprite.X < 0) {
            playerSprite.X = this.InternalResolutionWidth;
        }

        this.hud.Update(frametime);
        this.stages.Update(frametime);
        this.playerSprite.Update(frametime);
        this.heliSprite.Update(frametime);
        this.shipSprite.Update(frametime);
        this.fuelSprite.Update(frametime);
        this.airplaneSprite.Update(frametime);
    }

    public void Draw()
    {
        this.stages.Draw(this.internalGraphics);

        this.hud.Draw(this.internalGraphics);

        // Draw Player Sprite
        this.playerSprite.Draw(this.internalGraphics);

        this.heliSprite.Draw(this.internalGraphics);

        this.shipSprite.Draw(this.internalGraphics);

        this.fuelSprite.Draw(this.internalGraphics);

        this.airplaneSprite.Draw(this.internalGraphics);
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
    }

    public void Render(Graphics targetGraphics) {
        this.bufferedGraphics.Render(targetGraphics);
    }

    public void Resize(object sender, System.EventArgs e)
    {
        try {
            //calc new scale
            int width = ((Form)sender).Width;
            int height = ((Form)sender).Height;
            this.scaleW = (float)((float)width/(float)this.InternalResolutionWidth);
            this.scaleH = (float)((float)height/(float)this.InternalResolutionHeight);

            //apply new scale
            this.bufferedGraphics   = BufferedGraphicsManager.Current.Allocate(Graphics.FromImage(this.bufferedImage), new Rectangle(0, 0, width, height));        
            this.internalGraphics   = bufferedGraphics.Graphics;
            this.internalGraphics.ScaleTransform(scaleW, scaleH);
            this.internalGraphics.InterpolationMode = this.interpolationMode;

            this.stages.Resize(sender, e);

        } catch {
            Console.WriteLine("Fail to resize...");
        }
    }

    //Accessors
    public Graphics GetGraphics()               {   return (this.internalGraphics);         }
    public int GetInternalResolutionWidth()     {   return (this.InternalResolutionWidth);  }
    public int GetInternalResolutionHeight()    {   return (this.InternalResolutionHeight); }
    public float getScaleW()                    {  return (this.scaleW);                    }
    public float getScaleH()                    {   return (this.scaleH);                   }
}
