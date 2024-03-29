namespace Game.Stages;

/**
 * Author: Joao P B Faria
 * Date: Oct/2022
 * Definition: This class represent the Game Stages, with their background and enemy/static sprites.
 */
public class OldGameStages : IStagesDef 
{
    private IGame GameRef;
    private float ScaleW                                = 1.0F;
    private float ScaleH                                = 1.0F;
    private const short PIXEL_WIDTH                     = 18;
    private const byte PIXEL_HEIGHT                     = 4;
    private SolidBrush [] Brushes                       = new SolidBrush[] {new SolidBrush(Color.FromArgb(255, 0, 0, 0)),       //black
                                                                            new SolidBrush(Color.FromArgb(255, 110, 156, 66)),  //green
                                                                            new SolidBrush(Color.FromArgb(255, 53, 95, 24)),    //dark green
                                                                            new SolidBrush(Color.FromArgb(0, 0, 0, 0)),         //transparent black
                                                                            new SolidBrush(Color.FromArgb(255, 255, 255, 255)), //white
                                                                            new SolidBrush(Color.FromArgb(255, 111, 111, 111)), //silver
                                                                            new SolidBrush(Color.FromArgb(255, 170, 170, 170)), //dark gray
                                                                            new SolidBrush(Color.FromArgb(255, 234, 234, 70)),  //yellow
                                                                            new SolidBrush(Color.FromArgb(255, 45, 50, 184))};  //dark blue
    private RectangleF DrawRect                          = new RectangleF(0f, 0f, PIXEL_WIDTH, PIXEL_HEIGHT);
    protected volatile short CurrentLine                = 574;
    private const short SCREEN_LINES                    = 107 + 1; //one extra buffer line
    private const byte OPENING_LINES                    = 108;
    protected volatile short CurrentOpeningLine         = 0;
    private const byte STAGE_OFFSET                     = 1;
    private short CURRENT_STAGE                         = 1 - STAGE_OFFSET;
    private short CURRENT_STAGE_LINES_DIFF              = 0;
    private volatile float Offset                       = 0;
    private volatile float OpeningOffset                = 0;
    private volatile short TransitionOffset             = 0;
    private volatile int StartScreenFrame               = 0;
    private volatile int EndScreenFrame                 = 0;
    private volatile int CurrentLineYPosition           = 0;
    private volatile bool CanDrawBackground             = false;
    private volatile bool CanDrawStageOpening           = true;
    private volatile bool CanStartTheStage              = false;
    private volatile bool CanStartStageOpening          = true;
    private volatile bool RunStage                      = false;
    private volatile bool TransitionBtwStages           = false;
    private List<GameSprite> CurrentStageSprites;
    //private List<GameSprite> NextStageSprites;
    private Dictionary<int, GameSprite> CurrentStageSpritesDefition = new Dictionary<int, GameSprite>();
    private Dictionary<int, GameSprite> NextStageSpritesDefition    = new Dictionary<int, GameSprite>();

