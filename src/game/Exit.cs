namespace Game;

using Util;

public class Exit
{
    private IGame GameRef;
    private byte currentPosition    = 0;
    private int x = 0;
    private int y = 0;
    /**
     * Constructor
     */
    public Exit(IGame game) 
    {
        this.GameRef = game;
        this.x = (game.GetInternalResolutionWidth() / 2) - 120;
        this.y = (game.GetInternalResolutionHeight() / 2) - 80;
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
        gfx.FillRectangle(new SolidBrush(Color.FromArgb(255, 200, 200, 190)), x, y, 240, 120);
        gfx.DrawRectangle(new Pen(Color.FromArgb(255, 0, 0, 0)), x, y, 240, 120);
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
            if (this.currentPosition == 0)
            {
                this.GameRef.ToMenu();
                this.GameRef.Reset();
            }
            else if (this.currentPosition == 1)
            {
                this.GameRef.SetGameStateToInGame();    
            }
        }
        else if (e.KeyValue == 27)
        {
            this.GameRef.SetGameStateToInGame();
        }
    }
}