using Game;

public class Score
{
    private IGame GameRef;
    private const short HELI_POINTS         = 60;
    private const short SHIP_POINTS         = 30;
    private const short FUEL_POINTS         = 80;
    private const short AIRPLANE_POINTS     = 100;
    private const short BRIDGE_POINTS       = 500;
    private int Points                      = 0;
    private string StPoints                 = "";
    private const short RightPosX           = 447;
    private const short TopPosY             = 434;

    public Score(IGame gameRef)
    {
        this.GameRef = gameRef;
    }

    public void ItemDestructed(int type)
    {
        switch (type)
        {
            case GameSprite.SHIP:
                this.Points += SHIP_POINTS;
                break;
            case GameSprite.HELI:
                this.Points += HELI_POINTS;
                break;
            case GameSprite.FUEL:
                this.Points += FUEL_POINTS;
                break;
            case GameSprite.AIRPLANE:
                this.Points += AIRPLANE_POINTS;
                break;
        }
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