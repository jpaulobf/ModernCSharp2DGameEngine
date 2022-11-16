namespace Game.Stages;

/**
 * Author: Joao P B Faria
 * Date: Oct/2022
 * Definition: This class represent the Game Stages, with their background and enemy/static sprites.
 */
public class GameStages : IStagesDef 
{
    private IGame GameRef;
    private BufferedGraphics BufferedGraphics;
    private Bitmap BufferedImage;
    private Graphics InternalGraphics;
    private float ScaleW                                = 1.0F;
    private float ScaleH                                = 1.0F;
    private const byte PIXEL_WIDTH                      = 18;
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
    private Rectangle DrawRect                          = new Rectangle(0, 0, PIXEL_WIDTH, PIXEL_HEIGHT);
    protected volatile short CurrentLine                = 574;
    private const short SCREEN_LINES                    = 107 + 1; //one extra buffer line
    private const byte OPENING_LINES                    = 108;
    protected volatile short CurrentOpeningLine         = 0;
    private const byte STAGE_OFFSET                     = 1;
    private short CURRENT_STAGE                         = 1 - STAGE_OFFSET;
    private short CURRENT_STAGE_LINES_DIFF              = 0;
    private volatile short Offset                       = 0;
    private volatile byte OpeningOffset                 = 0;
    private volatile short TransitionOffset             = 0;
    private long Framecount                             = 0;
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
    private Dictionary<int, GameSprite> CurrentStageDef = new Dictionary<int, GameSprite>();
    private Dictionary<int, GameSprite> NextStageDef    = new Dictionary<int, GameSprite>();

    /**
     * Constructor
     */
    public GameStages(IGame game)
    {
        //store the game reference
        this.GameRef = game;

        //create the imagebuffer
        this.BufferedImage      = new Bitmap(GameRef.GetInternalResolutionWidth(), GameRef.GetInternalResolutionHeight());
        this.BufferedGraphics   = BufferedGraphicsManager.Current.Allocate(Graphics.FromImage(this.BufferedImage), new Rectangle(0, 0, this.GameRef.WindowSize.Width, this.GameRef.WindowSize.Height));
        this.InternalGraphics   = BufferedGraphics.Graphics;

        //define the interpolation mode
        this.InternalGraphics.InterpolationMode = this.GameRef.Interpolation;

        //calc the scale
        this.ScaleW = (float)((float)this.GameRef.WindowSize.Width / (float)this.GameRef.GetInternalResolutionWidth());
        this.ScaleH = (float)((float)this.GameRef.WindowSize.Height / (float)this.GameRef.GetInternalResolutionHeight());

        //transform the image based on calc scale
        this.InternalGraphics.ScaleTransform(ScaleW, ScaleH);

        //Load the Sprite List for the current stage
        this.LoadSpriteListForSpecifiedStage(CURRENT_STAGE);

        //store the sprites of current stage
        this.CurrentStageSprites = this.CurrentStageDef.Values.Where(item => item.Type != GameSprite.HOUSE && item.Type != GameSprite.HOUSE2).ToList();

        //the offset starts negative for the opening animation
        this.Offset = -PIXEL_HEIGHT * OPENING_LINES;
    }

    /**
     * Load the Sprite List for the Specified Stage
     */ 
    private void LoadSpriteListForSpecifiedStage(short stage)
    {
        Dictionary<int, GameSprite> temp = (stage == CURRENT_STAGE)?this.CurrentStageDef:this.NextStageDef;
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
        //add the framecounter
        this.Framecount += frametime;

        if (this.CanStartStageOpening) 
        {
            //control the BG vertical scroll
            if (this.Framecount >= 160_000)
            {
                //calc the offset
                this.OpeningOffset  += PIXEL_HEIGHT;

                if (this.OpeningOffset == PIXEL_HEIGHT) {
                    this.CurrentOpeningLine++;
                    this.OpeningOffset = 0;
                    this.Offset += PIXEL_HEIGHT;
                }

                //reset framecount
                this.Framecount = 0;

                //update draw stage opening flag
                this.CanDrawStageOpening = true;
            }
        } 
        else if (this.CanStartTheStage) 
        {
            int step = 0;
            if (this.GameRef.GetPlayer().DOUBLE_SPEED)
            {
                step = 50_000;
            }
            else if (this.GameRef.GetPlayer().HALF_SPEED)
            {
                step = 170_000;
            } 
            else //default velocity
            {
                step = 90_000;
            }

            //control the BG vertical scroll
            if (this.Framecount >= step) 
            {
                //flag the draw
                this.CanDrawBackground = true;

                //scroll down if not collided
                if (!colliding && this.RunStage) 
                {
                    if (this.CurrentLine > 95) {
                        //calc the offset
                        this.Offset++;
                        if (this.Offset == PIXEL_HEIGHT) {
                            this.CurrentLine--;
                            this.Offset = 0;
                        }
                    } else {
                        this.TransitionBtwStages = true;
                        this.TransitionOffset++;
                    }
                }

                //reset framecount
                this.Framecount = 0;
            }

            //if not already dead, check for dead (w/ bg)
            if (!colliding && this.RunStage) 
            {
                //Check if the Player is colliding with background
                this.CheckBackgroundCollision();
            }
        }

        //update the sprites
        int current                     = this.CurrentLine;
        this.StartScreenFrame           = (current - 115) * PIXEL_HEIGHT;
        this.EndScreenFrame             = (current + 13)  * PIXEL_HEIGHT;
        this.CurrentLineYPosition       = (current - 95)  * PIXEL_HEIGHT;
        CURRENT_STAGE_LINES_DIFF        = (short)((CURRENT_STAGE % 2 == 0)?29:0);

        //if exist an sprite in the current screen frame, render it
        foreach (var item in this.CurrentStageDef.Where(item => this.StartScreenFrame < item.Key && this.EndScreenFrame > item.Key)) 
        {
            item.Value.Y = (item.Key - this.CurrentLineYPosition) + this.Offset + this.TransitionOffset;
            item.Value.Update(frametime, colliding);
        }
    }

