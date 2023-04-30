namespace Game;

using Util;

public class GameOver
{
    private IGame GameRef;
    private long Framecounter = 0;
    private Bitmap GameOverImage;
    private float GameOverImageX = 0;
    private float GameOverImageY = 0;
    private float GameOverImageW = 0;
    private float GameOverImageH = 0;

    /**
     * Constructor
     */
    public GameOver(IGame game) 
    {
        this.GameRef        = game;
        this.GameOverImage  = LoadingStuffs.GetInstance().GetImage("gameover");
        
        this.GameOverImageW = this.GameOverImage.Width / this.GameRef.GetScaleW();
        this.GameOverImageH = this.GameOverImage.Height / this.GameRef.GetScaleH();

        this.GameOverImageX = (this.GameRef.GetInternalResolutionWidth() / 2) - (this.GameOverImageW / 2);
        this.GameOverImageY = (this.GameRef.GetInternalResolutionHeight() / 2) - (this.GameOverImageH / 2) - 30;

    }

    /**
     * Update the HUD
     */
    public void Update(long frametime) 
    {
        this.Framecounter += frametime;
        if (this.Framecounter == frametime) 
        {
            //stop music
            //start gameover music
        }
    }

    /**
     * Draw the HUD
     */
    public void Draw(Graphics gfx) 
    {
        gfx.FillRectangle(Brushes.Black, 0, 0, this.GameRef.GetInternalResolutionWidth(), this.GameRef.GetInternalResolutionHeight());
        gfx.DrawImage(this.GameOverImage, this.GameOverImageX, this.GameOverImageY, this.GameOverImageW, this.GameOverImageH);
    }

    /**
     * Reset HUD parameters
     */
    internal void Reset()
    {
    }
}