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
    private volatile Bitmap CurrentStageImage;
    private volatile Graphics CurrentStageGraphics;
    private volatile Bitmap NextStageImage;
    private volatile Graphics NextStageGraphics;
    private volatile Bitmap OddOpenBufferedImage;
    private volatile Graphics OddOpenStageGraphics;
    private volatile Bitmap EvenOpenBufferedImage;
    private volatile Graphics EvenOpenStageGraphics;
    private float ScaleW                                    = 1.0F;
    private float ScaleH                                    = 1.0F;
    private const short PIXEL_WIDTH                         = 18;
    private const byte PIXEL_HEIGHT                         = 4;
    private const byte STAGES_COLUMNS                       = 41;
    private const short MAX_STAGES                          = 2;
    private short CURRENT_STAGE                             = 0;
    private short NEXT_STAGE                                = 1;
    private const byte OPENING_LINES                        = 108;
    private const byte SCREEN_LINES                         = 108;
    private const short EVEN_STAGES_LINES                   = 587;
    private const short ODD_STAGES_LINES                    = 616;
    private short LinesInCurrentStage                       = 0;
    private short LinesInNextStage                          = 0;
    private float RenderAreaWidth                           = 0;
    private float RenderAreaHeight                          = 0;
    private volatile float OpeningLineY                     = 0f;
    private volatile float CurrentLineY                     = 0f;
    private volatile float CurrentLineDestY                 = 0f;
    private volatile float NextLineY                        = 0f;
    private volatile float NextLineDestY                    = 0f;
    private volatile bool IsToDrawStageOpening              = true;
    private volatile bool isToDrawCurrentStage              = false;
    private volatile bool isToDrawNextStage                 = false;
    private volatile bool CanStartStageOpening              = true;
    private volatile bool CanStartTheStage                  = false;
    private volatile bool IsStageRunning                    = false;


    protected volatile short PlayerCurrentLine              = 574;
    private volatile int StartScreenFrame                   = 0;
    private volatile int EndScreenFrame                     = 0;
    private volatile int CurrentLineYPosition               = 0;
    private volatile float Offset                           = 0;
    private volatile float OpeningOffset                    = 0;
    private volatile short TransitionOffset                 = 0;
    protected volatile short CurrentOpeningLine             = 0;
    
    
    private List<GameSprite> CurrentStageSprites;
    private Dictionary<int, GameSprite> CurrentStageSpritesDefition = new Dictionary<int, GameSprite>();
    private Dictionary<int, GameSprite> NextStageSpritesDefition    = new Dictionary<int, GameSprite>();
    private RectangleF DrawRect = new RectangleF(0f, 0f, PIXEL_WIDTH, PIXEL_HEIGHT);


    /**
     * Description: Game stage constructor
     * In parameters: IGame reference
     */
    public NGameStages(IGame gameRef)
    {
        //store the game reference
        this.GameRef = gameRef;

        //calc the scale
        this.ScaleW = (float)((float)this.GameRef.WindowSize.Width / (float)this.GameRef.GetInternalResolutionWidth());
        this.ScaleH = (float)((float)this.GameRef.WindowSize.Height / (float)this.GameRef.GetInternalResolutionHeight());

        //control the current stage lines count
        this.ControlStageLinesCount();

        //calc the render area
        this.RenderAreaWidth    = this.GameRef.GetInternalResolutionWidth() * this.ScaleW;
        this.RenderAreaHeight   = (this.GameRef.GetInternalResolutionHeight() - this.GameRef.GetHUD().GetHudHeight()) * this.ScaleH;

        //load opening imagebuffer 1 & 2
        this.LoadOpeningScenarioGraphics();

        //load the current scenario image (backbuffered)
        this.LoadCurrentScenarioGraphics();

        //load the next scenario image (backbuffered)
        this.LoadNextScenarioGraphics();

        //Load the Sprite List for the current stage
        //this.LoadSpriteListForSpecifiedStage(CURRENT_STAGE);

        //store the sprites of current stage
        //this.CurrentStageSprites = this.CurrentStageSpritesDefition.Values.Where(item => item.Type != GameSprite.HOUSE && item.Type != GameSprite.HOUSE2).ToList();
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

    /**
     * Update the Game Stage
     */
    public void Update(long frametime, bool colliding = false) 
    {
        
        //if the game is opening the stage (just an animation)
        if (this.CanStartStageOpening) 
        {
            //calc the offset
            //step = 4; ((float)(step * 100 * ((double)frametime / 10_000_000)));
            float step      = frametime * 0.00004f;
            this.OpeningLineY  -= step;

            //update draw stage opening flag
            this.IsToDrawStageOpening = (this.OpeningLineY > -this.RenderAreaHeight)?true:false;
            this.CanStartStageOpening = this.IsToDrawStageOpening;

            //enable current stage draw
            if (this.OpeningLineY < 0)
            {
                this.isToDrawCurrentStage = true;
                this.CurrentLineDestY += step;
            }

            //opening end
            if (!this.CanStartStageOpening)
            {
                this.CanStartTheStage = true;
                this.GameRef.TogglePlayerSprite();
            }
        } 
        else if (this.CanStartTheStage)
        {
            if (this.IsStageRunning)
            {
                float step = 0;
                if (this.GameRef.GetPlayer().DOUBLE_SPEED)
                {
                    //step = 2; ((float)(100 * ((double)frametime / 10_000_000))) * step;
                    step = frametime * 0.00002f;
                }
                else if (this.GameRef.GetPlayer().HALF_SPEED)
                {
                    //step = .5; ((float)(100 * ((double)frametime / 10_000_000))) * step;
                    step = frametime * 0.000005f;
                } 
                else //default velocity
                {
                    //step = 1; ((float)(100 * ((double)frametime / 10_000_000))) * step;
                    step = frametime * 0.00001f;
                }

                this.CurrentLineY -= step;

                //update draw stage opening flag
                this.isToDrawCurrentStage = (this.CurrentLineY > -this.RenderAreaHeight)?true:false;

                //enable next stage draw
                if (this.CurrentLineY <= 0)
                {
                    this.isToDrawNextStage = true;
                    this.NextLineDestY += step;
                }

                //current stage end
                if (!this.isToDrawCurrentStage)
                {
                    //TODO: SWAP....
                    Console.WriteLine("aui.....");
                    //this.CanStartTheStage = true;
                    //this.GameRef.TogglePlayerSprite();
                }
            }
        }
    }

    /**
     * Draw the Game Stage
     */
    public void Draw(Graphics gfx)
    {
        IntPtr dhdc = gfx.GetHdc();
        Graphics.FromHdc(dhdc).Clear(IStagesDef.Brushes[0].Color);

        if (this.IsToDrawStageOpening)
        {
            IntPtr shdc = this.OddOpenStageGraphics.GetHdc();
            BitmapEx.BitBlt(dhdc, 0, -2, (int)this.RenderAreaWidth, (int)this.RenderAreaHeight, shdc, 0, (int)this.OpeningLineY, BitmapEx.SRCCOPY);
            this.OddOpenStageGraphics.ReleaseHdc(shdc);
        }

        if (this.isToDrawCurrentStage)
        {
            IntPtr shdc = this.CurrentStageGraphics.GetHdc();
            BitmapEx.BitBlt(dhdc, 0, (int)this.CurrentLineDestY, (int)this.RenderAreaWidth, (int)this.RenderAreaHeight, shdc, 0, (int)this.CurrentLineY, BitmapEx.SRCCOPY);
            this.CurrentStageGraphics.ReleaseHdc(shdc);
        }

        if (this.isToDrawNextStage)
        {
            IntPtr shdc = this.NextStageGraphics.GetHdc();
            BitmapEx.BitBlt(dhdc, 0, (int)this.NextLineDestY, (int)this.RenderAreaWidth, (int)this.RenderAreaHeight, shdc, 0, (int)NextLineY, BitmapEx.SRCCOPY);
            this.NextStageGraphics.ReleaseHdc(shdc);
        }

        gfx.ReleaseHdc(dhdc);
    }

    /**
     *
     */
    private void LoadOpeningScenarioGraphics()
    {
        //create the current image buffer
        this.OddOpenBufferedImage       = new Bitmap((int)(PIXEL_WIDTH * STAGES_COLUMNS * this.ScaleW), (int)(PIXEL_HEIGHT * OPENING_LINES * this.ScaleH));
        this.EvenOpenBufferedImage      = new Bitmap((int)(PIXEL_WIDTH * STAGES_COLUMNS * this.ScaleW), (int)(PIXEL_HEIGHT * OPENING_LINES * this.ScaleH));
        Graphics graphics               = Graphics.FromImage(this.OddOpenBufferedImage);
        Graphics eGraphics              = Graphics.FromImage(this.EvenOpenBufferedImage);
        IntPtr hdc                      = graphics.GetHdc();
        IntPtr eHdc                     = eGraphics.GetHdc();
        this.OddOpenStageGraphics       = Graphics.FromHdc(hdc);
        this.EvenOpenStageGraphics      = Graphics.FromHdc(eHdc);
        
        //scale if necessary
        this.OddOpenStageGraphics.ScaleTransform(this.ScaleW, this.ScaleH);
        this.EvenOpenStageGraphics.ScaleTransform(this.ScaleW, this.ScaleH);

        //render
        for (int i = 0; i < OPENING_LINES; i++) 
        {
            for (int j = 0; j < STAGES_COLUMNS; j++) 
            {
                short renderBlock = IStagesDef.opening[CURRENT_STAGE, i, j];
                this.DrawRect.X =  j * PIXEL_WIDTH;
                this.DrawRect.Y = (i * PIXEL_HEIGHT);
                this.OddOpenStageGraphics.FillRectangle(IStagesDef.Brushes[renderBlock], this.DrawRect);

                renderBlock = IStagesDef.opening[NEXT_STAGE, i, j];
                this.DrawRect.X =  j * PIXEL_WIDTH;
                this.DrawRect.Y = (i * PIXEL_HEIGHT);
                this.EvenOpenStageGraphics.FillRectangle(IStagesDef.Brushes[renderBlock], this.DrawRect);
            }
        }

        //release the hdc
        graphics.ReleaseHdc(hdc);
        eGraphics.ReleaseHdc(eHdc);
    }

    /**
     *
     */
    private void LoadCurrentScenarioGraphics()
    {
        //create the current image buffer
        this.CurrentStageImage      = new Bitmap((int)(PIXEL_WIDTH * STAGES_COLUMNS * this.ScaleW), (int)(PIXEL_HEIGHT * this.LinesInCurrentStage * this.ScaleH));
        Graphics graphics           = Graphics.FromImage(this.CurrentStageImage);
        IntPtr hdc                  = graphics.GetHdc();
        this.CurrentStageGraphics   = Graphics.FromHdc(hdc);
        
        //scale if necessary
        this.CurrentStageGraphics.ScaleTransform(ScaleW, ScaleH);

        //render
        for (int i = 0; i < this.LinesInCurrentStage; i++) 
        {
            for (int j = 0; j < STAGES_COLUMNS; j++) 
            {
                short renderBlock = IStagesDef.stages[CURRENT_STAGE, i, j];
                this.DrawRect.X =  j * PIXEL_WIDTH;
                this.DrawRect.Y = (i * PIXEL_HEIGHT);
                this.CurrentStageGraphics.FillRectangle(IStagesDef.Brushes[renderBlock], this.DrawRect);
            }
        }

        //release the hdc
        graphics.ReleaseHdc(hdc);
    }

    /**
     * 
     */
    private void LoadNextScenarioGraphics()
    {
        //create the next scenario image buffer
        this.NextStageImage         = new Bitmap((int)(PIXEL_WIDTH * STAGES_COLUMNS * this.ScaleW), (int)(PIXEL_HEIGHT * this.LinesInNextStage * this.ScaleH));
        Graphics graphics           = Graphics.FromImage(this.NextStageImage);
        IntPtr hdc                  = graphics.GetHdc();
        this.NextStageGraphics      = Graphics.FromHdc(hdc);
        
        //scale if necessary
        this.NextStageGraphics.ScaleTransform(this.ScaleW, this.ScaleH);

        //render
        for (int i = 0; i < this.LinesInNextStage; i++) 
        {
            for (int j = 0; j < STAGES_COLUMNS; j++) 
            {
                short renderBlock = IStagesDef.stages[NEXT_STAGE, i, j];
                this.DrawRect.X =  j * PIXEL_WIDTH;
                this.DrawRect.Y = (i * PIXEL_HEIGHT);
                this.NextStageGraphics.FillRectangle(IStagesDef.Brushes[renderBlock], this.DrawRect);
            }
        }

        //release the hdc
        graphics.ReleaseHdc(hdc);
    }

    internal void Reset()
    {
        this.NextLineY                      = 0f;
        this.IsToDrawStageOpening           = true;
        this.isToDrawCurrentStage           = false;
        this.isToDrawNextStage              = false;
        this.CanStartStageOpening           = true;
        this.CanStartTheStage               = false;
        this.IsStageRunning                 = false;
        this.GameRef.DisablePlayerSprite();
        this.ControlStageLinesCount();
    }

    /**
     * Start the stage
     */
    public void Start()
    {
        this.IsStageRunning = true;
    }

    /**
     * 
     */
    public void ControlStageLinesCount()
    {
        if (this.CURRENT_STAGE % 2 == 0)
        {
            this.LinesInCurrentStage    = EVEN_STAGES_LINES;
            this.LinesInNextStage       = ODD_STAGES_LINES;
            this.PlayerCurrentLine      = 574;
        }
        else
        {
            this.LinesInCurrentStage    = ODD_STAGES_LINES;
            this.LinesInNextStage       = EVEN_STAGES_LINES;
            this.PlayerCurrentLine      = 603;
        }
        
        this.OpeningLineY       = (OPENING_LINES - SCREEN_LINES) * PIXEL_HEIGHT * this.ScaleH;
        this.CurrentLineY       = (this.LinesInCurrentStage - SCREEN_LINES) * PIXEL_HEIGHT * this.ScaleH;
        this.NextLineY          = (this.LinesInNextStage - SCREEN_LINES) * PIXEL_HEIGHT * this.ScaleH;
        this.CurrentLineDestY   = this.CurrentLineY - ((this.LinesInCurrentStage) * PIXEL_HEIGHT * this.ScaleH);
        this.NextLineDestY      = this.NextLineY - ((this.LinesInNextStage) * PIXEL_HEIGHT * this.ScaleH);
    }
}