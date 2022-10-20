public class StateMachine
{

    public static int MENU = 0;
    public static int OPTION = 1;
    public static int IN_GAME = 2;
    public static int EXITING = 3;

    private int CurrentGameState = IN_GAME;

    public int GetCurrentGameState() 
    {
        return (this.CurrentGameState);
    }

    public void SetStateToMenu()
    {
        this.CurrentGameState = MENU;
    }

    public void SetGameStateToOption()
    {
        this.CurrentGameState = OPTION;
    }
    



}