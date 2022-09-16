namespace game.stages;

public class Stages : StagesDef {

    private BufferedGraphics bufferedGraphics;
    private Bitmap bufferedImage;
    private Graphics internalGraphics;
    private float scaleW = 1.0F;
    private float scaleH = 1.0F;
    private SolidBrush green1 = new SolidBrush(Color.FromArgb(255, 110, 156, 66));
    private SolidBrush gray1 = new SolidBrush(Color.FromArgb(255, 111, 111, 111));
    private SolidBrush gray2 = new SolidBrush(Color.FromArgb(255, 170, 170, 170));
    private SolidBrush blue = new SolidBrush(Color.FromArgb(255, 45, 50, 184));
    private SolidBrush yellow = new SolidBrush(Color.FromArgb(255, 234, 234, 70));
    private byte renderBlock = 0;
    private Rectangle drawRect = new Rectangle(0, 0, 18, 4);
    private GameInterface gameref;
    private long framecount = 0;
    protected short currentLine = 574;
    private short CURRENT_STAGE = 1;
    private byte offset = 0;
    private Dictionary<int, Conf> stage1_sprites = new Dictionary<int, Conf>();

    public Stages(GameInterface game) {
        this.gameref = game;

        //create the imagebuffer
        this.bufferedImage      = new Bitmap(gameref.GetInternalResolutionWidth(), gameref.GetInternalResolutionHeight());
        this.bufferedGraphics   = BufferedGraphicsManager.Current.Allocate(Graphics.FromImage(this.bufferedImage), new Rectangle(0, 0, this.gameref.WindowSize.Width, this.gameref.WindowSize.Height));
        this.internalGraphics   = bufferedGraphics.Graphics;

        //define the interpolation mode
        this.internalGraphics.InterpolationMode = this.gameref.interpolationMode;

        //calc the scale
        this.scaleW = (float)((float)this.gameref.WindowSize.Width/(float)this.gameref.GetInternalResolutionWidth());
        this.scaleH = (float)((float)this.gameref.WindowSize.Height/(float)this.gameref.GetInternalResolutionHeight());

        //transform the image based on calc scale
        this.internalGraphics.ScaleTransform(scaleW, scaleH);

        //add stage 1 sprites
        this.stage1_sprites.Add(2216, new Conf(game, Conf.HOUSE, 85));
        this.stage1_sprites.Add(2159, new Conf(game, Conf.SHIP, 325, 1, true));
        this.stage1_sprites.Add(2063, new Conf(game, Conf.FUEL, 417));
        this.stage1_sprites.Add(2012, new Conf(game, Conf.HELI, 302));
        this.stage1_sprites.Add(1923, new Conf(game, Conf.FUEL, 372));
        this.stage1_sprites.Add(1850, new Conf(game, Conf.FUEL, 458));
        this.stage1_sprites.Add(1783, new Conf(game, Conf.HOUSE, 557));
        this.stage1_sprites.Add(1710, new Conf(game, Conf.HOUSE2, 81));
        this.stage1_sprites.Add(1637, new Conf(game, Conf.HOUSE2, 545));
        this.stage1_sprites.Add(1557, new Conf(game, Conf.FUEL, 394));
        this.stage1_sprites.Add(1499, new Conf(game, Conf.HELI, 261));
        this.stage1_sprites.Add(1410, new Conf(game, Conf.FUEL, 288));
        this.stage1_sprites.Add(1353, new Conf(game, Conf.HELI, 339));
        this.stage1_sprites.Add(1263, new Conf(game, Conf.FUEL, 439));
        this.stage1_sprites.Add(1197, new Conf(game, Conf.HOUSE, 581));
        this.stage1_sprites.Add(1140, new Conf(game, Conf.SHIP, 417));
        this.stage1_sprites.Add(1060, new Conf(game, Conf.HELI, 417));
        this.stage1_sprites.Add(993,  new Conf(game, Conf.SHIP, 302));
        this.stage1_sprites.Add(897,  new Conf(game, Conf.FUEL, 371));
        this.stage1_sprites.Add(840,  new Conf(game, Conf.HELI, 458));
        this.stage1_sprites.Add(757,  new Conf(game, Conf.HOUSE, 564));
        this.stage1_sprites.Add(684,  new Conf(game, Conf.HOUSE2, 94));
        this.stage1_sprites.Add(611,  new Conf(game, Conf.HOUSE2, 568));
        this.stage1_sprites.Add(547,  new Conf(game, Conf.HELI, 444, 1, true));
        this.stage1_sprites.Add(464,  new Conf(game, Conf.HOUSE, 586));
        this.stage1_sprites.Add(407,  new Conf(game, Conf.SHIP, 417));
        this.stage1_sprites.Add(327,  new Conf(game, Conf.HELI, 426));
    }
        /*

        if ( ((currentLine - 115) * 4) < 327 && ((currentLine + 13) * 4) > 327) {
            RenderHeli(frametime, 426, 327);
        }

        if ( ((currentLine - 115) * 4) < 245 && ((currentLine + 13) * 4) > 245) {
            RenderHouse(549, 245, 2);
        }

        if ( ((currentLine - 115) * 4) < 164 && ((currentLine + 13) * 4) > 164) {
            RenderFuel(407, 164);
        }

        if ( ((currentLine - 115) * 4) < 107 && ((currentLine + 13) * 4) > 107) {
            RenderHeli(frametime, 288, 107);
        }

        if ( ((currentLine - 115) * 4) < 41 && ((currentLine + 13) * 4) > 41) {
            RenderShip(320, 41);
        } */

