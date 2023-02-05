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
    private const byte STAGES_COLUMNS                       = 41;
    private const short MAX_STAGES                          = 2;
    private short CURRENT_STAGE                             = 0;
    private short NEXT_STAGE                                = 1;
    private const byte OPENING_LINES                        = 108;
    private const short EVEN_STAGES_LINES                   = 587;
    private const short ODD_STAGES_LINES                    = 616;
    private short LinesInCurrentStage                       = 0;
    private short LinesInNextStage                          = 0;
    private volatile Bitmap OddBufferedImage;
    private volatile Graphics OddStageGraphics;
    private volatile Bitmap EvenBufferedImage;
    private volatile Graphics EvenStageGraphics;
    private volatile Bitmap OddOpenBufferedImage;
    private volatile Graphics OddOpenStageGraphics;
    private volatile Bitmap EvenOpenBufferedImage;
    private volatile Graphics EvenOpenStageGraphics;




    protected volatile short PlayerCurrentLine              = 574;
    private volatile int StartScreenFrame                   = 0;
    private volatile int EndScreenFrame                     = 0;
    private volatile int CurrentLineYPosition               = 0;
    private volatile float Offset                           = 0;
    private volatile float OpeningOffset                    = 0;
    private volatile short TransitionOffset                 = 0;
    protected volatile short CurrentOpeningLine             = 0;
    private volatile bool CanStartStageOpening              = true;
    private volatile bool CanDrawStageOpening               = true;
    
    private float X = (616) * PIXEL_HEIGHT;
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

        //control the current stage lines count
        this.ControlStageLinesCount();

        //calc the scale
        this.ScaleW = (float)((float)this.GameRef.WindowSize.Width / (float)this.GameRef.GetInternalResolutionWidth());
        this.ScaleH = (float)((float)this.GameRef.WindowSize.Height / (float)this.GameRef.GetInternalResolutionHeight());

        //TEMP.......
        X *= this.ScaleH;

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

        graphics.ReleaseHdc(hdc);
        eGraphics.ReleaseHdc(eHdc);
    }

    private void LoadCurrentScenarioGraphics()
    {
        //create the current image buffer
        this.OddBufferedImage       = new Bitmap((int)(PIXEL_WIDTH * STAGES_COLUMNS * this.ScaleW), (int)(PIXEL_HEIGHT * this.LinesInCurrentStage * this.ScaleH));
        Graphics graphics           = Graphics.FromImage(this.OddBufferedImage);
        IntPtr hdc                  = graphics.GetHdc();
        this.OddStageGraphics       = Graphics.FromHdc(hdc);
        
        //scale if necessary
        this.OddStageGraphics.ScaleTransform(ScaleW, ScaleH);

        //render
        for (int i = 0; i < this.LinesInCurrentStage; i++) 
        {
            for (int j = 0; j < STAGES_COLUMNS; j++) 
            {
                short renderBlock = IStagesDef.stages[CURRENT_STAGE, i, j];
                this.DrawRect.X =  j * PIXEL_WIDTH;
                this.DrawRect.Y = (i * PIXEL_HEIGHT);
                this.OddStageGraphics.FillRectangle(IStagesDef.Brushes[renderBlock], this.DrawRect);
            }
        }

        graphics.ReleaseHdc(hdc);
    }

    private void LoadNextScenarioGraphics()
    {
        //create the next scenario image buffer
        this.EvenBufferedImage      = new Bitmap((int)(PIXEL_WIDTH * STAGES_COLUMNS * this.ScaleW), (int)(PIXEL_HEIGHT * this.LinesInNextStage * this.ScaleH));
        Graphics graphics           = Graphics.FromImage(this.EvenBufferedImage);
        IntPtr hdc                  = graphics.GetHdc();
        this.EvenStageGraphics      = Graphics.FromHdc(hdc);
        
        //scale if necessary
        this.EvenStageGraphics.ScaleTransform(this.ScaleW, this.ScaleH);

        //render
        for (int i = 0; i < this.LinesInNextStage; i++) 
        {
            for (int j = 0; j < STAGES_COLUMNS; j++) 
            {
                short renderBlock = IStagesDef.stages[NEXT_STAGE, i, j];
                this.DrawRect.X =  j * PIXEL_WIDTH;
                this.DrawRect.Y = (i * PIXEL_HEIGHT);
                this.EvenStageGraphics.FillRectangle(IStagesDef.Brushes[renderBlock], this.DrawRect);
            }
        }

        graphics.ReleaseHdc(hdc);
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
    }

    /**
     * Update the Game Stage
     */
    public void Update(long frametime, bool colliding = false) 
    {
        X -= 0.05f;

/*
        //if the game is opening the stage (just an animation)
        if (this.CanStartStageOpening) 
        {
            //calc the offset
            float velocity      = frametime * 0.00004f; //step = 4; ((float)(step * 100 * ((double)frametime / 10_000_000)));
            this.OpeningOffset  += velocity;
            this.Offset         += velocity;

            if (this.OpeningOffset >= PIXEL_HEIGHT) {
                this.CurrentOpeningLine++;
                this.Offset -= (this.OpeningOffset - PIXEL_HEIGHT);
                this.OpeningOffset = 0;
            }

            //update draw stage opening flag
            this.CanDrawStageOpening = true;
        }
        //if the game is running
        //else if (this.CanStartTheStage) 
       // {
            
       // }

        //update the sprites
        //110 = 95 (screen lines before current line) + 15 lines (60 pixels, max sprite height)
        this.StartScreenFrame           = (this.PlayerCurrentLine - 110) * PIXEL_HEIGHT;
        this.EndScreenFrame             = (this.PlayerCurrentLine + 13)  * PIXEL_HEIGHT;
        this.CurrentLineYPosition       = (this.PlayerCurrentLine - 95)  * PIXEL_HEIGHT;

        //if exist an sprite in the current screen frame, update it
        foreach (var item in this.CurrentStageSpritesDefition.Where(item => this.StartScreenFrame < item.Key && this.EndScreenFrame > item.Key)) 
        {
            item.Value.Y = (item.Key - this.CurrentLineYPosition) + this.Offset + this.TransitionOffset;
            item.Value.Update(frametime, colliding);
        }
*/

    }

    /**
     * Draw the Game Stage
     */
    public void Draw(Graphics gfx)
    {
        /*
        IntPtr dhdc = gfx.GetHdc();
        IntPtr shdc = this.OddStageGraphics.GetHdc();
        BitmapEx.BitBlt(dhdc, 0, 0, 1000, (int)(428 * this.ScaleH), shdc, 0, (int)X, BitmapEx.SRCCOPY);

        this.OddStageGraphics.ReleaseHdc(shdc);
        gfx.ReleaseHdc(dhdc);
        */

        IntPtr dhdc = gfx.GetHdc();
        IntPtr shdc = this.OddOpenStageGraphics.GetHdc();
        BitmapEx.BitBlt(dhdc, 0, 0, 1000, (int)(428 * this.ScaleH), shdc, 0, 0, BitmapEx.SRCCOPY);

        this.OddOpenStageGraphics.ReleaseHdc(shdc);
        gfx.ReleaseHdc(dhdc);

    }

    /**
     * This function is responsable for load the opening stages lines
     */
    private void LoadOpeningStageLines()
    {
        
    }
}