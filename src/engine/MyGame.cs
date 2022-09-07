namespace engine;

using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using game;
using HighResolutionTimer;

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
    public MyGame(int targetFPS, bool useThread = true) {
        
        Application.Run(new Canvas(targetFPS, useThread));

    }

    /**
     * Game empty canvas.
     * This class have to handle the draw, update & keyboard entries and foward to game class
     * Author: Joao Paulo B Faria
     * Date:   04/sept/2022
     */
    private class Canvas : Form, ICanvasEngine {
        
        private Graphics bmG;
        private System.ComponentModel.IContainer components;
        private IGame game;
        private GameEngine gameEngine;
        private bool goFullscreen = false;
        BufferedGraphicsContext currentContext = BufferedGraphicsManager.Current;
        BufferedGraphics myBuffer;

        /**
         * Canvas constructor
         * Author: Joao Paulo B Faria
         * Date:   04/sept/2022
         *
         */
        public Canvas(int targetFPS, bool useThread = true) {
            
            //define as double buffered canvas
            this.DoubleBuffered = true;

            //start the components
            this.components     = new System.ComponentModel.Container();
            this.AutoScaleMode  = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize     = new System.Drawing.Size(800, 450);
            this.Text           = "My C# Modern GameEngine";

            //create the backbuffer image
            this.myBuffer = currentContext.Allocate(this.CreateGraphics(), this.DisplayRectangle);
            this.bmG = myBuffer.Graphics;

            //go fullscreen
            this.GoFullscreen(goFullscreen);

            //foward the keyboard methods
            this.fowardKeyboard();

            //init the game class
            this.game = new Game();
            this.init();

            //starts the game engine, the empty canvas & engine init method.
            this.gameEngine = new GameEngine(targetFPS, this, useThread);
            this.gameEngine.Init();
        }

        private void fowardKeyboard()
        {
            this.KeyPreview = true;
            this.KeyPress   += Canvas_KeyPress;
            this.KeyDown    += Canvas_KeyDown;
            this.KeyUp      += Canvas_KeyUp;
            this.Closing    += Canvas_Closing;
        }

        private void GoFullscreen(bool fullscreen)
        {
            if (fullscreen) {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.Bounds = Screen.PrimaryScreen.Bounds;
            } else {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            }
        }

        void Canvas_Closing(object sender, System.ComponentModel.CancelEventArgs e) {   this.gameEngine.Stop(); Application.Exit(); }
        void Canvas_KeyPress(object sender, KeyPressEventArgs e)    {   this.game.KeyPress(sender, e);  }
        void Canvas_KeyDown(object sender, KeyEventArgs e)          {   this.game.KeyDown(sender, e);   }
        void Canvas_KeyUp(object sender, KeyEventArgs e) {
            if (e.KeyValue == 113) {
                this.goFullscreen = !this.goFullscreen;
                this.GoFullscreen(this.goFullscreen);
            }
            this.game.KeyUp(sender, e);     
        }
        
        private void init()
        {
            Rectangle resolution = Screen.PrimaryScreen.Bounds;

            // Initialize Game
            this.game.Resolution = new Size(resolution.Width, resolution.Height);
        }

        public void draw(long frametime)
        {
            this.game.Draw(this.bmG);
            this.RenderFPS(frametime);
            this.myBuffer.Render();
        }

        public void update(long frametime)
        {
            this.game.Update(frametime);
        }

        private void RenderFPS(long frametime) {
            this.bmG.DrawString(("FPS: " + (10_000_000 / frametime)), this.Font, Brushes.Black, 0, 0);
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
        private Task? task = null;
        private Timer? stateTimer;
        private long beforeTimer = 0;
        private long lastframetimer = 0;
        private bool useThread = true;
        private bool isEngineRunning    = true;
        private static long FPS240      = (long)(10_000_000 / 240);
        private static long FPS120      = (long)(10_000_000 / 120);
        private static long FPS90       = (long)(10_000_000 / 90);
        private static long FPS60       = (long)(10_000_000 / 60);
        private static long FPS30       = (long)(10_000_000 / 30);
        private long TARGET_FRAMETIME   = FPS60;
        private bool UNLIMITED_FPS      = false;
        private ICanvasEngine game;

        /**
         * Constructor. Handle with target FPS & foward actions to the empty canvas
         * Author: Joao Paulo B Faria
         * Date:   04/sept/2022
         */
        public GameEngine(int targetFPS, ICanvasEngine canvas, bool useThread = true) 
        {
            this.UNLIMITED_FPS = false;
            this.useThread = useThread;
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
                    this.UNLIMITED_FPS = true;
                    this.useThread = true;
                    break;
                default:
                    this.TARGET_FRAMETIME = (long)(10_000_000 / targetFPS);
                    break;
            }
            
            if (canvas == null) {
                this.game = new Canvas(targetFPS);
            } else {
                this.game = canvas;
            }

            this.lastframetimer = this.TARGET_FRAMETIME;
        }

        /**
         * Instantiate the class Thread & start the Run method
         * Author: Joao Paulo B Faria
         * Date:   04/sept/2022
         */
        public void Init() 
        {
            if (this.useThread) {
                /*
                alternative:
                */
                /*
                this.thread = new Thread(new ThreadStart(Run));    
                this.thread.Priority = ThreadPriority.Highest;
                this.thread.IsBackground = true;
                this.thread.Start();
                */
                this.task = new Task(Run, TaskCreationOptions.LongRunning);
                this.task.Start();
            } else {
                
                /*
                alternative:
                */
                /*
                this.stateTimer             = new Timer();
                this.stateTimer.Interval    = (int)(this.TARGET_FRAMETIME*0.0001);
                this.stateTimer.Tick        += RunTimer;
                this.stateTimer.Start();
                */
                /*
                Still have serious problems, but its the better i could found...
                */
                HighResolutionTimer timer = new HighResolutionTimer((int)(this.TARGET_FRAMETIME*0.0001));
                timer.Elapsed += RunTimer;
                timer.Start();
                
                //test the performance
                this.beforeTimer = Stopwatch.GetTimestamp();
            }
        }

        public void Stop() {
            this.isEngineRunning = false;
            if (this.stateTimer != null) {
                this.stateTimer.Dispose();
            }
        }

        public void RunTimer(object? sender, EventArgs e) {
            long beforeUpdate       = 0;
            long afterUpdate        = 0;
            long frequencyCalc      = (10_000_000 / Stopwatch.Frequency);

            if (this.isEngineRunning) {

                //calc the update time
                beforeUpdate = Stopwatch.GetTimestamp();

                //update the game (gathering input from user, and processing the necessary games updates)
                this.update(lastframetimer);

                //get the timestamp after the update
                afterUpdate = Stopwatch.GetTimestamp() - beforeUpdate;
                afterUpdate *= frequencyCalc;

                //only draw if there is some (any) enough time
                if ((TARGET_FRAMETIME - afterUpdate) > 0) {
                    //draw
                    this.draw(lastframetimer);
                }

                this.lastframetimer = (Stopwatch.GetTimestamp() - beforeTimer) * frequencyCalc;
                this.beforeTimer = Stopwatch.GetTimestamp();
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
                while (isEngineRunning) {
    
                    //mark the time before the iteration
                    timeStamp = Stopwatch.GetTimestamp();
    
                    //compute the time from previous iteration and the current
                    timeElapsed = (timeStamp - timeReference);
                    timeElapsed *= frequencyCalc;
    
                    //update the game (gathering input from user, and processing the necessary games updates)
                    this.update(timeElapsed);
    
                    //draw
                    this.draw(timeElapsed);

                    //Yield
                    Thread.Yield();

                    //update the referencial time with the initial time
                    timeReference = timeStamp;
                }
            } else {
                while (this.isEngineRunning) {

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
                        
                        //and than, store the time spent
                        afterDraw = Stopwatch.GetTimestamp() - beforeDraw;
                    }

                    //correct the timing by the stopwatch frequency (and using the same variable "afterdraw")
                    afterDraw = ((afterUpdate + afterDraw) * frequencyCalc);
                    accumulator = TARGET_FRAMETIME - afterDraw;

                    if (accumulator > 0) {
                        
                        beforeSleep = Stopwatch.GetTimestamp();

                        //This method is imprecise... Have to found another way...
                        Thread.Sleep((int)(accumulator * 0.0001));
                        Thread.Yield();
                                                
                        //new System.Threading.ManualResetEvent(false).WaitOne((int)(accumulator * 0.0001));

                        //this is another option, also imprecise
                        //Task.Delay((int)(accumulator * 0.0001)).Wait();
                        //await Task.Delay((int)(accumulator * 0.0001));

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
        }
    
        /* Método de update, só executa quando a flag permite */
        public void update(long frametime) 
        {
            this.game.update(frametime);
        }
    
        /* Método de desenho, só executa quando a flag permite */
        public void draw(long frametime) 
        {
            this.game.draw(frametime);
        }
    }
}