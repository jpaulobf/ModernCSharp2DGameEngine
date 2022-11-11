namespace Game;

using Util;

/**
 * Author: Joao P B Faria
 * Date: Oct/2022
 * Description: Class responsible for create the sprites (as a gateway)
 */
public class SpriteFactory 
{
    public static GameSprite CreateSprite(IGame game, byte type, int x, int y, bool renderReversed = false, short maxLeft = 0, short maxRight = 0, byte direction = 0)
    {
        if (type == GameSprite.HOUSE) 
        {
            return (new StaticSprite(game, 73, 44, GameSprite.HOUSE, x, y));
        } 
        else if (type == GameSprite.HOUSE2) 
        {
            return (new StaticSprite(game, 73, 44, GameSprite.HOUSE2, x, y));
        } 
        else if (type == GameSprite.FUEL) 
        {
            return (new StaticSprite(game, 32, 55, GameSprite.FUEL, x, y));
        } 
        else if (type == GameSprite.SHIP) 
        {
            return (new EnemySprite(game, type, 73, 18, x, y, 100, 0, renderReversed, maxLeft, maxRight, direction));
        } 
        else 
        {
            return (new EnemySprite(game, type, 36, 23, x, y, 100, 50, renderReversed, maxLeft, maxRight, direction));
        }
    }
}