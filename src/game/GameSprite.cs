namespace game;

using System.Drawing;

/**
 * Abstrac class for the game sprites
 */
public abstract class GameSprite
{
    protected const bool NORMAL     = true;
    protected const bool REVERSED   = false;
    protected bool Status           = NORMAL;
    protected short SourceStartX    = 0;
    protected short SourceStartY    = 0;
    protected byte TilesNumber      = 1;
    protected int MillisecsPerTile  = 0;
    public bool RenderReversed  { get; set; }
    public Bitmap SpriteImage   { get; set; }
    public float X              { get; set; }
    public float Y              { get; set; }
    public float Width          { get; set; }
    public float Height         { get; set; }
    public int Velocity         { get; set; }
    public bool Lefting         { get; set; }
    public bool Righting        { get; set; }
    protected Rectangle SourceRect;
    protected Rectangle DestineRect;

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

    /**
     * Abstract update method
     */
    public abstract void Update(long timeframe);

    /**
     * Draw is common for all subclasses, if necessary override it
     */
    public void Draw(Graphics gfx)
    {
        if (this.RenderReversed && this.Status == NORMAL) {
            this.SpriteImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
            this.Status = REVERSED;
        }

        // Draw sprite image on screen
        gfx.DrawImage(this.SpriteImage, this.DestineRect, this.SourceRect, System.Drawing.GraphicsUnit.Pixel);
    }

    /**
     * Default collision detection method
     */
    public bool CollisionDetection(GameSprite othersprite) {
        return (new Rectangle((short)this.X, (short)this.Y, (short)this.Width, (short)this.Height).IntersectsWith(
                new Rectangle((short)othersprite.X, (short)othersprite.Y, (short)othersprite.Width, (short)othersprite.Height)));
    }
}