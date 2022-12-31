namespace Game;

using Util;

public class Exit
{
    private IGame GameRef;
    private byte currentPosition    = 0;
    private short ExitWindowX = 0;
    private short ExitWindowY = 0;
    private short ReallyX = 0;
    private short ReallyY = 0;
    private short YesX = 0;
    private short YesY = 0;
    private short NoX = 0;
    private short NoY = 0;
    private short YesBtX = 0;
    private short YesBtY = 0;
    private short NoBtX = 0;
    private short NoBtY = 0;
    private Bitmap ReallyBitmap = Util.LoadingStuffs.GetInstance().GetImage("really");
    private Bitmap YesBitmap    = Util.LoadingStuffs.GetInstance().GetImage("bt-yes");
    private Bitmap NoBitmap    = Util.LoadingStuffs.GetInstance().GetImage("bt-no");

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
    }

    /**
     * Draw the HUD
     */
    public void Draw(Graphics gfx) 
    {
        gfx.FillRectangle(new SolidBrush(Color.FromArgb(255, 200, 200, 190)), ExitWindowX, ExitWindowY, 240, 120);
        gfx.DrawRectangle(new Pen(Color.FromArgb(255, 0, 0, 0)), ExitWindowX, ExitWindowY, 240, 120);
        gfx.DrawImage(ReallyBitmap, ReallyX, ReallyY, ReallyBitmap.Width, ReallyBitmap.Height);
        gfx.FillRectangle(new SolidBrush(Color.FromArgb(255, 170, 170, 170)), YesX, YesY, 80, 30);
        gfx.FillRectangle(new SolidBrush(Color.FromArgb(255, 170, 170, 170)), NoX, NoY, 80, 30);
        gfx.DrawRectangle(new Pen(Color.FromArgb(255, 0, 0, 0)), YesX, YesY, 80, 30);
        gfx.DrawRectangle(new Pen(Color.FromArgb(255, 0, 0, 0)), NoX, NoY, 80, 30);

        gfx.DrawImage(YesBitmap, YesBtX, YesBtY, YesBitmap.Width, YesBitmap.Height);
        gfx.DrawImage(NoBitmap, NoBtX, NoBtY, NoBitmap.Width, NoBitmap.Height);
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