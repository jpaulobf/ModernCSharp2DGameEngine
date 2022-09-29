namespace game;

using game.stages;

/**
 * Class representing enemies sprites
 */
public class EnemySprite : GameSprite {

    private GameInterface GameRef;
    protected long FrameCounter = 0;
    protected short MaxLeft     = 0;
    protected short MaxRight    = 0;
    protected byte Direction    = 0;

    /**
     * Enemy Sprite constructor
     */
    public EnemySprite(GameInterface game, 
                       string imageFilePath, 
                       int width, 
                       int height, 
                       int X = 0, 
                       int Y = 0, 
                       int velocity = 0, 
                       byte tilesNumber = 1, 
                       int millisecondsPerTile = 0, 
                       bool reversed = false, 
                       short maxLeft = 0,
                       short maxRight = 0, 
                       byte direction = 0) : base(imageFilePath, width, height, X, Y, velocity) {
        //after base constructor
        this.TilesNumber        = tilesNumber;
        this.MillisecsPerTile   = millisecondsPerTile;
        this.GameRef            = game;
        this.RenderReversed     = reversed;
        this.FrameCounter       = 0;
        this.MaxLeft            = maxLeft;
        this.MaxRight           = maxRight;
        this.Direction          = direction;
    }

    /**
     * Enemy Sprite update class
     */
    public override void Update(long frametime)
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
        if (this.CollisionDetection(this.GameRef.GetPlayerSprite())) 
        {
            this.TilesNumber = 0;
            this.Direction = 0;
            this.GameRef.GetPlayerSprite().SetCollision();
            this.GameRef.SetEnemyCollision();
            this.AnimateExplosion();
        }
    }

    /**
     * Animate the current sprite colision
     */
    private void AnimateExplosion()
    {
        //todo
    }

    private void FlipX()
    {
       this.RenderReversed = true;
       this.Status = NORMAL;
    }
}