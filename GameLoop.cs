namespace GameEngine;

using System;
using System.Drawing;
using System.Threading.Tasks;

public class GameLoop
{
    private Game _myGame;

    /// <summary>
    /// Status of GameLoop
    /// </summary>
    public bool Running { get; private set; }

    /// <summary>
    /// Load Game into GameLoop
    /// </summary>
    public void Load(Game gameObj)
    {
        _myGame = gameObj;
    }

    /// <summary>
    /// Start GameLoop
    /// </summary>
    public async void Start()
    {
        if (_myGame == null)
            throw new ArgumentException("Game not loaded!");

        // Load game content
        _myGame.Load();

        // Set gameloop state
        Running = true;

        // Set previous game time
        DateTime _previousGameTime = DateTime.Now;

        while (Running)
        {
            // Calculate the time elapsed since the last game loop cycle
            TimeSpan GameTime = DateTime.Now - _previousGameTime;
            // Update the current previous game time
            _previousGameTime = _previousGameTime + GameTime;
            // Update the game
            _myGame.Update(GameTime);
            // Update Game at 60fps
            await Task.Delay(8);
        }
    }

    /// <summary>
    /// Stop GameLoop
    /// </summary>
    public void Stop()
    {
        Running = false;
        _myGame?.Unload();
    }

    /// <summary>
    /// Draw Game Graphics
    /// </summary>
    public void Draw(Graphics gfx)
    {
        _myGame.Draw(gfx);
    }
}