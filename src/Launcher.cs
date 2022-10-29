using Engine;

namespace GameEngine;

/**
 * This is the Launcher to the program
 */
static class Launcher
{
    //const
    private const string VISUAL_STUDIO = "..\\..\\..\\";
    private const string VS_CODE = ".\\";
    
    //current gamepath
    public static string path = VS_CODE;
    
    /**
     * Just start the "Game" class, passing the desired FPS
     * 0 for unlimited
     */
    static void Main()
    {
        new MyGame(200);
    } 
}