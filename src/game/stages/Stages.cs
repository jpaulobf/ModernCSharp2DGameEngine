namespace game.stages;

public class Stages : StagesDef {

    private GameInterface GameRef;
    private BufferedGraphics bufferedGraphics;
    private Bitmap bufferedImage;
    private Graphics internalGraphics;
    private float scaleW            = 1.0F;
    private float scaleH            = 1.0F;
    private const byte PIXEL_WIDTH  = 18;
    private const byte PIXEL_HEIGHT = 4;
    private SolidBrush green1       = new SolidBrush(Color.FromArgb(255, 110, 156, 66));
    private SolidBrush gray1        = new SolidBrush(Color.FromArgb(255, 111, 111, 111));
    private SolidBrush gray2        = new SolidBrush(Color.FromArgb(255, 170, 170, 170));
    private SolidBrush blue         = new SolidBrush(Color.FromArgb(255, 45, 50, 184));
    private SolidBrush yellow       = new SolidBrush(Color.FromArgb(255, 234, 234, 70));
    private Rectangle drawRect      = new Rectangle(0, 0, PIXEL_WIDTH, PIXEL_HEIGHT);
    private byte renderBlock        = 0;
    protected short currentLine     = 574;
    private short CURRENT_STAGE     = 1;
    private byte offset             = 0;
    private long framecount         = 0;
    private Dictionary<int, SpriteConstructor> stage1_sprites = new Dictionary<int, SpriteConstructor>();

    /**
     * Constructor
     */
    public Stages(GameInterface game) {
        this.GameRef = game;

        //create the imagebuffer
        this.bufferedImage      = new Bitmap(GameRef.GetInternalResolutionWidth(), GameRef.GetInternalResolutionHeight());
        this.bufferedGraphics   = BufferedGraphicsManager.Current.Allocate(Graphics.FromImage(this.bufferedImage), new Rectangle(0, 0, this.GameRef.WindowSize.Width, this.GameRef.WindowSize.Height));
        this.internalGraphics   = bufferedGraphics.Graphics;

        //define the interpolation mode
        this.internalGraphics.InterpolationMode = this.GameRef.Interpolation;

        //calc the scale
        this.scaleW = (float)((float)this.GameRef.WindowSize.Width/(float)this.GameRef.GetInternalResolutionWidth());
        this.scaleH = (float)((float)this.GameRef.WindowSize.Height/(float)this.GameRef.GetInternalResolutionHeight());

        //transform the image based on calc scale
        this.internalGraphics.ScaleTransform(scaleW, scaleH);

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
        this.framecount += frametime;

        if (this.framecount >= 90_000) {
            //render just from time to time
            this.DrawBackground();

            //reset framecount
            this.framecount = 0;

            if (!colliding) 
            {
                //calc the offset
                this.offset++;
                if (this.offset == PIXEL_HEIGHT) {
                    this.currentLine--;
                    this.offset = 0;
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
        for (short i = (short)(currentLine + 3); i < (currentLine + 10); i++) {
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
                Console.WriteLine("aui....");
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
        int startScreenFrame        = (this.currentLine - 115) * PIXEL_HEIGHT;
        int endScreenFrame          = (this.currentLine + 13)  * PIXEL_HEIGHT;
        int currentLineYPosition    = (this.currentLine - 95)  * PIXEL_HEIGHT;

        //if exist an sprite in the current screen frame, render it
        foreach (var item in this.stage1_sprites.Where(item => startScreenFrame < item.Key && endScreenFrame > item.Key)) {
            
            //if (startScreenFrame < item.Key && endScreenFrame > item.Key) {
            item.Value.UpdateAndRender(this.internalGraphics, frametime, currentLineYPosition, this.offset, item.Key, colliding);
        }
    }

    /**
     * Draw method
     */
    public void Draw(Graphics gfx, long frametime) {
        this.bufferedGraphics.Render(gfx);
    }

    /**
     * Render the current stage at the current frame
     */
    private void DrawBackground() {
        this.internalGraphics.FillRectangle(blue, 0, 0, GameRef.GetInternalResolutionWidth(), GameRef.GetInternalResolutionHeight());
        for (short i = (short)(currentLine - 95), c = -1; i < (currentLine + 13); i++, c++) {
            for (short j = 0; j < StagesDef.stages.GetLength(2); j++) {
                this.renderBlock = StagesDef.stages[CURRENT_STAGE - 1, i, j];
                if (this.renderBlock == 1) {
                    this.drawRect.X =  j * PIXEL_WIDTH;
                    this.drawRect.Y = (c * PIXEL_HEIGHT) + this.offset;
                    this.internalGraphics.FillRectangle(this.green1, this.drawRect);
                } else if (this.renderBlock == 5) {
                    this.drawRect.X =  j * PIXEL_WIDTH;
                    this.drawRect.Y = (c * PIXEL_HEIGHT) + this.offset;
                    this.internalGraphics.FillRectangle(this.gray1, this.drawRect);
                } else if (this.renderBlock == 6) {
                    this.drawRect.X =  j * PIXEL_WIDTH;
                    this.drawRect.Y = (c * PIXEL_HEIGHT) + this.offset;
                    this.internalGraphics.FillRectangle(this.gray2, this.drawRect);
                } else if (this.renderBlock == 7) {
                    this.drawRect.X =  j * PIXEL_WIDTH;
                    this.drawRect.Y = (c * PIXEL_HEIGHT) + this.offset;
                    this.internalGraphics.FillRectangle(this.yellow, this.drawRect);
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
            this.scaleW = (float)((float)width/(float)this.GameRef.GetInternalResolutionWidth());
            this.scaleH = (float)((float)height/(float)this.GameRef.GetInternalResolutionHeight());

            //Invalidate the current buffer
            BufferedGraphicsManager.Current.Invalidate();

            await Task.Run(() => {
                //apply new scale
                this.bufferedGraphics = BufferedGraphicsManager.Current.Allocate(Graphics.FromImage(this.bufferedImage), new Rectangle(0, 0, width, height));        
                this.internalGraphics = bufferedGraphics.Graphics;
                this.internalGraphics.ScaleTransform(scaleW, scaleH);
                this.internalGraphics.InterpolationMode = this.GameRef.Interpolation;
            });
        } catch {}  
    }

    public void Reset()
    {
        this.currentLine    = 574;
        this.offset         = 0;
        this.framecount     = 0;
        foreach (var item in this.stage1_sprites) {
            item.Value.Reset();
        }
    }
}