namespace Game;

using Util;

public class Options
{
    private IGame GameRef;
    private Bitmap Selector;
    private Bitmap LabelPlayMusic;
    private Bitmap LabelPlaySFX;
    private Bitmap LabelExitOptions;
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
    private int LabelExitOptionsX   = 38;
    private int LabelExitOptionsY   = 627;

    /**
     * Constructor
     */
    public Options(IGame game) 
    {
        this.GameRef            = game;
        this.Selector           = LoadingStuffs.GetInstance().GetImage("selector");
        this.LabelPlayMusic     = LoadingStuffs.GetInstance().GetImage("label-play-music");
        this.LabelPlaySFX       = LoadingStuffs.GetInstance().GetImage("label-play-sfx");
        this.LabelExitOptions   = LoadingStuffs.GetInstance().GetImage("label-exit-options");
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
        gfx.DrawImage(this.LabelExitOptions, this.LabelExitOptionsX, this.LabelExitOptionsY, this.LabelExitOptions.Width, this.LabelExitOptions.Height);


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
            if (this.CurrentPosition == 2)
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

        if (this.CurrentPosition == 2)
        {
            diff = 373;
            diffY = -50;
        }

        this.SelectorY = SelectorYBase + (this.CurrentPosition * SelectorYDiff) + diff;
        this.SelectorX = OgSelectorX + diffY;
    }
}