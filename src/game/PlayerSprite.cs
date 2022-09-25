namespace game;

public class PlayerSprite : GameSprite {

    private GameInterface GameRef;
    private Bitmap LocalSpriteImage;
    private Bitmap SpriteSplosion;
    private SolidBrush YellowBrush = new SolidBrush(Color.FromArgb(255, 232, 232, 74));
    private bool Coliding = false;

    /**
     * Player Sprite constructor
     */
    public PlayerSprite(GameInterface game, string imageFilePath, int width, int height, int X, int Y, int velocity) : base(imageFilePath, width, height, X, Y, velocity) {
        this.GameRef = game;
        string splosionPath = "img\\sprite_splosion.png";
        this.SpriteSplosion = new Bitmap(@splosionPath);
        this.LocalSpriteImage = new Bitmap(width, height);
        // this.Render();
    }

    public void SetColision() {
        this.Coliding = true;
        this.AnimateExplosion();
    }

    /**
     * Animate the sprite colision
     */
    private void AnimateExplosion()
    {
        
    }

    private void Render() {
        Graphics g = Graphics.FromImage(this.LocalSpriteImage);
        g.FillRectangle(YellowBrush, new Rectangle(13,0,6,6));
        g.FillRectangle(YellowBrush, new Rectangle(9,6,14,3));
        g.FillRectangle(YellowBrush, new Rectangle(4,9,24,2));
        g.FillRectangle(YellowBrush, new Rectangle(0,11,32,5));
        g.FillRectangle(YellowBrush, new Rectangle(0,16,9,2));
        g.FillRectangle(YellowBrush, new Rectangle(23,16,9,2));
        g.FillRectangle(YellowBrush, new Rectangle(0,18,4,2));
        g.FillRectangle(YellowBrush, new Rectangle(28,18,4,2));
        g.FillRectangle(YellowBrush, new Rectangle(13,16,6,14));
        g.FillRectangle(YellowBrush, new Rectangle(4,25,5,5));
        g.FillRectangle(YellowBrush, new Rectangle(23,25,5,5));
        g.FillRectangle(YellowBrush, new Rectangle(9,23,4,4));
        g.FillRectangle(YellowBrush, new Rectangle(19,23,4,4));
    }

    /**
     * Player Sprite update method
     */
    public override void Update(long timeframe) {
        if (!this.Coliding) {
            if (!this.Lefting && !this.Righting) {
                //Default
                this.SourceStartX = (short)Width;
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

        this.SourceRect = new Rectangle(SourceStartX, SourceStartY, (short)Width - 1, (short)Height);
        this.DestineRect = new Rectangle((short)X, (short)Y, (short)Width, (short)Height);
    }
}