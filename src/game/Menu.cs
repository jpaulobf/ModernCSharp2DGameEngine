namespace Game;

public class Menu
{
    private IGame GameRef;

    /**
     * Constructor
     */
    public Menu(IGame game) 
    {
        this.GameRef        = game;
    }

    /**
     * Update the HUD
     */
    public void Update(long frametime) 
    {
        //TODO
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