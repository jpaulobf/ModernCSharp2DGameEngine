namespace game;

public class PlayerSprite : GameSprite {

    private GameInterface GameRef;
    private Bitmap LocalSpriteImage;
    private Bitmap SpriteSplosion;
    private float OgWidth           = 0;
    private float OgHeight          = 0;
    private SolidBrush YellowBrush  = new SolidBrush(Color.FromArgb(255, 232, 232, 74));
    private bool Coliding           = false;

    /**
     * Player Sprite constructor
     */
    public PlayerSprite(GameInterface game, string imageFilePath, int width, int height, int X, int Y, int velocity) : base(imageFilePath, width, height, X, Y, velocity) {
        this.GameRef            = game;
        this.SpriteSplosion     = new Bitmap(@"img\\sprite_splosion.png");
        this.LocalSpriteImage   = new Bitmap(@imageFilePath);
        this.OgWidth            = width;
        this.OgHeight           = height;
    }

    public void SetColision() {
        this.Coliding = true;
        this.AnimateExplosion();
    }

    /**
     * Animate the sprite colision
     */
    private void AnimateExplosion() {
        //TODO
    }   

    /**
     * Player Sprite update method
     */
    public override void Update(long timeframe) {
        if (!this.Coliding) {
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
    internal void Reset() {
        this.SpriteImage    = this.LocalSpriteImage;
        this.Coliding       = false;
        this.Width          = this.OgWidth;
        this.Height         = this.OgHeight;
    }
}