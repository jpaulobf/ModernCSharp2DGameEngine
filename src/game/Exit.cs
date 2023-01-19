namespace Game;

using Util;

public class Exit
{
    private IGame GameRef;
    private byte CurrentPosition    = 0;
    private short ExitWindowX       = 0;
    private short ExitWindowY       = 0;
    private short ReallyX           = 0;
    private short ReallyY           = 0;
    private short YesX              = 0;
    private short YesY              = 0;
    private short NoX               = 0;
    private short NoY               = 0;
    private short YesBtX            = 0;
    private short YesBtY            = 0;
    private short NoBtX             = 0;
    private short NoBtY             = 0;
    private volatile bool exit      = false;
    private Bitmap ReallyBitmap     = Util.LoadingStuffs.GetInstance().GetImage("really");
    private Bitmap YesBitmap        = Util.LoadingStuffs.GetInstance().GetImage("bt-yes");
    private Bitmap NoBitmap         = Util.LoadingStuffs.GetInstance().GetImage("bt-no");
    private Brush [] brushes        = { new SolidBrush(Color.FromArgb(255, 200, 200, 190)),
                                        new SolidBrush(Color.FromArgb(255, 170, 170, 170)),
                                        new SolidBrush(Color.FromArgb(150, 50, 69, 64))};
    private Pen [] pens             = { new Pen(Color.FromArgb(255, 0, 0, 0)),
                                        new Pen(Color.FromArgb(180, 0, 0, 0))};

    /**
     * Constructor
     */
    public Exit(IGame game) 
    {
        this.GameRef = game;
        this.ExitWindowX    = (short)((game.GetInternalResolutionWidth() / 2) - 120);
        this.ExitWindowY    = (short)((game.GetInternalResolutionHeight() / 2) - 80);
        this.ReallyX        = (short)(this.ExitWindowX + 20);
        this.ReallyY        = (short)(this.ExitWindowY + 20);
        this.YesX           = (short)(this.ExitWindowX + 30);
        this.YesY           = (short)(this.ExitWindowY + 60);
        this.NoX            = (short)(this.ExitWindowX + 130);
        this.NoY            = (short)(this.ExitWindowY + 60);
        this.YesBtX         = (short)(this.YesX + 18);
        this.YesBtY         = (short)(this.YesY + 8);
        this.NoBtX          = (short)(this.NoX + 24);
        this.NoBtY          = (short)(this.NoY + 8);
    }

    /**
     * Update the HUD
     */
    public void Update(long frametime) 
    {
        if (this.exit)
        {
            this.Reset();
            this.GameRef.ToMenu();
            this.GameRef.Reset();
            this.GameRef.SkipDrawOnce();
            this.GameRef.SkipRenderOnce();
        }
    }

    /**
     * Draw the HUD
     */
    public void Draw(Graphics gfx) 
    {
        gfx.FillRectangle(brushes[0], ExitWindowX, ExitWindowY, 240, 120);
        gfx.DrawRectangle(pens[0], ExitWindowX, ExitWindowY, 240, 120);
        gfx.DrawImage(ReallyBitmap, ReallyX, ReallyY, ReallyBitmap.Width, ReallyBitmap.Height);
        gfx.FillRectangle(brushes[1], YesX, YesY, 80, 30);
        gfx.FillRectangle(brushes[1], NoX, NoY, 80, 30);
        gfx.DrawRectangle(pens[1], YesX, YesY, 80, 30);
        gfx.DrawRectangle(pens[1], NoX, NoY, 80, 30);

        if (this.CurrentPosition == 0)
            gfx.FillRectangle(brushes[2], YesX, YesY, 80, 30);
        else 
            gfx.FillRectangle(brushes[2], NoX, NoY, 80, 30);

        gfx.DrawImage(YesBitmap, YesBtX, YesBtY, YesBitmap.Width, YesBitmap.Height);
        gfx.DrawImage(NoBitmap, NoBtX, NoBtY, NoBitmap.Width, NoBitmap.Height);
    }

    /**
     * Reset HUD parameters
     */
    internal void Reset()
    {
        this.CurrentPosition = 0;
    }

    public void KeyUp(object? sender, KeyEventArgs e)
    {
        if (e.KeyValue == 37) //left
        {
            if (this.CurrentPosition == 0)
            {
                this.CurrentPosition = 1;
            }
            else 
            {
                this.CurrentPosition--;
            }
        }
        else if (e.KeyValue == 39) //right
        {
            if (this.CurrentPosition == 1)
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
                this.exit = true;
            }
            else if (this.CurrentPosition == 1)
            {
                this.Reset();
                this.GameRef.SetGameStateToInGame();
            }
        }
        else if (e.KeyValue == 27)
        {
            this.GameRef.SetGameStateToInGame();
        }
    }
}