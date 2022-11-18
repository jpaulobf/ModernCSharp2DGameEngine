namespace Game;

using Util;
using System.Drawing;

/**
 * Author: Joao P B Faria
 * Date: Oct/2022
 * Description: Abstrac class for the game sprites
 */
public abstract class GameSprite
{
    protected const bool NORMAL         = true;
    protected const bool REVERSED       = false;
    protected short SourceStartX        = 0;
    protected short SourceStartY        = 0;
    protected byte TilesNumber          = 1;
    protected uint MillisecsPerTile     = 0;
    internal float OgX                  = 0;
    internal float OgY                  = 0;
    public byte Type                    { get; set; }
    public bool Destroyed               { get; set; } = false;
    public bool RenderReversed          { get; set; }
    internal Bitmap? SpriteImage        { get; set; }
    internal Bitmap? RSpriteImage       { get; set; }
    internal Bitmap Pixel               { get; set; }
    public float X                      { get; set; }
    public float Y                      { get; set; }
    public float Width                  { get; set; }
    public float Height                 { get; set; }
    public int Velocity                 { get; set; }
    protected Rectangle SourceRect;
    protected Rectangle DestineRect;

    //sprite types
    public const byte HOUSE             = 1;
    public const byte HOUSE2            = 2;
    public const byte FUEL              = 3;
    public const byte HELI              = 4;
    public const byte SHIP              = 5;
    public const byte AIRPLANE          = 6;
    public const byte BRIDGE            = 7;
    public const byte NONE              = 8;
    public const byte LEFT              = 1;
    public const byte RIGHT             = 2;

    /**
     * GameSprite constructor
     */
    public GameSprite(int width, int height, int X, int Y, int velocity, byte type) 
    {
        this.Pixel = LoadingStuffs.GetInstance().GetImage("pixel");
        
        // Set sprite height & width in pixels
        this.Width = width;
        this.Height = height;
        this.Type = type;

        // Set sprite coodinates
        this.X = X;
        this.Y = Y;
        this.OgX = X;
        this.OgY = Y;
        
        // Set sprite Velocity
        this.Velocity = velocity;
    }

    /**
     * Abstract update method
     */
    public abstract void Update(long timeframe, bool colliding = false);

    /**
     * Draw is common for all subclasses, if necessary override it
     */
    public virtual void Draw(Graphics gfx)
    {
        if (this.SpriteImage != null)
        {
            // Draw sprite image on screen
            gfx.DrawImage(this.SpriteImage, this.DestineRect, this.SourceRect, System.Drawing.GraphicsUnit.Pixel);
        }
    }

    /**
     * Default collision detection method
     */
    public bool CollisionDetection(GameSprite othersprite) 
    {
        return (new Rectangle((short)this.X, (short)this.Y, (short)this.Width, (short)this.Height).IntersectsWith(
                new Rectangle((short)othersprite.X, (short)othersprite.Y, (short)othersprite.Width, (short)othersprite.Height)));
    }

    /**
     * Abstract method to set the current sprite as collided
     */
    public abstract void SetCollision(bool isPlayerCollision);

    /**
     * Abstract method to reset
     */
    public abstract void Reset();
}