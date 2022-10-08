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
    private const byte STATE_DEC                = 1;
    private short CURRENT_STAGE                 = 1 - STATE_DEC;
    private volatile byte Offset                = 0;
    private volatile byte OpeningOffset         = 0;
    private long Framecount                     = 0;
    private volatile int StartScreenFrame       = 0;
    private volatile int EndScreenFrame         = 0;
    private volatile int CurrentLineYPosition   = 0;
    private volatile bool CanDrawBackground     = false;
    private volatile bool CanDrawStageOpening   = true;
    private volatile bool CanStartTheStage      = false;
    private volatile bool CanStartStageOpening  = true;
    private Dictionary<int, SpriteConstructor> stage1_sprites = new Dictionary<int, SpriteConstructor>();

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

        //add stage 1 sprites
        this.stage1_sprites.Add(2216, new SpriteConstructor(game, SpriteConstructor.HOUSE, 85));
        this.stage1_sprites.Add(2159, new SpriteConstructor(game, SpriteConstructor.SHIP, 325, 1, true));
        this.stage1_sprites.Add(2063, new SpriteConstructor(game, SpriteConstructor.FUEL, 417));
        this.stage1_sprites.Add(2012, new SpriteConstructor(game, SpriteConstructor.HELI, 302, 2));
        this.stage1_sprites.Add(1923, new SpriteConstructor(game, SpriteConstructor.FUEL, 372));
        this.stage1_sprites.Add(1850, new SpriteConstructor(game, SpriteConstructor.FUEL, 458));
        this.stage1_sprites.Add(1783, new SpriteConstructor(game, SpriteConstructor.HOUSE, 557));
        this.stage1_sprites.Add(1710, new SpriteConstructor(game, SpriteConstructor.HOUSE2, 81));
        this.stage1_sprites.Add(1637, new SpriteConstructor(game, SpriteConstructor.HOUSE2, 545));
        this.stage1_sprites.Add(1557, new SpriteConstructor(game, SpriteConstructor.FUEL, 394));
        this.stage1_sprites.Add(1499, new SpriteConstructor(game, SpriteConstructor.HELI, 261, 2));
        this.stage1_sprites.Add(1410, new SpriteConstructor(game, SpriteConstructor.FUEL, 288));
        this.stage1_sprites.Add(1353, new SpriteConstructor(game, SpriteConstructor.HELI, 339, 2));
        this.stage1_sprites.Add(1263, new SpriteConstructor(game, SpriteConstructor.FUEL, 439));
        this.stage1_sprites.Add(1197, new SpriteConstructor(game, SpriteConstructor.HOUSE, 581));
        this.stage1_sprites.Add(1140, new SpriteConstructor(game, SpriteConstructor.SHIP, 417));
        this.stage1_sprites.Add(1060, new SpriteConstructor(game, SpriteConstructor.HELI, 417, 2));
        this.stage1_sprites.Add(993,  new SpriteConstructor(game, SpriteConstructor.SHIP, 302));
        this.stage1_sprites.Add(897,  new SpriteConstructor(game, SpriteConstructor.FUEL, 371));
        this.stage1_sprites.Add(840,  new SpriteConstructor(game, SpriteConstructor.HELI, 458, 2));
        this.stage1_sprites.Add(757,  new SpriteConstructor(game, SpriteConstructor.HOUSE, 564));
        this.stage1_sprites.Add(684,  new SpriteConstructor(game, SpriteConstructor.HOUSE2, 94));
        this.stage1_sprites.Add(611,  new SpriteConstructor(game, SpriteConstructor.HOUSE2, 568));
        this.stage1_sprites.Add(547,  new SpriteConstructor(game, SpriteConstructor.HELI, 444, 2, true));
        this.stage1_sprites.Add(464,  new SpriteConstructor(game, SpriteConstructor.HOUSE, 586));
        this.stage1_sprites.Add(407,  new SpriteConstructor(game, SpriteConstructor.SHIP, 417));
        this.stage1_sprites.Add(327,  new SpriteConstructor(game, SpriteConstructor.HELI, 426, 2));
        this.stage1_sprites.Add(245,  new SpriteConstructor(game, SpriteConstructor.HOUSE2, 549));
        this.stage1_sprites.Add(164,  new SpriteConstructor(game, SpriteConstructor.FUEL, 407));
        this.stage1_sprites.Add(107,  new SpriteConstructor(game, SpriteConstructor.HELI, 288, 2, false, 198, 540, SpriteConstructor.RIGHT));
        this.stage1_sprites.Add(41,   new SpriteConstructor(game, SpriteConstructor.SHIP, 320, 1, false, 288, 450, SpriteConstructor.LEFT));
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
                this.OpeningOffset += 1;

                //reset framecount
                this.Framecount = 0;

                //update draw stage opening flag
                this.CanDrawStageOpening = true;
            }
        }

        if (this.CanStartTheStage) 
        {
            //control the BG vertical scroll
            if (this.Framecount >= 90_000) 
            {
                //flag the draw
                this.CanDrawBackground = true;

                //scroll down if not collided
                if (!colliding) 
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

            //update the sprites
            int current                 = this.CurrentLine;
            this.StartScreenFrame       = (current - 115) * PIXEL_HEIGHT;
            this.EndScreenFrame         = (current + 13)  * PIXEL_HEIGHT;
            this.CurrentLineYPosition   = (current - 95)  * PIXEL_HEIGHT;

            //if exist an sprite in the current screen frame, render it
            foreach (var item in this.stage1_sprites.Where(item => this.StartScreenFrame < item.Key && this.EndScreenFrame > item.Key)) 
            {
                item.Value.Update(frametime, this.CurrentLineYPosition, this.Offset, item.Key, colliding);
            }
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
                this.GameRef.SetEnemyCollision();
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
        int currentOpeningLine  = this.OpeningOffset;
        int maxOpeningLines     = 108;
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
                    this.DrawRect.Y =  i * PIXEL_HEIGHT;
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
                    this.DrawRect.Y =  z * PIXEL_HEIGHT;
                    this.InternalGraphics.FillRectangle(this.Brushes[renderBlock], this.DrawRect);
                }
            }
        }

        if (currentOpeningLine == maxOpeningLines) 
        {
            this.CanStartStageOpening   = false;
            this.CanStartTheStage       = true;
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
        this.Offset                 = 0;
        this.Framecount             = 0;
        this.OpeningOffset          = 0;
        this.CanDrawBackground      = false;
        this.CanDrawStageOpening    = true;
        this.CanStartTheStage       = false;
        this.CanStartStageOpening   = true;

        foreach (var item in this.stage1_sprites) 
        {
            item.Value.Reset();
        }
    }
}