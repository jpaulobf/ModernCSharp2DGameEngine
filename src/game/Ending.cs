namespace Game;

using Util;

public class Ending
{
    private IGame GameRef;
    private Bitmap TheEnd = Util.LoadingStuffs.GetInstance().GetImage("the-end");
    private float X = 0;
    private float Y = 0;

    /**
     * Constructor
     */
    public Ending(IGame game) 
    {
        this.GameRef = game;
        if (this.GameRef.WindowSize.Width > this.GameRef.GetInternalResolutionWidth())
        {
            this.X = (this.GameRef.WindowSize.Width / 2) - (this.TheEnd.Width * this.GameRef.GetScaleW() / 2);
            this.Y = (this.GameRef.WindowSize.Height / 2) - (this.TheEnd.Height * this.GameRef.GetScaleH() / 2);
        }
        else
        {
            this.X = (this.GameRef.GetInternalResolutionWidth() / 2) - (this.TheEnd.Width * this.GameRef.GetScaleW() / 2);
            this.Y = (this.GameRef.GetInternalResolutionHeight() / 2) - (this.TheEnd.Height * this.GameRef.GetScaleH() / 2);
        }

        this.X /= this.GameRef.GetScaleW();
        this.Y -= this.GameRef.GetHUD().GetHudHeight();
        this.Y /= this.GameRef.GetScaleH();
    }

    /**
     * Update the Ending
     */
    public void Update(long frametime) 
    {
        
    }

    /**
     * Draw the Ending Stage
     */
    public void Draw(Graphics gfx) 
    {
        //draw scenario

        //draw airplane

        //draw adictional sprites

        //draw effects

        //draw ending logo
        gfx.DrawImage(this.TheEnd, X, Y, this.TheEnd.Width, this.TheEnd.Height);
    }

    /**
     * Reset Ending parameters
     */
    internal void Reset()
    {

    }
}