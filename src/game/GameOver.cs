namespace Game;

using Util;

public class GameOver
{
    private IGame GameRef;
    private long Framecounter = 0;
    private Bitmap GameOverImage;
    private float GameOverImageX = 0;
    private float GameOverImageY = 0;
    private Util.SoundPlayerEx GameOverMusic;

    /**
     * Constructor
     */
    public GameOver(IGame game) 
    {
        this.GameRef        = game;
        this.GameOverImage  = LoadingStuffs.GetInstance().GetImage("gameover");
        this.GameOverMusic  = new Util.SoundPlayerEx(Util.Utility.getCurrentPath() + "\\sfx\\gameover-theme.wav");
        this.GameOverImageX = (this.GameRef.GetInternalResolutionWidth() / 2) - (this.GameOverImage.Width / 2);
        this.GameOverImageY = (this.GameRef.GetInternalResolutionHeight() / 2) - (this.GameOverImage.Height / 2) - 30;
    }

    /**
     * Update the HUD
     */
    public void Update(long frametime) 
    {
        this.Framecounter += frametime;
        if (this.Framecounter == frametime) 
        {
            this.GameOverMusic.Play();
        }
    }

    /**
     * Draw the HUD
     */
    public void Draw(Graphics gfx) 
    {
        gfx.FillRectangle(Brushes.Black, 0, 0, this.GameRef.GetInternalResolutionWidth(), this.GameRef.GetInternalResolutionHeight());
        gfx.DrawImage(this.GameOverImage, this.GameOverImageX, this.GameOverImageY, this.GameOverImage.Width, this.GameOverImage.Height);
    }

    /**
     * Reset HUD parameters
     */
    internal void Reset()
    {
    }
}