namespace game;

using System.Drawing;

public abstract class GameSprite
{
    protected const bool NORMAL   = true;
    protected const bool REVERSED = false;
    protected bool status         = NORMAL;
    public bool RenderReversed { get; set; }
    public Bitmap SpriteImage { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    protected short StartX = 0;
    protected short StartY = 0;
    public float Width { get; set; }
    public float Height { get; set; }
    public int Velocity { get; set; }
    public bool Lefting {get; set;}
    public bool Righting {get; set;}
    protected Rectangle SourceRect;
    protected Rectangle DestineRect;
    protected byte TilesNumber = 1;
    protected int MillisecondsPerFrame = 0;

    /**
     * GameSprite constructor
     */
    public GameSprite(string imageFilePath, int width, int height, int X, int Y, int velocity) {
        //Console.WriteLine(filepath);
        this.SpriteImage = new Bitmap(@imageFilePath);
        // Set sprite height & width in pixels
        this.Width = width;
        this.Height = height;

        // Set sprite coodinates
        this.X = X;
        this.Y = Y;
        
        // Set sprite Velocity
        this.Velocity = velocity;
    }

    public abstract void Update(long timeframe);

    public void Draw(Graphics gfx)
    {
        if (this.RenderReversed && this.status == NORMAL) {
            this.SpriteImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
            this.status = REVERSED;
        }

        // Draw sprite image on screen
        gfx.DrawImage(this.SpriteImage, this.DestineRect, this.SourceRect, System.Drawing.GraphicsUnit.Pixel);
    }
}