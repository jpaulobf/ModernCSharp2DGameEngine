namespace game;

using game.stages;

/**
 * Class representing enemies sprites
 */
public class EnemySprite : GameSprite {
    private GameInterface GameRef;
    protected long FrameCounter             = 0;
    protected short MaxLeft                 = 0;
    protected short MaxRight                = 0;
    protected byte Direction                = 0;
    protected byte DefaultDirection         = 0;
    protected byte DefaultTilesNumber       = 0;
    protected bool DefaultRenderReverse     = false;
    protected byte Type                     = 0;
    public static byte HELI                 = 4;
    public static byte SHIP                 = 5;
    public static byte AIRPLANE             = 6;
    private volatile bool AnimateExplosion  = false;
    private long AnimationCounter           = 0;
    private Bitmap ShipExplosion1           = new Bitmap(@"img\\ship_explosion_frame1.png");
    private Bitmap ShipExplosion2           = new Bitmap(@"img\\ship_explosion_frame2.png");
    private Bitmap HeliExplosion1           = new Bitmap(@"img\\heli_explosion_frame1.png");
    private Bitmap HeliExplosion2           = new Bitmap(@"img\\heli_explosion_frame2.png");
    private Bitmap DefaultBitmap;

    /**
     * Enemy Sprite constructor
     */
    public EnemySprite(GameInterface game,
                       byte type,
                       string imageFilePath, 
                       int width, 
                       int height, 
                       int X = 0, 
                       int Y = 0, 
                       int velocity = 0, 
                       byte tilesNumber = 1, 
                       uint millisecondsPerTile = 0, 
                       bool reversed = false, 
                       short maxLeft = 0,
                       short maxRight = 0, 
                       byte direction = 0) : base(imageFilePath, width, height, X, Y, velocity) {
        //after base constructor
        this.Type                   = type;
        this.TilesNumber            = tilesNumber;
        this.MillisecsPerTile       = millisecondsPerTile;
        this.GameRef                = game;
        this.RenderReversed         = reversed;
        this.FrameCounter           = 0;
        this.MaxLeft                = maxLeft;
        this.MaxRight               = maxRight;
        this.Direction              = direction;
        this.DefaultBitmap          = new Bitmap(@imageFilePath);
        this.DefaultDirection       = direction;
        this.DefaultTilesNumber     = tilesNumber;
        this.DefaultRenderReverse   = reversed;
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
            if (this.Direction == SpriteConstructor.LEFT) 
            {
                if (this.X > this.MaxLeft) 
                {
                    this.X -= step;
                } 
                else 
                {
                    this.Direction = SpriteConstructor.RIGHT;
                    this.FlipX();
                }
            } 
            else if (this.Direction == SpriteConstructor.RIGHT) 
            {
                if (this.X < (this.MaxRight - this.Width-1)) 
                {
                    this.X += step;
                } 
                else 
                {
                    this.Direction = SpriteConstructor.LEFT;
                    this.FlipX();
                }
            }
        }
        
        //set the source & dest sprite rectangles
        this.SourceRect  = new Rectangle(this.SourceStartX, this.SourceStartY, (short)this.Width, (short)this.Height);
        this.DestineRect = new Rectangle((short)this.X, (short)this.Y, (short)this.Width, (short)this.Height);

        //verify if the player sprite is coliding with this current
        if ((!colliding) && this.CollisionDetection(this.GameRef.GetPlayerSprite())) 
        {
            this.TilesNumber = 0;
            this.Direction = 0;
            this.GameRef.GetPlayerSprite().SetCollision();
            this.GameRef.SetEnemyCollision();
            this.StartExplosionAnimation();
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
     * Animate the current sprite colision
     */
    private void StartExplosionAnimation()
    {
        this.AnimateExplosion = true;
    }

    private void FlipX()
    {
       this.RenderReversed = true;
       this.Status = NORMAL;
    }

    /**
     * Reset the sprite
     */
    public override void Reset()
    {
        this.AnimateExplosion   = false;
        this.SpriteImage        = new Bitmap(this.DefaultBitmap);
        this.TilesNumber        = this.DefaultTilesNumber;
        this.Direction          = this.DefaultDirection;
        this.AnimationCounter   = 0;
        this.RenderReversed     = this.DefaultRenderReverse;
        this.X                  = this.DefaultX;
        this.Status             = NORMAL;
    }
}