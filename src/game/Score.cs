using Game;
using Util;

/**
 * Author:      Joao P B Faria
 * Date:        Oct/2022
 * Description: Class representing the Player Score
 */
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

    /**
     * Public constructor
     */
    public Score(IGame gameRef)
    {
        this.GameRef = gameRef;
    }

    /**
     * Store the images from numbers 0 to 9
     */
    private Dictionary<char, Bitmap> NumbersImages = new Dictionary<char, Bitmap> 
    {
        {'0', LoadingStuffs.GetInstance().GetImage("number-0")},
        {'1', LoadingStuffs.GetInstance().GetImage("number-1")},
        {'2', LoadingStuffs.GetInstance().GetImage("number-2")},
        {'3', LoadingStuffs.GetInstance().GetImage("number-3")},
        {'4', LoadingStuffs.GetInstance().GetImage("number-4")},
        {'5', LoadingStuffs.GetInstance().GetImage("number-5")},
        {'6', LoadingStuffs.GetInstance().GetImage("number-6")},
        {'7', LoadingStuffs.GetInstance().GetImage("number-7")},
        {'8', LoadingStuffs.GetInstance().GetImage("number-8")},
        {'9', LoadingStuffs.GetInstance().GetImage("number-9")}
    };

    /**
     * Add points based on destructed item
     */
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

    /**
     * Score update cycle
     */
    public void Update(long frametime)
    {
        this.StPoints = Points.ToString();
    }

    /**
     * Score draw cycle
     */
    public void Draw(Graphics gfx)
    {
        for (int i = (StPoints.Length - 1), j = 1; i >= 0; i--, j++)
        {
            var temp = NumbersImages[StPoints[i]];
            gfx.DrawImage(temp, RightPosX - (j * temp.Width), TopPosY, temp.Width, temp.Height);
        }
    }

    /**
     * Reset the score
     */
    public void Reset()
    {
        this.Points = 0;
        this.StPoints = "";
    }
}