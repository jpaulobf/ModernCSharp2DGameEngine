namespace game;

public class EnemySprite : GameSprite {

    private GameInterface gameref;

    /**
     * Enemy Sprite class
     */
    public EnemySprite(GameInterface game, string imageFilePath, int width, int height, int X = 0, int Y = 0, int velocity = 0, byte tilesNumber = 1, int millisecondsPerFrame = 0, bool reversed = false) : base(imageFilePath, width, height, X, Y, velocity) {
        this.TilesNumber = tilesNumber;
        this.MillisecondsPerFrame = millisecondsPerFrame;
        this.gameref = game;
        this.RenderReversed = reversed;
    }

    /**
     * Enemy Sprite update class
     */
    public override void Update(long timeframe)
    {
        this.framecounter += timeframe;

        if (this.TilesNumber > 1) {
            if (this.framecounter < this.MillisecondsPerFrame * 10_000) {
                this.StartX = 0;
            } else {
                this.StartX = (short)Width;
                if (this.framecounter > this.MillisecondsPerFrame * 10_000 * 2) {
                    this.framecounter = 0;
                }
            }
        } else {
            this.StartX = 0;
        }
        
        this.SourceRect = new Rectangle(this.StartX, this.StartY, (short)this.Width, (short)this.Height);
        this.DestineRect = new Rectangle((short)this.X, (short)this.Y, (short)this.Width, (short)this.Height);
    }
}