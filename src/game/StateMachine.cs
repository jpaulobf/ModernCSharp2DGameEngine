namespace Game;

/**
 * Author:      Joao P B Faria
 * Date:        Oct/2022
 * Description: Class representing the game state machine
 */
public class StateMachine
{
    public const int MENU           = 0;
    public const int OPTIONS        = 1;
    public const int IN_GAME        = 2;
    public const int EXITING        = 3;
    public const int GAME_OVER      = 4;
    public const int ENDING         = 5;
    private int CurrentGameState    = MENU;

    /**
     * Public constructor (the default initial state is MENU)
     */
    public StateMachine(int initialState = StateMachine.MENU)
    {
        if (initialState != MENU && 
            initialState != OPTIONS && 
            initialState != IN_GAME && 
            initialState != EXITING &&
            initialState != ENDING &&
            initialState != GAME_OVER)
        {
            this.CurrentGameState = MENU;
        } 
        else 
        {
            this.CurrentGameState = initialState;
        }
    }

    /**
     * Recovery the current game state
     */
    public int GetCurrentGameState() 
    {
        return (this.CurrentGameState);
    }

    /**
     * Define the current game to the menu state
     */
    public void SetStateToMenu()
    {
        this.CurrentGameState = MENU;
    }

    /**
     * Define the current game to the option state
     */
    public void SetGameStateToOptions()
    {
        this.CurrentGameState = OPTIONS;
    }

    /**
     * Define the current game to the GameOver state
     */
    public void SetGameStateToGameOver()
    {
        this.CurrentGameState = GAME_OVER;
    }

    public void SetStateToInGame()
    {
        this.CurrentGameState = IN_GAME;
    }

    public void SetStateToExiting()
    {
        this.CurrentGameState = EXITING;
    }

    internal void SetStateToEnding()
    {
        this.CurrentGameState = ENDING;
    }
}