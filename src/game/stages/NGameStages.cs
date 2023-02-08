namespace Game.Stages;

using System.Collections.Generic;
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
    private float InvertedScaleH                            = 1.0F;
    private float InvertedScaleInvertedPixelH                  = 1.0F;
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
    private const short EVEN_PLAYER_ORIG_LINE               = 576;
    private const short ODD_PLAYER_ORIG_LINE                = 605;
    private volatile float PlayerCurrentLine                = EVEN_PLAYER_ORIG_LINE;
    private volatile float PlayerCurrentLinePixel           = 0;
    private volatile float PlayerTopLinePixel               = 0;
    private volatile float PlayerBottomLinePixel            = 0;
    private short LinesInCurrentStage                       = 0;
    private short LinesInNextStage                          = 0;
    private float RenderAreaWidth                           = 0;
    private float RenderAreaHeight                          = 0;
    private float ScaledPixelHeight                         = 0f;
    //private volatile int TopOffSetScreenFrame               = 0;
    //private volatile int EndScreenFrame                     = 0;
    //private volatile int TopScreenFrame                     = 0;
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
    
    private Dictionary<int, GameSprite> CurrentStageSpritesDefinition = new Dictionary<int, GameSprite>();
    private Dictionary<int, GameSprite> NextStageSpritesDefinition    = new Dictionary<int, GameSprite>();
    private List<GameSprite> CurrentStageSprites;
    private List<GameSprite> NextStageSprites;
    private RectangleF DrawRect = new RectangleF(0f, 0f, PIXEL_WIDTH, PIXEL_HEIGHT);

    private volatile float Offset                           = 0;
    private volatile float OpeningOffset                    = 0;
    private volatile short TransitionOffset                 = 0;
    protected volatile short CurrentOpeningLine             = 0;

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
        this.InvertedScaleH                 = 1 / this.ScaleH;       
        this.InvertedScaleInvertedPixelH    = this.InvertedScaleH * (1 / (float)PIXEL_HEIGHT);
        this.ScaledPixelHeight              = this.ScaleH * PIXEL_HEIGHT;

        //112 = 97 (screen lines before current line) + 15 lines (60 pixels, max sprite height)
        this.PlayerCurrentLinePixel         = (this.PlayerCurrentLine - 97) * PIXEL_HEIGHT;
        this.PlayerTopLinePixel             = (this.PlayerCurrentLine - 112) * PIXEL_HEIGHT;
        this.PlayerBottomLinePixel          = (this.PlayerCurrentLine + 11) * PIXEL_HEIGHT;

        //control the current stage lines count
        this.ControlLinesCount();

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
        this.LoadSpriteListForSpecifiedStage(CURRENT_STAGE);

        //Load the Sprite List for the next stage
        this.LoadSpriteListForSpecifiedStage(NEXT_STAGE);

        //store the sprites of current stage
        this.CurrentStageSprites = this.CurrentStageSpritesDefinition.Values.Where(item => item.Type != GameSprite.HOUSE && item.Type != GameSprite.HOUSE2).ToList();

        //store the sprites of the next stage
        this.NextStageSprites = this.NextStageSpritesDefinition.Values.Where(item => item.Type != GameSprite.HOUSE && item.Type != GameSprite.HOUSE2).ToList();

        //the offset starts negative for the opening animation
        this.Offset = -PIXEL_HEIGHT * OPENING_LINES;
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
            float step          = frametime * 0.00004f;
            this.OpeningLineY  -= step;
            this.Offset        += step / this.ScaleH;

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
                this.Offset = 0;
            }
        } 
        else if (this.CanStartTheStage)
        {
            if (this.IsStageRunning)
            {
                float step = 0;
                float stepInv = 0;
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

                //calc stev * inverted scaleH
                stepInv = (step * this.InvertedScaleH);

                //calc next step
                this.CurrentLineY           -= step;
                this.PlayerCurrentLine      -= step * this.InvertedScaleInvertedPixelH;
                this.PlayerCurrentLinePixel -= stepInv;
                this.PlayerTopLinePixel     -= stepInv;
                this.PlayerBottomLinePixel  -= stepInv;

                //update draw stage opening flag
                this.isToDrawCurrentStage = (this.CurrentLineY > -this.RenderAreaHeight)?true:false;

                //enable next stage draw
                if (this.CurrentLineY <= 0)
                {
                    this.isToDrawNextStage = true;
                    this.NextLineDestY += step;
                }

                //if not already dead, check for dead (w/ bg)
                if (!colliding) 
                {
                    //Check if the Player is colliding with background
                    this.CheckBackgroundCollision();
                }

                //current stage end - runs once
                //Swap with next stage
                if (!this.isToDrawCurrentStage)
                {
                    //TODO: verify if the next stage exists
                    this.CURRENT_STAGE          += 1;
                    this.NEXT_STAGE             = (short)(this.CURRENT_STAGE + 1);
                    
                    //Swap current elements with next
                    this.isToDrawNextStage      = false;
                    this.isToDrawCurrentStage   = true;

                    this.CurrentStageImage      = this.NextStageImage;
                    this.CurrentStageGraphics   = this.NextStageGraphics;
                    this.CurrentLineY           = this.NextLineY;
                    this.CurrentLineDestY       = this.NextLineDestY;

                    //Load the next elements (async)
                    Task task = LoadNextStage();
                }
            }
        }

        //if exist an sprite in the current screen frame, update it
        foreach (var item in this.CurrentStageSpritesDefinition.Where(item => this.PlayerTopLinePixel < item.Key && this.PlayerBottomLinePixel > item.Key)) 
        {
            item.Value.Y = (item.Key - this.PlayerCurrentLinePixel) + this.Offset;
            item.Value.Update(frametime, colliding);
        }
    }


    /**
     * Verify if the player is colliding with the background
     */
    private void CheckBackgroundCollision()
    {
        int firstFromLeftToRight    = 0;
        int firstFromRightToLeft    = 0;
        int value                   = -1;
        int columns                 = IStagesDef.stages.GetLength(2);
        int clBefore                = (int)(this.PlayerCurrentLine + 3);
        int clAfter                 = clBefore + 7;

        //check 8 lines (each line has 4 pixels, the player has 32 pixels height)
        for (int i = clBefore; i < clAfter; i++) 
        {
            for (int j = 0; j < columns; j++) 
            {
                value = IStagesDef.stages[CURRENT_STAGE, i, j];
                if (value == 0) 
                {
                    firstFromLeftToRight = j;
                    break;
                }
            }

            for (int j = columns - 1; j > 0; j--) 
            {
                value = IStagesDef.stages[CURRENT_STAGE, i, j];
                if (value == 0) 
                {
                    firstFromRightToLeft = j + 1;
                    break;
                }
            }

            if ((this.GameRef.GetPlayer().GetXPosition() < (firstFromLeftToRight * PIXEL_WIDTH)) || 
                (this.GameRef.GetPlayer().GetXPosition() + this.GameRef.GetPlayer().GetSpriteWidth() > (firstFromRightToLeft * PIXEL_WIDTH))) 
            {
                this.GameRef.PlayerCollided();
                break;
            } 
        }

        //calc airplane nose position (begining and ending)
        short columnP1 = (short)(this.GameRef.GetPlayer().GetAirplaneNoseX() / PIXEL_WIDTH);
        short columnP2 = (short)(this.GameRef.GetPlayer().GetAirplaneNoseW() / PIXEL_WIDTH);

        //check if the pixel in front of the nose is an obstacle.
        short p1Value = IStagesDef.stages[CURRENT_STAGE, (int)(this.PlayerCurrentLine + 1), columnP1];
        short p2Value = IStagesDef.stages[CURRENT_STAGE, (int)(this.PlayerCurrentLine + 1), columnP2];
        if (!(p1Value == 0 && p2Value == 0))
        {
            //if it's an obstacle, collide
            this.GameRef.PlayerCollided();
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

        //Draw the sprites
        foreach (var item in this.CurrentStageSpritesDefinition.Where(item => this.PlayerTopLinePixel < item.Key && this.PlayerBottomLinePixel > item.Key))
        {
            item.Value.Draw(gfx);
        }

        gfx.DrawString(this.PlayerCurrentLine + "", new Font("Arial", 10), Brushes.Black, 0, 20);
        //gfx.DrawString(this.TopOffSetScreenFrame + "", new Font("Arial", 10), Brushes.Black, 200, 220);
        //gfx.DrawString(this.EndScreenFrame + "", new Font("Arial", 10), Brushes.Black, 200, 240);
        //gfx.DrawString(this.EndScreenFrame + "", new Font("Arial", 10), Brushes.Black, 200, 260);
    }

    /**
     * Start the stage
     */
    public void Start()
    {
        this.IsStageRunning = true;
    }

    /**
     * Control the number of lines of each stage (current & next)
     */
    public void ControlLinesCount()
    {
        ControlCurrentStageLinesCount();
        ControlNextStageLinesCount();
    }

    /**
     * 
     */
    private void ControlCurrentStageLinesCount()
    {
        if (this.CURRENT_STAGE % 2 == 0)
        {
            this.LinesInCurrentStage    = EVEN_STAGES_LINES;
            this.PlayerCurrentLine      = EVEN_PLAYER_ORIG_LINE;
        }
        else
        {
            this.LinesInCurrentStage    = ODD_STAGES_LINES;
            this.PlayerCurrentLine      = ODD_PLAYER_ORIG_LINE;
        }
        
        this.OpeningLineY       = (OPENING_LINES - SCREEN_LINES) * PIXEL_HEIGHT * this.ScaleH;
        this.CurrentLineY       = (this.LinesInCurrentStage - SCREEN_LINES) * PIXEL_HEIGHT * this.ScaleH;
        this.CurrentLineDestY   = this.CurrentLineY - ((this.LinesInCurrentStage) * PIXEL_HEIGHT * this.ScaleH);
    }

    /**
     * 
     */
    private void ControlNextStageLinesCount()
    {
        this.LinesInNextStage   = (this.CURRENT_STAGE % 2 == 0)?ODD_STAGES_LINES:EVEN_STAGES_LINES;
        this.OpeningLineY       = (OPENING_LINES - SCREEN_LINES) * PIXEL_HEIGHT * this.ScaleH;
        this.NextLineY          = (this.LinesInNextStage - SCREEN_LINES) * PIXEL_HEIGHT * this.ScaleH;
        this.NextLineDestY      = this.NextLineY - ((this.LinesInNextStage) * PIXEL_HEIGHT * this.ScaleH);
    }

    /**
     * TODO:
     */
    public IEnumerable<GameSprite> GetSpritesInScreen()
    {
        return (new List<GameSprite>());
    }

    /**
     * Load the Sprite List for the Specified Stage
     * The values goes or to CurrentStageDef or to NextStageDef
     */
    private void LoadSpriteListForSpecifiedStage(short stage)
    {
        Dictionary<int, GameSprite> temp = (stage == CURRENT_STAGE)?this.CurrentStageSpritesDefinition:this.NextStageSpritesDefinition;
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
     * Load next stage assets
     */
    private async Task LoadNextStage()
    {
        this.ControlNextStageLinesCount();
        this.LoadNextScenarioGraphics();
    }

    /**
     * Reset the current state of stage
     * TODO: VERIFY STAGE CHANGE
     */
    internal void Reset()
    {
        this.GameRef.DisablePlayerSprite();
        this.NextLineY                      = 0f;
        this.IsToDrawStageOpening           = true;
        this.isToDrawCurrentStage           = false;
        this.isToDrawNextStage              = false;
        this.CanStartStageOpening           = true;
        this.CanStartTheStage               = false;
        this.IsStageRunning                 = false;
        this.Offset                         = -PIXEL_HEIGHT * OPENING_LINES;
        this.ControlLinesCount();
        this.PlayerCurrentLinePixel         = (this.PlayerCurrentLine - 97) * PIXEL_HEIGHT;
        this.PlayerTopLinePixel             = (this.PlayerCurrentLine - 112) * PIXEL_HEIGHT;
        this.PlayerBottomLinePixel          = (this.PlayerCurrentLine + 11) * PIXEL_HEIGHT;

        foreach (var item in this.CurrentStageSpritesDefinition) 
        {
            item.Value.Reset();
        }

        foreach (var item in this.NextStageSpritesDefinition) 
        {
            item.Value.Reset();
        }
    }

    //----------------------------------------------------------------------------------//
    //-------------------------- RENDER IN BACKBUFFER ----------------------------------//
    //----------------------------------------------------------------------------------//
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

        int stage = NEXT_STAGE;
        if (stage % 2 == 0)
        {
            stage = 0;
        }

        //render
        for (int i = 0; i < this.LinesInNextStage; i++) 
        {
            for (int j = 0; j < STAGES_COLUMNS; j++) 
            {
                short renderBlock = IStagesDef.stages[stage, i, j];
                this.DrawRect.X =  j * PIXEL_WIDTH;
                this.DrawRect.Y = (i * PIXEL_HEIGHT);
                this.NextStageGraphics.FillRectangle(IStagesDef.Brushes[renderBlock], this.DrawRect);
            }
        }

        //release the hdc
        graphics.ReleaseHdc(hdc);
    }
    //----------------------------------------------------------------------------------//
}