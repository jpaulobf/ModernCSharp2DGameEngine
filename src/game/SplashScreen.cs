namespace Game;

using Util;
using Engine;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;

/**
 *
 */
public class SplashScreen : Form, ICanvasEngine 
{
    //this window properties
    private int WindowWidth                     = 800;
    private int WindowHeight                    = 500;
    private IContainer Components;

    //desktop properties
    private int ResolutionH                     = 0;
    private int ResolutionW                     = 0;

    //Game FPS
    private int FPS                             = 0;

    //Splash Screen Image
    private Bitmap SplashImage;
    private Graphics Graphics;
    private Task? Task;
    private long Framecounter                   = 0;
    private volatile bool ContinueLoop          = true;

    /**
     * Constructor
     */
    public SplashScreen(int FPS)
    {
        //////////////////////////////////////////////////////////////////////
        // ->>>  for the window
        //////////////////////////////////////////////////////////////////////
        LoadingStuffs.GetInstance();

        //set some properties for this window
        this.Components                 = new System.ComponentModel.Container();
        this.AutoScaleMode              = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize                 = new Size(WindowWidth, WindowHeight);
        this.StartPosition              = FormStartPosition.CenterScreen;
        this.Text                       = string.Empty;
        this.ControlBox                 = false;
        this.FormBorderStyle            = FormBorderStyle.None;

        //double buffered
        this.DoubleBuffered             = true;

        //and save this values
        this.ResolutionH                = (int)Screen.PrimaryScreen.Bounds.Height;
        this.ResolutionW                = (int)Screen.PrimaryScreen.Bounds.Width;

        //Get the already loaded image from loader
        this.SplashImage                = LoadingStuffs.GetInstance().GetImage("splash");

        //create the backbuffer image
        this.Graphics                   = this.CreateGraphics();

        //foward the key control
        this.FowardKeyboard();

        //////////////////////////////////////////////////////////////////////
        // ->>>  now, for the canvas
        //////////////////////////////////////////////////////////////////////
        this.FPS    = FPS;

        this.Visible = true;
        this.Focus();

        //loop
        this.Task = new Task(Render, TaskCreationOptions.LongRunning);
        this.Task.Start();
    }

    /**
     * Foward the keyboard actions
     */
    private void FowardKeyboard()
    {
        this.KeyPreview = true;
        this.KeyUp      += Canvas_KeyUp;
    }

    /**
     * On keyUp
     */
    void Canvas_KeyUp(object? sender, KeyEventArgs e) 
    {
        if (e.KeyValue == 27)
        {
            //exit....
        }
    }

    public void Render()
    {
        long timeReference      = Stopwatch.GetTimestamp();
        long beforeUpdate       = 0;
        long afterUpdate        = 0;
        long beforeDraw         = 0;
        long afterDraw          = 0;
        long beforeSleep        = 0;
        long afterSleep         = 0;
        long accumulator        = 0;
        long lastframetime      = 60;
        int TARGET_FRAMETIME    = 60;
        long frequencyCalc      = (10_000_000 / Stopwatch.Frequency);

        while (this.ContinueLoop)
        {
            accumulator = 0;

            //calc the update time
            beforeUpdate = Stopwatch.GetTimestamp();

            //update the game (gathering input from user, and processing the necessary games updates)
            this.Update(lastframetime);

            //get the timestamp after the update
            afterUpdate = Stopwatch.GetTimestamp() - beforeUpdate;
            
            //only draw if there is some (any) enough time
            if ((TARGET_FRAMETIME - afterUpdate) > 0) 
            {
                
                beforeDraw = Stopwatch.GetTimestamp();

                //draw
                this.Draw(lastframetime);

                //render
                this.Render();
                
                //and than, store the time spent
                afterDraw = Stopwatch.GetTimestamp() - beforeDraw;
            }

            //correct the timing by the stopwatch frequency (and using the same variable "afterdraw")
            afterDraw = ((afterUpdate + afterDraw) * frequencyCalc);
            accumulator = TARGET_FRAMETIME - afterDraw;

            if (accumulator > 0) 
            {
                
                beforeSleep = Stopwatch.GetTimestamp();

                //This method is imprecise... Timer Resolution in .Net Takes 12~14 ms to tick 
                //Use, if possible, max power (target-fps: 0)
                //Or utilize the Timer Resolution mode (useThread = false [default])
                Thread.Sleep((short)(accumulator * 0.0001));
                Thread.Yield();

                afterSleep = Stopwatch.GetTimestamp() - beforeSleep;

            } 
            else 
            {
                if (accumulator < 0) 
                {
                    this.Update(TARGET_FRAMETIME);
                }
            }

            lastframetime = afterDraw + (afterSleep * frequencyCalc);
        }
    }

    public void Update(long frametime)
    {
        this.Framecounter += frametime;
        if (this.Framecounter >= 10_000)
        {
            this.CloseAndGoToGame();
        }
    }
    
    public void Draw(long frametime)
    {
        this.Graphics.DrawImage(this.SplashImage, 0, 0, this.SplashImage.Width, this.SplashImage.Height);
    }

    public void GraphicDispose()
    {
    }

    private void CloseAndGoToGame()
    {
        this.Framecounter   = 0;
        this.ContinueLoop   = false;
        this.Invoke(new Action(()=> this.Hide()));

        //Go to the Game
        new MyGame(this.FPS);
    }

    public void Release(long frametime)
    {
        throw new NotImplementedException();
    }
}