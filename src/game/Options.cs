namespace Game;

using Util;

public class Options
{
    private IGame GameRef;
    private Bitmap Selector;
    private Bitmap LabelPlayMusic;
    private Bitmap LabelPlaySFX;
    private Bitmap LabelFullScreen;
    private Bitmap LabelStretched;
    private Bitmap LabelExitOptions;
    private Bitmap ButtonToggleOn;
    private Bitmap ButtonToggleOff;
    private bool ButtonToggle1      = true;
    private bool ButtonToggle2      = false;
    private bool ButtonToggle3      = false;
    private bool ButtonToggle4      = false;
    private byte CurrentPosition    = 0;
    private const int OgSelectorX   = 66;
    private int SelectorX           = OgSelectorX;
    private const int SelectorYBase = 106;
    private const int SelectorYDiff = 70;
    private int SelectorY           = SelectorYBase;
    private int LabelPlayMusicX     = 128;
    private int LabelPlayMusicY     = 120;
    private int LabelPlaySFXX       = 128;
    private int LabelPlaySFXY       = 190;
    private int LabelFullScreenX    = 128;
    private int LabelFullScreenY    = 260;
    private int LabelExitOptionsX   = 38;
    private int LabelExitOptionsY   = 627;
    private int LabelStretchedX     = 128;
    private int LabelStretchedY     = 330;
    private int ButtonToggle1X      = 773;
    private int ButtonToggle1Y      = 120;
    private int ButtonToggle2X      = 773;
    private int ButtonToggle2Y      = 190;
    private int ButtonToggle3X      = 773;
    private int ButtonToggle3Y      = 260;
    private int ButtonToggle4X      = 773;
    private int ButtonToggle4Y      = 330;
    internal bool Stretched {get;set;}  = false;
    internal bool Fullscreen {get;set;} = false;

    /**
     * Constructor
     */
    public Options(IGame game) 
    {
        this.GameRef            = game;
        this.Selector           = LoadingStuffs.GetInstance().GetImage("selector");
        this.LabelPlayMusic     = LoadingStuffs.GetInstance().GetImage("label-play-music");
        this.LabelPlaySFX       = LoadingStuffs.GetInstance().GetImage("label-play-sfx");
        this.LabelFullScreen    = LoadingStuffs.GetInstance().GetImage("label-fullscreen");
        this.LabelExitOptions   = LoadingStuffs.GetInstance().GetImage("label-exit-options");
        this.LabelStretched     = LoadingStuffs.GetInstance().GetImage("label-stretched");
        this.ButtonToggleOn     = LoadingStuffs.GetInstance().GetImage("button-toggle-on");
        this.ButtonToggleOff    = LoadingStuffs.GetInstance().GetImage("button-toggle-off");
    }

    /**
     * Update the HUD
     */
    public void Update(long frametime) {}

