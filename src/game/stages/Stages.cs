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
    private StaticSprite houseSprite;
    private StaticSprite house2Sprite;

    private byte offset = 0;
    
    //protected int[,] stage2 = new int[41,587];

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

        //load the house sprite
        this.houseSprite = new StaticSprite(game, "img\\house.png", 73, 44, 85);
        this.house2Sprite = new StaticSprite(game, "img\\house2.png", 73, 44, 81);

        //render the current stage at the current frame
        this.RenderBackground();
    }

    public void Update(long frametime) {
        this.framecount += frametime;

        if (this.framecount >= 100_000) {
            this.RenderBackground();
            this.CheckSprites();
            this.framecount = 0;
            this.offset++;
            if (this.offset >= 4) {
                this.currentLine--;
                this.offset = 0;
            }
        }
    }

    private void CheckSprites() {

        if ( ((currentLine - 115) * 4) < 2216 && ((currentLine + 13) * 4) > 2216) {
            RenderHouse(85, 2216, 1);
        }

        if ( ((currentLine - 115) * 4) < 1783 && ((currentLine + 13) * 4) > 1783) {
            RenderHouse(557, 1783, 1);
        }

        if ( ((currentLine - 115) * 4) < 1710 && ((currentLine + 13) * 4) > 1710) {
            RenderHouse(81, 1710, 2);
        }

        if ( ((currentLine - 115) * 4) < 1637 && ((currentLine + 13) * 4) > 1637) {
            RenderHouse(545, 1637, 2);
        }

        if ( ((currentLine - 115) * 4) < 1197 && ((currentLine + 13) * 4) > 1197) {
            RenderHouse(581, 1197, 1);
        }

        if ( ((currentLine - 115) * 4) < 757 && ((currentLine + 13) * 4) > 757) {
            RenderHouse(564, 757, 2);
        }

        if ( ((currentLine - 115) * 4) < 464 && ((currentLine + 13) * 4) > 464) {
            RenderHouse(586, 464, 1);
        }

        if ( ((currentLine - 115) * 4) < 464 && ((currentLine + 13) * 4) > 464) {
            RenderHouse(549, 245, 2);
        }

        if ( ((currentLine - 115) * 4) < 684 && ((currentLine + 13) * 4) > 684) {
            RenderHouse(94, 684, 1);
        }
    }

    private void RenderHouse(int X, int Y, int type) {
        if (type == 1) {
            this.houseSprite.X = X;
            this.houseSprite.Y = Y - ((currentLine - 95) * 4) + this.offset;
            this.houseSprite.Update(0);
            this.houseSprite.Draw(this.internalGraphics);
        } else if (type == 2) {
            this.house2Sprite.X = X;
            this.house2Sprite.Y = Y - ((currentLine - 95) * 4) + this.offset;
            this.house2Sprite.Update(0);
            this.house2Sprite.Draw(this.internalGraphics);
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
                    this.drawRect.Y = (c * 4) + offset;
                    this.internalGraphics.FillRectangle(this.green1, this.drawRect);
                } else if (this.renderBlock == 5) {
                    this.drawRect.X = j * 18;
                    this.drawRect.Y = (c * 4) + offset;
                    this.internalGraphics.FillRectangle(this.gray1, this.drawRect);
                } else if (this.renderBlock == 6) {
                    this.drawRect.X = j * 18;
                    this.drawRect.Y = (c * 4) + offset;
                    this.internalGraphics.FillRectangle(this.gray2, this.drawRect);
                } else if (this.renderBlock == 7) {
                    this.drawRect.X = j * 18;
                    this.drawRect.Y = (c * 4) + offset;
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

        //Dispose the buffer
        BufferedGraphicsManager.Current.Dispose();

        //apply new scale
        this.bufferedGraphics = BufferedGraphicsManager.Current.Allocate(Graphics.FromImage(this.bufferedImage), new Rectangle(0, 0, width, height));        
        this.internalGraphics = bufferedGraphics.Graphics;
        this.internalGraphics.ScaleTransform(scaleW, scaleH);
        this.internalGraphics.InterpolationMode = this.gameref.interpolationMode;
    }
}