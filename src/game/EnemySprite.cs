namespace game;

public class EnemySprite : GameSprite {

    /**
     * Enemy Sprite class
     */
    public EnemySprite(string imageFilePath, int width, int height, int X, int Y, int velocity, byte tilesNumber, int millisecondsPerFrame) : base(imageFilePath, width, height, X, Y, velocity) {
        this.TilesNumber = tilesNumber;
        this.MillisecondsPerFrame = millisecondsPerFrame;
    }

    /**
     * Enemy Sprite update class
     */
    public override void Update(long timeframe)
    {
        this.framecounter += timeframe;

        if (this.framecounter < this.MillisecondsPerFrame * 10_000) {
            StartX = 0;
        } else {
            StartX = (short)Width;
            if (this.framecounter > this.MillisecondsPerFrame * 10_000 * 2) {
                this.framecounter = 0;
            }
        }
        
        this.SourceRect = new Rectangle(this.StartX, this.StartY, (short)this.Width, (short)this.Height);
        this.DestineRect = new Rectangle((short)this.X, (short)this.Y, (short)this.Width, (short)this.Height);
    }
}