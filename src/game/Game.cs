namespace game;

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;
using engine;

public class Game : IGame
{
    private GameSprite playerSprite;

    public Size Resolution { get; set; }

    /**
        Game-class constructor
    */
    public Game() {
        this.Load();
    }

    public void Load()
    {
        // Load new sprite class
        playerSprite = new GameSprite();
        // Load sprite image
        string filepath = "img\\bomber-sprite.png";
        Console.WriteLine(filepath);
        playerSprite.SpriteImage = new Bitmap(@filepath);
        // Set sprite height & width in pixels
        playerSprite.Width = playerSprite.SpriteImage.Width;
        playerSprite.Height = playerSprite.SpriteImage.Height;

        // Set sprite coodinates
        playerSprite.X = 300;
        playerSprite.Y = 300;
        
        // Set sprite Velocity
        playerSprite.Velocity = 300;
    }

    public void Unload()
    {
        // Unload graphics
        // Turn off game music
    }

    public void Update(long frametime)
    {
        // Gametime elapsed
        double gameTimeElapsed = frametime / 1000;
        // Calculate sprite movement based on Sprite Velocity and GameTimeElapsed
        //int moveDistance = (int)(playerSprite.Velocity * gameTimeElapsed);

        /*
        // Move player sprite, when Arrow Keys are pressed on Keyboard
        if ((Keyboard.GetKeyStates(Key.Right) & KeyStates.Down) > 0)
        {
            playerSprite.X += moveDistance;
        }
        else if ((Keyboard.GetKeyStates(Key.Left) & KeyStates.Down) > 0)
        {
            playerSprite.X -= moveDistance;
        }
        else if ((Keyboard.GetKeyStates(Key.Up) & KeyStates.Down) > 0)
        {
            playerSprite.Y -= moveDistance;
        }
        else if ((Keyboard.GetKeyStates(Key.Down) & KeyStates.Down) > 0)
        {
            playerSprite.Y += moveDistance;
        }*/
    }

    public void Draw(Graphics gfx)
    {
        // Draw Background Color
        gfx.Clear(Color.CornflowerBlue);

        // Draw Player Sprite
        playerSprite.Draw(gfx);
    }

    public void KeyDown(object sender, KeyEventArgs e)
    {
        Console.WriteLine( $"KeyDown code: {e.KeyCode}, value: {e.KeyValue}, modifiers: {e.Modifiers}" + "\r\n");
    }

    public void KeyPress(object sender, KeyPressEventArgs e)
    {
        Console.WriteLine($"KeyPress keychar: {e.KeyChar}" + "\r\n");
    }

    public void KeyUp(object sender, KeyEventArgs e)
    {
        Console.WriteLine( $"KeyUp code: {e.KeyCode}, value: {e.KeyValue}, modifiers: {e.Modifiers}" + "\r\n");
    }
}
