namespace GameEngine;

using System;
using System.Drawing;
using System.Windows.Forms;

public class Canvas : Form
{
    /*
    //System.Windows.Forms.Timer graphicsTimer;
    */
    
    private System.ComponentModel.IContainer components;
    private Bitmap buffer;
    private Graphics bmG;
    private GameLoop? gameLoop;
    private Thread thread;

    private class MyCanvas : Form {

    }

    private class Engine {
        
    }

    public Canvas()
    {

        this.DoubleBuffered = true;

        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "My C# Modern GameEngine";

        /*
        graphicsTimer = new System.Windows.Forms.Timer();
        graphicsTimer.Interval = 1000 / 120;
        graphicsTimer.Tick += GraphicsTimer_Tick;
        */

        thread = new Thread(new ThreadStart(Loop));
        thread.Priority = ThreadPriority.Normal;

        this.buffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
        
        bmG = Graphics.FromImage(buffer);

        this.init();

    }

    private void init()
    {
        Rectangle resolution = Screen.PrimaryScreen.Bounds;

        // Initialize Game
        Game myGame = new Game();
        myGame.Resolution = new Size(resolution.Width, resolution.Height);

        // Initialize & Start GameLoop
        gameLoop = new GameLoop();
        gameLoop.Load(myGame);
        gameLoop.Start();

        // Start Graphics Timer
        thread.Start();
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

}