namespace game;

public class PlayerSprite : GameSprite {

    private GameInterface GameRef;
    private Bitmap LocalSpriteImage;
    private Bitmap SpriteSplosion;
    private float OgWidth           = 0;
    private float OgHeight          = 0;
    private int OgX                 = 0;
    private int OgY                 = 0;
    private SolidBrush YellowBrush  = new SolidBrush(Color.FromArgb(255, 232, 232, 74));
    public bool Colliding { get; set; } = false;

    /**
     * Player Sprite constructor
     */
    public PlayerSprite(GameInterface game, string imageFilePath, int width, int height, int X, int Y, int velocity) : base(imageFilePath, width, height, X, Y, velocity) {
        this.GameRef            = game;
        this.SpriteSplosion     = new Bitmap(@"img\\sprite_splosion.png");
        this.LocalSpriteImage   = new Bitmap(@imageFilePath);
        this.OgWidth            = width;
        this.OgHeight           = height;
        this.OgX                = X;
        this.OgY                = Y;
    }

    public void SetCollision() {
        this.Colliding = true;
    }

    /**
     * Player Sprite update method
     */
    public override void Update(long timeframe, bool colliding = false) {
        if (!this.Colliding) {
            if (!this.Lefting && !this.Righting) {
                this.SourceStartX = (short)Width; //Default
            } else if (this.Lefting) { //Lefting
                this.SourceStartX = 0;
            } else if (this.Righting) { //Righting
                this.SourceStartX = (short)(Width * 2);
            }
        } else {
            this.SourceStartX = 0;
            this.Width = 32;
            this.Height = 25;
            this.SpriteImage = this.SpriteSplosion;
        }

        //define the source & destine rect
        this.SourceRect = new Rectangle(SourceStartX, SourceStartY, (short)Width - 1, (short)Height);
        this.DestineRect = new Rectangle((short)X, (short)Y, (short)Width, (short)Height);
    }

    /**
     * Reset player sprite
     */
    public override void Reset()
    {
        this.SpriteImage    = this.LocalSpriteImage;
        this.Colliding      = false;
        this.Width          = this.OgWidth;
        this.Height         = this.OgHeight;
        this.X              = this.OgX;
        this.Y              = this.OgY;
    }
}