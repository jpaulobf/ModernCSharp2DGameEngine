namespace game;

using System.Drawing;

public abstract class GameSprite
{
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
    protected long framecounter = 0;
    protected Boolean RenderReversed = false;

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
        // Draw sprite image on screen
        //public void DrawImage(Image image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit);
        gfx.DrawImage(this.SpriteImage, this.DestineRect, this.SourceRect, System.Drawing.GraphicsUnit.Pixel);
    }
}