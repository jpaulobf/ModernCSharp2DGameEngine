namespace Engine;

using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Game;
using System.Drawing.Drawing2D;

/**
 * This is the MyGame class
 * usually, this name will be replace by the game name, like: Tetris for instance
 * Author: Joao Paulo B Faria
 * Date:   setp/2022
 */
public class MyGame
{
    /**
     * Game engine constructor.
     * This method will start the GameLoop & Empty Canvas elements
     * Author: Joao Paulo B Faria
     * Date:   04/sept/2022
     */
    public MyGame(int targetFPS, bool useThread = false) {
        //run the application
        Application.Run(new Canvas(targetFPS, useThread));
    }

    /**
     * Game empty canvas.
     * This class have to handle the draw, update & keyboard entries and foward to game class
     * Author: Joao Paulo B Faria
     * Date:   04/sept/2022
     */
    private class Canvas : Form, ICanvasEngine {
        private IGame Game;
        private GameEngine GameEngine;
        private Graphics Graphics;
        private System.ComponentModel.IContainer Components;
        private bool GoFullscreen                   = false;
        private bool ShowFPS                        = true;
        private int ExternalResolutionWidth         = 738;
        private int ExternalResolutionHeight        = 516;
        private const int FPS_MAX_ARRAY             = 10;
        private int[] FPS_AVERAGE                   = new int[FPS_MAX_ARRAY];
        private byte Fps_Aux_Counter                = 0;
        private InterpolationMode InterpolationMode = InterpolationMode.HighQualityBicubic;
        
