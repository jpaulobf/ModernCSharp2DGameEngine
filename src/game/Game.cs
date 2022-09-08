namespace game;

using System;
using System.Drawing;
using System.Windows.Forms;

public class Game : GameInterface
{
    private BufferedGraphics bufferedGraphics;
    private Bitmap bufferedImage;
    private Graphics internalGraphics;
    private GameSprite playerSprite;
    public Size Resolution { get; set; }
    private bool KEY_LEFT = false;
    private bool KEY_RIGHT = false;
    private int InternalResolutionWidth = 800;
    private int InternalResolutionHeight = 450;
    private float scaleW = 1.0F;
    private float scaleH = 1.0F;

    /**
        Game-class constructor
    */
    public Game(Size resolution, Size windowSize) {

        //store the window resolution
        this.Resolution         = resolution;

        //create the imagebuffer
        this.bufferedImage      = new Bitmap(InternalResolutionWidth, InternalResolutionHeight);
        this.bufferedGraphics   = BufferedGraphicsManager.Current.Allocate(Graphics.FromImage(this.bufferedImage), new Rectangle(0, 0, windowSize.Width + 1, windowSize.Height + 1));
        this.internalGraphics   = bufferedGraphics.Graphics;

        //define the interpolation mode
        this.internalGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

        //calc the scale
        this.scaleW = (float)((float)windowSize.Width/(float)this.InternalResolutionWidth);
        this.scaleH = (float)((float)windowSize.Height/(float)this.InternalResolutionHeight);

        this.internalGraphics.ScaleTransform(scaleW, scaleH);

        this.Load();
    }

    public void Load()
    {
        // Load new sprite class
        this.playerSprite = new GameSprite();
        // Load sprite image
        string filepath = "img\\bomber-sprite.png";
        //Console.WriteLine(filepath);
        playerSprite.SpriteImage = new Bitmap(@filepath);
        // Set sprite height & width in pixels
        playerSprite.Width = playerSprite.SpriteImage.Width;
        playerSprite.Height = playerSprite.SpriteImage.Height;

        // Set sprite coodinates
        playerSprite.X = 300;
        playerSprite.Y = 300;
        
        // Set sprite Velocity
        playerSprite.Velocity = 200;
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

        if (KEY_LEFT) {
            playerSprite.X -= moveDistance;
        }

        if (KEY_RIGHT) {
            playerSprite.X += moveDistance;
        }

        if (playerSprite.X > this.InternalResolutionWidth) {
            playerSprite.X = 0;
        } else if (playerSprite.X < 0) {
            playerSprite.X = this.InternalResolutionWidth;
        }
    }

    public void Draw()
    {
        // Draw Background Color
        this.internalGraphics.FillRectangle(Brushes.CornflowerBlue, 0, 0, this.InternalResolutionWidth, this.InternalResolutionHeight);

        // Draw Player Sprite
        this.playerSprite.Draw(this.internalGraphics);
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
            this.bufferedGraphics   = BufferedGraphicsManager.Current.Allocate(Graphics.FromImage(this.bufferedImage), new Rectangle(0, 0, width + 1, height + 1));        
            this.internalGraphics   = bufferedGraphics.Graphics;
            this.internalGraphics.ScaleTransform(scaleW, scaleH);
        } catch {
            Console.WriteLine("Fail to resize...");
        }
    }
}
