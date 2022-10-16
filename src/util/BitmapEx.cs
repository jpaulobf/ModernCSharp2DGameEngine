namespace Util;

using GameEngine;

public class BitmapEx
{
    private BitmapEx() 
    {}

    public static Bitmap New(string filepath) 
    {
        return new Bitmap(@"" + Launcher.path + filepath);
    }
}