    /**
     * Draw method
     */
    public void Draw(Graphics gfx, long frametime) 
    {
        //draw the stage opening
        if (this.CanDrawStageOpening) 
        {
            //draw the river
            this.InternalGraphics.FillRectangle(Brushes[8], 0, 0, GameRef.GetInternalResolutionWidth(), GameRef.GetInternalResolutionHeight());
            this.DrawStageOpening();
        }
        
        //after the opening, draw the background
        if (this.CanDrawBackground) 
        {
            this.InternalGraphics.FillRectangle(Brushes[8], 0, 0, GameRef.GetInternalResolutionWidth(), GameRef.GetInternalResolutionHeight());
            this.DrawBackground();
        }

        //Draw the sprites
        foreach (var item in this.CurrentStageDef.Where(item => this.StartScreenFrame < item.Key && this.EndScreenFrame > item.Key))
        {
            item.Value.Draw(this.InternalGraphics);
        }

        //Render
        this.BufferedGraphics.Render(gfx);
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

        //check 8 lines
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
    }

    /**
     * Draw the opening scene
     */
    private void DrawStageOpening()
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
                    this.InternalGraphics.FillRectangle(this.Brushes[renderBlock], this.DrawRect);
                }
            }
        }

        int sceneBeginning = stageLinesCount - currentOpeningLine - CURRENT_STAGE_LINES_DIFF;
        for (int i = sceneBeginning, z = 0; i < stageLinesCount; i++, z++) 
        {
            for (int j = 0; j < openingColumnsCount; j++) 
            {
                short renderBlock = IStagesDef.stages[CURRENT_STAGE, i, j];
               
                if (renderBlock == 1 || renderBlock == 2 || renderBlock == 5 || renderBlock == 6 || renderBlock == 7) 
                {
                    this.DrawRect.X =  j * PIXEL_WIDTH;
                    this.DrawRect.Y =  (z * PIXEL_HEIGHT) + this.OpeningOffset - 4;
                    this.InternalGraphics.FillRectangle(this.Brushes[renderBlock], this.DrawRect);
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
    private void DrawBackground() 
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
                    if (renderBlock == 1 || renderBlock == 2 || renderBlock == 5 || renderBlock == 6 || renderBlock == 7) 
                    {
                        this.DrawRect.X =  j * PIXEL_WIDTH;
                        this.DrawRect.Y = (c * PIXEL_HEIGHT) + this.Offset;
                        this.InternalGraphics.FillRectangle(this.Brushes[renderBlock], this.DrawRect);
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
                        this.InternalGraphics.FillRectangle(this.Brushes[renderBlock], this.DrawRect);
                    }
                }
            }

            //Draw the next stage
            //...
            

        }
        
        
        this.CanDrawBackground = false;
    }

    /**
     * Resize the background graphics, when the window resize.
     */
    internal async void Resize(object sender, System.EventArgs e)
    {
        try 
        {
            //calc new scale
            int width = ((Form)sender).Width;
            int height = ((Form)sender).Height;
            this.ScaleW = (float)((float)width/(float)this.GameRef.GetInternalResolutionWidth());
            this.ScaleH = (float)((float)height/(float)this.GameRef.GetInternalResolutionHeight());

            //Invalidate the current buffer
            BufferedGraphicsManager.Current.Invalidate();

            await Task.Run(() => 
            {
                //apply new scale
                this.BufferedGraphics = BufferedGraphicsManager.Current.Allocate(Graphics.FromImage(this.BufferedImage), new Rectangle(0, 0, width, height));        
                this.InternalGraphics = BufferedGraphics.Graphics;
                this.InternalGraphics.ScaleTransform(ScaleW, ScaleH);
                this.InternalGraphics.InterpolationMode = this.GameRef.Interpolation;
            });
        } 
        catch {}  
    }

    /**
     * Start the stage
     */
    public void Start()
    {
        this.RunStage = true;
    }

    /**
     * Reset stages elements
     */
    public void Reset()
    {
        this.CurrentLine            = 574;
        this.Framecount             = 0;
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

        foreach (var item in this.CurrentStageDef) 
        {
            item.Value.Reset();
        }
    }

    /**
     * Return the current sprite list
     */
    public IEnumerable<GameSprite> GetCurrentScreenSprites() {

        return (this.CurrentStageSprites.Where(item => this.StartScreenFrame < item.OgY && this.EndScreenFrame > item.OgY));

        //return (this.stage1.Values.Where(item => item.Type != GameSprite.HOUSE && item.Type != GameSprite.HOUSE2 && 
        //                                 this.StartScreenFrame < item.OgY && this.EndScreenFrame > item.OgY));
    }
}