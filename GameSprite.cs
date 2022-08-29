namespace GameEngine;

using System.Drawing;

public class GameSprite
{
    public Bitmap SpriteImage { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Width { get; set; }
    public float Height { get; set; }
    public int Velocity { get; set; }

    public GameSprite()
    {
    }

    public void Draw(Graphics gfx)
    {
        // Draw sprite image on screen
        gfx.DrawImage(SpriteImage, X, Y, Width, Height);
    }
}