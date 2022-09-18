namespace game;

public class EnemySprite : GameSprite {

    private GameInterface GameRef;
    protected long FrameCounter = 0;
    protected short MaxLeft = 0;
    protected short MaxRight = 0;
    protected byte Direction = 0;

    /**
     * Enemy Sprite class
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
        
        this.TilesNumber = tilesNumber;
        this.MillisecondsPerTile = millisecondsPerTile;
        this.GameRef = game;
        this.RenderReversed = reversed;
        this.FrameCounter = 0;
        this.MaxLeft = maxLeft;
        this.MaxRight = maxRight;
        this.Direction = direction;
    }

    /**
     * Enemy Sprite update class
     */
    public override void Update(long timeframe)
    {
        if (this.TilesNumber > 1) {            
            this.FrameCounter += timeframe;

            if (this.FrameCounter < this.MillisecondsPerTile * 1_000) {
                this.StartX = 0;
            } else {
                this.StartX = (short)Width;
                if (this.FrameCounter > this.MillisecondsPerTile * 1_000 * 2) {
                    this.FrameCounter = 0;
                }
            }
        } else {
            this.StartX = 0;
        }
        
        this.SourceRect  = new Rectangle(this.StartX, this.StartY, (short)this.Width, (short)this.Height);
        this.DestineRect = new Rectangle((short)this.X, (short)this.Y, (short)this.Width, (short)this.Height);
    }
}