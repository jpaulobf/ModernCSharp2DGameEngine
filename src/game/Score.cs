using Game;

public class Score
{
    private IGame GameRef;
    private const short HELI_POINTS     = 100;
    private const short SHIP_POINTS     = 100;
    private const short FUEL_POINTS     = 100;
    private const short AIRPLANE_POINTS = 100;
    private const short BRIDGE_POINTS   = 1000;

    public Score(IGame gameRef)
    {
        this.GameRef = gameRef;
    }

    public void ItemDestructed(int type)
    {
        
    }

    public void Update(long frametime)
    {

    }

    public void Draw(Graphics gfx)
    {

    }
}