    public void Update(long frametime) {
        this.framecount += frametime;

        if (this.framecount >= 250_000) {
            this.RenderBackground();
            this.CheckSprites(frametime);
            this.framecount = 0;
            this.offset++;
            if (this.offset == 4) {
                this.currentLine--;
                this.offset = 0;
            }
        }
    }

    private void CheckSprites(long frametime) {
        int startScreenFrame        = (this.currentLine - 115) * 4;
        int endScreenFrame          = (this.currentLine + 13) * 4;
        int currentLineYPosition    = (this.currentLine - 95) * 4;

        foreach (var item in this.stage1_sprites) {

            if (startScreenFrame < item.Key && endScreenFrame > item.Key) {
                item.Value.Render(frametime, this.internalGraphics, currentLineYPosition, this.offset, item.Key);
            }
            
        }
    }

    /**
     * Render the current stage at the current frame
     */
    private void RenderBackground() {
        this.internalGraphics.FillRectangle(blue, 0, 0, gameref.GetInternalResolutionWidth(), gameref.GetInternalResolutionHeight());
        for (short i = (short)(currentLine - 95), c = -1; i < (currentLine + 13); i++, c++) {
            for (short j = 0; j < StagesDef.stages.GetLength(2); j++) {
                this.renderBlock = StagesDef.stages[CURRENT_STAGE - 1, i, j];
                if (this.renderBlock == 1) {
                    this.drawRect.X = j * 18;
                    this.drawRect.Y = (c * 4) + this.offset;
                    this.internalGraphics.FillRectangle(this.green1, this.drawRect);
                } else if (this.renderBlock == 5) {
                    this.drawRect.X = j * 18;
                    this.drawRect.Y = (c * 4) + this.offset;
                    this.internalGraphics.FillRectangle(this.gray1, this.drawRect);
                } else if (this.renderBlock == 6) {
                    this.drawRect.X = j * 18;
                    this.drawRect.Y = (c * 4) + this.offset;
                    this.internalGraphics.FillRectangle(this.gray2, this.drawRect);
                } else if (this.renderBlock == 7) {
                    this.drawRect.X = j * 18;
                    this.drawRect.Y = (c * 4) + this.offset;
                    this.internalGraphics.FillRectangle(this.yellow, this.drawRect);
                }
            }
        }
    }

    public void Draw(Graphics gfx) {
        this.bufferedGraphics.Render(gfx);
    }

    internal void Resize(object sender, System.EventArgs e)
    {
        //calc new scale
        int width = ((Form)sender).Width;
        int height = ((Form)sender).Height;
        this.scaleW = (float)((float)width/(float)this.gameref.GetInternalResolutionWidth());
        this.scaleH = (float)((float)height/(float)this.gameref.GetInternalResolutionHeight());

        //Invalidate the current buffer
        BufferedGraphicsManager.Current.Invalidate();

        //apply new scale
        this.bufferedGraphics = BufferedGraphicsManager.Current.Allocate(Graphics.FromImage(this.bufferedImage), new Rectangle(0, 0, width, height));        
        this.internalGraphics = bufferedGraphics.Graphics;
        this.internalGraphics.ScaleTransform(scaleW, scaleH);
        this.internalGraphics.InterpolationMode = this.gameref.interpolationMode;
    }
}