    /**
     * Constructor
     */
    public OldGameStages(IGame game)
    {
        //store the game reference
        this.GameRef = game;

        //control the current stage lines count
        //  if the stage is odd there are 574 lines / otherwise 603
        this.ControlStageLinesCount();

        //calc the scale
        this.ScaleW = (float)((float)this.GameRef.WindowSize.Width / (float)this.GameRef.GetInternalResolutionWidth());
        this.ScaleH = (float)((float)this.GameRef.WindowSize.Height / (float)this.GameRef.GetInternalResolutionHeight());

        //Load the Sprite List for the current stage
        this.LoadSpriteListForSpecifiedStage(CURRENT_STAGE);

        //store the sprites of current stage
        this.CurrentStageSprites = this.CurrentStageSpritesDefition.Values.Where(item => item.Type != GameSprite.HOUSE && item.Type != GameSprite.HOUSE2).ToList();

        //the offset starts negative for the opening animation
        this.Offset = -PIXEL_HEIGHT * OPENING_LINES;
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
     * Upgrade method
     */
    public void Update(long frametime, bool colliding = false) 
    {
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
        else if (this.CanStartTheStage) 
        {
            float step = 0;
            if (this.GameRef.GetPlayer().DOUBLE_SPEED)
            {
                //step = 2; ((float)(100 * ((double)frametime / 10_000_000))) * step;
                step = frametime * 0.00002f;
                Console.WriteLine("double");
            }
            else if (this.GameRef.GetPlayer().HALF_SPEED)
            {
                //step = .5; ((float)(100 * ((double)frametime / 10_000_000))) * step;
                step = frametime * 0.000005f;
                Console.WriteLine("half");
            } 
            else //default velocity
            {
                //step = 1; ((float)(100 * ((double)frametime / 10_000_000))) * step;
                step = frametime * 0.00001f;
            }

            //flag the draw
            this.CanDrawBackground = true;

            //scroll down if not collided
            if (!colliding && this.RunStage) 
            {
                if (this.CurrentLine > 95) {
                    //calc the offset
                    this.Offset += step;
                    if (this.Offset >= PIXEL_HEIGHT) {
                        this.CurrentLine--;
                        this.Offset = 0;
                    }
                } else {
                    this.TransitionBtwStages = true;
                    this.TransitionOffset++;
                }
            }

            //if not already dead, check for dead (w/ bg)
            if (!colliding && this.RunStage) 
            {
                //Check if the Player is colliding with background
                this.CheckBackgroundCollision();
            }
        }

        //update the sprites
        //110 = 95 (screen lines before current line) + 15 lines (60 pixels, max sprite height)
        this.StartScreenFrame           = (this.CurrentLine - 110) * PIXEL_HEIGHT;
        this.EndScreenFrame             = (this.CurrentLine + 13)  * PIXEL_HEIGHT;
        this.CurrentLineYPosition       = (this.CurrentLine - 95)  * PIXEL_HEIGHT;

        //if exist an sprite in the current screen frame, update it
        foreach (var item in this.CurrentStageSpritesDefition.Where(item => this.StartScreenFrame < item.Key && this.EndScreenFrame > item.Key)) 
        {
            item.Value.Y = (item.Key - this.CurrentLineYPosition) + this.Offset + this.TransitionOffset;
            item.Value.Update(frametime, colliding);
        }
    }

    /**
     * Draw method
     */
    public void Draw(Graphics gfx) 
    {
        //draw the stage opening
        if (this.CanDrawStageOpening) 
        {
            //draw the river
            gfx.FillRectangle(Brushes[0], 0, 0, GameRef.GetInternalResolutionWidth(), GameRef.GetInternalResolutionHeight());
            gfx.FillRectangle(Brushes[8], 0, 0, 738, 516);
            this.DrawStageOpening(gfx);
        }
        else
        {
            //after the opening, draw the background
            if (this.CanDrawBackground) 
            {
                gfx.FillRectangle(Brushes[0], 0, 0, GameRef.GetInternalResolutionWidth(), GameRef.GetInternalResolutionHeight());
                gfx.FillRectangle(Brushes[8], 0, 0, 738, 516);
                this.DrawBackground(gfx);
            }

            if (this.TransitionBtwStages)
            {
                foreach (var item in this.NextStageSpritesDefition.Where(item => this.StartScreenFrame < item.Key && this.EndScreenFrame > item.Key))
                {
                    item.Value.Draw(gfx);
                }   
            }
        }

        //Draw the sprites
        foreach (var item in this.CurrentStageSpritesDefition.Where(item => this.StartScreenFrame < item.Key && this.EndScreenFrame > item.Key))
        {
            item.Value.Draw(gfx);
        }

        //Render
        //this.BufferedGraphics.Render(gfx);
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
        int clBefore                = this.CurrentLine + 3;
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
        short p1Value = IStagesDef.stages[CURRENT_STAGE, this.CurrentLine + 1, columnP1];
        short p2Value = IStagesDef.stages[CURRENT_STAGE, this.CurrentLine + 1, columnP2];
        if (!(p1Value == 0 && p2Value == 0))
        {
            //if it's an obstacle, collide
            this.GameRef.PlayerCollided();
        }
    }

    /**
     * Check if bullet is colliding with the background
     */
    internal bool IsShotCollidingWithBackground(GameSprite sprite)
    {
        //an offset to improve the visual impact of bullet with the background
        byte offset         = 1;
        
        //get the column of the left side of the bullet
        int column1         = (int)(sprite.X / PIXEL_WIDTH);

        //get the column of the right side of the bullet (can be the same)
        int column2         = (int)((sprite.X + sprite.Width) / PIXEL_WIDTH);

        //get the line of the top of the bullet
        int lineTop         = (int)(sprite.Y / PIXEL_HEIGHT);
        int pixelToCheck    = this.CurrentLine - 95 + lineTop + offset;

        //like this:
        //-----------////-----------
        //-----------/  /-----------

        //check the current value of both pixels (column1 & line) && (column2 && line)
        short currentValue1 = IStagesDef.stages[CURRENT_STAGE, pixelToCheck, column1];
        short currentValue2 = IStagesDef.stages[CURRENT_STAGE, pixelToCheck, column2];
        
        //if both are 0, way to go, otherwise, destroy
        return (!(currentValue1 == 0 && currentValue2 == 0));
    }

    /**
     * Draw the opening scene
     */
    private void DrawStageOpening(Graphics gfx)
    {
        int currentOpeningLine  = this.CurrentOpeningLine;
        int stageLinesCount     = IStagesDef.stages.GetLength(1);
        int openingColumnsCount = IStagesDef.opening.GetLength(2);

        for (int i = currentOpeningLine; i < OPENING_LINES; i++) 
        {
            for (int j = 0; j < openingColumnsCount; j++) 
            {
                byte renderBlock = IStagesDef.opening[CURRENT_STAGE, i, j];
                if (renderBlock == 1 || renderBlock == 2) 
                {
                    this.DrawRect.X =  j * PIXEL_WIDTH;
                    this.DrawRect.Y =  (i * PIXEL_HEIGHT) + this.OpeningOffset - 4;
                    gfx.FillRectangle(this.Brushes[renderBlock], this.DrawRect);
                }
            }
        }

        int sceneBeginning = stageLinesCount - currentOpeningLine - CURRENT_STAGE_LINES_DIFF;
        for (int i = sceneBeginning, z = 0; i < (stageLinesCount - CURRENT_STAGE_LINES_DIFF); i++, z++) 
        {
            for (int j = 0; j < openingColumnsCount; j++) 
            {
                short renderBlock = IStagesDef.stages[CURRENT_STAGE, i, j];
               
                if (renderBlock == 1 || renderBlock == 2 || renderBlock == 5 || renderBlock == 6 || renderBlock == 7) 
                {
                    this.DrawRect.X =  j * PIXEL_WIDTH;
                    this.DrawRect.Y = (z * PIXEL_HEIGHT) + this.OpeningOffset - 4;
                    gfx.FillRectangle(this.Brushes[renderBlock], this.DrawRect);
                }
            }
        }

        if (currentOpeningLine == OPENING_LINES) 
        {
            this.Offset = 0;
            this.CanStartStageOpening   = false;
            this.CanStartTheStage       = true;
            this.GameRef.TogglePlayerSprite();
        }

        this.CanDrawStageOpening = false;
    }

    /**
     * Render the current stage at the current frame
     */
    private void DrawBackground(Graphics gfx) 
    {
        int currentMinus95  = this.CurrentLine - 95;
        int currentPlus13   = currentMinus95 + 108;
        int stagesColumns   = IStagesDef.stages.GetLength(2);

        if (!this.TransitionBtwStages)
        {
            for (int i = currentMinus95, c = -1; i < currentPlus13; i++, c++) 
            {
                for (int j = 0; j < stagesColumns; j++) 
                {
                    short renderBlock = IStagesDef.stages[CURRENT_STAGE, i, j];
                    if (renderBlock != 0) 
                    {
                        this.DrawRect.X =  j * PIXEL_WIDTH;
                        this.DrawRect.Y = (c * PIXEL_HEIGHT) + this.Offset;
                        gfx.FillRectangle(this.Brushes[renderBlock], this.DrawRect);
                    }
                }
            }
        }
        else
        {
            for (int i = currentMinus95; i < currentPlus13; i++) 
            {
                for (int j = 0; j < stagesColumns; j++) 
                {
                    short renderBlock = IStagesDef.stages[CURRENT_STAGE, i, j];
                    if (renderBlock == 1 || renderBlock == 2 || renderBlock == 5 || renderBlock == 6 || renderBlock == 7) 
                    {
                        this.DrawRect.X =  j * PIXEL_WIDTH;
                        this.DrawRect.Y = (i * PIXEL_HEIGHT) + this.TransitionOffset;
                        gfx.FillRectangle(this.Brushes[renderBlock], this.DrawRect);
                    }
                }
            }

            //Draw the next stage
            //...
            

        }
        
        this.CanDrawBackground = false;
    }

    /**
     * Start the stage
     */
    public void Start()
    {
        this.RunStage = true;
    }

    public void ControlStageLinesCount()
    {
        //CURRENT_STAGE++;
        CURRENT_STAGE_LINES_DIFF = (short)((CURRENT_STAGE % 2 == 0)?29:0);
        CurrentLine = (short)((CURRENT_STAGE % 2 == 0)?574:603);
    }

    /**
     * Reset stages elements
     */
    public void Reset()
    {
        this.CurrentLine            = 574;
        this.OpeningOffset          = 0;
        this.CurrentOpeningLine     = 0;
        this.CanDrawBackground      = false;
        this.CanDrawStageOpening    = true;
        this.CanStartTheStage       = false;
        this.CanStartStageOpening   = true;
        this.RunStage               = false;
        this.GameRef.DisablePlayerSprite();
        this.TransitionBtwStages    = false;
        this.TransitionOffset       = 0;
        this.Offset                 = -PIXEL_HEIGHT * OPENING_LINES; //the offset starts negative for the opening animation
        this.ControlStageLinesCount();

        foreach (var item in this.CurrentStageSpritesDefition) 
        {
            item.Value.Reset();
        }
    }

    /**
     * Return the current sprite list
     */
    public IEnumerable<GameSprite> GetCurrentScreenSprites() 
    {
        return (this.CurrentStageSprites.Where(item => this.StartScreenFrame < item.OgY && this.EndScreenFrame > item.OgY));
    }
}