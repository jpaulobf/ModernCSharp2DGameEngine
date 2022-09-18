namespace game.stages;

public class SpriteConstructor {
    public int X { get; set; }
    public int Y { get; set; }
    public int type { get; set; }
    public int parameter { get; set; }
    public bool flag { get; set; }
    private GameSprite gamesprite;
    public static byte HOUSE = 1;
    public static byte HOUSE2 = 2;
    public static byte FUEL = 3;
    public static byte SHIP = 4;
    public static byte HELI = 5;
    public static byte LEFT = 1;
    public static byte RIGHT = 2;

    /**
     * Constructor
     */
    public SpriteConstructor(GameInterface game, int type, int X, byte parameter = 1, bool flag = false, short maxLeft = 0, short maxRight = 0, byte direction = 0) {
        this.X = X;
        this.Y = Y;
        this.type = type;
        this.parameter = parameter;
        this.flag = flag;
        if (type == HOUSE) {
            this.gamesprite = new StaticSprite(game, "img\\house.png", 73, 44, X);
        } else if (type == HOUSE2) {
            this.gamesprite = new StaticSprite(game, "img\\house2.png", 73, 44, X);
        } else if (type == FUEL) {
            this.gamesprite = new StaticSprite(game, "img\\fuel.png", 32, 55, X);
        } else if (type == SHIP) {
            this.gamesprite = new EnemySprite(game, "img\\ship.png", 73, 18, X);
        } else {
            this.gamesprite = new EnemySprite(game, "img\\helitile.png", 36, 23, X, 0, 100, parameter, 25, flag, maxLeft, maxRight, direction);
        }
    }

    /**
     * Render method
     */
    public void Render(long frametime, Graphics gfx, int currentLineYPosition, int offset, int Y) {
        this.gamesprite.Y = Y - currentLineYPosition + offset;
        this.gamesprite.RenderReversed = this.flag;
        this.gamesprite.Update(frametime);
        this.gamesprite.Draw(gfx);
    }
}