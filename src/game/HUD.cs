namespace Game;

using Util;

/**
 * Author: Joao P B Faria
 * Date: Oct/2022
 * Description: HUD control class
 */
public class HUD 
{
    private Rectangle SeparatorRect;
    private Rectangle HudRect;
    private IGame GameRef;
    public byte PlayerLives {get;set;} = 5;
    private Bitmap FuelFrame = LoadingStuffs.GetInstance().GetImage("fuel-frame");
    private Bitmap FuelMeter = LoadingStuffs.GetInstance().GetImage("fuel-meter");

    private int FuelFrameX = 0;
    private int FuelFrameY = 0;
    private int FuelMeterX = 0;
    private int FuelMeterY = 0;

    /**
     * Constructor
     */
    public HUD(IGame game) 
    {
        this.GameRef        = game;
        this.SeparatorRect  = new Rectangle(0, 428, 738, 3);
        this.HudRect        = new Rectangle(0, 431, 738, 85);

        this.FuelFrameX     = (int)((this.HudRect.Size.Width / 2) - (this.FuelFrame.Width / 2));
        this.FuelFrameY     = (int)((this.HudRect.Size.Height / 2) - (this.FuelFrame.Height / 2) + this.HudRect.Y);
        this.FuelMeterX     = (int)((this.HudRect.Size.Height / 2) - (this.FuelFrame.Height / 2) + this.HudRect.Y);;
        this.FuelMeterY     = (int)((this.HudRect.Size.Height / 2) - (this.FuelFrame.Height / 2) + this.HudRect.Y);;
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
        gfx.FillRectangle(Brushes.Black, this.SeparatorRect);
        gfx.FillRectangle(new SolidBrush(Color.FromArgb(255, 144, 144, 144)), this.HudRect);

        gfx.DrawImage(this.FuelMeter, this.FuelMeterX, this.FuelMeterY, this.FuelMeter.Width, this.FuelMeter.Height);
        gfx.DrawImage(this.FuelFrame, this.FuelFrameX, this.FuelFrameY, this.FuelFrame.Width, this.FuelFrame.Height);
    }

    /**
     * Reset HUD parameters
     */
    internal void Reset()
    {
       this.PlayerLives = 5;
    }

    /**
     * Verify if player is still alive
     */
    internal bool PlayerIsAlive()
    {
        return (this.PlayerLives > 0);
    }

    /**
     * Decrease the current player live number
     */
    internal void PlayerDecreaseLive()
    {
        this.PlayerLives--;
    }
}