    /**
     * Draw the HUD
     */
    public void Draw(Graphics gfx) 
    {
        gfx.FillRectangle(new SolidBrush(Color.FromArgb(255, 58, 80, 74)), 0, 0, GameRef.GetInternalResolutionWidth(), GameRef.GetInternalResolutionHeight());
        gfx.DrawImage(this.Selector, this.SelectorX, this.SelectorY, this.Selector.Width, this.Selector.Height);
        gfx.DrawImage(this.LabelPlayMusic, this.LabelPlayMusicX, this.LabelPlayMusicY, this.LabelPlayMusic.Width, this.LabelPlayMusic.Height);
        gfx.DrawImage(this.LabelPlaySFX, this.LabelPlaySFXX, this.LabelPlaySFXY, this.LabelPlaySFX.Width, this.LabelPlaySFX.Height);
        gfx.DrawImage(this.LabelFullScreen, this.LabelFullScreenX, this.LabelFullScreenY, this.LabelFullScreen.Width, this.LabelFullScreen.Height);
        gfx.DrawImage(this.LabelExitOptions, this.LabelExitOptionsX, this.LabelExitOptionsY, this.LabelExitOptions.Width, this.LabelExitOptions.Height);
        gfx.DrawImage((this.ButtonToggle1)?this.ButtonToggleOn:this.ButtonToggleOff, this.ButtonToggle1X, this.ButtonToggle1Y, this.ButtonToggleOn.Width, this.ButtonToggleOn.Height);
        gfx.DrawImage((this.ButtonToggle2)?this.ButtonToggleOn:this.ButtonToggleOff, this.ButtonToggle2X, this.ButtonToggle2Y, this.ButtonToggleOn.Width, this.ButtonToggleOn.Height);
        gfx.DrawImage((this.ButtonToggle3)?this.ButtonToggleOn:this.ButtonToggleOff, this.ButtonToggle3X, this.ButtonToggle3Y, this.ButtonToggleOn.Width, this.ButtonToggleOn.Height);
        if (this.ButtonToggle3)
        {
            gfx.DrawImage(this.LabelStretched, this.LabelStretchedX, this.LabelStretchedY, this.LabelStretched.Width, this.LabelStretched.Height);
            gfx.DrawImage((this.ButtonToggle4)?this.ButtonToggleOn:this.ButtonToggleOff, this.ButtonToggle4X, this.ButtonToggle4Y, this.ButtonToggleOn.Width, this.ButtonToggleOn.Height);
        }
    }

    /**
     * Reset HUD parameters
     */
    internal void Reset()
    {
    }

    public void KeyUp(object? sender, KeyEventArgs e)
    {
        int diff = 0;
        int diffY = 0;

        if (e.KeyValue == 38) //up
        {
            if (this.CurrentPosition == 0)
            {
                this.CurrentPosition = 4;
            }
            else 
            {
                this.CurrentPosition--;
                if (!this.Fullscreen && this.CurrentPosition == 3)
                {
                    this.CurrentPosition = 2;
                }
            }
        }
        else if (e.KeyValue == 40) //down
        {
            if (this.CurrentPosition == 4)
            {
                this.CurrentPosition = 0;
            }
            else 
            {
                this.CurrentPosition++;
                if (!this.Fullscreen && this.CurrentPosition == 3)
                {
                    this.CurrentPosition = 4;
                }
            }
        }
        else if (e.KeyValue == 37 || e.KeyValue == 39) //left || right
        {
            if (this.CurrentPosition == 0)
            {
                this.ButtonToggle1 = !this.ButtonToggle1;
            }
            else if (this.CurrentPosition == 1)
            {
                this.ButtonToggle2 = !this.ButtonToggle2;
            }
            else if (this.CurrentPosition == 2)
            {
                this.ButtonToggle3 = !this.ButtonToggle3;
                this.Fullscreen = this.ButtonToggle3;
                this.GameRef.ToogleFullScreen();
            }
            else if (this.CurrentPosition == 3)
            {
                this.ButtonToggle4 = !this.ButtonToggle4;
                this.Stretched = this.ButtonToggle4;
                this.GameRef.ControlStretched();
            }
        }
        else if (e.KeyValue == 32 || e.KeyValue == 13) //space or enter
        {
            if (this.CurrentPosition == 4)
            {
                this.CurrentPosition = 0;
                this.GameRef.SetGameStateToMenu();    
            }
        } 
        else if (e.KeyValue == 27) //ESC
        {
            this.CurrentPosition = 0;
            this.GameRef.SetGameStateToMenu();
        }

        //Correct the X & Y position of selection bar for the "exit command"
        if (this.CurrentPosition == 4)
        {
            diff    = 233;
            diffY   = -50;
        }

        //Define the selector position
        this.SelectorY = SelectorYBase + (this.CurrentPosition * SelectorYDiff) + diff;
        this.SelectorX = OgSelectorX + diffY;
    }
}