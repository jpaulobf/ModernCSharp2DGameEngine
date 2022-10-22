using Util;

namespace Game;

/**
 * Author: Joao P B Faria
 * Date: Sept/2022
 * Description: Class representing the Main Player (Player Sprite)
 */
public class PlayerSprite : GameSprite 
{
    private IGame GameRef;
    private Player PControllerRef;
    private Bitmap OGSpriteImage;
    private Bitmap SpriteSplosion               = LoadingStuffs.GetInstance().GetImage("sprite-explosion");
    private float OgWidth                       = 0;
    private float OgHeight                      = 0;

    /**
     * Player Sprite constructor
     */
    public PlayerSprite(IGame game, Player playerController, Bitmap spriteImage, int width, int height, int X, int Y, int velocity) : base(spriteImage, width, height, X, Y, velocity, 0) 
    {
        this.GameRef            = game;
        this.PControllerRef     = playerController;
        this.OGSpriteImage      = spriteImage;
        
        this.OgWidth            = width;
        this.OgHeight           = height;
        this.OgX                = X;
        this.OgY                = Y;
    }

    /**
     * Player Sprite update method
     */
    public override void Update(long timeframe, bool colliding = false) 
    {
        if (!this.PControllerRef.Colliding) 
        {
            if (!this.PControllerRef.Lefting && !this.PControllerRef.Righting) 
            {
                this.SourceStartX = (short)Width; //Default
            } 
            else if (this.PControllerRef.Lefting) 
            {
                this.SourceStartX = 0;
            } 
            else if (this.PControllerRef.Righting) 
            { 
                this.SourceStartX = (short)(Width * 2);
            }
        } 
        else 
        {
            this.SourceStartX = 0;
            this.Width = 32;
            this.Height = 25;
            this.SpriteImage = this.SpriteSplosion;
        }

        //define the source & destine rect
        this.SourceRect = new Rectangle(SourceStartX, SourceStartY, (short)Width - 1, (short)Height);
        this.DestineRect = new Rectangle((short)X, (short)Y, (short)Width, (short)Height);
    }

    /**
     * Draw Player Sprite
     */
    public override void Draw(Graphics gfx)
    {
        //call base draw
        base.Draw(gfx);
    }

    /**
     * Reset player sprite
     */
    public override void Reset()
    {
        this.SpriteImage    = this.OGSpriteImage;
        this.Width          = this.OgWidth;
        this.Height         = this.OgHeight;
        this.X              = this.OgX;
        this.Y              = this.OgY;
    }

    public override void SetCollision(bool isPlayerCollision) {}
}