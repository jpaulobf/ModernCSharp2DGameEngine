using Engine;

namespace GameEngine;

/**
 * This is the Laucher to the program
 */
static class Laucher
{
    /**
     * Just start the "Game" class, passing the desired FPS
     * 0 for unlimited
     */
    static void Main()
    {
        new MyGame(0);
    } 
}