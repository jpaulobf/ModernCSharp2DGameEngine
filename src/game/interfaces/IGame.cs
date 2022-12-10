using Game.Stages;

namespace Game;

/**
 * Author: Joao P B Faria
 * Date: Sept/2022
 * Description: GameController public Interface
 */
public interface IGame 
{
    /**
     * Update Method, responsible for the game loop update process.
     */
    public void Update(long frametime);

    /**
     * Draw Method, responsible for the game loop draw process.
     */
    public void Draw(long frametime);

    /**
     * Get the instance of current Graphics object
     */
    public Graphics GetGraphics();

    /**
     * Render is the method responsible for "printing" the backbuffer to the current buffer
     */
    public void Render(Graphics targetGraphics);

    /**
     * Define the game Resolution (accessor)
     */
    public Size Resolution { get; set; }

    /**
     * Key Down event control
     */
    public void KeyDown(object? sender, System.Windows.Forms.KeyEventArgs e);

    /**
     * Key Press event control
     */
    public void KeyPress(object? sender, System.Windows.Forms.KeyPressEventArgs e);

    /**
     * Key Up event control
     */
    public void KeyUp(object? sender, System.Windows.Forms.KeyEventArgs e);
    
    /**
     * Window Resize event control
     */
    public void Resize(object? sender, System.EventArgs e);

    /**
     * Accessor Interpolation mode
     */
    public System.Drawing.Drawing2D.InterpolationMode Interpolation { get; }

    public IEnumerable<GameSprite> GetCurrentScreenSprites();

    public int GetInternalResolutionWidth();

    public int GetInternalResolutionHeight();

    /**
     * Get the current scaled width
     */
    public float getScaleW();
    
    /**
     * Get the current scaled height
     */
    public float getScaleH();

    public Size WindowSize { get; }

    public void PlayerCollided();

    public void TogglePlayerSprite();

    public void DisablePlayerSprite();
    
    public Player GetPlayer();

    public void UpdateFuelMarker();

    public void UpdateScore(int type);
    public GameStages GetStages();
}