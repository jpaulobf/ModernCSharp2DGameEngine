namespace game.stages;

public class SpriteConstructor {
    private GameInterface gameRef;
    public int X { get; set; }
    public int Y { get; set; }
    public byte type { get; set; }
    public int parameter { get; set; }
    public bool flag { get; set; }
    private GameSprite gamesprite;
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
    public SpriteConstructor(GameInterface game, byte type, int X, byte parameter = 1, bool flag = false, short maxLeft = 0, short maxRight = 0, byte direction = 0) {
        this.gameRef = game;
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
            this.gamesprite = new EnemySprite(game, type, "img\\ship.png", 73, 18, X, 0, 100, parameter, 25, flag, maxLeft, maxRight, direction);
        } else {
            this.gamesprite = new EnemySprite(game, type, "img\\helitile.png", 36, 23, X, 0, 100, parameter, 25, flag, maxLeft, maxRight, direction);
        }
    }

    public void Render(Graphics gfx, long frametime, int currentLineYPosition, int offset, int Y, bool colliding) {
        this.Update(frametime, currentLineYPosition, offset, Y, colliding);
        this.Draw(gfx);
    }

    public void Reset() {
        this.gamesprite.Reset();
    }

    private void Update(long frametime, int currentLineYPosition, int offset, int Y, bool colliding) {
        this.gamesprite.Y = Y - currentLineYPosition + offset;
        this.gamesprite.RenderReversed = this.flag;
        this.gamesprite.Update(frametime, colliding);
    }

    /**
     * Render method
     */
    private void Draw(Graphics gfx) {
        this.gamesprite.Draw(gfx);
    }
}