        /**
         * Canvas constructor
         * Author: Joao Paulo B Faria
         * Date:   04/sept/2022
         *
         */
        public Canvas(int targetFPS, bool useThread = false) {
            
            //define as double buffered canvas
            //this.DoubleBuffered = true;

            //start the components
            this.Components                 = new System.ComponentModel.Container();
            this.AutoScaleMode              = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize                 = new Size(ExternalResolutionWidth, ExternalResolutionHeight);
            this.Text                       = "My C# Modern GameEngine";

            this.StartPosition              = FormStartPosition.CenterScreen;

            //create the backbuffer image
            this.Graphics                   = this.CreateGraphics();
            this.Graphics.InterpolationMode = InterpolationMode;

            //no resizible
            this.FormBorderStyle            = FormBorderStyle.FixedSingle;
            
            //go fullscreen
            this.ToggleFullScreen(GoFullscreen);

            //foward the keyboard methods
            this.FowardKeyboard();

            if (this.GoFullscreen) {
                //init the game class
                this.Game = new MainGameController(new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height),
                                     new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height),
                                     InterpolationMode);
            } else {
                //init the game class
                this.Game = new MainGameController(new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height),
                                     new Size(ExternalResolutionWidth, ExternalResolutionHeight), 
                                     InterpolationMode);
            }

            this.Resize += this.Game.Resize;

            //starts the game engine, the empty canvas & engine init method.
            this.GameEngine = new GameEngine(targetFPS, this, useThread);
            this.GameEngine.Init();

            for (int i = 0; i < FPS_AVERAGE.Length; i++) {
                FPS_AVERAGE[i] = targetFPS;
            }
        }

        /**
         * Foward the keyboard actions
         */
        private void FowardKeyboard()
        {
            this.KeyPreview = true;
            this.KeyPress   += Canvas_KeyPress;
            this.KeyDown    += Canvas_KeyDown;
            this.KeyUp      += Canvas_KeyUp;
            this.Closing    += Canvas_Closing;
        }

        private void ToggleFullScreen(bool fullscreen)
        {
            if (fullscreen) {
                this.WindowState        = FormWindowState.Normal;
                this.FormBorderStyle    = System.Windows.Forms.FormBorderStyle.None;
                this.Bounds             = Screen.PrimaryScreen.Bounds;
            } else {
                this.WindowState        = FormWindowState.Normal;
                this.FormBorderStyle    = System.Windows.Forms.FormBorderStyle.Sizable;
                this.ClientSize         = new Size(ExternalResolutionWidth, ExternalResolutionHeight);
                this.Location           = new Point((Screen.PrimaryScreen.Bounds.Width / 2) - (ExternalResolutionWidth / 2), 
                                                    (Screen.PrimaryScreen.Bounds.Height / 2) - (ExternalResolutionHeight / 2));
            }
        }

        void Canvas_Closing(object? sender, System.ComponentModel.CancelEventArgs e)    {   this.GameEngine.Stop(); Application.Exit(); }
        void Canvas_KeyPress(object? sender, KeyPressEventArgs e)                       {   this.Game.KeyPress(sender, e);              }
        void Canvas_KeyDown(object? sender, KeyEventArgs e)                             {   this.Game.KeyDown(sender, e);               }
        void Canvas_KeyUp(object? sender, KeyEventArgs e) {
            if (e.KeyValue == 113) {
                this.GoFullscreen = !this.GoFullscreen;
                this.ToggleFullScreen(this.GoFullscreen);
            }
            this.Game.KeyUp(sender, e);     
        }

        public void Draw(long frametime)
        {
            this.Game.Draw(frametime);
            if (ShowFPS) this.RenderFPS(this.Game.GetGraphics(), frametime);
        }      

        public void Update(long frametime)
        {
            this.Game.Update(frametime);
        }

        public void Render() 
        {
            this.Game.Render(this.Graphics);
        }

        private void RenderFPS(Graphics graphics, long frametime) {
            FPS_AVERAGE[Fps_Aux_Counter++%FPS_MAX_ARRAY] = (int)(10_000_000 / frametime);
            graphics.DrawString(("FPS: " + (FPS_AVERAGE.Sum() / FPS_MAX_ARRAY)), this.Font, Brushes.White, 0, 0);
        }

        public void GraphicDispose()
        {
            this.Graphics.Dispose();
        }
    }

    /**
     * Game engine (game loop) main class
     * Have to handle with target fps update, instanciate the empty canvas, and foward update & draw methods.
     * Author: Joao Paulo B Faria
     * Date:   04/sept/2022
     */
    private class GameEngine {

        //private Thread? thread = null;
        private Task? Task                      = null;
        private Timer? StateTimer               = null;
        private long BeforeTimer                = 0;
        private long LastframeTimer             = 0;
        private volatile bool UseThread         = true;
        private volatile bool IsEngineRunning   = true;
        private static long FPS240              = (long)(10_000_000 / 240);
        private static long FPS120              = (long)(10_000_000 / 120);
        private static long FPS90               = (long)(10_000_000 / 90);
        private static long FPS60               = (long)(10_000_000 / 60);
        private static long FPS30               = (long)(10_000_000 / 30);
        private long TARGET_FRAMETIME           = FPS60;
        private bool UNLIMITED_FPS              = false;
        private ICanvasEngine Canvas;

        /**
         * Constructor. Handle with target FPS & foward actions to the empty canvas
         * Author: Joao Paulo B Faria
         * Date:   04/sept/2022
         */
        public GameEngine(int targetFPS, ICanvasEngine canvas, bool useThread = false) 
        {
            this.UNLIMITED_FPS = false;
            this.UseThread = useThread;
            switch(targetFPS) {
                case 30:
                    this.TARGET_FRAMETIME = FPS30;
                    break;
                case 60:
                    this.TARGET_FRAMETIME = FPS60;
                    break;
                case 90:
                    this.TARGET_FRAMETIME = FPS90;
                    break;
                case 120:
                    this.TARGET_FRAMETIME = FPS120;
                    break;
                case 240:
                    this.TARGET_FRAMETIME = FPS240;
                    break;
                case 0:
                    this.UNLIMITED_FPS  = true;
                    this.UseThread      = true;
                    break;
                default:
                    this.TARGET_FRAMETIME = (10_000_000 / targetFPS);
                    break;
            }

            //store the canvas            
            this.Canvas = canvas;

            this.LastframeTimer = this.TARGET_FRAMETIME;
        }

        /**
         * Instantiate the class Thread & start the Run method
         * Author: Joao Paulo B Faria
         * Date:   04/sept/2022
         */
        public void Init() 
        {
            if (this.UseThread) {
                /*
                alternative:
                */
                /*
                this.thread = new Thread(new ThreadStart(Run));    
                this.thread.Priority = ThreadPriority.Highest;
                this.thread.IsBackground = true;
                this.thread.Start();
                */
                this.Task = new Task(Run, TaskCreationOptions.LongRunning);
                this.Task.Start();
            } else {
                /**
                 * Thanks to Mike Zboray - https://github.com/mzboray/HighPrecisionTimer
                 */
                var timer = new HighResolutionTimer.WinMMWrapper((int)(this.TARGET_FRAMETIME*0.0001), 0, HighResolutionTimer.WinMMWrapper.TimerEventType.Repeating, () =>
                {
                    RunTimer();
                });
                timer.StartElapsedTimer();
                
                //test the performance
                this.BeforeTimer = Stopwatch.GetTimestamp();
            }
        }

        public void Stop() {
            this.IsEngineRunning = false;
            if (this.StateTimer != null) {
                this.StateTimer.Dispose();
            }
        }

        public void RunTimer() {
            long beforeUpdate       = 0;
            long afterUpdate        = 0;
            long frequencyCalc      = (10_000_000 / Stopwatch.Frequency);

            if (this.IsEngineRunning) {

                //calc the update time
                beforeUpdate = Stopwatch.GetTimestamp();

                //update the game (gathering input from user, and processing the necessary games updates)
                this.update(LastframeTimer);

                //get the timestamp after the update
                afterUpdate = Stopwatch.GetTimestamp() - beforeUpdate;
                afterUpdate *= frequencyCalc;

                //only draw if there is some (any) enough time
                if ((TARGET_FRAMETIME - afterUpdate) > 0) {
                    //draw
                    this.draw(LastframeTimer);
                    this.render();
                }

                this.LastframeTimer = (Stopwatch.GetTimestamp() - BeforeTimer) * frequencyCalc;
                this.BeforeTimer = Stopwatch.GetTimestamp();
            }
        }

        /**
         * Gameloop method
         * Author: Joao Paulo B Faria
         * Date:   sept/2022
         */
        private void Run() 
        {
            long timeReference      = Stopwatch.GetTimestamp();
            long beforeUpdate       = 0;
            long afterUpdate        = 0;
            long beforeDraw         = 0;
            long afterDraw          = 0;
            long beforeSleep        = 0;
            long afterSleep         = 0;
            long accumulator        = 0;
            long timeElapsed        = 0;
            long timeStamp          = 0;
            long lastframetime      = TARGET_FRAMETIME;
            long frequencyCalc      = (10_000_000 / Stopwatch.Frequency);

            //gameloop
            if (UNLIMITED_FPS) {
                while (this.IsEngineRunning) {
    
                    //mark the time before the iteration
                    timeStamp = Stopwatch.GetTimestamp();
    
                    //compute the time from previous iteration and the current
                    timeElapsed = (timeStamp - timeReference);
                    timeElapsed *= frequencyCalc;
    
                    //update the game (gathering input from user, and processing the necessary games updates)
                    this.update(timeElapsed);
    
                    //draw
                    this.draw(timeElapsed);

                    //render
                    this.render();

                    //Yield
                    Thread.Yield();

                    //update the referencial time with the initial time
                    timeReference = timeStamp;
                }
            } else {
                while (this.IsEngineRunning) {

                    accumulator = 0;

                    //calc the update time
                    beforeUpdate = Stopwatch.GetTimestamp();

                    //update the game (gathering input from user, and processing the necessary games updates)
                    this.update(lastframetime);

                    //get the timestamp after the update
                    afterUpdate = Stopwatch.GetTimestamp() - beforeUpdate;
                    
                    //only draw if there is some (any) enough time
                    if ((TARGET_FRAMETIME - afterUpdate) > 0) {
                        
                        beforeDraw = Stopwatch.GetTimestamp();

                        //draw
                        this.draw(lastframetime);

                        //render
                        this.render();
                        
                        //and than, store the time spent
                        afterDraw = Stopwatch.GetTimestamp() - beforeDraw;
                    }

                    //correct the timing by the stopwatch frequency (and using the same variable "afterdraw")
                    afterDraw = ((afterUpdate + afterDraw) * frequencyCalc);
                    accumulator = TARGET_FRAMETIME - afterDraw;

                    if (accumulator > 0) {
                        
                        beforeSleep = Stopwatch.GetTimestamp();

                        //This method is imprecise... Timer Resolution in .Net Takes 12~14 ms to tick 
                        //Use, if possible, max power (target-fps: 0)
                        //Or utilize the Timer Resolution mode (useThread = false [default])
                        Thread.Sleep((short)(accumulator * 0.0001));
                        Thread.Yield();

                        afterSleep = Stopwatch.GetTimestamp() - beforeSleep;

                    } else {
                        /*  
                        Explanation:
                            if the total time to execute, consumes more miliseconds than the target-frame's amount, 
                            is necessary to keep updating without render, to recover the pace.
                        Important: Something here isn't working with very slow machines. 
                                    So, this compensation have to be re-tested with this new approuch (exiting beforeUpdate).
                                    Please test this code with your scenario.
                        */
                        //System.out.println("Skip 1 frame... " + ++counter + " time(s)");
                        if (accumulator < 0) {
                            this.update(TARGET_FRAMETIME);
                        }
                    }

                    lastframetime = afterDraw + (afterSleep * frequencyCalc);
                }
            }

            this?.Dispose();
        }

        /**
         * Render the canvas
         */
        private void render()
        {
           this.Canvas.Render();
        }

        /* Método de update, só executa quando a flag permite */
        public void update(long frametime) 
        {
            this.Canvas.Update(frametime);
        }
    
        /* Método de desenho, só executa quando a flag permite */
        public void draw(long frametime) 
        {
            this.Canvas.Draw(frametime);
        }

        private void Dispose() {
            try {
                this.Canvas.GraphicDispose();
            } catch {}
        }
    }
}