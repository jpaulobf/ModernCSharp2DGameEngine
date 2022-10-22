/**
 * 
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
     *
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
     *
     */
    public int GetCurrentGameState() 
    {
        return (this.CurrentGameState);
    }

    /**
     *
     */
    public void SetStateToMenu()
    {
        this.CurrentGameState = MENU;
    }

    /**
     *
     */
    public void SetGameStateToOption()
    {
        this.CurrentGameState = OPTION;
    }
}