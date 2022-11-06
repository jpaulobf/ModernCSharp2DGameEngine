namespace Util;

using GameEngine;

/**
 * Author:      Joao Paulo B Faria
 * Date:        Oct/2022
 * Description: Class utility to create a bitmap with the current system path
 */
public class BitmapEx
{
    //no public constructor
    private BitmapEx() 
    {}

    //method factory
    public static Bitmap New(string filepath) 
    {
        return new Bitmap(@"" + Launcher.path + filepath);
    }
}