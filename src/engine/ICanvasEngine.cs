namespace Engine;

/**
 * Public interface for Canvas from GameEngine
 */
public interface ICanvasEngine {
    /**
     * Update the game logic / receives the frametime
     * @param frametime
     */
    public void Update(long frametime);

    /**
     * Draw the game / receives the frametime
     * @param frametime
     */
    public void Draw(long frametime);

    /**
     * Render the backbuffer in the canvas
     */
    public void Render();

    /**
     * Release anything in the gameloop
     */
    public void Release(long frametime);
    
    /**
     * Dispose the graphics
     */
    void GraphicDispose();
}