namespace Game;

/**
 * Author: Joao P B Faria
 * Date: Oct/2022
 * Description: Represents player shot
 */
public class Shot : GameSprite
{
    private IGame GameRef;

    /**
     * Constructor
     */
    public Shot(string imageFilePath, int width, int height, int X, int Y, int velocity) : base(imageFilePath, width, height, X, Y, velocity)
    {

    }

    /**
     * control & update each shot
     */
    public override void Update(long timeframe, bool colliding = false)
    {
        this.SourceRect = new Rectangle(0, 0, (short)this.Width, (short)this.Height);
        this.DestineRect = new Rectangle((short)this.X, (short)this.Y, (short)this.Width, (short)this.Height);

        foreach (var item in this.GameRef.GetStageSprites()) {
            if (this.CollisionDetection(item.GetGameSprite())) {
                Console.WriteLine("boooommm");
            }
        }
    }
    
    /**
     * reset
     */
    public override void Reset()
    {

    }
}