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

    private bool KEY_LEFT = false;
    private bool KEY_RIGHT = false;

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
        //Console.WriteLine(filepath);
        playerSprite.SpriteImage = new Bitmap(@filepath);
        // Set sprite height & width in pixels
        playerSprite.Width = playerSprite.SpriteImage.Width;
        playerSprite.Height = playerSprite.SpriteImage.Height;

        // Set sprite coodinates
        playerSprite.X = 300;
        playerSprite.Y = 300;
        
        // Set sprite Velocity
        playerSprite.Velocity = 200;
    }

    public void Unload()
    {
        // Unload graphics
        // Turn off game music
    }

    public void Update(long frametime)
    {
        // Calculate sprite movement based on Sprite Velocity and GameTimeElapsed
        float moveDistance = (float)(playerSprite.Velocity * ((double)frametime / 10_000_000));

        if (KEY_LEFT) {
            playerSprite.X -= moveDistance;
        }

        if (KEY_RIGHT) {
            playerSprite.X += moveDistance;
        }

        if (playerSprite.X > 800) {
            playerSprite.X = 0;
        } else if (playerSprite.X < 0) {
            playerSprite.X = 800;
        }
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
        if (e.KeyValue == 37) {
            KEY_LEFT = true;
        } else if (e.KeyValue == 39) {
            KEY_RIGHT = true;
        }
    }

    public void KeyPress(object sender, KeyPressEventArgs e) {}

    public void KeyUp(object sender, KeyEventArgs e)
    {
        if (e.KeyValue == 37) {
            KEY_LEFT = false;
        } else if (e.KeyValue == 39) {
            KEY_RIGHT = false;
        }
    }
}
