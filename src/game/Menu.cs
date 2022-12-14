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
    private int MainLogoX           = 222;
    private int MainLogoY           = 48;
    private int SelectorX           = 51;
    private const int SelectorYBase = 450;
    private const int SelectorYDiff = 76;
    private int SelectorY           = SelectorYBase;
    private int LabelStartX         = 362;
    private int LabelStartY         = 464;
    private int LabelOptionsX       = 428;
    private int LabelOptionsY       = 540;
    private int LabelExitX          = 409;
    private int LabelExitY          = 616;
    private byte CurrentPosition    = 0;

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
        this.SelectorY = SelectorYBase + (this.CurrentPosition * SelectorYDiff);
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

    public void KeyUp(object? sender, KeyEventArgs e)
    {
        if (e.KeyValue == 38) //up
        {
            if (this.CurrentPosition == 0)
            {
                this.CurrentPosition = 2;
            }
            else 
            {
                this.CurrentPosition--;
            }
        }
        else if (e.KeyValue == 40) //down
        {
            if (this.CurrentPosition == 2)
            {
                this.CurrentPosition = 0;
            }
            else 
            {
                this.CurrentPosition++;
            }
        }
        else if (e.KeyValue == 32 || e.KeyValue == 13) //space or enter
        {
            if (this.CurrentPosition == 0)
            {
                this.GameRef.InitGameConfigurations();
                this.GameRef.SetGameStateToInGame();
            }
            else if (this.CurrentPosition == 1)
            {
                this.GameRef.SetGameStateToOptions();
            }
            else if (this.CurrentPosition == 2)
            {
                this.GameRef.ExitGame();
            }
        }
    }
}