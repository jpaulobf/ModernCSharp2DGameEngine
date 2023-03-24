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
    public void Render();

    /**
     * Release anything (if necessary)
     */
    public void Release(long frametime);

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

    /**
     * Get the sprites of current screen moment
     */
    public IEnumerable<GameSprite> GetCurrentScreenSprites();

    /**
     * Get the width value of internal resolution
     */
    public int GetInternalResolutionWidth();

    /**
     * Get the height value of internal resolution
     */
    public int GetInternalResolutionHeight();

    /**
     * Get the current scaled width
     */
    public float GetScaleW();
    
    /**
     * Get the current scaled height
     */
    public float GetScaleH();

    /**
     * Get the current Window Size
     */
    public Size WindowSize { get; }

    /**
     * Define the current player as Collided
     */
    public void PlayerCollided();

    /**
     * turns player sprite on/off
     */
    public void TogglePlayerSprite();

    /**
     * turns player sprite off
     */
    public void DisablePlayerSprite();
    
    /**
     * Getter - Get the player object
     */
    public Player GetPlayer();
    
    /**
     * Getter - Get the stage object
     */
    public GameStages GetStages();

    /**
     * Update the Fuel Mark
     */
    public void UpdateFuelMarker();

    /**
     * Update the score, based on the enemy type
     */
    public void UpdateScore(int type);

    /**
     * Change game state to "In Game"
     */
    public void SetGameStateToInGame();

    /**
     * Change game state to "Menu"
     */
    public void SetGameStateToMenu();
    
    /**
     * Change game state to "Options"
     */
    public void SetGameStateToOptions();

    /**
     * Exit the game
     */
    public void ExitGame();

    /**
     * Recovery the flag that indicates the game must terminate
     */
    public bool GetTerminateStatus();
    
    /**
     * Start the configurations for in-game status
     */
    public void InitGameConfigurations();
    
    /**
     * Reset the game
     */
    public void Reset();

    /**
     * Define the configurations to go to menu
     */
    public void ToMenu();
    
    /**
     * Skip the Draw Method, just once
     */
    public void SkipDrawOnce();
    
    /**
     * Skip the Render Method, once
     */
    public void SkipRenderOnce();

    /**
     * Get the HUD Object
     */
    public HUD GetHUD();

    /**
     * Verify if the shot is colliding with the background
     */
    public bool IsShotCollidingWithBackground(GameSprite sprite);
    
    public void SetGameStateToEnding();
    
    public void ToogleFullScreen();

    public void ControlStretched();
}