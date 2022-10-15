namespace Game;

/**
 * Author: Joao P B Faria
 * Date: Sept/2022
 * Description: Class representing the Main Player (Player Sprite)
 */
public class PlayerSprite : GameSprite 
{
    private IGame GameRef;
    private PlayerController PControllerRef;
    private Bitmap LocalSpriteImage;
    private Bitmap SpriteSplosion               = new Bitmap(@"img\\sprite_splosion.png");
    private float OgWidth                       = 0;
    private float OgHeight                      = 0;
    private float OgX                           = 0;
    private float OgY                           = 0;

    /**
     * Player Sprite constructor
     */
    public PlayerSprite(IGame game, PlayerController playerController, string imageFilePath, int width, int height, int X, int Y, int velocity) : base(imageFilePath, width, height, X, Y, velocity) {
        this.GameRef            = game;
        this.PControllerRef     = playerController;
        this.LocalSpriteImage   = new Bitmap(@imageFilePath);
        
        this.OgWidth            = width;
        this.OgHeight           = height;
        this.OgX                = X;
        this.OgY                = Y;
    }

    /**
     * Player Sprite update method
     */
    public override void Update(long timeframe, bool colliding = false) {
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
        this.SpriteImage    = this.LocalSpriteImage;
        this.Width          = this.OgWidth;
        this.Height         = this.OgHeight;
        this.X              = this.OgX;
        this.Y              = this.OgY;
    }

    public override void SetCollision(bool isPlayerCollision) {}
}