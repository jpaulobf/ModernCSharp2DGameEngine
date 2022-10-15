using System.Collections.Generic;

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
    private const byte STATE_DEC                = 1;
    private short CURRENT_STAGE                 = 1 - STATE_DEC;
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
    private Dictionary<int, SpriteConstructor> stage1_sprites = new Dictionary<int, SpriteConstructor>();
    private List<SpriteConstructor> currentSprites;
    private List<SpriteConstructor> nextSprites;

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

        int key = 0;

        //add stage 1 sprites
        this.stage1_sprites.Add(key = 2216, new SpriteConstructor(game, SpriteConstructor.HOUSE, 85, key));
        this.stage1_sprites.Add(key = 2159, new SpriteConstructor(game, SpriteConstructor.SHIP, 325, key, 1, true));
        this.stage1_sprites.Add(key = 2063, new SpriteConstructor(game, SpriteConstructor.FUEL, 417, key));
        this.stage1_sprites.Add(key = 2012, new SpriteConstructor(game, SpriteConstructor.HELI, 302, key, 2));
        this.stage1_sprites.Add(key = 1923, new SpriteConstructor(game, SpriteConstructor.FUEL, 372, key));
        this.stage1_sprites.Add(key = 1850, new SpriteConstructor(game, SpriteConstructor.FUEL, 458, key));
        this.stage1_sprites.Add(key = 1783, new SpriteConstructor(game, SpriteConstructor.HOUSE, 557, key));
        this.stage1_sprites.Add(key = 1710, new SpriteConstructor(game, SpriteConstructor.HOUSE2, 81, key));
        this.stage1_sprites.Add(key = 1637, new SpriteConstructor(game, SpriteConstructor.HOUSE2, 545, key));
        this.stage1_sprites.Add(key = 1557, new SpriteConstructor(game, SpriteConstructor.FUEL, 394, key));
        this.stage1_sprites.Add(key = 1499, new SpriteConstructor(game, SpriteConstructor.HELI, 261, key, 2));
        this.stage1_sprites.Add(key = 1410, new SpriteConstructor(game, SpriteConstructor.FUEL, 288, key));
        this.stage1_sprites.Add(key = 1353, new SpriteConstructor(game, SpriteConstructor.HELI, 339, key, 2));
        this.stage1_sprites.Add(key = 1263, new SpriteConstructor(game, SpriteConstructor.FUEL, 439, key));
        this.stage1_sprites.Add(key = 1197, new SpriteConstructor(game, SpriteConstructor.HOUSE, 581, key));
        this.stage1_sprites.Add(key = 1140, new SpriteConstructor(game, SpriteConstructor.SHIP, 417, key));
        this.stage1_sprites.Add(key = 1060, new SpriteConstructor(game, SpriteConstructor.HELI, 417, key, 2));
        this.stage1_sprites.Add(key = 993,  new SpriteConstructor(game, SpriteConstructor.SHIP, 302, key));
        this.stage1_sprites.Add(key = 897,  new SpriteConstructor(game, SpriteConstructor.FUEL, 371, key));
        this.stage1_sprites.Add(key = 840,  new SpriteConstructor(game, SpriteConstructor.HELI, 458, key, 2));
        this.stage1_sprites.Add(key = 757,  new SpriteConstructor(game, SpriteConstructor.HOUSE, 564, key));
        this.stage1_sprites.Add(key = 684,  new SpriteConstructor(game, SpriteConstructor.HOUSE2, 94, key));
        this.stage1_sprites.Add(key = 611,  new SpriteConstructor(game, SpriteConstructor.HOUSE2, 568, key));
        this.stage1_sprites.Add(key = 547,  new SpriteConstructor(game, SpriteConstructor.HELI, 444, key, 2, true));
        this.stage1_sprites.Add(key = 464,  new SpriteConstructor(game, SpriteConstructor.HOUSE, 586, key));
        this.stage1_sprites.Add(key = 407,  new SpriteConstructor(game, SpriteConstructor.SHIP, 417, key));
        this.stage1_sprites.Add(key = 327,  new SpriteConstructor(game, SpriteConstructor.HELI, 426, key, 2));
        this.stage1_sprites.Add(key = 245,  new SpriteConstructor(game, SpriteConstructor.HOUSE2, 549, key));
        this.stage1_sprites.Add(key = 164,  new SpriteConstructor(game, SpriteConstructor.FUEL, 407, key));
        this.stage1_sprites.Add(key = 107,  new SpriteConstructor(game, SpriteConstructor.HELI, 288, key, 2, false, 198, 540, SpriteConstructor.RIGHT));
        this.stage1_sprites.Add(key = 41,   new SpriteConstructor(game, SpriteConstructor.SHIP, 320, key, 1, false, 288, 450, SpriteConstructor.LEFT));

        //store the sprites of current stage
        this.currentSprites = this.stage1_sprites.Values.Where(item => item.Type != SpriteConstructor.HOUSE && item.Type != SpriteConstructor.HOUSE2).ToList();

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

        int step = 90_000;
        if (this.GameRef.GetPlayerSprite().DOUBLE_SPEED)
        {
            step = 50_000;
        }
        else if (this.GameRef.GetPlayerSprite().HALF_SPEED)
        {
            step = 170_000;
        }

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

        if (this.CanStartTheStage) 
        {
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
            if (!colliding) 
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
        foreach (var item in this.stage1_sprites.Where(item => this.StartScreenFrame < item.Key && this.EndScreenFrame > item.Key)) 
        {
            item.Value.Update(frametime, this.CurrentLineYPosition, this.Offset, item.Key, colliding);
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

            if ((this.GameRef.GetPlayerSprite().X < (firstFromLeftToRight * PIXEL_WIDTH)) || 
                (this.GameRef.GetPlayerSprite().X + this.GameRef.GetPlayerSprite().Width > (firstFromRightToLeft * PIXEL_WIDTH))) 
            {
                this.GameRef.GetPlayerSprite().SetCollision();
                this.GameRef.SetCollidingWithAnEnemy();
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
        foreach (var item in this.stage1_sprites.Where(item => this.StartScreenFrame < item.Key && this.EndScreenFrame > item.Key))
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

        foreach (var item in this.stage1_sprites) 
        {
            item.Value.Reset();
        }
    }

    /**
     * Return the current sprite list
     */
    public IEnumerable<SpriteConstructor> GetCurrentScreenSprites() {
        return (this.currentSprites.Where(item => this.StartScreenFrame < item.Y && this.EndScreenFrame > item.Y));
    }
}