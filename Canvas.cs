namespace GameEngine;

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

public class MyGame
{
    public MyGame(int targetFPS) {
        new GameEngine(targetFPS, new Canvas()).init();   
    }

    private class Canvas : Form, ICanvasEngine {
        
        private Bitmap buffer;
        private Graphics bmG;
        private System.ComponentModel.IContainer components;

        public Canvas() {
            
            this.DoubleBuffered = true;
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "My C# Modern GameEngine";

            this.buffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            this.bmG = Graphics.FromImage(buffer);
        }

        public void draw(long frametime)
        {
        }

        public void update(long frametime)
        {
        }

        private void init()
        {
            Rectangle resolution = Screen.PrimaryScreen.Bounds;

            // Initialize Game
            Game myGame = new Game();
            myGame.Resolution = new Size(resolution.Width, resolution.Height);
        }
    }

    private class GameEngine {

        private GameLoop? gameLoop      = null;
        private Thread? thread          = null;
        private bool isEngineRunning    = true;
        private static long FPS240      = (long)(1_000_000_000 / 240);
        private static long FPS120      = (long)(1_000_000_000 / 120);
        private static long FPS90       = (long)(1_000_000_000 / 90);
        private static long FPS60       = (long)(1_000_000_000 / 60);
        private static long FPS30       = (long)(1_000_000_000 / 30);
        private long TARGET_FRAMETIME   = FPS60;
        private bool UNLIMITED_FPS      = false;
        private ICanvasEngine? game     = null;

        /**
            Constructor
            TargetFPS = 0, 30, 60, 120, 240
        */
        public GameEngine(int targetFPS, ICanvasEngine canvas) {

            this.UNLIMITED_FPS = false;
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
                    break;
                default:
                    this.TARGET_FRAMETIME = FPS30;
                    break;
            }
            this.game = canvas;
            
            /*
            // Initialize & Start GameLoop
            gameLoop = new GameLoop();
            gameLoop.Load(myGame);
            gameLoop.Start();

            */
        }

        public void init() {
            this.thread = new Thread(new ThreadStart(run));    
            this.thread.Priority = ThreadPriority.Normal;
            this.thread.Start();
        }

        public void run() {
            long timeReference      = Stopwatch.GetTimestamp();
            long beforeUpdate       = 0;
            long afterUpdate        = 0;
            long beforeDraw         = 0;
            long afterDraw          = 0;
            long accumulator        = 0;
            long timeElapsed        = 0;
            long timeStamp          = 0;
            long counter            = 0;
    
  
            while (isEngineRunning) {

                accumulator = 0;

                //calc the update time
                beforeUpdate = Stopwatch.GetTimestamp();

                //update the game (gathering input from user, and processing the necessary games updates)
                this.update(TARGET_FRAMETIME);

                //get the timestamp after the update
                afterUpdate = Stopwatch.GetTimestamp() - beforeUpdate;
                
                //only draw if there is some (any) enough time
                if ((TARGET_FRAMETIME - afterUpdate) > 0) {
                    
                    beforeDraw = Stopwatch.GetTimestamp();

                    //draw
                    this.draw(TARGET_FRAMETIME);
                    
                    //and than, store the time spent
                    afterDraw = Stopwatch.GetTimestamp() - beforeDraw;
                }

                //reset the accumulator
                accumulator = TARGET_FRAMETIME - (afterUpdate + afterDraw);

                if (accumulator > 0) {
                    try {
                        Thread.Sleep(((int)accumulator));
                    } catch (Exception e) {}
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
            }
        }
    
        /* Método de update, só executa quando a flag permite */
        public void update(long frametime) {
            Console.Write("update...");
            this.game.update(frametime);
        }
    
        /* Método de desenho, só executa quando a flag permite */
        public void draw(long frametime) {
            Console.Write("Draw...");
            this.game.draw(frametime);
        }
    }


    /*

    public MyGame()
    {
        /*
        graphicsTimer = new System.Windows.Forms.Timer();
        graphicsTimer.Interval = 1000 / 120;
        graphicsTimer.Tick += GraphicsTimer_Tick;
     
        this.init();

    }
    

    private void Loop()
    {
        // Refresh Form1 graphics
        //this.Invalidate();
        this.gameLoop.Draw(bmG);

        this.Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e) {
        e.Graphics.DrawImage(buffer, 0, 0);

        Console.WriteLine("aqui...");
    }
    */
}