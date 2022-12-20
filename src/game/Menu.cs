namespace Game;

using Util;

public class Menu
{
    private IGame GameRef;
    private Bitmap MainLogo;
    private Bitmap Selector;
    private Bitmap LabelStart;
    private Bitmap LabelOptions;
    private Bitmap LabelExit;
    private int MainLogoX       = 372;
    private int MainLogoY       = 40;
    private int SelectorX       = 78;
    private int SelectorY       = 484;
    private int LabelStartX     = 545;
    private int LabelStartY     = 498;
    private int LabelOptionsX   = 611;
    private int LabelOptionsY   = 574;
    private int LabelExitX      = 592;
    private int LabelExitY      = 650;

    /**
     * Constructor
     */
    public Menu(IGame game) 
    {
        this.GameRef        = game;
        this.MainLogo       = LoadingStuffs.GetInstance().GetImage("main-logo");
        this.Selector       = LoadingStuffs.GetInstance().GetImage("selector");
        this.LabelStart     = LoadingStuffs.GetInstance().GetImage("label-start");
        this.LabelOptions   = LoadingStuffs.GetInstance().GetImage("label-options");
        this.LabelExit      = LoadingStuffs.GetInstance().GetImage("label-exit");
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
        gfx.DrawImage(this.MainLogo, this.MainLogoX, this.MainLogoY, this.MainLogo.Width, this.MainLogo.Height);
        gfx.DrawImage(this.Selector, this.SelectorX, this.SelectorY, this.Selector.Width, this.Selector.Height);
        gfx.DrawImage(this.LabelStart, this.LabelStartX, this.LabelStartY, this.LabelStart.Width, this.LabelStart.Height);
        gfx.DrawImage(this.LabelOptions, this.LabelOptionsX, this.LabelOptionsY, this.LabelOptions.Width, this.LabelOptions.Height);
        gfx.DrawImage(this.LabelExit, this.LabelExitX, this.LabelExitY, this.LabelExit.Width, this.LabelExit.Height);
    }

    /**
     * Reset HUD parameters
     */
    internal void Reset()
    {
    }
}