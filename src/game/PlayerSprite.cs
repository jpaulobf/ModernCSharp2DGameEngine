namespace game;

public class PlayerSprite : GameSprite {

    private GameInterface gameref;
    private Bitmap LocalSpriteImage;
    private SolidBrush yellow = new SolidBrush(Color.FromArgb(255, 232, 232, 74));

    /**
     * Player Sprite constructor
     */
    public PlayerSprite(GameInterface game, string imageFilePath, int width, int height, int X, int Y, int velocity) : base(imageFilePath, width, height, X, Y, velocity) {
        this.gameref = game;
        this.LocalSpriteImage = new Bitmap(width, height);
        this.Render();
    }

    private void Render() {
        Graphics g = Graphics.FromImage(this.LocalSpriteImage);
        g.FillRectangle(yellow, new Rectangle(13,0,6,6));
        g.FillRectangle(yellow, new Rectangle(9,6,14,3));
        g.FillRectangle(yellow, new Rectangle(4,9,24,2));
        g.FillRectangle(yellow, new Rectangle(0,11,32,5));
        g.FillRectangle(yellow, new Rectangle(0,16,9,2));
        g.FillRectangle(yellow, new Rectangle(23,16,9,2));
        g.FillRectangle(yellow, new Rectangle(0,18,4,2));
        g.FillRectangle(yellow, new Rectangle(28,18,4,2));
        g.FillRectangle(yellow, new Rectangle(13,16,6,14));
        g.FillRectangle(yellow, new Rectangle(4,25,5,5));
        g.FillRectangle(yellow, new Rectangle(23,25,5,5));
        g.FillRectangle(yellow, new Rectangle(9,23,4,4));
        g.FillRectangle(yellow, new Rectangle(19,23,4,4));
    }

    /**
     * Player Sprite update method
     */
    public override void Update(long timeframe) {
        if (!this.Lefting && !this.Righting) {
            //Default
            this.StartX = (short)Width;
        } else if (this.Lefting) { //Lefting
            this.StartX = 0;
        } else if (this.Righting) { //Righting
            this.StartX = (short)(Width * 2);
        }

        this.SourceRect = new Rectangle(StartX, StartY, (short)Width - 1, (short)Height);
        //this.SourceRect = new Rectangle(0, 0, (short)Width, (short)Height);
        this.DestineRect = new Rectangle((short)X, (short)Y, (short)Width, (short)Height);
    }

    //public void Draw(Graphics gfx) {
    //    gfx.DrawImage(this.LocalSpriteImage, this.DestineRect, this.SourceRect, System.Drawing.GraphicsUnit.Pixel);
   // }
}