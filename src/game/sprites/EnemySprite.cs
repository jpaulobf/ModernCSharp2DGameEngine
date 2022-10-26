namespace Game;

using Util;

/**
 * Author: Joao P B Faria
 * Date: Oct/2022
 * Description: Class representing enemies sprites
 */
public class EnemySprite : GameSprite 
{
    private IGame GameRef;
    protected long FrameCounter             = 0;
    protected short MaxLeft                 = 0;
    protected short MaxRight                = 0;
    protected byte Direction                = 0;
    protected byte DefaultDirection         = 0;
    protected byte DefaultTilesNumber       = 0;
    protected bool DefaultRenderReverse     = false;
    private volatile bool AnimateExplosion  = false;
    private long AnimationCounter           = 0;
    private Bitmap ShipExplosion1           = LoadingStuffs.GetInstance().GetImage("ship-explosion-1");
    private Bitmap ShipExplosion2           = LoadingStuffs.GetInstance().GetImage("ship-explosion-2");
    private Bitmap HeliExplosion1           = LoadingStuffs.GetInstance().GetImage("heli-explosion-1");
    private Bitmap HeliExplosion2           = LoadingStuffs.GetInstance().GetImage("heli-explosion-2");
    private Bitmap? DefaultBitmap;

    /**
     * Author: Joao P B Faria
     * Date: Oct/2022
     * Description: Enemy Sprite constructor
     */
    public EnemySprite(IGame game,
                       byte type,
                       int width, 
                       int height, 
                       int X = 0, 
                       int Y = 0, 
                       int velocity = 0, 
                       byte tilesNumber = 1, 
                       uint millisecondsPerTile = 0, 
                       bool renderReversed = false, 
                       short maxLeft = 0,
                       short maxRight = 0, 
                       byte direction = 0) : base(width, height, X, Y, velocity, type) {
        //after base constructor
        this.Type                   = type;
        this.TilesNumber            = tilesNumber;
        this.MillisecsPerTile       = millisecondsPerTile;
        this.GameRef                = game;
        this.RenderReversed         = renderReversed;
        this.DefaultRenderReverse   = renderReversed;
        this.FrameCounter           = 0;
        this.MaxLeft                = maxLeft;
        this.MaxRight               = maxRight;
        this.Direction              = direction;
        this.DefaultDirection       = direction;
        this.DefaultTilesNumber     = tilesNumber;

        switch(type)
        {
            case HELI:
                this.SpriteImage    = LoadingStuffs.GetInstance().GetImage("heli-tile");
                this.DefaultBitmap  = LoadingStuffs.GetInstance().GetImage("heli-tile");
                this.RSpriteImage   = LoadingStuffs.GetInstance().GetImage("heli-tile-r");
                break;
            case SHIP:
                this.SpriteImage    = LoadingStuffs.GetInstance().GetImage("ship");
                this.DefaultBitmap  = LoadingStuffs.GetInstance().GetImage("ship");
                this.RSpriteImage   = LoadingStuffs.GetInstance().GetImage("ship-r");
                break;
            case AIRPLANE:
                //this.SpriteImage    = LoadingStuffs.GetInstance().GetImage("house-2");
                //this.DefaultBitmap  = LoadingStuffs.GetInstance().GetImage("house-2");
                break;
        }

        if (this.RenderReversed)
        {
            this.SpriteImage = this.RSpriteImage;
        }
    }

    /**
     * Enemy Sprite update class
     */
    public override void Update(long frametime, bool colliding = false)
    {
        //if the current sprite has more than 1 tile
        if (this.TilesNumber > 1)
        {            
            this.FrameCounter += frametime;
            if (this.FrameCounter < this.MillisecsPerTile * 10_000) 
            {
                this.SourceStartX = 0;
            } 
            else 
            {
                this.SourceStartX = (short)Width;
                if (this.FrameCounter > this.MillisecsPerTile * 10_000 * 2) 
                {
                    this.FrameCounter = 0;
                }
            }
        } 
        else 
        {
            this.FrameCounter = 0;
            this.SourceStartX = 0;
        }
        
        if (this.Direction != 0) 
        {
            float step = (float)(this.Velocity * ((double)frametime / 10_000_000));
            if (this.Direction == LEFT) 
            {
                if (this.X > this.MaxLeft) 
                {
                    this.X -= step;
                } 
                else 
                {
                    this.Direction = RIGHT;
                    this.SpriteImage = this.DefaultBitmap;
                }
            } 
            else if (this.Direction == RIGHT) 
            {
                if (this.X < (this.MaxRight - this.Width-1)) 
                {
                    this.X += step;
                } 
                else 
                {
                    this.Direction = LEFT;
                    this.SpriteImage = this.RSpriteImage;
                }
            }
        }
        
        //set the source & dest sprite rectangles
        this.SourceRect  = new Rectangle(this.SourceStartX, this.SourceStartY, (short)this.Width, (short)this.Height);
        this.DestineRect = new Rectangle((short)this.X, (short)this.Y, (short)this.Width, (short)this.Height);

        //verify if the player sprite is coliding with this current
        if ((!colliding) && !this.Destroyed && this.CollisionDetection(this.GameRef.GetPlayer().GetPlayerSprite())) 
        {
            this.SetCollision(true);
        }

        //this will start after collision
        if (this.AnimateExplosion) {
            this.AnimationCounter += frametime;

            //this will start after explosion start
            if (this.AnimationCounter > 1_000_000 && this.AnimationCounter < 4_000_000) {
                if (this.Type == SHIP) {
                    this.SpriteImage = this.ShipExplosion1;
                } else if (this.Type == HELI) {
                    this.SpriteImage = this.HeliExplosion1;
                }
            } else if (this.AnimationCounter >= 4_000_000 && this.AnimationCounter < 8_000_000) {
                if (this.Type == SHIP) {
                    this.SpriteImage = this.ShipExplosion2;
                } else if (this.Type == HELI) {
                    this.SpriteImage = this.HeliExplosion2;
                }
            } else if (this.AnimationCounter >= 8_000_000) {
                this.SpriteImage = this.Pixel;
                this.AnimateExplosion = false;
                this.AnimationCounter = 0;
            }
        }
    }

    /**
     * Set sprite as colliding
     */
    public override void SetCollision(bool playerCollision)
    {
        this.TilesNumber = 0;
        this.Direction = 0;
        this.Destroyed = true;
        if (playerCollision) 
        {   
            this.GameRef.GetPlayer().SetCollision();
            this.GameRef.SetCollidingWithAnEnemy();
        }
        this.AnimateExplosion = true;
    }

    /**
     * Reset the sprite
     */
    public override void Reset()
    {
        this.AnimateExplosion   = false;
        if (this.DefaultBitmap != null)
        {
            this.SpriteImage    = new Bitmap(this.DefaultBitmap);
        }
        this.TilesNumber        = this.DefaultTilesNumber;
        this.Direction          = this.DefaultDirection;
        this.AnimationCounter   = 0;
        this.RenderReversed     = this.DefaultRenderReverse;
        this.X                  = this.OgX;
        this.Status             = NORMAL;
        this.Destroyed          = false;
    }
}