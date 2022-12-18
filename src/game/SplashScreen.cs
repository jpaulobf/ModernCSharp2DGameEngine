namespace Game;

using Util;
using Engine;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Game;
using System.Drawing.Drawing2D;

/**
 *
 */
public class SplashScreen : Form, ICanvasEngine 
{
    //this window properties
    private int PositionX                       = 0;
    private int PositionY                       = 0;
    private int WindowWidth                     = 800;
    private int WindowHeight                    = 500;
    private int W, H, X, Y                      = 0;

    //desktop properties
    private int ResolutionH                     = 0;
    private int ResolutionW                     = 0;

    //splash screen image
    private Bitmap SplashImage = LoadingStuffs.GetInstance().GetImage("splash");

    /**
     * Constructor
     */
    public SplashScreen()
    {
        //////////////////////////////////////////////////////////////////////
        // ->>>  for the window
        //////////////////////////////////////////////////////////////////////
        LoadingStuffs.GetInstance();

        //set some properties for this window
        Dimension basic = new Dimension(this.windowWidth, this.windowHeight);
        this.setPreferredSize(basic);
        this.setMinimumSize(basic);
        this.setUndecorated(true);

        //default operation on close (exit in this case)
        this.setDefaultCloseOperation(EXIT_ON_CLOSE);

        //recover the desktop resolution
        Dimension size = Toolkit.getDefaultToolkit(). getScreenSize();

        //and save this values
        this.resolutionH = (int)size.getHeight();
        this.resolutionW = (int)size.getWidth();

        //center the current window regards the desktop resolution
        this.positionX = (int)((size.getWidth() / 2) - (this.windowWidth / 2));
        this.positionY = (int)((size.getHeight() / 2) - (this.windowHeight / 2));
        this.setLocation(this.positionX, this.positionY);

        //create the backbuffer from the size of screen resolution to avoid any resize process penalty
        this.ge             = GraphicsEnvironment.getLocalGraphicsEnvironment();
        this.dsd            = ge.getDefaultScreenDevice();
        this.bufferImage    = dsd.getDefaultConfiguration().createCompatibleVolatileImage(this.resolutionW, this.resolutionH);
        this.g2d            = (Graphics2D)bufferImage.getGraphics();
        
        //Get the already loaded image from loader
        this.splashImage    = LoadingStuffs.getInstance().getImage("splashImage");

        //////////////////////////////////////////////////////////////////////
        // ->>>  now, for the canvas
        //////////////////////////////////////////////////////////////////////
        this.w      = this.splashImage.getWidth();
        this.h      = this.splashImage.getHeight();
        this.x      = (this.windowWidth - this.w) / 2;
        this.y      = (this.windowHeight - this.h) / 2;
        this.FPS    = FPS;

        //initialize the canvas
        this.canvas = new JPanel(null);
        this.canvas.setSize(windowWidth, windowHeight);
        this.canvas.setBackground(Color.BLACK);
        this.setVisible(true);
        this.canvas.setOpaque(true);
        
        //final parameters for the window
        this.add(canvas);
        this.pack();
        this.setLocationRelativeTo(null);
        this.setVisible(true);
        this.requestFocus();
    }

    public void Draw(long frametime)
    {
    }

    public void GraphicDispose()
    {
    }

    public void Render()
    {
    }

    public void Update(long frametime)
    {
    }
}