namespace game;

public class HUD {

    private Rectangle rectSep;
    private Rectangle rect;
    private GameInterface gameref;
    public byte PlayerLives {get;set;} = 5;

    public HUD(GameInterface game) {
        this.gameref = game;
        this.rectSep = new Rectangle(0, 428, 738, 3);
        this.rect = new Rectangle(0, 431, 738, 85);
    }

    public void Update(long frametime) {

    }

    public void Draw(Graphics gfx) {
        gfx.FillRectangle(Brushes.Black, this.rectSep);
        gfx.FillRectangle(new SolidBrush(Color.FromArgb(255, 144, 144, 144)), this.rect);
    }

    internal void Reset()
    {
        //TODO
    }

    /**
     * Verify if player is still alive
     */
    internal bool PlayerIsAlive()
    {
        return (this.PlayerLives > 0);
    }

    /**
     * Decrease the current player live number
     */
    internal void PlayerDecreaseLive()
    {
        this.PlayerLives--;
    }
}