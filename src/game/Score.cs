using Game;

public class Score
{
    private IGame GameRef;
    private const short HELI_POINTS         = 100;
    private const short SHIP_POINTS         = 100;
    private const short FUEL_POINTS         = 100;
    private const short AIRPLANE_POINTS     = 100;
    private const short BRIDGE_POINTS       = 1000;
    private int Points                      = 0;
    private string StPoints                 = "";
    private const short RightPosX           = 0;
    private const short TopPosY             = 0;

    public Score(IGame gameRef)
    {
        this.GameRef = gameRef;
    }

    public void ItemDestructed(int type)
    {
        
    }

    public void Update(long frametime)
    {
        this.StPoints = Points.ToString();
    }

    public void Draw(Graphics gfx)
    {

    }

    public void Reset()
    {
        this.Points = 0;
    }

}