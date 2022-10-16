namespace Game;

/**
 * Author: Joao P B Faria
 * Date: Sept/2022
 * Description: GameController public Interface
 */
public interface IGame 
{
    public void Update(long frametime);

    public void Draw(long frametime);

    public Graphics GetGraphics();

    public void Render(Graphics targetGraphics);

    public Size Resolution { get; set; }

    public void KeyDown(object? sender, System.Windows.Forms.KeyEventArgs e);

    public void KeyPress(object? sender, System.Windows.Forms.KeyPressEventArgs e);

    public void KeyUp(object? sender, System.Windows.Forms.KeyEventArgs e);
    
    public void Resize(object? sender, System.EventArgs e);

    public System.Drawing.Drawing2D.InterpolationMode Interpolation { get; }

    public IEnumerable<GameSprite> GetCurrentScreenSprites();

    public int GetInternalResolutionWidth();

    public int GetInternalResolutionHeight();

    public float getScaleW();
    
    public float getScaleH();

    public Size WindowSize { get; }

    public void SetCollidingWithAnEnemy();

    public void TogglePlayerSprite();

    public void DisablePlayerSprite();
    
    public PlayerController GetPlayerController();
}