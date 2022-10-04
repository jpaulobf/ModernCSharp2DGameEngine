namespace game.stages;

public class Stages : StagesDef {

    private GameInterface GameRef;
    private BufferedGraphics BufferedGraphics;
    private Bitmap BufferedImage;
    private Graphics InternalGraphics;
    private float ScaleW                    = 1.0F;
    private float ScaleH                    = 1.0F;
    private const byte PIXEL_WIDTH          = 18;
    private const byte PIXEL_HEIGHT         = 4;
    private SolidBrush Green1               = new SolidBrush(Color.FromArgb(255, 110, 156, 66));
    private SolidBrush Gray1                = new SolidBrush(Color.FromArgb(255, 111, 111, 111));
    private SolidBrush Gray2                = new SolidBrush(Color.FromArgb(255, 170, 170, 170));
    private SolidBrush Blue                 = new SolidBrush(Color.FromArgb(255, 45, 50, 184));
    private SolidBrush Yellow               = new SolidBrush(Color.FromArgb(255, 234, 234, 70));
    private Rectangle DrawRect              = new Rectangle(0, 0, PIXEL_WIDTH, PIXEL_HEIGHT);
    private byte RenderBlock                = 0;
    protected volatile short CurrentLine    = 574;
    private short CURRENT_STAGE             = 1;
    private volatile byte Offset            = 0;
    private long Framecount                 = 0;
    private Dictionary<int, SpriteConstructor> stage1_sprites = new Dictionary<int, SpriteConstructor>();

    /**
     * Constructor
     */
    public Stages(GameInterface game) {
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
    public void Update(long frametime, bool colliding = false) {
        //add the framecounter
        this.Framecount += frametime;

        if (this.Framecount >= 90_000) {
            //render just from time to time
            this.DrawBackground();

            //reset framecount
            this.Framecount = 0;

            if (!colliding) 
            {
                //calc the offset
                this.Offset++;
                if (this.Offset == PIXEL_HEIGHT) {
                    this.CurrentLine--;
                    this.Offset = 0;
                }

                //Check if the Player is colliding with background
                this.CheckBackgroundCollision();
            }
        }

        //after render the background update the sprites
        this.CheckSprites(frametime, colliding);
    }

    /**
     * Verify if the player is colliding with the background
     */
    private void CheckBackgroundCollision()
    {
        short firstFromLeftToRight = 0;
        short firstFromRightToLeft = 0;
        short value = -1;

        //check 8 lines
        for (short i = (short)(this.CurrentLine + 3); i < (this.CurrentLine + 10); i++) {
            for (short j = 0; j < StagesDef.stages.GetLength(2); j++) {
                value = StagesDef.stages[CURRENT_STAGE - 1, i, j];
                if (value == 0) {
                    firstFromLeftToRight = j;
                    break;
                }
            }

            for (short j = (short)(StagesDef.stages.GetLength(2) - 1); j > 0; j--) {
                value = StagesDef.stages[CURRENT_STAGE - 1, i, j];
                if (value == 0) {
                    firstFromRightToLeft = (short)(j + 1);
                    break;
                }
            }

            if ((this.GameRef.GetPlayerSprite().X < (firstFromLeftToRight * PIXEL_WIDTH)) || 
                (this.GameRef.GetPlayerSprite().X + this.GameRef.GetPlayerSprite().Width > (firstFromRightToLeft * PIXEL_WIDTH))) {
                this.GameRef.GetPlayerSprite().SetCollision();
                this.GameRef.SetEnemyCollision();
                break;
            } 
        }
    }

    /**
     * Checksprites method.
     */
    internal void CheckSprites(long frametime, bool colliding) {
        int startScreenFrame        = (this.CurrentLine - 115) * PIXEL_HEIGHT;
        int endScreenFrame          = (this.CurrentLine + 13)  * PIXEL_HEIGHT;
        int currentLineYPosition    = (this.CurrentLine - 95)  * PIXEL_HEIGHT;

        //if exist an sprite in the current screen frame, render it
        foreach (var item in this.stage1_sprites.Where(item => startScreenFrame < item.Key && endScreenFrame > item.Key)) {
            
            //if (startScreenFrame < item.Key && endScreenFrame > item.Key) {
            item.Value.UpdateAndRender(this.InternalGraphics, frametime, currentLineYPosition, this.Offset, item.Key, colliding);
        }
    }

    /**
     * Draw method
     */
    public void Draw(Graphics gfx, long frametime) {
        this.BufferedGraphics.Render(gfx);
    }

    /**
     * Render the current stage at the current frame
     */
    private void DrawBackground() {
        this.InternalGraphics.FillRectangle(Blue, 0, 0, GameRef.GetInternalResolutionWidth(), GameRef.GetInternalResolutionHeight());
        for (short i = (short)(CurrentLine - 95), c = -1; i < (CurrentLine + 13); i++, c++) {
            for (short j = 0; j < StagesDef.stages.GetLength(2); j++) {
                this.RenderBlock = StagesDef.stages[CURRENT_STAGE - 1, i, j];
                if (this.RenderBlock == 1) {
                    this.DrawRect.X =  j * PIXEL_WIDTH;
                    this.DrawRect.Y = (c * PIXEL_HEIGHT) + this.Offset;
                    this.InternalGraphics.FillRectangle(this.Green1, this.DrawRect);
                } else if (this.RenderBlock == 5) {
                    this.DrawRect.X =  j * PIXEL_WIDTH;
                    this.DrawRect.Y = (c * PIXEL_HEIGHT) + this.Offset;
                    this.InternalGraphics.FillRectangle(this.Gray1, this.DrawRect);
                } else if (this.RenderBlock == 6) {
                    this.DrawRect.X =  j * PIXEL_WIDTH;
                    this.DrawRect.Y = (c * PIXEL_HEIGHT) + this.Offset;
                    this.InternalGraphics.FillRectangle(this.Gray2, this.DrawRect);
                } else if (this.RenderBlock == 7) {
                    this.DrawRect.X =  j * PIXEL_WIDTH;
                    this.DrawRect.Y = (c * PIXEL_HEIGHT) + this.Offset;
                    this.InternalGraphics.FillRectangle(this.Yellow, this.DrawRect);
                }
            }
        }
    }

    /**
     * Resize the background graphics, when the window resize.
     */
    internal async void Resize(object sender, System.EventArgs e)
    {
        try {
            //calc new scale
            int width = ((Form)sender).Width;
            int height = ((Form)sender).Height;
            this.ScaleW = (float)((float)width/(float)this.GameRef.GetInternalResolutionWidth());
            this.ScaleH = (float)((float)height/(float)this.GameRef.GetInternalResolutionHeight());

            //Invalidate the current buffer
            BufferedGraphicsManager.Current.Invalidate();

            await Task.Run(() => {
                //apply new scale
                this.BufferedGraphics = BufferedGraphicsManager.Current.Allocate(Graphics.FromImage(this.BufferedImage), new Rectangle(0, 0, width, height));        
                this.InternalGraphics = BufferedGraphics.Graphics;
                this.InternalGraphics.ScaleTransform(ScaleW, ScaleH);
                this.InternalGraphics.InterpolationMode = this.GameRef.Interpolation;
            });
        } catch {}  
    }

    public void Reset()
    {
        this.CurrentLine    = 574;
        this.Offset         = 0;
        this.Framecount     = 0;
        foreach (var item in this.stage1_sprites) {
            item.Value.Reset();
        }
    }
}