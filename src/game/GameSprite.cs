namespace game;

using System.Drawing;

public class GameSprite
{
    public Bitmap SpriteImage { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    private short StartX = 0;
    private short StartY = 0;
    public float Width { get; set; }
    public float Height { get; set; }
    public int Velocity { get; set; }
    public bool Lefting {get; set;}
    public bool Righting {get; set;}
    private Rectangle SourceRect;
    private Rectangle DestineRect;
    private byte TilesNumber = 1;
    private int MillisecondsPerFrame = 0;
    private long framecounter = 0;

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

    public GameSprite(string imageFilePath, int width, int height, int X, int Y, int velocity, byte tilesNumber, int millisecondsPerFrame) : this(imageFilePath, width, height, X, Y, velocity) {
        this.TilesNumber = tilesNumber;
        this.MillisecondsPerFrame = millisecondsPerFrame;
    }

    public void Update(long timeframe) {
        if (!this.Lefting && !this.Righting) {
            //Default
            StartX = (short)Width;
        } else if (this.Lefting) { //Lefting
            StartX = 0;
        } else if (this.Righting) { //Righting
            StartX = (short)(Width * 2);
        }

        this.SourceRect = new Rectangle(StartX, StartY, (short)Width, (short)Height);
        this.DestineRect = new Rectangle((short)X, (short)Y, (short)Width, (short)Height);
    }

    public void UpdateE(long timeframe) {
        
        this.framecounter += timeframe;

        if (this.framecounter < this.MillisecondsPerFrame * 10_000) {
            StartX = 0;
        } else {
            StartX = (short)Width;
            if (this.framecounter > this.MillisecondsPerFrame * 10_000 * 2) {
                this.framecounter = 0;
            }
        }
        
        this.SourceRect = new Rectangle(StartX, StartY, (short)Width, (short)Height);
        this.DestineRect = new Rectangle((short)X, (short)Y, (short)Width, (short)Height);
    }

    public void Draw(Graphics gfx)
    {
        // Draw sprite image on screen
        //public void DrawImage(Image image, Rectangle destRect, Rectangle srcRect, GraphicsUnit srcUnit);
        gfx.DrawImage(SpriteImage, this.DestineRect, this.SourceRect, System.Drawing.GraphicsUnit.Pixel);
    }
}