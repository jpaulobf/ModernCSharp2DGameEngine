namespace Game;

/**
 * Author: Joao P B Faria
 * Date: Sept/2022
 * Description: Class representing static sprites (like houses & fuel)
 */
public class StaticSprite : GameSprite
{
    private IGame GameRef;

    /**
     * Static Sprite constructor
     */
    public StaticSprite(IGame game, string imageFilePath, int width, int height, int X = 0, int Y = 0) : base(imageFilePath, width, height, X, Y, 0) {
        this.GameRef = game;
    }

    /**
     * update
     */
    public override void Update(long timeframe, bool colliding = false) {
        this.SourceRect = new Rectangle(0, 0, (short)this.Width, (short)this.Height);
        this.DestineRect = new Rectangle((short)this.X, (short)this.Y, (short)this.Width, (short)this.Height);

        if (this.CollisionDetection(this.GameRef.GetPlayerSprite())) {
            //Console.WriteLine("Fuel...");
        }
    }
    
    /**
     * Reset the static sprite
     */
    public override void Reset()
    {
    }
}