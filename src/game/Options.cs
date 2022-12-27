namespace Game;

public class Options
{
    private IGame GameRef;
    private byte currentPosition = 0;

    /**
     * Constructor
     */
    public Options(IGame game) 
    {
        this.GameRef = game;
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
        gfx.FillRectangle(new SolidBrush(Color.FromArgb(255, 58, 80, 74)), 0, 0, GameRef.GetInternalResolutionWidth(), GameRef.GetInternalResolutionHeight());
        //gfx.DrawImage(this.FuelMeter, this.FuelMeterX, this.FuelMeterY, this.FuelMeter.Width, this.FuelMeter.Height);
        //TODO
    }

    /**
     * Reset HUD parameters
     */
    internal void Reset()
    {
    }

    public void KeyUp(object? sender, KeyEventArgs e)
    {
        if (e.KeyValue == 38) //up
        {
            if (this.currentPosition == 0)
            {
                this.currentPosition = 2;
            }
            else 
            {
                this.currentPosition--;
            }
        }
        else if (e.KeyValue == 40) //down
        {
            if (this.currentPosition == 2)
            {
                this.currentPosition = 0;
            }
            else 
            {
                this.currentPosition++;
            }
        }
        else if (e.KeyValue == 32 || e.KeyValue == 13) //space or enter
        {
            //todo
        } 
        else if (e.KeyValue == 27) //ESC
        {
            //back to menu
            this.GameRef.SetGameStateToMenu();
        }
    }
}