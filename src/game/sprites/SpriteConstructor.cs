namespace Game;

/**
 * Author: Joao P B Faria
 * Date: Oct/2022
 * Description: Class responsible for create the sprites (as a gateway)
 */
public class SpriteConstructor 
{
    private IGame GameRef;
    public int X { get; set; }
    public int Y { get; set; }
    public byte Type { get; set; }
    public int Parameter { get; set; }
    public bool Flag { get; set; }
    private GameSprite GameSprite;
    public static byte HOUSE = 1;
    public static byte HOUSE2 = 2;
    public static byte FUEL = 3;
    public static byte HELI = 4;
    public static byte SHIP = 5;
    public static byte LEFT = 1;
    public static byte RIGHT = 2;

    /**
     * Constructor
     */
    public SpriteConstructor(IGame game, byte type, int X, int Y, byte parameter = 1, bool flag = false, short maxLeft = 0, short maxRight = 0, byte direction = 0) {
        this.GameRef = game;
        this.X = X;
        this.Y = Y;
        this.Type = type;
        this.Parameter = parameter;
        this.Flag = flag;
        if (type == HOUSE) {
            this.GameSprite = new StaticSprite(game, "img\\house.png", 73, 44, X);
        } else if (type == HOUSE2) {
            this.GameSprite = new StaticSprite(game, "img\\house2.png", 73, 44, X);
        } else if (type == FUEL) {
            this.GameSprite = new StaticSprite(game, "img\\fuel.png", 32, 55, X);
        } else if (type == SHIP) {
            this.GameSprite = new EnemySprite(game, type, "img\\ship.png", 73, 18, X, 0, 100, parameter, 25, flag, maxLeft, maxRight, direction);
        } else {
            this.GameSprite = new EnemySprite(game, type, "img\\helitile.png", 36, 23, X, 0, 100, parameter, 25, flag, maxLeft, maxRight, direction);
        }
    }

    /**
     * Reset the Sprite
     */
    public void Reset() {
        this.GameSprite.Reset();
    }

    /**
     * Update the sprites
     */
    internal void Update(long frametime, int currentLineYPosition, int offset, int Y, bool colliding) {
        this.GameSprite.Y = Y - currentLineYPosition + offset;
        this.GameSprite.RenderReversed = this.Flag;
        this.GameSprite.Update(frametime, colliding);
    }

    /**
     * Draw the sprite
     */
    internal void Draw(Graphics gfx) {
        this.GameSprite.Draw(gfx);
    }

    /**
     * Return the GameSprite
     */
    internal GameSprite GetGameSprite() 
    { 
        return (this.GameSprite);
    }
}