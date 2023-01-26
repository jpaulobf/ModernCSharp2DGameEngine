namespace Game.Stages;

using Util;

/**
 * Author: Joao P B Faria
 * Date: Oct/2022
 * Definition: This class represent the Game Stages, with their background and enemy/static sprites.
 */
public class NGameStages : IStagesDef 
{
    private IGame GameRef;
    private float ScaleW                                    = 1.0F;
    private float ScaleH                                    = 1.0F;
    private const short PIXEL_WIDTH                         = 18;
    private const byte PIXEL_HEIGHT                         = 4;
    private const short OPENING_LINES                       = 108;
    private const byte STAGES_COLUMNS                       = 41;
    private SolidBrush [] NBrushes                          = new SolidBrush[] {new SolidBrush(Color.FromArgb(255, 0, 0, 0)),       //black
                                                                                new SolidBrush(Color.FromArgb(255, 110, 156, 66)),  //green
                                                                                new SolidBrush(Color.FromArgb(255, 53, 95, 24)),    //dark green
                                                                                new SolidBrush(Color.FromArgb(0, 0, 0, 0)),         //transparent black
                                                                                new SolidBrush(Color.FromArgb(255, 255, 255, 255)), //white
                                                                                new SolidBrush(Color.FromArgb(255, 111, 111, 111)), //silver
                                                                                new SolidBrush(Color.FromArgb(255, 170, 170, 170)), //dark gray
                                                                                new SolidBrush(Color.FromArgb(255, 234, 234, 70)),  //yellow
                                                                                new SolidBrush(Color.FromArgb(255, 45, 50, 184))};  //dark blue
    private RectangleF DrawRect                             = new RectangleF(0f, 0f, PIXEL_WIDTH, PIXEL_HEIGHT);
    private short CURRENT_STAGE                             = 1;
    private short CURRENT_STAGE_LINES                       = 0;
    private volatile BufferedGraphics OddBufferedGraphics;
    private volatile Bitmap OddBufferedImage;
    private volatile Graphics OddStageGraphics;
    private volatile BufferedGraphics EvenBufferedGraphics;
    private volatile Bitmap EvenBufferedImage;
    private volatile Graphics EvenStageGraphics;
    private volatile BufferedGraphics OpenBufferedGraphics;
    private volatile Bitmap OpenBufferedImage;
    private volatile Graphics OpenStageGraphics;
    private float X = (616 - 29) * PIXEL_HEIGHT;

    /**
     * Description: Game stage constructor
     * In parameters: IGame reference
     */
    public NGameStages(IGame gameRef)
    {
        this.GameRef = gameRef;
        this.CURRENT_STAGE_LINES    = (short)((CURRENT_STAGE % 2 == 1)?587:616);

        this.ScaleW                 = (float)((float)this.GameRef.WindowSize.Width / (float)this.GameRef.GetInternalResolutionWidth());
        this.ScaleH                 = (float)((float)this.GameRef.WindowSize.Height / (float)this.GameRef.GetInternalResolutionHeight());

        X *= this.ScaleH;

        //create two imagebuffers
        this.OddBufferedImage       = new Bitmap((int)(PIXEL_WIDTH * STAGES_COLUMNS * this.ScaleW), (int)(PIXEL_HEIGHT * CURRENT_STAGE_LINES * this.ScaleH));
        Graphics graphics           = Graphics.FromImage(this.OddBufferedImage);
        IntPtr hdc                  = graphics.GetHdc();
        this.OddStageGraphics       = Graphics.FromHdc(hdc);
        
        this.OddStageGraphics.ScaleTransform(ScaleW, ScaleH);

        for (int i = 0; i < CURRENT_STAGE_LINES; i++) 
        {
            for (int j = 0; j < STAGES_COLUMNS; j++) 
            {
                short renderBlock = IStagesDef.stages[CURRENT_STAGE, i, j];
                if (renderBlock != 0)
                {
                    this.DrawRect.X =  j * PIXEL_WIDTH;
                    this.DrawRect.Y = (i * PIXEL_HEIGHT);
                    this.OddStageGraphics.FillRectangle(this.NBrushes[renderBlock], this.DrawRect);
                }
            }
        }

        graphics.ReleaseHdc(hdc);
    }

    public void Update(long frametime, bool colliding = false) 
    {
        X -= 0.05f;
    }

    public void Draw(Graphics gfx)
    {
        IntPtr dhdc = gfx.GetHdc();
        IntPtr shdc = this.OddStageGraphics.GetHdc();
        BitmapEx.BitBlt(dhdc, 0, 0, 1000, (int)(516 * this.ScaleH), shdc, 0, (int)X, BitmapEx.SRCCOPY);

        this.OddStageGraphics.ReleaseHdc(shdc);
        gfx.ReleaseHdc(dhdc);
    }

    private void LoadOpeningStageLines()
    {
        
    }
}