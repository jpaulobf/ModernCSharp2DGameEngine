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
    private float ScaleW                        = 1.0F;
    private float ScaleH                        = 1.0F;
    private const byte PIXEL_WIDTH              = 18;
    private const byte PIXEL_HEIGHT             = 4;
    private SolidBrush [] Brushes;
    private SolidBrush Black                    = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
    private SolidBrush Green1                   = new SolidBrush(Color.FromArgb(255, 110, 156, 66));
    private SolidBrush Green2                   = new SolidBrush(Color.FromArgb(255, 53, 95, 24));
    private SolidBrush Gray1                    = new SolidBrush(Color.FromArgb(255, 111, 111, 111));
    private SolidBrush Gray2                    = new SolidBrush(Color.FromArgb(255, 170, 170, 170));
    private SolidBrush Blue                     = new SolidBrush(Color.FromArgb(255, 45, 50, 184));
    private SolidBrush Yellow                   = new SolidBrush(Color.FromArgb(255, 234, 234, 70));
    private Rectangle DrawRect                  = new Rectangle(0, 0, PIXEL_WIDTH, PIXEL_HEIGHT);
    protected volatile short CurrentLine        = 574;
    private const byte OPENING_LINES            = 108;
    protected volatile short CurrentOpeningLine = 0;
    private const byte STAGE_OFFSET             = 1;
    private short CURRENT_STAGE                 = 1 - STAGE_OFFSET;
    private volatile short Offset               = 0;
    private volatile byte OpeningOffset         = 0;
    private long Framecount                     = 0;
    private volatile int StartScreenFrame       = 0;
    private volatile int EndScreenFrame         = 0;
    private volatile int CurrentLineYPosition   = 0;
    private volatile bool CanDrawBackground     = false;
    private volatile bool CanDrawStageOpening   = true;
    private volatile bool CanStartTheStage      = false;
    private volatile bool CanStartStageOpening  = true;
    private volatile bool RunStage              = false;
    private Dictionary<int, GameSprite> stage1  = new Dictionary<int, GameSprite>();
    private List<GameSprite> currentSprites;
    //private List<GameSprite> nextSprites;

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
        this.ScaleW = (float)((float)this.GameRef.WindowSize.Width/(float)this.GameRef.GetInternalResolutionWidth());
        this.ScaleH = (float)((float)this.GameRef.WindowSize.Height/(float)this.GameRef.GetInternalResolutionHeight());

        //transform the image based on calc scale
        this.InternalGraphics.ScaleTransform(ScaleW, ScaleH);
        this.Brushes = new SolidBrush[] {this.Black, this.Green1, this.Green2, this.Black, this.Black, this.Gray1, this.Gray2, this.Yellow, this.Blue };

        int spriteYPosition = 0;

        //add stage 1 sprites
        this.stage1.Add(spriteYPosition = 2216, SpriteFactory.CreateSprite(game, GameSprite.HOUSE, 85, spriteYPosition));
        this.stage1.Add(spriteYPosition = 2159, SpriteFactory.CreateSprite(game, GameSprite.SHIP, 325, spriteYPosition, 1, true));
        this.stage1.Add(spriteYPosition = 2063, SpriteFactory.CreateSprite(game, GameSprite.FUEL, 417, spriteYPosition));
        this.stage1.Add(spriteYPosition = 2012, SpriteFactory.CreateSprite(game, GameSprite.HELI, 302, spriteYPosition, 2));
        this.stage1.Add(spriteYPosition = 1923, SpriteFactory.CreateSprite(game, GameSprite.FUEL, 372, spriteYPosition));
        this.stage1.Add(spriteYPosition = 1850, SpriteFactory.CreateSprite(game, GameSprite.FUEL, 458, spriteYPosition));
        this.stage1.Add(spriteYPosition = 1783, SpriteFactory.CreateSprite(game, GameSprite.HOUSE, 557, spriteYPosition));
        this.stage1.Add(spriteYPosition = 1710, SpriteFactory.CreateSprite(game, GameSprite.HOUSE2, 81, spriteYPosition));
        this.stage1.Add(spriteYPosition = 1637, SpriteFactory.CreateSprite(game, GameSprite.HOUSE2, 545, spriteYPosition));
        this.stage1.Add(spriteYPosition = 1557, SpriteFactory.CreateSprite(game, GameSprite.FUEL, 394, spriteYPosition));
        this.stage1.Add(spriteYPosition = 1499, SpriteFactory.CreateSprite(game, GameSprite.HELI, 261, spriteYPosition, 2));
        this.stage1.Add(spriteYPosition = 1410, SpriteFactory.CreateSprite(game, GameSprite.FUEL, 288, spriteYPosition));
        this.stage1.Add(spriteYPosition = 1353, SpriteFactory.CreateSprite(game, GameSprite.HELI, 339, spriteYPosition, 2));
        this.stage1.Add(spriteYPosition = 1263, SpriteFactory.CreateSprite(game, GameSprite.FUEL, 439, spriteYPosition));
        this.stage1.Add(spriteYPosition = 1197, SpriteFactory.CreateSprite(game, GameSprite.HOUSE, 581, spriteYPosition));
        this.stage1.Add(spriteYPosition = 1140, SpriteFactory.CreateSprite(game, GameSprite.SHIP, 417, spriteYPosition));
        this.stage1.Add(spriteYPosition = 1060, SpriteFactory.CreateSprite(game, GameSprite.HELI, 417, spriteYPosition, 2));
        this.stage1.Add(spriteYPosition = 993,  SpriteFactory.CreateSprite(game, GameSprite.SHIP, 302, spriteYPosition));
        this.stage1.Add(spriteYPosition = 897,  SpriteFactory.CreateSprite(game, GameSprite.FUEL, 371, spriteYPosition));
        this.stage1.Add(spriteYPosition = 840,  SpriteFactory.CreateSprite(game, GameSprite.HELI, 458, spriteYPosition, 2));
        this.stage1.Add(spriteYPosition = 757,  SpriteFactory.CreateSprite(game, GameSprite.HOUSE, 564, spriteYPosition));
        this.stage1.Add(spriteYPosition = 684,  SpriteFactory.CreateSprite(game, GameSprite.HOUSE2, 94, spriteYPosition));
        this.stage1.Add(spriteYPosition = 611,  SpriteFactory.CreateSprite(game, GameSprite.HOUSE2, 568, spriteYPosition));
        this.stage1.Add(spriteYPosition = 547,  SpriteFactory.CreateSprite(game, GameSprite.HELI, 444, spriteYPosition, 2, true));
        this.stage1.Add(spriteYPosition = 464,  SpriteFactory.CreateSprite(game, GameSprite.HOUSE, 586, spriteYPosition));
        this.stage1.Add(spriteYPosition = 407,  SpriteFactory.CreateSprite(game, GameSprite.SHIP, 417, spriteYPosition));
        this.stage1.Add(spriteYPosition = 327,  SpriteFactory.CreateSprite(game, GameSprite.HELI, 426, spriteYPosition, 2));
        this.stage1.Add(spriteYPosition = 245,  SpriteFactory.CreateSprite(game, GameSprite.HOUSE2, 549, spriteYPosition));
        this.stage1.Add(spriteYPosition = 164,  SpriteFactory.CreateSprite(game, GameSprite.FUEL, 407, spriteYPosition));
        this.stage1.Add(spriteYPosition = 107,  SpriteFactory.CreateSprite(game, GameSprite.HELI, 288, spriteYPosition, 2, false, 198, 540, GameSprite.RIGHT));
        this.stage1.Add(spriteYPosition = 41,   SpriteFactory.CreateSprite(game, GameSprite.SHIP, 320, spriteYPosition, 1, false, 288, 450, GameSprite.LEFT));

        //store the sprites of current stage
        this.currentSprites = this.stage1.Values.Where(item => item.Type != GameSprite.HOUSE && item.Type != GameSprite.HOUSE2).ToList();

        //the offset starts negative for the opening animation
        this.Offset = PIXEL_HEIGHT * OPENING_LINES * -1;
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
                this.OpeningOffset += 4;
                if (this.OpeningOffset == PIXEL_HEIGHT) {
                    this.CurrentOpeningLine++;
                    this.OpeningOffset = 0;

                    this.Offset += 4;
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
                    //calc the offset
                    this.Offset++;
                    if (this.Offset == PIXEL_HEIGHT) {
                        this.CurrentLine--;
                        this.Offset = 0;
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

        //if exist an sprite in the current screen frame, render it
        foreach (var item in this.stage1.Where(item => this.StartScreenFrame < item.Key && this.EndScreenFrame > item.Key)) 
        {
            item.Value.Y = (item.Key - this.CurrentLineYPosition) + this.Offset;
            item.Value.Update(frametime, colliding);
        }
    }

    /**
     * Start the stage
     */
    internal void Start()
    {
        this.RunStage = true;
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
     * Draw method
     */
    public void Draw(Graphics gfx, long frametime) 
    {
        //draw the stage opening
        if (this.CanDrawStageOpening) 
        {
            //draw the river
            this.InternalGraphics.FillRectangle(Blue, 0, 0, GameRef.GetInternalResolutionWidth(), GameRef.GetInternalResolutionHeight());
            this.DrawStageOpening();
        }
        
        //after the opening, draw the background
        if (this.CanDrawBackground) 
        {
            this.InternalGraphics.FillRectangle(Blue, 0, 0, GameRef.GetInternalResolutionWidth(), GameRef.GetInternalResolutionHeight());
            this.DrawBackground();
        }

        //Draw the sprites
        foreach (var item in this.stage1.Where(item => this.StartScreenFrame < item.Key && this.EndScreenFrame > item.Key))
        {
            item.Value.Draw(this.InternalGraphics);
        }

        //Render
        this.BufferedGraphics.Render(gfx);
    }

    /**
     * Draw the opening scene
     */
    private void DrawStageOpening()
    {
        int currentOpeningLine  = this.CurrentOpeningLine;
        int maxOpeningLines     = OPENING_LINES;
        int stageLinesCount     = IStagesDef.stages.GetLength(1);
        int openingColumnsCount = IStagesDef.opening.GetLength(2);

        for (int i = currentOpeningLine; i < maxOpeningLines; i++) 
        {
            for (int j = 0; j < openingColumnsCount; j++) 
            {
                byte renderBlock = IStagesDef.opening[CURRENT_STAGE, i, j];
                if (renderBlock == 1) 
                {
                    this.DrawRect.X =  j * PIXEL_WIDTH;
                    this.DrawRect.Y =  (i * PIXEL_HEIGHT) + this.OpeningOffset - 3;
                    this.InternalGraphics.FillRectangle(this.Brushes[renderBlock], this.DrawRect);
                }
            }
        }

        int sceneBeginning = stageLinesCount - currentOpeningLine;
        for (int i = sceneBeginning, z = 0; i < stageLinesCount; i++, z++) 
        {
            for (int j = 0; j < openingColumnsCount; j++) 
            {
                byte renderBlock = IStagesDef.stages[CURRENT_STAGE, i, j];
               
                if (renderBlock == 1 || renderBlock == 2 || renderBlock == 5 || renderBlock == 6 || renderBlock == 7) 
                {
                    this.DrawRect.X =  j * PIXEL_WIDTH;
                    this.DrawRect.Y =  (z * PIXEL_HEIGHT) + this.OpeningOffset - 3;
                    this.InternalGraphics.FillRectangle(this.Brushes[renderBlock], this.DrawRect);
                }
            }
        }

        if (currentOpeningLine == maxOpeningLines) 
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
        for (int i = currentMinus95, c = -1; i < currentPlus13; i++, c++) 
        {
            for (int j = 0; j < stagesColumns; j++) 
            {
                byte renderBlock = IStagesDef.stages[CURRENT_STAGE, i, j];
                if (renderBlock == 1 || renderBlock == 2 || renderBlock == 5 || renderBlock == 6 || renderBlock == 7) 
                {
                    this.DrawRect.X =  j * PIXEL_WIDTH;
                    this.DrawRect.Y = (c * PIXEL_HEIGHT) + this.Offset;
                    this.InternalGraphics.FillRectangle(this.Brushes[renderBlock], this.DrawRect);
                }
            }
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
        this.Offset                 = PIXEL_HEIGHT * OPENING_LINES * -1; //the offset starts negative for the opening animation

        foreach (var item in this.stage1) 
        {
            item.Value.Reset();
        }
    }

    /**
     * Return the current sprite list
     */
    public IEnumerable<GameSprite> GetCurrentScreenSprites() {

        return (this.currentSprites.Where(item => this.StartScreenFrame < item.OgY && this.EndScreenFrame > item.OgY));

        //return (this.stage1.Values.Where(item => item.Type != GameSprite.HOUSE && item.Type != GameSprite.HOUSE2 && 
        //                                 this.StartScreenFrame < item.OgY && this.EndScreenFrame > item.OgY));
    }
}