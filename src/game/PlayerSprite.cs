namespace game;

public class PlayerSprite : GameSprite {

    private GameInterface gameref;

    /**
     * Player Sprite constructor
     */
    public PlayerSprite(GameInterface game, string imageFilePath, int width, int height, int X, int Y, int velocity) : base(imageFilePath, width, height, X, Y, velocity) {
        this.gameref = game;
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
        this.DestineRect = new Rectangle((short)X, (short)Y, (short)Width, (short)Height);
    }
}