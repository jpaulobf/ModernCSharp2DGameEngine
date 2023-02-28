namespace Game;

using Util;

public class Ending
{
    private IGame GameRef;

    /**
     * Constructor
     */
    public Ending(IGame game) 
    {
        this.GameRef = game;
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
        Console.WriteLine("aui........");
    }

    /**
     * Reset HUD parameters
     */
    internal void Reset()
    {

    }
}