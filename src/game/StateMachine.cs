/**
 * Author:      Joao P B Faria
 * Date:        Oct/2022
 * Description: Class representing the game state machine
 */
public class StateMachine
{
    public const int MENU = 0;
    public const int OPTION = 1;
    public const int IN_GAME = 2;
    public const int EXITING = 3;
    public const int GAME_OVER = 4;
    private int CurrentGameState = IN_GAME;

    /**
     * Public constructor (the default initial state is MENU)
     */
    public StateMachine(int initialState = StateMachine.MENU)
    {
        if (initialState != MENU && 
            initialState != OPTION && 
            initialState != IN_GAME && 
            initialState != EXITING &&
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
    public void SetGameStateToOption()
    {
        this.CurrentGameState = OPTION;
    }
}