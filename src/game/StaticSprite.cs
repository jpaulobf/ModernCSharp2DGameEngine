namespace game;

public class StaticSprite : GameSprite
{
    private GameInterface GameRef;

    /**
     * Static Sprite constructor
     */
    public StaticSprite(GameInterface game, string imageFilePath, int width, int height, int X = 0, int Y = 0) : base(imageFilePath, width, height, X, Y, 0) {
        this.GameRef = game;
    }

    /**
     * update
     */ 
    public override void Update(long timeframe) {
        this.SourceRect = new Rectangle(0, 0, (short)this.Width, (short)this.Height);
        this.DestineRect = new Rectangle((short)this.X, (short)this.Y, (short)this.Width, (short)this.Height);

        if (this.ColisionDetection(this.GameRef.GetPlayerSprite())) {
            Console.WriteLine("Fuel...");
        }
    }
}