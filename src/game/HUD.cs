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
    private Player PlayerRef;
    private Bitmap LifeCounter;
    private Bitmap FuelFrame                    = LoadingStuffs.GetInstance().GetImage("fuel-frame");
    private Bitmap FuelMeter                    = LoadingStuffs.GetInstance().GetImage("fuel-meter");
    private Dictionary<byte, string> NumbersMap = new Dictionary<byte, string>() {{0, "number-0"}, {1, "number-1"}, {2, "number-2"}, {3, "number-3"}, {4, "number-4"}, {5, "number-5"}, {6, "number-6"}, {7, "number-7"}, {8, "number-8"}, {9, "number-9"}};
    private int FuelFrameX                      = 0;
    private int FuelFrameY                      = 0;
    private float FuelMeterX                    = 0;
    private float OGFuelMeterX                  = 0;
    private int FuelMeterY                      = 0;
    private const short LifeCounterX            = 250;
    private const short FuelZeroX               = 295;
    private const short LifeCounterY            = 495;
    private const float FuelSpentUnit           = 1.4f;
    private const byte SEPARATOR_HEIGHT         = 3; //px
    private const byte HUD_HEIGHT               = 85; //px
    private const short HUD_WIDTH               = 738; //px

    /**
     * Constructor
     */
    public HUD(IGame game) 
    {
        this.GameRef        = game;
        this.PlayerRef      = this.GameRef.GetPlayer();
        this.SeparatorRect  = new Rectangle(0, 428, HUD_WIDTH, SEPARATOR_HEIGHT);
        this.HudRect        = new Rectangle(0, 431, HUD_WIDTH, HUD_HEIGHT);

        this.FuelFrameX     = (int)((this.HudRect.Size.Width / 2) - (this.FuelFrame.Width / 2));
        this.FuelFrameY     = (int)((this.HudRect.Size.Height / 2) - (this.FuelFrame.Height / 2) + this.HudRect.Y);
        this.OGFuelMeterX   = (float)FuelZeroX + (100 * FuelSpentUnit);
        this.FuelMeterX     = this.OGFuelMeterX;
        this.FuelMeterY     = (int)((this.HudRect.Size.Height / 2) - (this.FuelFrame.Height / 2) + this.HudRect.Y) + 8;

        this.LifeCounter    = LoadingStuffs.GetInstance().GetImage(NumbersMap[this.PlayerRef.Lives]);
    }

    /**
     * Update the fuel mark position X
     */
    public void UpdateFuelMarker(float fuelCounter)
    {
        this.FuelMeterX = ((float)FuelZeroX) + (FuelSpentUnit * fuelCounter);
    }

    /**
     * Update the HUD
     */
    public void Update(long frametime) 
    {
        this.GetCurrentLifeCounterImage();
    }

    /**
     * Get current life counter image
     */
    private void GetCurrentLifeCounterImage()
    {
        this.LifeCounter = LoadingStuffs.GetInstance().GetImage(NumbersMap[this.PlayerRef.Lives]);
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

        gfx.DrawImage(this.LifeCounter, LifeCounterX, LifeCounterY, this.LifeCounter.Width, this.LifeCounter.Height);
    }

    /**
     * Reset HUD parameters
     */
    internal void Reset()
    {
        this.FuelMeterX = this.OGFuelMeterX;
    }
}