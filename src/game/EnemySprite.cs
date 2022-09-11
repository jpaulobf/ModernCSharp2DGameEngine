namespace game;

public class EnemySprite : GameSprite {

    private GameInterface gameref;

    /**
     * Enemy Sprite class
     */
    public EnemySprite(GameInterface game, string imageFilePath, int width, int height, int X, int Y, int velocity, byte tilesNumber = 1, int millisecondsPerFrame = 0) : base(imageFilePath, width, height, X, Y, velocity) {
        this.TilesNumber = tilesNumber;
        this.MillisecondsPerFrame = millisecondsPerFrame;
        this.gameref = game;
        //this.RenderReversed = true;
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

        if (this.RenderReversed) {
            this.SpriteImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
            this.RenderReversed = false;
        }
    }
}