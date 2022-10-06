namespace Game;

public class HUD 
{
    private Rectangle SeparatorRect;
    private Rectangle HudRect;
    private IGame GameRef;
    public byte PlayerLives {get;set;} = 5;

    public HUD(IGame game) 
    {
        this.GameRef        = game;
        this.SeparatorRect  = new Rectangle(0, 428, 738, 3);
        this.HudRect        = new Rectangle(0, 431, 738, 85);
    }

    /**
     * Update the HUD
     */
    public void Update(long frametime) 
    {

    }

    /**
     * Draw the HUD
     */
    public void Draw(Graphics gfx) 
    {
        gfx.FillRectangle(Brushes.Black, this.SeparatorRect);
        gfx.FillRectangle(new SolidBrush(Color.FromArgb(255, 144, 144, 144)), this.HudRect);
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