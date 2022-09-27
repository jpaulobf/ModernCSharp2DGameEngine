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

    /**
     * Update the HUD
     */
    public void Update(long frametime) {

    }

    /**
     * Draw the HUD
     */
    public void Draw(Graphics gfx) {
        gfx.FillRectangle(Brushes.Black, this.rectSep);
        gfx.FillRectangle(new SolidBrush(Color.FromArgb(255, 144, 144, 144)), this.rect);
    }

    /**
     * Reset HUD parameters
     */
    internal void Reset()
    {
       this.PlayerLives = 5;
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