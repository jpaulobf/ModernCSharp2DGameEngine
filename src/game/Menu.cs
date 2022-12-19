namespace Game;

using Util;

public class Menu
{
    private IGame GameRef;
    private Bitmap MainLogo;

    /**
     * Constructor
     */
    public Menu(IGame game) 
    {
        this.GameRef        = game;
        this.MainLogo       = LoadingStuffs.GetInstance().GetImage("main-logo");
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
        gfx.DrawImage(this.MainLogo, 0, 0, this.MainLogo.Width, this.MainLogo.Height);
        //TODO
    }

    /**
     * Reset HUD parameters
     */
    internal void Reset()
    {
    }
}