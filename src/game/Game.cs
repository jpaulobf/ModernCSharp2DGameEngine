namespace game;

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

public class Game : GameInterface
{
    private BufferedGraphics bufferedGraphics;
    private Bitmap bufferedImage;
    private Graphics internalGraphics;
    private PlayerSprite playerSprite;
    private EnemySprite enemySprite;
    public Size Resolution { get; set; }
    private bool KEY_LEFT = false;
    private bool KEY_RIGHT = false;
    private int InternalResolutionWidth = 1000;
    private int InternalResolutionHeight = 800;
    private float scaleW = 1.0F;
    private float scaleH = 1.0F;
    private InterpolationMode interpolationMode;

    /**
        Game-class constructor
    */
    public Game(Size resolution, Size windowSize, InterpolationMode interpolationMode) {

        //store the window resolution
        this.Resolution         = resolution;

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
        this.playerSprite = new PlayerSprite("img\\airplanetile.png", 32, 32, 300, 300, 100);
        this.enemySprite = new EnemySprite("img\\helitile.png", 36, 23, 100, 150, 100, 2, 50);
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

        this.playerSprite.Update(frametime);
        this.enemySprite.Update(frametime);
    }

    public void Draw()
    {
        // Draw Background Color
        this.internalGraphics.FillRectangle(Brushes.CornflowerBlue, 0, 0, this.InternalResolutionWidth, this.InternalResolutionHeight);

        // Draw Player Sprite
        this.playerSprite.Draw(this.internalGraphics);

        this.enemySprite.Draw(this.internalGraphics);
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

    public Graphics GetGraphics() {
        return (this.internalGraphics);
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
        } catch {
            Console.WriteLine("Fail to resize...");
        }
    }
}
