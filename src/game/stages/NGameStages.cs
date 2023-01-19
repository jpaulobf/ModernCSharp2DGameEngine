namespace Game.Stages;

using Util;

public enum TernaryRasterOperations : System.Int32 {
    SRCCOPY     = 0x00CC0020,
    SRCPAINT    = 0x00EE0086,
    SRCAND      = 0x008800C6,
    SRCINVERT   = 0x00660046,
    SRCERASE    = 0x00440328,
    NOTSRCCOPY  = 0x00330008,
    NOTSRCERASE = 0x001100A6,
    MERGECOPY   = 0x00C000CA,
    MERGEPAINT  = 0x00BB0226,
    PATCOPY     = 0x00F00021,
    PATPAINT    = 0x00FB0A09,
    PATINVERT   = 0x005A0049,
    DSTINVERT   = 0x00550009,
    BLACKNESS   = 0x00000042,
    WHITENESS   = 0x00FF0062,
    CAPTUREBLT  = 0x40000000 //only if WinVer >= 5.0.0 (see wingdi.h)
}


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
    private SolidBrush [] NBrushes                           = new SolidBrush[] {new SolidBrush(Color.FromArgb(255, 0, 0, 0)),       //black
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
    private float X = 0f;

    public NGameStages(IGame gameRef)
    {
        this.GameRef = gameRef;
        this.CURRENT_STAGE_LINES    = (short)((CURRENT_STAGE % 2 == 1)?587:616);

        //create two imagebuffers
        this.OddBufferedImage       = new Bitmap(PIXEL_WIDTH * STAGES_COLUMNS, PIXEL_HEIGHT * CURRENT_STAGE_LINES);
        Graphics graphics           = Graphics.FromImage(this.OddBufferedImage);
        IntPtr hdc                  = graphics.GetHdc();
        this.OddStageGraphics       = Graphics.FromHdc(hdc);

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

        //graphics.ReleaseHdc(hdc);
    }

    [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
    public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
    public static extern IntPtr SelectObject(IntPtr hdc, IntPtr obj);

    [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
    public static extern void DeleteObject(IntPtr obj);

    public void Update(long frametime, bool colliding = false) 
    {
        X += 0.05f;
    }

    public void Draw(Graphics gfx)
    {
        // IntPtr pTarget = gfx.GetHdc();
        // IntPtr pSource = CreateCompatibleDC(pTarget);
        // IntPtr pOrig = SelectObject(pSource, this.OddStageGraphics.GetHdc());
        
        // BitBlt(pTarget, 0,0, 500, 500, pSource, 0, 0, SRCCOPY);
        // DeleteObject(pSource);
        // gfx.ReleaseHdc(pTarget);

        IntPtr dhdc = gfx.GetHdc();
        IntPtr shdc = this.OddStageGraphics.GetHdc();
        BitmapEx.BitBlt(dhdc, 0, 0, 500, 500, shdc, 0, (int)X, BitmapEx.SRCCOPY);

        this.OddStageGraphics.ReleaseHdc(shdc);
        gfx.ReleaseHdc(dhdc);

        //gfx.DrawImage(OddBufferedImage, 0, 0);
    }

    private void LoadOpeningStageLines()
    {
        
    }
}