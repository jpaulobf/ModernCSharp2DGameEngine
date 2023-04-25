namespace Game;

public class GameOver
{
    private IGame GameRef;
    private long Framecounter = 0;

    /**
     * Constructor
     */
    public GameOver(IGame game) 
    {
        this.GameRef = game;
    }

    /**
     * Update the HUD
     */
    public void Update(long frametime) 
    {
        this.Framecounter += frametime;
        if (this.Framecounter == frametime) 
        {
            //stop music
            //start gameover music
        }
    }

    /**
     * Draw the HUD
     */
    public void Draw(Graphics gfx) 
    {
        //gfx.FillRectangle(Brushes.Black, this.SeparatorRect);
        //gfx.DrawImage(this.FuelMeter, this.FuelMeterX, this.FuelMeterY, this.FuelMeter.Width, this.FuelMeter.Height);
        //TODO
    }

    /**
     * Reset HUD parameters
     */
    internal void Reset()
    {
    }
}