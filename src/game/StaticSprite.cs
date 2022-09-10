namespace game;

public class StaticSprite : GameSprite
{
    /**
     * Static Sprite constructor
     */
    public StaticSprite(string imageFilePath, int width, int height, int X, int Y) : base(imageFilePath, width, height, X, Y, 0) {

    }

    /**
     * update
     */ 
    public override void Update(long timeframe) {
        this.SourceRect = new Rectangle(0, 0, (short)this.Width, (short)this.Height);
        this.DestineRect = new Rectangle((short)this.X, (short)this.Y, (short)this.Width, (short)this.Height);
    }
}