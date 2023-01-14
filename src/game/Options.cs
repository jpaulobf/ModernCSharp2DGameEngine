namespace Game;

using Util;

public class Options
{
    private IGame GameRef;
    private byte CurrentPosition = 0;
    private Bitmap Selector;
    private int SelectorX           = 66;
    private const int SelectorYBase = 106;
    private const int SelectorYDiff = 70;
    private int SelectorY           = SelectorYBase;

    /**
     * Constructor
     */
    public Options(IGame game) 
    {
        this.GameRef    = game;
        this.Selector   = LoadingStuffs.GetInstance().GetImage("selector");
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
            //todo
        } 
        else if (e.KeyValue == 27) //ESC
        {
            //back to menu
            this.GameRef.SetGameStateToMenu();
        }

        this.SelectorY = SelectorYBase + (this.CurrentPosition * SelectorYDiff);
    }
}