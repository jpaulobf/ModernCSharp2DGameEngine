namespace Game;

using Util;

public class Ending
{
    private IGame GameRef;
    private Bitmap TheEnd     = Util.LoadingStuffs.GetInstance().GetImage("the-end");

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
        gfx.DrawImage(this.TheEnd, 200, 200, this.TheEnd.Width, this.TheEnd.Height);
    }

    /**
     * Reset HUD parameters
     */
    internal void Reset()
    {

    }
}