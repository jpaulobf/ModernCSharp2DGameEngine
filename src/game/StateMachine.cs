public class StateMachine
{

    public static int MENU = 0;
    public static int OPTION = 1;
    public static int IN_GAME = 2;
    public static int EXITING = 3;

    public int CurrentGameState {get; set;} = IN_GAME;

    

}