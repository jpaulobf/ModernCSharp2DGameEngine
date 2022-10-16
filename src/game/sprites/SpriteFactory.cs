namespace Game;

/**
 * Author: Joao P B Faria
 * Date: Oct/2022
 * Description: Class responsible for create the sprites (as a gateway)
 */
public class SpriteFactory 
{
    public static GameSprite CreateSprite(IGame game, byte type, int x, int y, byte parameter = 1, bool flag = false, short maxLeft = 0, short maxRight = 0, byte direction = 0)
    {
        if (type == GameSprite.HOUSE) {
            return (new StaticSprite(game, "img\\house.png", 73, 44, GameSprite.HOUSE, x, y));
        } else if (type == GameSprite.HOUSE2) {
            return (new StaticSprite(game, "img\\house2.png", 73, 44, GameSprite.HOUSE2, x, y));
        } else if (type == GameSprite.FUEL) {
            return (new StaticSprite(game, "img\\fuel.png", 32, 55, GameSprite.FUEL, x, y));
        } else if (type == GameSprite.SHIP) {
            return (new EnemySprite(game, type, "img\\ship.png", 73, 18, x, y, 100, parameter, 25, flag, maxLeft, maxRight, direction));
        } else {
            return (new EnemySprite(game, type, "img\\helitile.png", 36, 23, x, y, 100, parameter, 25, flag, maxLeft, maxRight, direction));
        }
    }
}