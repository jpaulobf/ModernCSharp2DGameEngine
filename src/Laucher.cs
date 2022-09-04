using engine;

namespace GameEngine;

static class Laucher
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        Application.Run(new MyGame(60));
    } 
}