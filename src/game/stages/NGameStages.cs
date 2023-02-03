namespace Game.Stages;

using Util;

/**
 * Author: Joao P B Faria
 * Date: Oct/2022
 * Definition: This class represent the Game Stages, with their background and enemy/static sprites.
               This is a test class. If the performance surpass the OG class, it will replace it.
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
    private SolidBrush [] NBrushes                          = new SolidBrush[] {new SolidBrush(Color.FromArgb(255, 45, 50, 184)),  //dark blue
                                                                                new SolidBrush(Color.FromArgb(255, 110, 156, 66)),  //green
                                                                                new SolidBrush(Color.FromArgb(255, 53, 95, 24)),    //dark green
                                                                                new SolidBrush(Color.FromArgb(0, 0, 0, 0)),         //transparent black
                                                                                new SolidBrush(Color.FromArgb(255, 255, 255, 255)), //white
                                                                                new SolidBrush(Color.FromArgb(255, 111, 111, 111)), //silver
                                                                                new SolidBrush(Color.FromArgb(255, 170, 170, 170)), //dark gray
                                                                                new SolidBrush(Color.FromArgb(255, 234, 234, 70)),  //yellow
                                                                                new SolidBrush(Color.FromArgb(255, 0, 0, 0))};
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
    private float X = (616) * PIXEL_HEIGHT;
    private List<GameSprite> CurrentStageSprites;
    private Dictionary<int, GameSprite> CurrentStageSpritesDefition = new Dictionary<int, GameSprite>();
    private Dictionary<int, GameSprite> NextStageSpritesDefition    = new Dictionary<int, GameSprite>();

    /**
     * Description: Game stage constructor
     * In parameters: IGame reference
     */
    public NGameStages(IGame gameRef)
    {
        //store the game reference
        this.GameRef = gameRef;

        //control the current stage lines count
        //  if the stage is odd there are 574 lines / otherwise 603
        this.ControlStageLinesCount();

        //calc the scale
        this.ScaleW                 = (float)((float)this.GameRef.WindowSize.Width / (float)this.GameRef.GetInternalResolutionWidth());
        this.ScaleH                 = (float)((float)this.GameRef.WindowSize.Height / (float)this.GameRef.GetInternalResolutionHeight());

        //TEMP.......
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
                this.DrawRect.X =  j * PIXEL_WIDTH;
                this.DrawRect.Y = (i * PIXEL_HEIGHT);
                this.OddStageGraphics.FillRectangle(this.NBrushes[renderBlock], this.DrawRect);
            }
        }

        graphics.ReleaseHdc(hdc);

        //Load the Sprite List for the current stage
        this.LoadSpriteListForSpecifiedStage(CURRENT_STAGE);

        //store the sprites of current stage
        this.CurrentStageSprites = this.CurrentStageSpritesDefition.Values.Where(item => item.Type != GameSprite.HOUSE && item.Type != GameSprite.HOUSE2).ToList();
    }


    /**
     * Load the Sprite List for the Specified Stage
     * The values goes or to CurrentStageDef or to NextStageDef
     */
    private void LoadSpriteListForSpecifiedStage(short stage)
    {
        Dictionary<int, GameSprite> temp = (stage == CURRENT_STAGE)?this.CurrentStageSpritesDefition:this.NextStageSpritesDefition;
        for (short i = 0; i < IStagesDef.StagesSpritesConfig.GetLength(1); i++)
        {
            temp.Add(IStagesDef.StagesSpritesConfig[stage, i, 0],
                     SpriteFactory.CreateSprite(this.GameRef,
                     (byte)IStagesDef.StagesSpritesConfig[stage, i, 1],
                     IStagesDef.StagesSpritesConfig[stage, i, 2],
                     IStagesDef.StagesSpritesConfig[stage, i, 0],
                     (IStagesDef.StagesSpritesConfig[stage, i, 3] == 0) ? false : true,
                     IStagesDef.StagesSpritesConfig[stage, i, 4],
                     IStagesDef.StagesSpritesConfig[stage, i, 5],
                     (byte)IStagesDef.StagesSpritesConfig[stage, i, 6]));
        }
    }

    private void ControlStageLinesCount()
    {
        this.CURRENT_STAGE_LINES = (short)((CURRENT_STAGE % 2 == 0)?587:616);
    }

    /**
     * Update the Game Stage
     */
    public void Update(long frametime, bool colliding = false) 
    {
        X -= 0.05f;
    }

    /**
     * Draw the Game Stage
     */
    public void Draw(Graphics gfx)
    {
        IntPtr dhdc = gfx.GetHdc();
        IntPtr shdc = this.OddStageGraphics.GetHdc();
        BitmapEx.BitBlt(dhdc, 0, 0, 1000, (int)(428 * this.ScaleH), shdc, 0, (int)X, BitmapEx.SRCCOPY);

        this.OddStageGraphics.ReleaseHdc(shdc);
        gfx.ReleaseHdc(dhdc);
    }

    /**
     * This function is responsable for load the opening stages lines
     */
    private void LoadOpeningStageLines()
    {
        
    }
}